using SMT.Domain;
using SMT.Services.Feeder;
using System.Collections.Generic;
using Xunit;

namespace SMT.Access.Test.Feeder
{
    public class FeederCodeParsingTests
    {
        [Theory]
        [InlineData("07", "7")]
        [InlineData(" 7 ", "7")]
        [InlineData("000", "0")]
        [InlineData("12l", "12L")]
        public void NormalizeSlotNumber_StripsZerosAndSpaces(string input, string expected)
        {
            Assert.Equal(expected, FeederCodes.NormalizeSlotNumber(input));
        }

        [Fact]
        public void BuildSlotCode_PadsSingleDigitAndNormalizesModule()
        {
            Assert.Equal("FS-3-M2-07", FeederCodes.BuildSlotCode(3, " m 2", "7"));
            Assert.Equal("FS-3-M2-12", FeederCodes.BuildSlotCode(3, "M2", "012"));
        }

        [Fact]
        public void IsSlotCode_DistinguishesStickersFromReels()
        {
            Assert.True(FeederCodes.IsSlotCode("fs-3-M2-07"));
            Assert.False(FeederCodes.IsSlotCode("RC0402-10K@R0001@A1@14.11.2026 10:00:00"));
        }

        [Fact]
        public void Parse_PrinterAppLabel_TriesRCodeBeforePartNumber()
        {
            var parsed = ReelBarcodeParser.Parse("RC0402-10K@R0001@A-01@14.11.2026 10:00:00");

            Assert.Equal("RC0402-10K", parsed.PartNumber);
            Assert.Equal("R0001", parsed.RCode);
            Assert.Equal(new[] { "R0001", "RC0402-10K" }, parsed.Candidates);
        }

        [Fact]
        public void Parse_LabelWithEmptyPartNumber_StillHasRCode()
        {
            // PrinterApp prints an empty part number when it found the component by RCode.
            var parsed = ReelBarcodeParser.Parse("@R0001@A-01@14.11.2026 10:00:00");

            Assert.Null(parsed.PartNumber);
            Assert.Equal(new[] { "R0001" }, parsed.Candidates);
        }

        [Fact]
        public void Parse_BareCode_IsSingleNormalizedCandidate()
        {
            Assert.Equal(new[] { "RC0402-10K" }, ReelBarcodeParser.Parse(" rc0402-10k ").Candidates);
        }

        [Fact]
        public void ComponentIndex_ResolvesRCodePrimaryAndAlternate()
        {
            var resistor = new Component { Id = 1, RCode = "R0001", PartNumber = new List<string> { "RC0402-10K", "ERJ2-10K" } };
            var index = new ComponentIndex(new[] { resistor });

            Assert.Equal(ComponentMatch.RCode, index.Resolve("r0001").Match);
            Assert.Equal(ComponentMatch.PrimaryPartNumber, index.Resolve("RC0402-10K").Match);

            var alternate = index.Resolve("ERJ2-10K");
            Assert.Equal(ComponentMatch.AlternatePartNumber, alternate.Match);
            Assert.Same(resistor, alternate.Component);

            Assert.Equal(ComponentMatch.None, index.Resolve("UNKNOWN").Match);
        }

        [Fact]
        public void ComponentIndex_SharedPartNumber_IsAmbiguous()
        {
            var index = new ComponentIndex(new[]
            {
                new Component { Id = 1, RCode = "R0001", PartNumber = new List<string> { "SHARED" } },
                new Component { Id = 2, RCode = "R0002", PartNumber = new List<string> { "SHARED" } },
            });

            Assert.Equal(ComponentMatch.Ambiguous, index.Resolve("SHARED").Match);
        }

        [Fact]
        public void SlotNumberComparer_OrdersNumericallyThenText()
        {
            var numbers = new List<string> { "10", "2", "12L", "1" };

            numbers.Sort(FeederSlotNumberComparer.Instance);

            Assert.Equal(new[] { "1", "2", "10", "12L" }, numbers);
        }
    }
}
