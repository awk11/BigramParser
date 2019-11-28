using System;
using Xunit;
using BigramParser;

namespace BigramParserTests
{
    public class BigramTests
    {
        [Fact]
        public void TestCleanStringAllFalseSettings()
        {
            //Given
            Program.caseSensitive = false;
            Program.punctuationSensitive = false;
            string str = "    The Quick   Brown Fox jumped over \n the Lazy Dog.    ";

            //When
            string result = Program.CleanString(str);

            //Then
            string expected = "the quick brown fox jumped over the lazy dog";
            Assert.Equal(expected, result);
        }

        [Fact]
        public void TestCleanStringCaseSensative()
        {
            //Given
            Program.caseSensitive = true;
            Program.punctuationSensitive = false;
            string str = "    The Quick Brown Fox jumped over the Lazy Dog.    ";

            //When
            string result = Program.CleanString(str);

            //Then
            string expected = "The Quick Brown Fox jumped over the Lazy Dog";
            Assert.Equal(expected, result);
        }

        [Fact]
        public void TestCleanStringPunctuationSensative()
        {
            //Given
            Program.caseSensitive = false;
            Program.punctuationSensitive = true;
            string str = "    The Quick Brown Fox, jumped, over the Lazy Dog.    ";

            //When
            string result = Program.CleanString(str);

            //Then
            string expected = "the quick brown fox, jumped, over the lazy dog.";
            Assert.Equal(expected, result);
        }

        [Fact]
        public void TestGetCount()
        {
            //Given
            var words = new string[] { "this", "is", "a", "test", "this", "is", "a", "test" };

            //When
            int result = Program.GetCount(words, 1);

            //Then
            Assert.Equal(2, result);
        }

        [Fact]
        public void TestGetBigrams()
        {
            //Given
            var str = "this is a test this is a test";

            //When
            var bigramSet = Program.GetBigrams(str);

            //Then
            Assert.Equal(4, bigramSet.Count);
            Assert.Equal(2, bigramSet["this is"]);
            Assert.Equal(2, bigramSet["is a"]);
            Assert.Equal(2, bigramSet["a test"]);
            Assert.Equal(1, bigramSet["test this"]);
        }
    }
}
