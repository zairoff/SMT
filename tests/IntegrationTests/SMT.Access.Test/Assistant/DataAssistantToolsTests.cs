using Microsoft.EntityFrameworkCore;
using SMT.Access.Data;
using SMT.Access.Repository.Statics;
using SMT.Access.Test.Feeder;
using SMT.Agents.DataAssistant;
using SMT.Domain;
using SMT.Services.Interfaces;
using SMT.ViewModel.Dto.LineDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace SMT.Access.Test.Assistant
{
    // The data assistant's tools run against the real StaticsRepository on an in-memory DB.
    // No model calls here - these check that each tool returns the right figures and rejects bad input.
    public class DataAssistantToolsTests : IDisposable
    {
        private static readonly DateTime Day = new DateTime(2026, 11, 20);

        private readonly InMemoryAppDbContext _db;
        private readonly DataAssistantTools _tools;
        private Line _line1;
        private Line _line2;
        private Model _model;

        public DataAssistantToolsTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _db = new InMemoryAppDbContext(options);
            Seed();

            _tools = new DataAssistantTools(new StaticsRepository(_db), new FakeLineService(_db));
        }

        public void Dispose() => _db.Dispose();

        [Fact]
        public void EveryDefinedToolIsHandled()
        {
            var names = DataAssistantTools.Definitions.Select(d => d.Json.GetProperty("name").GetString()).ToList();

            Assert.Equal(new[] { "list_lines", "defect_ranking", "daily_plan", "daily_quality", "day_breakdown" }, names);
        }

        [Fact]
        public async Task ListLines_ReturnsIdsAndNames()
        {
            var result = Parse(await _tools.ExecuteAsync("list_lines", Input(new { })));

            Assert.Equal(new[] { "SMT-1", "SMT-2" }, result.EnumerateArray().Select(l => l.GetProperty("name").GetString()));
            Assert.Equal(_line1.Id, result[0].GetProperty("id").GetInt32());
        }

        [Fact]
        public async Task DefectRanking_ByDefectOnOneLine_SortedByCount()
        {
            var result = Parse(await _tools.ExecuteAsync("defect_ranking",
                Input(new { group_by = "defect", from = "2026-11-20", to = "2026-11-20", line_id = _line1.Id })));

            Assert.Equal("Tombstone", result[0].GetProperty("name").GetString());
            Assert.Equal(3, result[0].GetProperty("count").GetInt32());
            Assert.Equal("Bridge", result[1].GetProperty("name").GetString());
            Assert.Equal(1, result[1].GetProperty("count").GetInt32());
        }

        [Fact]
        public async Task DefectRanking_ByLine_CountsAllLines()
        {
            var result = Parse(await _tools.ExecuteAsync("defect_ranking",
                Input(new { group_by = "line", from = "2026-11-20", to = "2026-11-20" })));

            Assert.Equal(4, result.EnumerateArray().First(r => r.GetProperty("name").GetString() == "SMT-1").GetProperty("count").GetInt32());
            Assert.Equal(1, result.EnumerateArray().First(r => r.GetProperty("name").GetString() == "SMT-2").GetProperty("count").GetInt32());
        }

        [Fact]
        public async Task DailyQuality_ComputesFtqPerLine()
        {
            var result = Parse(await _tools.ExecuteAsync("daily_quality",
                Input(new { from = "2026-11-20", to = "2026-11-20", line_id = _line1.Id })));

            // 4 defects on 200 boards -> 98%.
            Assert.Equal(98.0, result[0].GetProperty("ftq").GetDouble());
            Assert.Equal(200, result[0].GetProperty("produced").GetInt32());
        }

        [Fact]
        public async Task DayBreakdown_MergesPlanAndDefects()
        {
            var result = Parse(await _tools.ExecuteAsync("day_breakdown", Input(new { date = "2026-11-20" })));

            var line1 = result.EnumerateArray().Single(r => r.GetProperty("lineName").GetString() == "SMT-1");
            Assert.Equal(250, line1.GetProperty("planned").GetInt32());
            Assert.Equal(200, line1.GetProperty("produced").GetInt32());
            Assert.Equal(4, line1.GetProperty("defectCount").GetInt32());
        }

        [Theory]
        [InlineData("{\"from\":\"20.11.2026\",\"to\":\"2026-11-20\"}")]
        [InlineData("{\"from\":\"2026-11-20\",\"to\":\"2026-11-01\"}")]
        [InlineData("{\"from\":\"2024-01-01\",\"to\":\"2026-11-20\"}")]
        [InlineData("{\"to\":\"2026-11-20\"}")]
        [InlineData("{\"from\":\"2026-11-20\",\"to\":\"2026-11-20\",\"line_id\":\"one\"}")]
        public async Task DailyPlan_BadInput_IsRejected(string json)
        {
            await Assert.ThrowsAsync<ToolInputException>(() =>
                _tools.ExecuteAsync("daily_plan", JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(json)));
        }

        [Fact]
        public async Task UnknownToolOrGroup_IsRejected()
        {
            await Assert.ThrowsAsync<ToolInputException>(() => _tools.ExecuteAsync("drop_table", Input(new { })));
            await Assert.ThrowsAsync<ToolInputException>(() => _tools.ExecuteAsync("defect_ranking",
                Input(new { group_by = "employee", from = "2026-11-20", to = "2026-11-20" })));
        }

        private static IReadOnlyDictionary<string, JsonElement> Input(object value)
        {
            return JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(JsonSerializer.Serialize(value));
        }

        private static JsonElement Parse(string json) => JsonDocument.Parse(json).RootElement.Clone();

        private void Seed()
        {
            _line1 = new Line { Name = "SMT-1" };
            _line2 = new Line { Name = "SMT-2" };
            var productBrand = new ProductBrand { Product = new Product { Name = "TV" }, Brand = new Brand { Name = "Artel" } };
            _model = new Model { Name = "MB-55", ProductBrand = productBrand };
            var tombstone = new Defect { Name = "Tombstone" };
            var bridge = new Defect { Name = "Bridge" };
            _db.AddRange(_line1, _line2, _model, tombstone, bridge);
            _db.SaveChanges();

            _db.AddRange(
                new Plan { LineId = _line1.Id, ModelId = _model.Id, Date = Day, RequiredCount = 250, ProducedCount = 200 },
                new Plan { LineId = _line2.Id, ModelId = _model.Id, Date = Day, RequiredCount = 100, ProducedCount = 100 });

            Report Defect(Line line, Defect defect) =>
                new Report { LineId = line.Id, ModelId = _model.Id, DefectId = defect.Id, CreatedDate = Day.AddHours(10) };

            _db.AddRange(
                Defect(_line1, tombstone), Defect(_line1, tombstone), Defect(_line1, tombstone),
                Defect(_line1, bridge),
                Defect(_line2, bridge));
            _db.SaveChanges();
        }

        private class FakeLineService : ILineService
        {
            private readonly AppDbContext _db;

            public FakeLineService(AppDbContext db) => _db = db;

            public Task<IEnumerable<LineResponse>> GetAllAsync(bool? isActive) =>
                Task.FromResult<IEnumerable<LineResponse>>(_db.Lines.OrderBy(l => l.Id)
                    .Select(l => new LineResponse { Id = l.Id, Name = l.Name, IsActive = l.IsActive }).ToList());

            public Task<LineResponse> GetAsync(int id) => throw new NotImplementedException();
            public Task<LineResponse> GetByNameAsync(string name) => throw new NotImplementedException();
            public Task<LineResponse> AddAsync(LineCreate lineCreate) => throw new NotImplementedException();
            public Task<LineResponse> UpdateAsync(int id, LineUpdate lineUpdate) => throw new NotImplementedException();
            public Task<LineResponse> DeleteAsync(int id) => throw new NotImplementedException();
        }
    }
}
