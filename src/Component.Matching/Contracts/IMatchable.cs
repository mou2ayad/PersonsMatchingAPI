using System;
using System.Collections.Generic;

namespace Component.Matching.Contracts
{
    public interface IMatchable<T> : IEquatable<T>
    {
        IEnumerable<string> OrderedPropertiesToMatch { get; }
    }
}