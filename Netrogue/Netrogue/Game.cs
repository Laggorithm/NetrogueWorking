using Netrogue_working_;
using System;
using System.Numerics;
using ZeroElectric.Vinculum;

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
        private int game_width;
        private int game_height;
        private bool isMenuActive = true;
        private bool isClassSelectionActive = false;
        private bool isSettingsActive = false;// Track if class selection is active
        RenderTexture game_screen;
        Texture imageTexture;
        Music backgroundMusic;

        public void DrawMainMenu()
        {
            // Clear the screen and begin drawing
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Raylib.BLACK);

            // Calculate button dimensions and positions
            int button_width = 250;
            int button_height = 40;
            int button_x = Raylib.GetScreenWidth() / 2 - button_width / 2;
            int button_y = Raylib.GetScreenHeight() / 2 - button_height / 2;

            // Draw game title
            RayGui.GuiLabel(new Rectangle(button_x, button_y - button_height * 2, button_width, button_height), "Rogue");

            // Draw instructions
            RayGui.GuiLabel(new Rectangle(button_x, button_y - button_height, button_width, button_height), "Use Mouse to Navigate");

            // Start Game button
            if (RayGui.GuiButton(new Rectangle(button_x, button_y, button_width, button_height), "Start Game") == 1)
            {
                isMenuActive = false; // Exit the menu
                isClassSelectionActive = true; // Activate class selection
            }

            // Move to next button position
            button_y += button_height * 2;

            // Quit button
            if (RayGui.GuiButton(new Rectangle(button_x, button_y, button_width, button_height), "Quit") == 1)
            {
                Raylib.CloseWindow();
            }

            button_y += button_height * 2;

            if (RayGui.GuiButton(new Rectangle(button_x, button_y, button_width, button_height), "Settings") == 1)
            {
                isMenuActive = false;
                isSettingsActive = true;
            }

            Raylib.EndDrawing();
        }

        private void DrawClassSelection()
        {
            // Clear the screen and begin drawing
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Raylib.DARKGRAY);

            // Calculate button dimensions and positions
            int button_width = 250;
            int button_height = 40;
            int button_x = Raylib.GetScreenWidth() / 2 - button_width / 2;
            int button_y = Raylib.GetScreenHeight() / 2 - button_height * 2;

            // Draw class selection title
            RayGui.GuiLabel(new Rectangle(button_x, button_y - button_height * 2, button_width, button_height), "Select Your Class");

            // Mage button
            if (RayGui.GuiButton(new Rectangle(button_x, button_y, button_width, button_height), "Mage") == 1)
            {
                AssignPlayerClass(Role.Mage);
            }

            // Warrior button
            button_y += button_height + 10; // Add space between buttons
            if (RayGui.GuiButton(new Rectangle(button_x, button_y, button_width, button_height), "Warrior") == 1)
            {
                AssignPlayerClass(Role.Warrior);
            }

            // Rogue button
            button_y += button_height + 10; // Add space between buttons
            if (RayGui.GuiButton(new Rectangle(button_x, button_y, button_width, button_height), "Rogue") == 1)
            {
                AssignPlayerClass(Role.Rogue);
            }

            Raylib.EndDrawing();
        }

        private void DrawPause()
        {
            // Clear the screen and begin drawing
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Raylib.DARKGRAY);

            // Calculate button dimensions and positions
            int button_width = 250;
            int button_height = 40;
            int button_x = Raylib.GetScreenWidth() / 2 - button_width / 2;
            int button_y = Raylib.GetScreenHeight() / 2 - button_height * 2;

            // Draw class selection title
            RayGui.GuiLabel(new Rectangle(button_x, button_y - button_height * 2, button_width, button_height), "Select Your Class");

            // Mage button
            if (RayGui.GuiButton(new Rectangle(button_x, button_y, button_width, button_height), "LOUDEEEER!!!") == 1)
            {
                
            }

            // Warrior button
            button_y += button_height + 10; // Add space between buttons
            if (RayGui.GuiButton(new Rectangle(button_x, button_y, button_width, button_height), "Silence!") == 1)
            {
                 
            }

            // Rogue button
            button_y += button_height + 10; // Add space between buttons
            if (RayGui.GuiButton(new Rectangle(button_x, button_y, button_width, button_height), "Back to Main Menu") == 1)
            {
                isSettingsActive = false;
                isMenuActive = true;
            }

            button_y += button_height + 10; // Add space between buttons
            if (RayGui.GuiButton(new Rectangle(button_x, button_y, button_width, button_height), "Back to Action!") == 1)
            {
                bool IsPlayerClassNull()
                {
                    return player == null || player.role == default(Role);
                }

                if (IsPlayerClassNull())
                {
                    isMenuActive = true;
                    isSettingsActive = false;
                    Console.WriteLine("Player class is not set.");
                }
                else
                {
                    isMenuActive =false;
                    isSettingsActive = false;
                }

            }

            Raylib.EndDrawing();
        }
 
        private void AssignPlayerClass(Role selectedRole)
        {
            player.role = selectedRole;

            // Assign ImageIndex based on selected role
            switch (selectedRole)
            {
                case Role.Mage:
                    player.ImageIndex = 85; // Set the image index for Mage
                    break;
                case Role.Warrior:
                    player.ImageIndex = 86; // Set the image index for Warrior
                    break;
                case Role.Rogue:
                    player.ImageIndex = 87; // Set the image index for Rogue
                    break;
            }

            Console.WriteLine($"You have chosen {player.role} as your role.");
            isClassSelectionActive = false; // Exit class selection
        }

        private void Init()
        {
            Update();

            game_width = 16 * 16;
            game_height = 16 * 16;
            const int screen_width = 900;
            const int screen_height = 460;
            Raylib.InitWindow(screen_width, screen_height, "Rogue");
            Raylib.InitAudioDevice(); // Initialize audio device

            Raylib.SetWindowMinSize(game_width, game_height);
            Raylib.SetWindowState(ConfigFlags.FLAG_WINDOW_RESIZABLE);
            game_screen = Raylib.LoadRenderTexture(game_width, game_height);
            Raylib.SetTextureFilter(game_screen.texture, TextureFilter.TEXTURE_FILTER_POINT);

            imageTexture = Raylib.LoadTexture("RoguePics/tilemap_packed.png");
            backgroundMusic = Raylib.LoadMusicStream("LevelMusic/BeatTrack45.mp3");  

            SetImageAndIndex(player, imageTexture, imagesPerRow, index);
            

        }

        private void DrawGameScaled()
        {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Raylib.DARKGRAY);

            int draw_width = Raylib.GetScreenWidth();
            int draw_height = Raylib.GetScreenHeight();
            float scale = Math.Min((float)draw_width / game_width, (float)draw_height / game_height);

            Rectangle source = new Rectangle(0.0f, 0.0f, game_screen.texture.width, game_screen.texture.height * -1.0f);

            Rectangle destination = new Rectangle(
                (draw_width - (float)game_width * scale) * 0.5f,
                (draw_height - (float)game_height * scale) * 0.5f,
                game_width * scale,
                game_height * scale
            );

            Raylib.DrawTexturePro(game_screen.texture, source, destination, new Vector2(0, 0), 0.0f, Raylib.WHITE);

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
           
            Raylib.PlayMusicStream(backgroundMusic);
            Raylib.UnloadRenderTexture(game_screen);
            Console.WriteLine("Welcome to Netrogue!");
            Console.WriteLine("Press Enter to start the game...");
            while (Console.ReadKey().Key != ConsoleKey.Enter) { }
            Console.Clear();

            player = CreatePlayerCharacter();

            MapLoader loader = new MapLoader();
            level = loader.ReadMapFromFile("RogueMap.tmj");
            level.InitMap();
            Init();
            level.MapImage = imageTexture;
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

            level.LoadEnemiesAndItems();

            // Start the game loop
            Raylib.PlayMusicStream(backgroundMusic); // Start playing the music

            while (!Raylib.WindowShouldClose())
            {
                Raylib.UpdateMusicStream(backgroundMusic); // Update the music stream

                if (isMenuActive)
                {
                    DrawMainMenu(); // Draw the main menu
                }
                else if (isClassSelectionActive)
                {
                    DrawClassSelection(); // Draw the class selection menu
                }
                else if (isSettingsActive)
                {
                    DrawPause();
                }
                else
                {
                    if (Raylib.IsKeyPressed(KeyboardKey.KEY_P))
                    {
                        isSettingsActive = true; // Activate pause menu
                    }

                    MovePlayer();
                    level.MapImage = imageTexture;

                    Raylib.BeginTextureMode(game_screen);
                    level.Draw();

                    DrawPlayerInfo();
                    DrawPlayer();
                    Raylib.EndTextureMode();
                    DrawGameScaled();

                     
                }
            }
            Raylib.CloseWindow();

        }

         

        private void ResetGameState()
        {
            // Reset any game-specific state here if necessary
            player = null;
            level = null;
            Init(); // Reinitialize game or load the main menu
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
                else if (newPlayer.name.Length > 20)
                {
                    Console.WriteLine("Name cannot exceed 20 characters. Please enter a valid name.");
                    newPlayer.name = string.Empty; // Reset to allow for re-entry
                }
            } while (string.IsNullOrWhiteSpace(newPlayer.name) || newPlayer.name.Length > 20);

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
            if (Raylib.IsKeyPressed(KeyboardKey.KEY_DOWN))
            {
                moveY = 1;
            }
            if (Raylib.IsKeyPressed(KeyboardKey.KEY_LEFT))
            {
                moveX = -1;
            }
            if (Raylib.IsKeyPressed(KeyboardKey.KEY_RIGHT))
            {
                moveX = 1;
            }

            // Calculate new position
            Vector2 newPosition = player.position + new Vector2(moveX, moveY);

            // Get tile ID at the new position
            MapTile tileIdAtNewPosition = level.GetTileId((int)newPosition.X, (int)newPosition.Y);
            Console.WriteLine($"Tile ID at new position ({newPosition.X}, {newPosition.Y}): {tileIdAtNewPosition}");

            // Check for collision with walls
            //if (level.WallPositions.Any(pos => pos == newPosition) || tileIdAtNewPosition < 0)
            if (tileIdAtNewPosition == MapTile.Wall)
            {
                Console.Clear();
                Console.WriteLine("Cannot move through walls!");
                DisplayWallPositions(); // Display the wall positions
                return;
            }

            // Check for collision with mobs
            if (level.MobPositions.Any(pos => pos == newPosition))
            {
                Console.WriteLine("Encountered a mob!");
                // Handle mob encounter
                timer += 3;
                return; // Prevent moving onto the mob tile
            }

            // Check for collision with items
            if (level.ItemPositions.Any(pos => pos == newPosition))
            {

                Console.WriteLine("Collected an item!");
                // Handle item collection
                return; // Prevent moving onto the item tile
            }

            // Move the player to the new position
            player.position = newPosition;

            // Clear the screen and redraw player
            level.Draw();
            DrawPlayerInfo();
            DrawPlayer();
            Raylib.DrawText(tileIdAtNewPosition.ToString(), 10, 10, 12, Raylib.RED);
        }

        private void DisplayWallPositions()
        {
            Console.WriteLine("Wall Positions:");
            foreach (var pos in level.WallPositions)
            {

                Console.WriteLine($"X: {pos.X}, Y: {pos.Y}");
            }
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
