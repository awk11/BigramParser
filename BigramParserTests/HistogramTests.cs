using System;
using Xunit;
using BigramParser;
using System.Collections.Generic;

namespace BigramParserTests
{
    public class HistogramTests
    {
        [Fact]
        public void TestDetermineHistogramRangesLowMaximum()
        {
            //Given
            Program.columnCount = 4;

            //When
            List<IntegerRange> results = Program.DetermineHistogramRanges(4);

            //Then
            Assert.Equal(4, results.Count);
            Assert.Equal("(0-1)", results[0].ToString());
            Assert.Equal("(2-3)", results[1].ToString());
            Assert.Equal("(4-5)", results[2].ToString());
            Assert.Equal("(6-7)", results[3].ToString());
        }

        [Fact]
        public void TestDetermineHistogramRangesHighMaximum()
        {
            //Given
            Program.columnCount = 4;

            //When
            List<IntegerRange> results = Program.DetermineHistogramRanges(11);

            //Then
            Assert.Equal(4, results.Count);
            Assert.Equal("(0-2)", results[0].ToString());
            Assert.Equal("(3-5)", results[1].ToString());
            Assert.Equal("(6-8)", results[2].ToString());
            Assert.Equal("(9-11)", results[3].ToString());
        }
    }
}
