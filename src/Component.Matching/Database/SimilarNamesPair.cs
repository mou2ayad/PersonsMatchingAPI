using System;

namespace Component.Matching.Database
{
    public class SimilarNamesPair : IEquatable<SimilarNamesPair>
    {
        private SimilarNamesPair(string name, string nickName)
        {
            Name = name;
            NickName = nickName;
        }
        public static SimilarNamesPair Create(string name, string nickName) => 
            new SimilarNamesPair(name, nickName);

        public string Name { get; }
        public string NickName { get; }

        public bool Equals(SimilarNamesPair other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return (Name.Equals(other.Name, StringComparison.OrdinalIgnoreCase) &&
                   NickName.Equals(other.NickName, StringComparison.OrdinalIgnoreCase)) ||
                   (Name.Equals(other.NickName, StringComparison.OrdinalIgnoreCase) &&
                   NickName.Equals(other.Name, StringComparison.OrdinalIgnoreCase)); 
        }

        public override bool Equals(object obj) => 
            Equals(obj as SimilarNamesPair);

        public override int GetHashCode()
        {
            unchecked
            {
                return (Name != null ? Name.GetHashCode() : 0) + (NickName != null ? NickName.GetHashCode() : 0);
            }
        }
    }
}
