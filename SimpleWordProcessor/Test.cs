using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace SimpleWordProcessor
{
    [TestFixture]
    class Test
    {
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void Continues_Same_Case_String_Should_Replace_To_Single_Char()
        {
            var inputString = "AAAbcAA";
            var removeDuplicateCharacterProcessor = new RemoveDuplicateCharacterProcessor();
            var outPutstring = removeDuplicateCharacterProcessor.ProcessAsync(inputString).Result;
            Assert.AreEqual("AbcA", outPutstring);
        }
        [Test]
        public void Continues_Different_Case_String_Should_Not_Replace_To_Single_Char()
        {
            var inputString = "AaAaAaAAAbcAA";
            var removeDuplicateCharacterProcessor = new RemoveDuplicateCharacterProcessor();
            var outPutstring = removeDuplicateCharacterProcessor.ProcessAsync(inputString).Result;
            Assert.AreEqual("AaAaAaAbcA", outPutstring);
        }


        [Test]

        public void When_The_To_Be_String_IsNull_Throw_ArgumentNullException()
        {
            const string toBeReplaced = "";
            const string replaceBy = "";
            var exception = Assert.Throws<ArgumentNullException>(() => new CharacterReplaceProcessor(toBeReplaced, replaceBy));
            Assert.That(exception.GetType().ToString(), Is.EqualTo("System.ArgumentNullException"));
            Assert.That(exception.Message, Is.EqualTo("String to be replaced is null\r\nParameter name: toBeReplaced"));

        }
        [Test]
        public void UnderScore_Should_be_Replace_By_4()
        {
            var inputString = "1ci3_848v3d__K";
            const string toBeReplaced = "_";
            const string replaceBy = "4";
            var characterReplaceProcessor = new CharacterReplaceProcessor(toBeReplaced, replaceBy);
            var outPutstring = characterReplaceProcessor.ProcessAsync(inputString).Result;
            Assert.AreEqual("1ci34848v3d44K", outPutstring);
        }
        [Test]
        public void When_No_UnderScore_Nothing_Shoud_Be_Replaced()
        {
            var inputString = "1ci3848v3dK";
            const string toBeReplaced = "_";
            const string replaceBy = "4";
            var characterReplaceProcessor = new CharacterReplaceProcessor(toBeReplaced, replaceBy);
            var outPutstring = characterReplaceProcessor.ProcessAsync(inputString).Result;
            Assert.AreEqual("1ci3848v3dK", outPutstring);
        }
        [Test]
        public void Doller_Should_be_Replace_By_Pound()
        {
            var inputString = "c$WwWkLq$1ci3";
            const string toBeReplaced = "$";
            const string replaceBy = "£";
            var characterReplaceProcessor = new CharacterReplaceProcessor(toBeReplaced, replaceBy);
            var outPutstring = characterReplaceProcessor.ProcessAsync(inputString).Result;
            Assert.AreEqual("c£WwWkLq£1ci3", outPutstring);
        }
        [Test]
        public void When_No_Dollar_Nothing_Shoud_Be_Replaced()
        {
            var inputString = "cWwWkLq1ci3";
            const string toBeReplaced = "$";
            const string replaceBy = "£";
            var characterReplaceProcessor = new CharacterReplaceProcessor(toBeReplaced, replaceBy);
            var outPutstring = characterReplaceProcessor.ProcessAsync(inputString).Result;
            Assert.AreEqual("cWwWkLq1ci3", outPutstring);
        }

        [Test]
        public void When_Input_Truncate_Lenth_Zero_Throw_ArgumentException()
        {
            const int maxCharLength = 0;
            var exception = Assert.Throws<ArgumentException>(() => new TruncateProcessor(maxCharLength));
            Assert.That(exception.GetType().ToString(), Is.EqualTo("System.ArgumentException"));
            Assert.That(exception.Message, Is.EqualTo("Length must be grater than zero\r\nParameter name: maxCharLength"));
        }
        [Test]
        public void When_Input_Lenth_More_Than_15_Should_Truncate_To_15()
        {
            const int lenghtToTruncate = 15;
            const string inputString = "AAAc91%cWwWkLq$1ci3_848v3d__K";
            var truncateProcessor = new TruncateProcessor(lenghtToTruncate);
            var outPutstring = truncateProcessor.ProcessAsync(inputString).Result;
            Assert.AreEqual("AAAc91%cWwWkLq$", outPutstring);
        }
        [Test]
        public void When_Input_Lenth_Less_Than_15_Nothing_Should_Truncated()
        {
            const int lenghtToTruncate = 15;
            const string inputString = "AAAc91%cWwWkLq";
            var truncateProcessor = new TruncateProcessor(lenghtToTruncate);
            var outPutstring = truncateProcessor.ProcessAsync(inputString).Result;
            Assert.AreEqual("AAAc91%cWwWkLq", outPutstring);
        }

        [Test]
        public void When_Invoke_All_Process_Should_Give_Expeced_Output()
        {
            var input = "AAAc91%cWwWkLq$1ci3_848v3d__K";

            var stringProcessors = new List<IStringProcessor>()
            {
                new RemoveDuplicateCharacterProcessor(),
                new CharacterReplaceProcessor("_",""),
                new CharacterReplaceProcessor("4",""),
                new CharacterReplaceProcessor("$","£"),
                new TruncateProcessor()
            };

            foreach (var processor in stringProcessors)
            {
                input = processor.ProcessAsync(input).Result;
            }
            Assert.AreEqual("Ac91%cWwWkLq£1c", input);
        }
    }
}
