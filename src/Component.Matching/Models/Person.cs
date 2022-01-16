using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Component.Matching.Contracts;

namespace Component.Matching.Models
{
    public class Person : IMatchable<Person>
    {
        public Person(string firstName, string lastName, DateTime? dateOfBirth = null,
            string identificationNumber = null)
        {
            FirstName = firstName;
            LastName = lastName;
            DateOfBirth = dateOfBirth;
            IdentificationNumber = identificationNumber;
        }

        public string FirstName { get; }
        public string LastName { get; }
        public DateTime? DateOfBirth { get; }
        public string IdentificationNumber { get; }

        public bool Equals(Person other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return FirstName == other.FirstName && LastName == other.LastName &&
                   Nullable.Equals(DateOfBirth, other.DateOfBirth) &&
                   IdentificationNumber == other.IdentificationNumber;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Person) obj);
        }

        public override int GetHashCode() =>
            HashCode.Combine(FirstName, LastName, DateOfBirth, IdentificationNumber);

        public string GetCacheKey()
        {
            string dob = DateOfBirth.HasValue ? DateOfBirth.Value.ToString("yyyyMMdd") : string.Empty;
            return $"{FirstName}_{LastName}_{dob}_{IdentificationNumber}";
        }

        [JsonIgnore]
        public IEnumerable<string> OrderedPropertiesToMatch =>
            new[]
            {
                nameof(IdentificationNumber),
                nameof(LastName),
                nameof(DateOfBirth),
                nameof(FirstName)
            };
    }
}
