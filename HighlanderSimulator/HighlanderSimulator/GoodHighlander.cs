using System;

namespace HighlanderSimulator
{
    internal class GoodHighlander : Highlander
    {
        public GoodHighlander(string name, int powerLevel, int age) : base(name, powerLevel, age)
        {
            IsGood = true;
        }

        public void GoodAction()
        {
            Console.WriteLine($"{Name} is doing something good.");
        }

        public override void DisplayInfo()
        {
            Console.WriteLine($"Good Highlander - Name: {Name}, Power Level: {PowerLevel}, Age: {Age}");
        }

        public override void Interact(Highlander other)
        {
            if (!other.IsGood)
            {
                Escape(other);
            }
        }

        protected override void Escape(Highlander other)
        {
            int chanceToEscape = new Random().Next(100);
            if (chanceToEscape < 50)
            {
                Console.WriteLine($"{Name} successfully escapes from {other.Name}");
            }
            else
            {
                Kill(other);
            }
        }

        protected override void Kill(Highlander other)
        {
            AbsorbPower(other.PowerLevel);
            Console.WriteLine($"{Name} absorbs power from {other.Name}");
        }
    }
}
