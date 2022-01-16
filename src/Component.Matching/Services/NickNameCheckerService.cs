using Component.Matching.Contracts;
using Component.Matching.Database;
using Component.Matching.Models;

namespace Component.Matching.Services
{
    public class NickNameDetectorService : INameSimilarity
    {
        public SimilarityServiceType Type => SimilarityServiceType.NickName;

        private bool IsNickName(string name, string anotherName) =>
            CommonNicknameDb.Db.Contains(SimilarNamesPair.Create(name, anotherName));

        public bool Match(string name, string anotherName)
            => IsNickName(name, anotherName);
    }
}
