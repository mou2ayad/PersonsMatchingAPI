using System;
using Component.Matching.Models;

namespace Test
{
    public class PersonBuilder
    {
        private readonly string _firstName ;
        private readonly string _lastName;
        private DateTime? _dateOfBirth ;
        private string _identificationNumber ;
        private PersonBuilder(string firstName, string lastName)
        {
            _firstName = firstName;
            _lastName = lastName;
        }

        public static PersonBuilder Create(string firstName,string lastName) =>
            new (firstName,lastName);


        public PersonBuilder With(DateTime dateOfBirth)
        {
            _dateOfBirth = dateOfBirth;
            return this;
        }

        public PersonBuilder With(string identificationNumber)
        {
            _identificationNumber = identificationNumber;
            return this;
        }

        public Person Build() => new(_firstName, _lastName, _dateOfBirth, _identificationNumber);

    }
}
