using SMT.Services.Interfaces;
using SMT.ViewModel.Dto.ComponentDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SMT.Services.GoogleSheets
{
    public class GoogleSheetsComponentImportService : IGoogleSheetsComponentImportService
    {
        // Order matters: fields are resolved in this sequence, and each sheet column can only be
        // claimed by one field. This lets two fields share an alias (e.g. the source sheet repeats
        // its "SAP code" column) without both fields grabbing the same column.
        private static readonly (string Field, string[] Aliases)[] ColumnMappings =
        {
            ("PartNumber", new[] { "PartNumber", "Артикул завода" }),
            ("RCode", new[] { "RCode", "Сап код" }),
            ("StorePlaceNumber", new[] { "StorePlaceNumber" }),
            ("SapPlace", new[] { "SapPlace", "Сап код" }),
            ("PlaceCode", new[] { "PlaceCode" }),
            ("Specification", new[] { "Specification", "Краткий текст материала" })
        };

        private readonly HttpClient _httpClient;
        private readonly IComponentService _componentService;
        private readonly string _spreadsheetId;

        public GoogleSheetsComponentImportService(HttpClient httpClient, IComponentService componentService, string spreadsheetId)
        {
            _httpClient = httpClient;
            _componentService = componentService;
            _spreadsheetId = spreadsheetId;
        }

        public async Task<IEnumerable<ComponentBulkImportResult>> SyncFromSheetAsync()
        {
            var csv = await _httpClient.GetStringAsync(
                $"https://docs.google.com/spreadsheets/d/{_spreadsheetId}/export?format=csv");

            var components = MapRowsToComponents(ParseCsv(csv));

            // The sheet only ever grows by appending rows, so anything whose PartNumber is
            // already in the database was handled by a previous sync and can be skipped —
            // this keeps re-syncing cheap instead of re-upserting thousands of unchanged rows.
            var existingPartNumbers = await _componentService.GetAllPartNumbersAsync();
            var newComponents = components
                .Where(c => string.IsNullOrWhiteSpace(c.PartNumber) || !existingPartNumbers.Contains(c.PartNumber))
                .ToList();

            return await _componentService.BulkAddAsync(newComponents);
        }

        public static List<ComponentCreate> MapRowsToComponents(List<List<string>> rows)
        {
            var components = new List<ComponentCreate>();

            if (rows == null)
                return components;

            var headerRowIndex = rows.FindIndex(row => row.Any(cell => !string.IsNullOrWhiteSpace(cell)));
            if (headerRowIndex < 0)
                return components;

            var headers = rows[headerRowIndex].Select(header => header?.Trim() ?? string.Empty).ToList();
            var columnIndexes = ResolveColumnIndexes(headers);

            for (var i = headerRowIndex + 1; i < rows.Count; i++)
            {
                var row = rows[i];
                if (row.All(string.IsNullOrWhiteSpace))
                    continue;

                components.Add(new ComponentCreate
                {
                    PartNumber = GetCell(row, columnIndexes["PartNumber"]),
                    RCode = GetCell(row, columnIndexes["RCode"]),
                    StorePlaceNumber = GetCell(row, columnIndexes["StorePlaceNumber"]),
                    SapPlace = GetCell(row, columnIndexes["SapPlace"]),
                    PlaceCode = GetCell(row, columnIndexes["PlaceCode"]),
                    Specification = GetCell(row, columnIndexes["Specification"])
                });
            }

            return components;
        }

        private static Dictionary<string, int> ResolveColumnIndexes(List<string> headers)
        {
            var columnIndexes = new Dictionary<string, int>();
            var usedIndexes = new HashSet<int>();

            foreach (var (field, aliases) in ColumnMappings)
            {
                var index = -1;

                foreach (var alias in aliases)
                {
                    for (var h = 0; h < headers.Count; h++)
                    {
                        if (usedIndexes.Contains(h) || !string.Equals(headers[h], alias, StringComparison.OrdinalIgnoreCase))
                            continue;

                        index = h;
                        break;
                    }

                    if (index >= 0)
                        break;
                }

                columnIndexes[field] = index;
                if (index >= 0)
                    usedIndexes.Add(index);
            }

            return columnIndexes;
        }

        private static string GetCell(List<string> row, int columnIndex)
        {
            if (columnIndex < 0 || columnIndex >= row.Count)
                return string.Empty;

            return row[columnIndex] ?? string.Empty;
        }

        public static List<List<string>> ParseCsv(string csv)
        {
            var rows = new List<List<string>>();
            var row = new List<string>();
            var field = new StringBuilder();
            var inQuotes = false;

            for (var i = 0; i < csv.Length; i++)
            {
                var c = csv[i];

                if (inQuotes)
                {
                    if (c == '"')
                    {
                        if (i + 1 < csv.Length && csv[i + 1] == '"')
                        {
                            field.Append('"');
                            i++;
                        }
                        else
                        {
                            inQuotes = false;
                        }
                    }
                    else
                    {
                        field.Append(c);
                    }

                    continue;
                }

                switch (c)
                {
                    case '"':
                        inQuotes = true;
                        break;
                    case ',':
                        row.Add(field.ToString());
                        field.Clear();
                        break;
                    case '\r':
                        break;
                    case '\n':
                        row.Add(field.ToString());
                        field.Clear();
                        rows.Add(row);
                        row = new List<string>();
                        break;
                    default:
                        field.Append(c);
                        break;
                }
            }

            if (field.Length > 0 || row.Count > 0)
            {
                row.Add(field.ToString());
                rows.Add(row);
            }

            return rows;
        }
    }
}
