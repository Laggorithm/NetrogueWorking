using Netrogue_working_;
using System;

namespace Netrogue
{
    internal class Game
    {
        private PlayerCharacter player;

        public void Run()
        {
            player = new PlayerCharacter();

            // Selecting player name
            do
            {
                Console.WriteLine("Who are you? ");
                player.name = Console.ReadLine();

                if (ContainsDigits(player.name))
                {
                    Console.WriteLine("Invalid name. Please enter a name without digits.");
                }
                else
                {
                    Console.WriteLine($"Hello, {player.name}! Welcome to the game.");
                }
            } while (ContainsDigits(player.name)); // Loop until a name without digits is entered

            // Selecting player race
            bool validRace = false;
            do
            {
                Console.WriteLine("Pick race: Elf- 1, Orc- 2, Dwarf- 3");
                string raceNumber = Console.ReadLine();

                switch (raceNumber)
                {
                    case "1":
                        player.race = Netrogue_working_.Race.Elf;
                        Console.WriteLine($"You have chosen {player.race} as your race. Enjoying some grass and trees, eh?");
                        validRace = true;
                        break;
                    case "2":
                        player.race = Netrogue_working_.Race.Orc;
                        Console.WriteLine($"You have chosen {player.race} as your race. Unga-bunga. Oh! I meant welcome to Horde.");
                        validRace = true;
                        break;
                    case "3":
                        player.race = Netrogue_working_.Race.Dwarf;
                        Console.WriteLine($"You have chosen {player.race} as your race... Really?...");
                        validRace = true;
                        break;
                    default:
                        Console.WriteLine("Invalid input. Please choose a valid race.");
                        break;
                }
            } while (!validRace); // Keep looping until a valid race is chosen

            // Selecting player role (class)
            bool validRole = false;
            do
            {
                Console.WriteLine("Pick class: Mage- 1, Warrior- 2, Rogue- 3");
                string roleNumber = Console.ReadLine();

                switch (roleNumber)
                {
                    case "1":
                        player.role = Netrogue_working_.Role.Mage;
                        Console.WriteLine($"You have chosen {player.role} as your class.");
                        validRole = true;
                        break;
                    case "2":
                        player.role = Netrogue_working_.Role.Warrior;
                        Console.WriteLine($"You have chosen {player.role} as your class.");
                        validRole = true;
                        break;
                    case "3":
                        player.role = Netrogue_working_.Role.Rogue;
                        Console.WriteLine($"You have chosen {player.role} as your class.");
                        validRole = true;
                        break;
                    default:
                        Console.WriteLine("Invalid input. Please choose a valid class.");
                        break;
                }
            } while (!validRole || player.role == null); // Keep looping until a valid class is chosen

            // Outputting message
            Console.WriteLine($"Oh mighty {player.race}, one of the best {player.role}s, {player.name}, This is where your story begins. Get ready to embark on an epic adventure!");

            // Continue with the rest of your game logic here
        }

        private bool ContainsDigits(string input)
        {
            foreach (char c in input)
            {
                if (char.IsDigit(c))
                {
                    return true;
                }
            }
            return false;
        }
    }

    internal class PlayerCharacter
    {
        public string name;
        public Netrogue_working_.Race race;
        public Netrogue_working_.Role role;
    }
}
