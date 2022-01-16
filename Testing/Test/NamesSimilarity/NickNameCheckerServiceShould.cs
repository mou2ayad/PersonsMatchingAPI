using FluentAssertions;
using Component.Matching.Services;
using NUnit.Framework;

namespace Test.NamesSimilarity
{
    public class NickNameCheckerServiceShould
    {
        
        [TestCase("Andrew", "Andy", true)]
        [TestCase("Andy", "Andrew", true)]
        [TestCase("Andrew", "Petty", false)]
        [TestCase("Andrew", "Andrew", false)]
        [TestCase(null, "Andrew", false)]

        public void Return_figure_out_if_one_name_is_a_nick_name_to_another(string name,string anotherName,bool expected)
        {
            var sut = Sut();

            var isNickName = sut.Match(name, anotherName);

            isNickName.Should().Be(expected);

        }
        public static NickNameDetectorService Sut() => new();
    }
}