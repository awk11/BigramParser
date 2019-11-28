using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace BigramParser
{
    public class Program
    {
        // Settings
        public static bool caseSensitive = false;
        public static bool punctuationSensitive = false;
        public static bool numberSensitive = true;
        public static bool showBigramCounts = true;
        public static int columnCount = 4;

        #region Main Application
        static void Main(string[] args)
        {
            string end = "";
            Console.Write("Welcome to my bigram finder! I hope you enjoy using my little application.");

            do
            {
                Console.WriteLine("\nPlease enter a string: ");
                string input = Console.ReadLine();
                ConfigureSettings();

                input = CleanString(input);
                Dictionary<string, int> bigram = GetBigrams(input);

                if (showBigramCounts)
                {
                    Console.WriteLine("\nBigram results:");
                    foreach (var pair in bigram)
                    {
                        Console.WriteLine(pair.Key + ": " + pair.Value);
                    }
                }

                Console.WriteLine("\nHistogram:\n");
                DrawHistogram(bigram.Values.ToList());

                Console.Write("\n\nWould you like to try again with a new string? (y/n): ");
                end = Console.ReadLine();
            } while (end.ToLower() == "y");
        }

        /// <summary>
        /// Allows the user to configure global settings for running the application
        /// </summary>
        public static void ConfigureSettings()
        {
            Console.WriteLine("\nCurrent Bigram settings:");
            Console.WriteLine("1. Case Sensitive (If true, 'hello' and 'Hello' are different): " + caseSensitive);
            Console.WriteLine("2. Punctuation Sensitive (If true, 'hello' and 'hello.' are different): " + punctuationSensitive);
            Console.WriteLine("3. Show individual bigram counts: " + showBigramCounts);
            Console.WriteLine("4. Number of columns in the histogram: " + columnCount);
            Console.Write("Would you like to proceed with these these settings? (y/n): ");
            string settingsConfirmation = Console.ReadLine();
            if (settingsConfirmation.ToLower() != "y")
            {
                Console.Write("Which setting would you like to change? (1-4 or any other value to cancel) ");
                settingsConfirmation = Console.ReadLine();
                switch (settingsConfirmation)
                {
                    case "1":
                        caseSensitive = !caseSensitive;
                        break;
                    case "2":
                        punctuationSensitive = !punctuationSensitive;
                        break;
                    case "3":
                        showBigramCounts = !showBigramCounts;
                        break;
                    case "4":
                        Console.Write("New Column Count: ");
                        try
                        {
                            columnCount = Int32.Parse(Console.ReadLine());
                        }
                        catch
                        {
                            Console.WriteLine("Invalid count.");
                        }
                        break;
                }
                ConfigureSettings();
            }
        }

        #endregion

        #region Bigram-related Functions

        /// <summary>
        /// Gets the set of bigrams and their counts
        /// </summary>
        /// <param name="input">The string to be parsed for bigrams</param>
        /// <returns>A dictionary of bigrams and counts</returns>
        public static Dictionary<string, int> GetBigrams(string input)
        {
            var words = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            Dictionary<string, int> bigrams = new Dictionary<string, int>();

            for (int i = 0; i < words.Length - 1; i++)
            {
                string key = words[i] + " " + words[i + 1];
                if (!bigrams.ContainsKey(key))
                {
                    bigrams[key] = GetCount(words, i);
                }
            }
            return bigrams;
        }

        /// <summary>
        /// Gets the amount of instances of a specfied bigram
        /// </summary>
        /// <param name="words">The list of words in the string</param>
        /// <param name="index">The index of the first word of the bigram</param>
        /// <returns>The number of times the bigram appears</returns>
        public static int GetCount(string[] words, int index)
        {
            int count = 0;

            for (int i = 0; i < words.Length - 1; i++)
            {
                if (words[i] == words[index] && words[i + 1] == words[index + 1])
                {
                    count++;
                }
            }
            return count;
        }

        /// <summary>
        /// Cleans up the string based on user settings to account for edge cases when searching for bigrams
        /// </summary>
        /// <param name="str">The string being cleaned</param>
        /// <returns>The cleaned up string</returns>
        public static string CleanString(string str)
        {
            str = str.Trim();
            if (!caseSensitive)
            {
                str = str.ToLower();
            }

            if (!punctuationSensitive)
            {
                // Removes punctuation from the string
                str = new string(str.ToCharArray().Where(c => !char.IsPunctuation(c)).ToArray());
            }

            //Converts all whitespace in the string to a single space character
            str = Regex.Replace(str, @"\s+", " ");

            return str;
        }

        #endregion

        #region Histogram-related functions

        /// <summary>
        /// Generates the histogram and draws it on screen in a horizontal format.
        /// Histogram will always have 4 rows
        /// </summary>
        /// <param name="bigrams">The list of bigram counts</param>
        public static void DrawHistogram(List<int> bigrams)
        {
            List<IntegerRange> histogramKeys = new List<IntegerRange>();
            List<int> histogramValues = new List<int>();

            // Determine the columns for the histogram
            int max = bigrams.Max();
            histogramKeys = DetermineHistogramRanges(max);

            for (int i = 0; i < histogramKeys.Count; i++)
            {
                histogramValues.Add(0);
            }

            // Goes through the bigrams and adds them to the appropriate value count
            for (int i = 0; i < bigrams.Count; i++)
            {
                for (int j = 0; j < histogramKeys.Count; j++)
                {
                    if (histogramKeys[j].ContainsValue(bigrams[i]))
                    {
                        histogramValues[j]++;
                    }
                }
            }

            float maxVal = histogramValues.Max();

            // Draw the histogram

            // Create the top label for column sizes
            float lastPad = 0;
            List<string> usedNums = new List<string> { "0" };
            Console.Write("\nRng     |0");
            for (float i = (maxVal * .25f); i < maxVal + 1; i += (maxVal * .25f))
            {
                string s = ((int)Math.Floor(i)).ToString();

                // Checks to ensure that a number isn't duplicated
                // Necessary for max values less than 4
                if (!usedNums.Contains(s))
                {
                    Console.Write(s.PadLeft((int)((Math.Floor(i / maxVal * 25)) - lastPad)));
                    usedNums.Add(s);
                }
                else if (i < maxVal)
                {
                    Console.Write(" ".PadLeft((int)((Math.Floor(i / maxVal * 25)) - lastPad)));
                }

                lastPad = (int)(Math.Floor(i / maxVal * 25));
            }

            Console.WriteLine("   |Ttl");

            // Draw the actual histogram
            for (int i = 0; i < histogramValues.Count; i++)
            {
                // Prints the column name
                Console.Write(String.Format("{0, -8}", histogramKeys[i].ToString()) + '|');
                string bar = "";
                for (int j = 0; j < (histogramValues[i] / maxVal * 25); j++)
                {
                    bar += '#';
                }

                // The totals for each column
                Console.WriteLine(String.Format("{0, -25}", bar) + "    |(" + histogramValues[i].ToString() + ")");
            }
        }

        /// <summary>
        /// Helper function for determining what the ranges for each column of the histogram should be
        /// </summary>
        /// <param name="max">The highest bigram count found</param>
        /// <returns>The Ranges to be used for the histogram</returns>
        public static List<IntegerRange> DetermineHistogramRanges(int max)
        {

            List<IntegerRange> histogramKeys = new List<IntegerRange>();

            // If there arent any bigrams with a count greater than he column count * 2, then the ranges need to be split up into increments of 2.
            if (max < columnCount * 2)
            {
                for (int i = 0; i < columnCount * 2 - 1; i += 2)
                {
                    histogramKeys.Add(new IntegerRange(i, i + 1));
                }
            }
            // Otherwise split it up based on the max value
            else
            {
                double divider = (double) max / columnCount;
                histogramKeys.Add(new IntegerRange(0, (int)divider));
                for (int i = 1; i < columnCount; i++)
                {
                    histogramKeys.Add(new IntegerRange((int)(divider * i + 1), (int)(divider * (i + 1))));
                }
            }

            return histogramKeys;
        }

        #endregion
    }
}
