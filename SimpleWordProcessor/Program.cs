using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SimpleWordProcessor
{
    class Program
    {
        static void Main(string[] args)
        {
            int totalStringCount = GetTotalStringCount();
            var userInputs = GetInputStringCollection(totalStringCount);

            var stringProcessors = new List<IStringProcessor>()
            {
                new RemoveDuplicateCharacterProcessor(),
                new CharacterReplaceProcessor("_",""),
                new CharacterReplaceProcessor("4",""),
                new CharacterReplaceProcessor("$","£"),
                new TruncateProcessor()
            };

            var output = ProcessInputs(userInputs, stringProcessors).Result;
            Console.WriteLine("Processed string collection:");
            output.ForEach(Console.WriteLine);
            Console.Read();
        }

        private static int GetTotalStringCount()
        {
            int totalString;
            Console.WriteLine("Please enter total number of string");
            do
            {
                bool isValidInteger = int.TryParse(Console.ReadLine(), out totalString);
                if (!isValidInteger)
                {
                    Console.WriteLine("Please enter a valid number");
                }
                else if (totalString <= 0)
                {
                    Console.WriteLine("Please enter positive non zero number");
                }

            } while (totalString <= 0);

            return totalString;
        }

        private static List<string> GetInputStringCollection(int totalStringCount)
        {
            var userInputs = new List<string>();
            Console.WriteLine("Please enter the first string:");
            do
            {
                userInputs.Add(Console.ReadLine());
                --totalStringCount;
                if (totalStringCount >= 1)
                {
                    Console.WriteLine(totalStringCount == 1
                        ? "Please enter the last string:"
                        : "Please enter the next string:");
                }

            } while (totalStringCount != 0);

            return userInputs;
        }

        private static async Task<List<string>> ProcessInputs(List<string> userInputs, List<IStringProcessor> stringProcessors)
        {
            var outputStringCollection = new List<string>();

            foreach (var userInput in userInputs)
            {
                var processedInput = userInput;
                foreach (var processor in stringProcessors)
                {
                    processedInput = await processor.ProcessAsync(processedInput);
                }

                outputStringCollection.Add(processedInput);
            }

            return outputStringCollection;
        }
    }
    interface IStringProcessor
    {
        Task<string> ProcessAsync(string inputString);
    }
    class RemoveDuplicateCharacterProcessor : IStringProcessor
    {
        public async Task<string> ProcessAsync(string inputString)
        {
            if (String.IsNullOrWhiteSpace(inputString))
            {
                return inputString;
            }

            return await Task.Run(() => Regex.Replace(inputString, @"([A-Za-z])\1+", "$1"));
        }
    }
    class TruncateProcessor : IStringProcessor
    {
        private readonly int _maxCharLength;

        public TruncateProcessor(int maxCharLength = 15)
        {
            if (maxCharLength <= 0)
            {
                throw new ArgumentException("Length must be grater than zero", nameof(maxCharLength));
            }
            _maxCharLength = maxCharLength;
        }

        public async Task<string> ProcessAsync(string inputString)
        {
            if (String.IsNullOrWhiteSpace(inputString) || inputString.Length <= _maxCharLength)
            {
                return inputString;
            }

            return await Task.Run(() => inputString.Substring(0, _maxCharLength));
        }
    }
    class CharacterReplaceProcessor : IStringProcessor
    {
        private readonly string _toBeReplaced;
        private readonly string _replaceBy;

        public CharacterReplaceProcessor(string toBeReplaced, string replaceBy)
        {
            if (String.IsNullOrWhiteSpace(toBeReplaced))
            {
                throw new ArgumentNullException(nameof(toBeReplaced), "String to be replaced is null");
            }

            _toBeReplaced = toBeReplaced;
            _replaceBy = replaceBy;

        }

        public async Task<string> ProcessAsync(string inputString)
        {
            if (String.IsNullOrWhiteSpace(inputString))
            {
                return inputString;
            }

            return await Task.Run(() => inputString.Replace(_toBeReplaced, _replaceBy));
        }
    }


}
