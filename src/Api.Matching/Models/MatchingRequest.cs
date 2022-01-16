using Component.Matching.Models;

namespace Api.Matching.Models
{
    public class MatchingRequest
    {
        public static MatchingRequest From(Person first, Person second)
            => new()
            {
                First = first,
                Second = second
            };

        public Person First { set; get; }
        public Person Second { set; get; }
    }
}
