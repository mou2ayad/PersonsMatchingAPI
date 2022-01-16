using FluentAssertions;
using Component.Matching.Services;
using NUnit.Framework;

namespace Test.NamesSimilarity
{
    public class InitialsMatchingServiceShould
    {

        [TestCase(" T .  ", "T.")]
        [TestCase("A .s ", "A.s")]
        [TestCase("A ", "A")]
        public void Clean_text_from_whitespaces(string input, string expected)
        {
            var sut = Sut();

            var result = sut.CallRemoveWhiteSpaces(input);

            result.Should().Be(expected);
        }

        [TestCase("A.",true)]
        [TestCase("a.", true)]
        [TestCase("A.s", false)]
        [TestCase("A.B.", false)]
        [TestCase("A", false)]
        [TestCase("Ab.", false)]
        public void Check_if_the_name_is_initial(string input,bool expected)
        {
            var sut = Sut();

            var isInitial = sut.CallIsInitial(input);

            isInitial.Should().Be(expected);
        }

        [TestCase("Andrew", "A.",Description = "Pass name to get initial")]
        [TestCase("A.", "A.", Description = "Pass initial should return same initial")]
        public void Get_correct_initial(string input, string expected)
        {
            var sut = Sut();

            var result = sut.CallGetInitial(input);

            result.Should().Be(expected);
        }

        [TestCase("", Description = "initial should be null if the name is empty string")]
        [TestCase("   ", Description = "initial should be null if the name is whitespaces string")]
        public void Get_null_initial_when_input_is_null_or_empty(string input)
        {
            var sut = Sut();

            var result = sut.CallGetInitial(input);

            result.Should().Be(null);
        }

        [TestCase("Andrew","A.",true)]
        [TestCase("A.", "Andrew", true)]
        [TestCase("Andrew", "a.", true)]
        [TestCase("A.", "a.", true)]
        [TestCase("A", "A.", true)]
        [TestCase("Andrew", "An", false)]
        [TestCase("Andrew", "Andrew", false)]
        public void Possible_initial_return_correct_value(string name,string anotherName,bool expected)
        {
            var sut = Sut();

            var result = sut.Match(name, anotherName);

            result.Should().Be(expected);
        }

        [TestCase("", "A.")]
        [TestCase("A.", "")]
        [TestCase("", "")]
        [TestCase("  ", "  ")]
        [TestCase(null, "Andrew")]
        [TestCase(null, null)]
        public void Possible_initial_return_false_if_one_of_two_name_is_null_or_empty(string name, string anotherName)
        {
            var sut = Sut();

            var result = sut.Match(name, anotherName);

            result.Should().Be(false);
        }


        private static TestableInitialsMatchingService Sut() => new();

    }

    public class TestableInitialsMatchingService : InitialsMatchingService
    {
        public bool CallIsInitial(string name) => IsInitial(name);
        public string CallRemoveWhiteSpaces(string text) => RemoveWhiteSpaces(text);
        public string CallGetInitial(string name) => GetInitial(name);
    }
}
