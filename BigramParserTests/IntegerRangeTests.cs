using System;
using Xunit;
using BigramParser;

namespace BigramParserTests
{
    public class IntegerRangeTests
    {
        [Fact]
        public void TestCreateIntegerRangeSuccess()
        {
            //Given
            //When
            IntegerRange result = new IntegerRange(0, 5);

            //Then
            Assert.Equal("(0-5)", result.ToString());
            Assert.Equal(0, result.Min);
            Assert.Equal(5, result.Max);
        }

        [Fact]
        public void TestCreateIntegerRangeWithEqualMinMaxFails()
        {
            Assert.Throws<ArgumentException>(() => { new IntegerRange(5, 5); });
        }

        [Fact]
        public void TestCreateIntegerRangeWithLargeMinFails()
        {
            Assert.Throws<ArgumentException>(() => { new IntegerRange(10, 5); });
        }

        [Fact]
        public void TestIntegerRangeContainsValueTrue()
        {
            //Given
            var range = new IntegerRange(0, 5);

            //When
            var result = range.ContainsValue(3);

            //Then
            Assert.True(result);
        }

        [Fact]
        public void TestIntegerRangeContainsValueFalse()
        {
            //Given
            var range = new IntegerRange(0, 5);

            //When
            var result = range.ContainsValue(6);

            //Then
            Assert.False(result);
        }
    }
}
