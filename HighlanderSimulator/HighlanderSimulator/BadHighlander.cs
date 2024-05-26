using System;

namespace HighlanderSimulator
{
    internal class BadHighlander : Highlander
    {
        public BadHighlander(string name, int powerLevel, int age) : base(name, powerLevel, age)
        {
            IsGood = false;
        }

        public void BadAction()
        {
            Console.WriteLine($"{Name} is doing something bad.");
        }

        public override void DisplayInfo()
        {
            Console.WriteLine($"Bad Highlander - Name: {Name}, Power Level: {PowerLevel}, Age: {Age}");
        }

        public override void Interact(Highlander other)
        {
            if (other.IsGood)
            {
                Kill(other);
            }
        }

        protected override void Kill(Highlander other)
        {
            Console.WriteLine($"{Name} kills {other.Name}");
        }
    }
}
