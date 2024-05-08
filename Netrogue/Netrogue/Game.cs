﻿using Netrogue_working_;
using System;
using System.Numerics;
using ZeroElectric.Vinculum;
using static System.Net.Mime.MediaTypeNames;

namespace Netrogue
{
     
    internal class Game
    {
        public static readonly int tileSize = 16;
        private PlayerCharacter player;
        private Map level;
        private int mapWidth;
        private int mapHeight;
        public static int imagesPerRow = 12;
        private int index;
        private int timer = 0;
        int game_width;
        int game_height;
        RenderTexture game_screen;
        Texture imageTexture;
        private void Init()
        {

            Update();
            
            game_width = 480;
            game_height = 270;
            const int screen_width = 900;
            const int screen_height = 460;
            Raylib.InitWindow(screen_width, screen_height, "Rogue");
            Raylib.SetWindowMinSize(game_width, game_height);
            Raylib.SetWindowState(ConfigFlags.FLAG_WINDOW_RESIZABLE);
            game_screen = Raylib.LoadRenderTexture(game_width, game_height);
            Raylib.SetTextureFilter(game_screen.texture, TextureFilter.TEXTURE_FILTER_POINT);
            
            
            imageTexture = Raylib.LoadTexture("RoguePics/tilemap_packed.png");
            SetImageAndIndex(player, imageTexture, imagesPerRow, index);


        }
        private void DrawGameScaled()
        {

            // Tässä piirretään tekstuuri ruudulle
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Raylib.DARKGRAY);

            int draw_width = Raylib.GetScreenWidth();
            int draw_height = Raylib.GetScreenHeight();
            float scale = Math.Min((float)draw_width / game_width, (float)draw_height / game_height);

            // Note: when drawing on texture, the Y-axis is
            //flipped, need to multiply height by -1
            Rectangle source = new Rectangle(0.0f, 0.0f,
                game_screen.texture.width,
                game_screen.texture.height * -1.0f);

            Rectangle destination = new Rectangle(
                (draw_width - (float)game_width * scale) * 0.5f,
                (draw_height - (float)game_height * scale) * 0.5f,
                game_width * scale,
                game_height * scale);

            Raylib.DrawTexturePro(game_screen.texture,
                source, destination,
                new Vector2(0, 0), 0.0f, Raylib.WHITE);

            Raylib.EndDrawing();
        }

        void SetImageAndIndex(PlayerCharacter player, Texture PlayerTexture, int imagesPerRow, int index)
        {
            player.image = PlayerTexture;
            player.imagePixelX = (index % imagesPerRow) * tileSize;
            player.imagePixelY = (int)(index / imagesPerRow) * tileSize;
        }

        public void Run()
        {
            Raylib.UnloadRenderTexture(game_screen);
            Console.WriteLine("Welcome to Netrogue!");
            Console.WriteLine("Press Enter to start the game...");
            while (Console.ReadKey().Key != ConsoleKey.Enter) { }
            Console.Clear();

            player = CreatePlayerCharacter();

            MapLoader loader = new MapLoader();
            level = loader.ReadMapFromFile("mapfile.json");
            level.InitMap();
            Init();
            level.MapImage = (imageTexture);
            mapWidth = level.mapWidth;
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
            
   

            // Start the game loop
            while (!Raylib.WindowShouldClose())
            {
                MovePlayer();

                Raylib.BeginTextureMode(game_screen);
                level.Draw();
                    DrawPlayerInfo();
                DrawPlayer();
                Raylib.EndTextureMode();
                DrawGameScaled();
                
            }
            Raylib.CloseWindow();
        }


        void Update()
        {
            while (timer > 0)
            {
                Raylib.DrawText("You hit a Mob!", Raylib.GetScreenWidth() - 16 * 11, Raylib.GetScreenHeight() - 16, 16, Raylib.WHITE);
                timer -= 3;
            }
        }

        private PlayerCharacter CreatePlayerCharacter()
        {
            PlayerCharacter newPlayer = new PlayerCharacter();

            do
            {
                Console.WriteLine("Who are you? ");
                newPlayer.name = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(newPlayer.name))
                {
                    Console.WriteLine("Name cannot be empty. Please enter a valid name.");
                }
                else if (ContainsDigits(newPlayer.name))
                {
                    Console.WriteLine("Invalid name. Please enter a name without digits.");
                }
                else
                {
                    Console.WriteLine($"Hello, {newPlayer.name}!");
                }
            } while (string.IsNullOrWhiteSpace(newPlayer.name) || ContainsDigits(newPlayer.name));

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
                        newPlayer.ImageIndex = 85; // Set the image index for Mage
                        validRole = true;
                        break;
                    case "2":
                        newPlayer.role = Role.Warrior;
                        Console.WriteLine($"You have chosen {newPlayer.role} as your role.");
                        newPlayer.ImageIndex = 86; // Set the image index for Warrior
                        validRole = true;
                        break;
                    case "3":
                        newPlayer.role = Role.Rogue;
                        Console.WriteLine($"You have chosen {newPlayer.role} as your role.");
                        newPlayer.ImageIndex = 87; // Set the image index for Rogue
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
            

                // Determine the image index based on the player's class
                int rowIndex = (int)player.ImageIndex;

                int ImageX = rowIndex % imagesPerRow;
                int ImageY = (int)(rowIndex / imagesPerRow);
                player.imagePixelX = ImageX * tileSize;
                player.imagePixelY = ImageY * tileSize;
                int pixelPositionX = (int)player.position.X * Game.tileSize;
                int pixelPositionY = (int)player.position.Y * Game.tileSize;
                Vector2 pixelPosition = new Vector2(pixelPositionX, pixelPositionY);
                Rectangle imageRect = new Rectangle(player.imagePixelX, player.imagePixelY, Game.tileSize, Game.tileSize);
                Raylib.DrawTextureRec(player.image, imageRect, pixelPosition, Raylib.WHITE);
            }

        private void MovePlayer()
        {
            // Prepare movement variables
            int moveX = 0;
            int moveY = 0;

            if (Raylib.IsKeyPressed(KeyboardKey.KEY_UP))
            {
                moveY = -1;
            }
            if (Raylib.IsKeyDown(KeyboardKey.KEY_DOWN))
            {
                moveY = 1;
            }
            if (Raylib.IsKeyDown(KeyboardKey.KEY_LEFT))
            {
                moveX = -1;
            }
            if (Raylib.IsKeyDown(KeyboardKey.KEY_RIGHT))
            {
                moveX = 1;
            }

            // Move the player
            player.position.X += moveX;
            player.position.Y += moveY;

            // Check for collision with walls
            if (level.GetTile((int)player.position.X, (int)player.position.Y) == MapTile.Wall)
            {
                // If player hits a wall, reset the position
                player.position.X -= moveX;
                player.position.Y -= moveY;
            }

            if (level.GetTile((int)player.position.X, (int)player.position.Y) == MapTile.Mob)
            {
                // If player hits a mob, reset the position and increase the timer
                player.position.X -= moveX;
                player.position.Y -= moveY;
                timer += 3;
            }

            // Check for exit
            if (level.GetTile((int)player.position.X, (int)player.position.Y) == MapTile.Exit)
            {
                // Re-generate the map and reset player position
                MapLoader loader = new MapLoader();
               
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
