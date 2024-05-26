using System;

namespace HighlanderSimulator
{
    internal class Highlander
    {
        public string Name { get; set; }
        public int PowerLevel { get; set; }
        public int Age { get; set; }
        public bool IsGood { get; set; }
        public int PositionX { get; set; }
        public int PositionY { get; set; }

        public Highlander(string name, int powerLevel, int age)
        {
            Name = name;
            PowerLevel = powerLevel;
            Age = age;
        }

        public virtual void DisplayInfo()
        {
            Console.WriteLine($"Name: {Name}, Power Level: {PowerLevel}, Age: {Age}");
        }

        public virtual void AbsorbPower(int power)
        {
            PowerLevel += power;
        }

        public virtual void Interact(Highlander other)
        {
            if (other.IsGood && this.IsGood)
            {
                // Two good Highlanders meet: They don't fight
            }
            else if (!other.IsGood && !this.IsGood)
            {
                // Two bad Highlanders meet: They fight
                Fight(other);
            }
            else
            {
                // One good Highlander and one bad Highlander meet
                if (this.IsGood)
                {
                    // Good Highlander tries to escape
                    Escape(other);
                }
                else
                {
                    // Bad Highlander tries to kill the good Highlander
                    Kill(other);
                }
            }
        }

        protected virtual void Fight(Highlander other)
        {
            int thisFightPower = PowerLevel;
            int otherFightPower = other.PowerLevel;

            if (thisFightPower > otherFightPower)
            {
                Console.WriteLine($"{Name} wins the fight against {other.Name}");
            }
            else if (thisFightPower < otherFightPower)
            {
                Console.WriteLine($"{other.Name} wins the fight against {Name}");
            }
            else
            {
                Console.WriteLine($"{Name} and {other.Name} have equal power. It's a draw.");
            }
        }

        protected virtual void Escape(Highlander other)
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

        protected virtual void Kill(Highlander other)
        {
            Console.WriteLine($"{Name} kills {other.Name}");
        }
    }
}
