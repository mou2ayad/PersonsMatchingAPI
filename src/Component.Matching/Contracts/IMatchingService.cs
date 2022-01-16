using System.Threading.Tasks;

namespace Component.Matching.Contracts
{
    public interface IMatchingService<in T> where T : IMatchable<T>
    { 
        Task<decimal> Match(T first, T second);
    }
}