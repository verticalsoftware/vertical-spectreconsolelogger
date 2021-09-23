namespace Vertical.SpectreLogger.Tests.Infrastructure
{
    public class Person
    {
        public Person(string name, Person[] children)
        {
            Name = name;
            Children = children;
        }
        
        public string Name { get; }
        
        public Person[] Children { get; }

        /// <inheritdoc />
        public override string ToString() => $"{Name}, Children={Children?.Length}";
    }
}