namespace API.Test.Utils
{
    public class HttpTestHeader
    {
        private HttpTestHeader(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public static HttpTestHeader From(string name, string value) => 
            new (name, value);

        public string Name { get; }

        public string Value { get; }
    }
}
