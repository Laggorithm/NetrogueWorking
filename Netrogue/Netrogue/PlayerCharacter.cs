using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

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
        public string name;
        public Race race;
        public Role role;
        public Vector2 position;
    }
}
