using SMT.Domain.Feeder;
using System.Collections.Generic;

namespace SMT.ViewModel.Dto.FeederSetupDto
{
    // Rows come from the operator's Excel (parsed in the browser), same columns as the old
    // model-changer import: Module | Feeder | Code. Code may be a part number, an RCode or a scanned reel QR.
    public class FeederSetupImport
    {
        public int LineId { get; set; }

        public int ModelId { get; set; }

        public BoardSide Side { get; set; } = BoardSide.Top;

        public string SourceFileName { get; set; }

        public List<FeederSetupImportRow> Rows { get; set; } = new List<FeederSetupImportRow>();
    }

    public class FeederSetupImportRow
    {
        // Excel row number, echoed back in the preview so the operator can find the row.
        public int RowNumber { get; set; }

        public string Module { get; set; }

        public string Feeder { get; set; }

        public string Code { get; set; }
    }
}
