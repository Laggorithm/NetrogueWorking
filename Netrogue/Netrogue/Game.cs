﻿using System;
using System.Numerics;
using Netrogue_working_;

namespace Netrogue
{
    internal class Game
    {
        private PlayerCharacter player;
        private Map level;
        private int mapWidth;
        private int mapHeight;

        public void Run()
        {
            Console.WriteLine("Welcome to Netrogue!");

            Console.WriteLine("Press Enter to start the game...");
            while (Console.ReadKey().Key != ConsoleKey.Enter) { }

            Console.Clear();

            player = CreatePlayerCharacter();

            MapLoader loader = new MapLoader();
            level = loader.LoadTestMap();
            mapWidth = level.Width;
            mapHeight = level.Height;

            // Check if the map was loaded successfully
            if (level == null)
            {
                Console.WriteLine("Failed to load the map. Exiting the game.");
                return;
            }

            // Set player starting position randomly within the borders of the map
            Random random = new Random();
            int startX, startY;
            do
            {
                startX = random.Next(1, mapWidth - 1); // Exclude border
                startY = random.Next(1, mapHeight - 1); // Exclude border
            } while (level.GetTile(startX, startY) != MapTile.Floor); // Ensure player spawns on floor tile
            player.position = new Vector2(startX, startY);

            // Draw the map
            level.Draw();

            // Draw player info
            DrawPlayerInfo();

            // Draw the player
            DrawPlayer();

            // Start the game loop
            while (true)
            {
                // Listen for keypress
                ConsoleKeyInfo key = Console.ReadKey(true);
                // Move the player based on keypress
                MovePlayer(key);
            }
        }

        private PlayerCharacter CreatePlayerCharacter()
        {
            PlayerCharacter newPlayer = new PlayerCharacter();

            // Selecting player name
            do
            {
                Console.WriteLine("Who are you? ");
                newPlayer.name = Console.ReadLine();

                if (ContainsDigits(newPlayer.name))
                {
                    Console.WriteLine("Invalid name. Please enter a name without digits.");
                }
                else
                {
                    Console.WriteLine($"Hello, {newPlayer.name}!");
                }
            } while (ContainsDigits(newPlayer.name)); // Loop until a name without digits is entered

            // Selecting player race
            bool validRace = false;
            do
            {
                Console.WriteLine("Pick race: Elf- 1, Orc- 2, Dwarf- 3");
                string raceNumber = Console.ReadLine();

                switch (raceNumber)
                {
                    case "1":
                        newPlayer.race = Race.Elf;
                        Console.WriteLine($"You have chosen {newPlayer.race} as your race.");
                        validRace = true;
                        break;
                    case "2":
                        newPlayer.race = Race.Orc;
                        Console.WriteLine($"You have chosen {newPlayer.race} as your race.");
                        validRace = true;
                        break;
                    case "3":
                        newPlayer.race = Race.Dwarf;
                        Console.WriteLine($"You have chosen {newPlayer.race} as your race.");
                        validRace = true;
                        break;
                    default:
                        Console.WriteLine("Invalid input. Please choose a valid race.");
                        break;
                }
            } while (!validRace); // Keep looping until a valid race is chosen

            // Selecting player role
            bool validRole = false;
            do
            {
                Console.WriteLine("Pick role: Mage- 1, Warrior- 2, Rogue- 3");
                string roleNumber = Console.ReadLine();

                switch (roleNumber)
                {
                    case "1":
                        newPlayer.role = Role.Mage;
                        Console.WriteLine($"You have chosen {newPlayer.role} as your role.");
                        validRole = true;
                        break;
                    case "2":
                        newPlayer.role = Role.Warrior;
                        Console.WriteLine($"You have chosen {newPlayer.role} as your role.");
                        validRole = true;
                        break;
                    case "3":
                        newPlayer.role = Role.Rogue;
                        Console.WriteLine($"You have chosen {newPlayer.role} as your role.");
                        validRole = true;
                        break;
                    default:
                        Console.WriteLine("Invalid input. Please choose a valid role.");
                        break;
                }
            } while (!validRole); // Keep looping until a valid role is chosen

            Console.WriteLine("Press Enter to start your adventure...");
            while (Console.ReadKey().Key != ConsoleKey.Enter) { }

            Console.Clear();

            return newPlayer;
        }

        private void DrawPlayer()
        {
            // Set cursor position to player's position
            Console.SetCursorPosition((int)player.position.X, (int)player.position.Y);
            // Draw player character
            Console.Write("@");
        }

        private void MovePlayer(ConsoleKeyInfo key)
        {
            // Prepare movement variables
            int moveX = 0;
            int moveY = 0;

            // Determine movement direction based on key pressed
            switch (key.Key)
            {
                case ConsoleKey.UpArrow:
                    moveY = -1;
                    break;
                case ConsoleKey.DownArrow:
                    moveY = 1;
                    break;
                case ConsoleKey.LeftArrow:
                    moveX = -1;
                    break;
                case ConsoleKey.RightArrow:
                    moveX = 1;
                    break;
                default:
                    // Ignore other keys
                    return;
            }

            // Move the player
            player.position.X += moveX;
            player.position.Y += moveY;

            // Check for collision with map boundaries
            if (player.position.X < 1)
                player.position.X = 1;
            else if (player.position.X >= mapWidth - 1)
                player.position.X = mapWidth - 2;

            if (player.position.Y < 1)
                player.position.Y = 1;
            else if (player.position.Y >= mapHeight - 1)
                player.position.Y = mapHeight - 2;

            // Check for collision with walls
            if (level.GetTile((int)player.position.X, (int)player.position.Y) == MapTile.Wall)
            {
                // If player hits a wall, reset the position
                player.position.X -= moveX;
                player.position.Y -= moveY;
            }

            // Check for exit
            if (level.GetTile((int)player.position.X, (int)player.position.Y) == MapTile.Exit)
            {
                // Re-generate the map and reset player position
                MapLoader loader = new MapLoader();
                level = loader.LoadTestMap();
                player.position = FindValidPlayerSpawnPosition();
                // Clear the screen and redraw everything
                Console.Clear();
                level.Draw();
                DrawPlayerInfo();
            }

            // Clear the screen and redraw player
            Console.Clear();
            level.Draw();
            DrawPlayerInfo();
            DrawPlayer();
        }

        private Vector2 FindValidPlayerSpawnPosition()
        {
            Random random = new Random();
            int startX, startY;
            do
            {
                startX = random.Next(1, mapWidth - 1); // Exclude border
                startY = random.Next(1, mapHeight - 1); // Exclude border
            } while (level.GetTile(startX, startY) != MapTile.Floor); // Ensure player spawns on floor tile
            return new Vector2(startX, startY);
        }

        private void DrawPlayerInfo()
        {
            // Set cursor position to the first line
            Console.SetCursorPosition(0, 0);
            // Write player info
            Console.WriteLine($"Race: {player.race}, Name: {player.name}, Role: {player.role}");
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
}