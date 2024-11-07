using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using ZeroElectric.Vinculum;

namespace Netrogue_working_
{
    

    enum Race
    {
        Elf, Orc, Dwarf
    }

    enum Role
    {
        Mage, Warrior, Rogue
    }
    
    internal class PlayerCharacter
    {
        public Texture image;
        public int imagePixelX;
        public int imagePixelY;

        public string name;
        public Race race;
        public Role role;
        public Vector2 position;
        public int ImageIndex; // Added ImageIndex property

        public PlayerCharacter()
        {
            ImageIndex = 0; // Initialize ImageIndex to 0
        }
    }

    class PlayerClasses
    {
        public double Mana { get; set; }
        public double Hp { get; set; }

        public PlayerClasses(double mana, double hp)
        {
            Mana = mana;
            Hp = hp;
        }

        public override string ToString()
        {
            return this.GetType().Name;
        }
    }

    class Mage : PlayerClasses
    {
        public Mage() : base(10, 3.5) { }
        public override string ToString()
        {
            return ("Mage");
        }
    }

    class Warrior : PlayerClasses
    {
        public Warrior() : base(5, 6) { }
        public override string ToString()
        {
            return ("Warrior");
        }
    }

    class Rogue : PlayerClasses
    {
        public Rogue() : base(2, 3) { }
        public override string ToString()
        {
            return ("Rogue");
        }
    }
}