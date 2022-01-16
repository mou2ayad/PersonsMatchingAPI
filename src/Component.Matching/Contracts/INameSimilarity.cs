using Component.Matching.Models;

namespace Component.Matching.Contracts
{
    public interface INameSimilarity
    {
        bool Match(string name, string anotherName);
        SimilarityServiceType Type { get; }
    }
}
