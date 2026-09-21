using System;
using System.Collections.Generic;
using System.Threading;

// ═══════════════════════════════════════════════════════════
//  🐍 SAANP SEEDHI - Snake & Ladder Game 🪜
//  Classic Indian Board Game in C#
// ═══════════════════════════════════════════════════════════

namespace SnakeLadder
{
    class Player
    {
        public string Name { get; set; }
        public int Position { get; set; }
        public ConsoleColor Color { get; set; }
        public string Symbol { get; set; }
        public int SixCount { get; set; }
        public int SnakeBites { get; set; }
        public int LaddersClimbed { get; set; }
        public int TotalRolls { get; set; }
        public bool IsComputer { get; set; }

        public Player(string name, ConsoleColor color, string symbol, bool isComputer = false)
        {
            Name = name;
            Position = 0;
            Color = color;
            Symbol = symbol;
            SixCount = 0;
            SnakeBites = 0;
            LaddersClimbed = 0;
            TotalRolls = 0;
            IsComputer = isComputer;
        }
    }

    class Game
    {
        // Snakes: Key = head, Value = tail (go DOWN)
        Dictionary<int, int> snakes = new Dictionary<int, int>()
        {
            { 99, 12 },
            { 95, 36 },
            { 92, 51 },
            { 83, 42 },
            { 73, 1  },
            { 62, 18 },
            { 54, 34 },
            { 46, 5  },
            { 38, 20 },
            { 27, 7  },
            { 16, 4  }
        };

        // Ladders: Key = bottom, Value = top (go UP)
        Dictionary<int, int> ladders = new Dictionary<int, int>()
        {
            { 2,  23 },
            { 8,  29 },
            { 14, 77 },
            { 21, 56 },
            { 28, 44 },
            { 36, 57 },
            { 43, 76 },
            { 50, 69 },
            { 61, 80 },
            { 71, 90 },
            { 78, 98 }
        };

        List<Player> players = new List<Player>();
        Random rng = new Random();
        bool gameOver = false;

        public void Run()
        {
            Console.Title = "Saanp Seedhi - Snake & Ladder";
            try { Console.SetWindowSize(85, 45); } catch { }
            try { Console.SetBufferSize(85, 500); } catch { }
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            ShowTitle();
            SetupPlayers();
            PlayGame();
        }

        void ShowTitle()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(@"
   ╔═══════════════════════════════════════════════════════════╗
   ║                                                           ║
   ║      ____                          ____               _ _ ║
   ║     / ___|  __ _  __ _ _ __  _ __ / ___|  ___  ___  _| | |║
   ║     \___ \ / _` |/ _` | '_ \| '_ \___ \ / _ \/ _ \/ _` | |║
   ║      ___) | (_| | (_| | | | | |_) |__) |  __/  __/ (_| |_|║
   ║     |____/ \__,_|\__,_|_| |_| .__/____/ \___|\___|\__,_(_)║
   ║                              |_|                           ║
   ║                                                           ║");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(@"   ║           SNAKE  &  LADDER  GAME                        ║");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(@"   ║                                                           ║
   ║        Classic Indian Board Game  (Saanp Seedhi)          ║
   ║                                                           ║");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(@"   ╚═══════════════════════════════════════════════════════════╝");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(@"
        ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
           HOW TO PLAY:
           * Press ENTER to roll the dice
           * Climb LADDERS to go UP
           * Beware of SNAKES - they bite you DOWN!
           * First to reach 100 WINS!
        ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("              [ Press ENTER to start the game ] ");
            Console.ResetColor();
            Console.ReadLine();
        }

        void SetupPlayers()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(@"
   ╔═══════════════════════════════════════╗
   ║         PLAYER SETUP                  ║
   ╚═══════════════════════════════════════╝
");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("   How many players? (2-4)");
            Console.ResetColor();
            Console.Write("   >> ");
            string input = Console.ReadLine();
            int count;
            if (!int.TryParse(input, out count) || count < 2 || count > 4)
                count = 2;

            ConsoleColor[] colors = { ConsoleColor.Cyan, ConsoleColor.Yellow, ConsoleColor.Green, ConsoleColor.Magenta };
            string[] symbols = { "P1", "P2", "P3", "P4" };
            string[] defaultNames = { "Player 1", "Player 2", "Player 3", "Player 4" };

            for (int i = 0; i < count; i++)
            {
                Console.ForegroundColor = colors[i];
                Console.Write("\n   Enter name for " + defaultNames[i] + " (or press Enter for default): ");
                Console.ResetColor();
                string name = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(name)) name = defaultNames[i];

                Console.ForegroundColor = colors[i];
                Console.Write("   Is " + name + " a Computer player? (Y/N): ");
                Console.ResetColor();
                string compInput = Console.ReadLine();
                bool isComp = compInput != null && compInput.Trim().ToUpper().StartsWith("Y");

                players.Add(new Player(name, colors[i], symbols[i], isComp));
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n   All players ready! Let's play!");
            Console.ResetColor();
            Thread.Sleep(1000);
        }

        void DrawBoard()
        {
            Console.Clear();

            // Header
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("  ╔═══════════════════════════════════════════════════════════════╗");
            Console.Write("  ║  ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("SAANP SEEDHI");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("  |  ");

            // Show player positions in header
            for (int i = 0; i < players.Count; i++)
            {
                Console.ForegroundColor = players[i].Color;
                Console.Write(players[i].Name + ":" + players[i].Position + "  ");
            }
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            int headerLen = 14 + 5; // base text
            foreach (var p in players) headerLen += p.Name.Length + 1 + p.Position.ToString().Length + 2;
            int pad = 61 - headerLen;
            if (pad < 0) pad = 0;
            Console.Write(new string(' ', pad));
            Console.WriteLine("║");
            Console.WriteLine("  ╠═══════════════════════════════════════════════════════════════╣");
            Console.ResetColor();

            // Board: 10x10 grid, numbers 1-100
            // Row 10: 100 99 98 ... 91 (right to left)
            // Row  9: 81  82 83 ... 90 (left to right)
            // ...alternating

            for (int row = 9; row >= 0; row--)
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.Write("  ║ ");
                Console.ResetColor();

                for (int col = 0; col < 10; col++)
                {
                    int cellNum;
                    if (row % 2 == 1) // odd rows go right to left (when 0-indexed from bottom)
                        cellNum = (row + 1) * 10 - col;
                    else // even rows go left to right
                        cellNum = row * 10 + col + 1;

                    // Check if any player is on this cell
                    bool hasPlayer = false;
                    foreach (var p in players)
                    {
                        if (p.Position == cellNum)
                        {
                            Console.ForegroundColor = p.Color;
                            string display = " " + p.Symbol + " ";
                            Console.Write(display);
                            hasPlayer = true;
                            break;
                        }
                    }

                    if (!hasPlayer)
                    {
                        if (snakes.ContainsKey(cellNum))
                        {
                            // Snake head
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write(cellNum.ToString().PadLeft(3) + "~");
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                            Console.Write("S");
                            Console.ResetColor();
                            Console.Write(" ");
                        }
                        else if (ladders.ContainsKey(cellNum))
                        {
                            // Ladder bottom
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write(cellNum.ToString().PadLeft(3) + "^");
                            Console.ForegroundColor = ConsoleColor.DarkGreen;
                            Console.Write("L");
                            Console.ResetColor();
                            Console.Write(" ");
                        }
                        else if (cellNum == 100)
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.Write(" WIN ");
                            Console.ResetColor();
                            Console.Write(" ");
                        }
                        else
                        {
                            // Normal cell
                            if (snakes.ContainsValue(cellNum))
                                Console.ForegroundColor = ConsoleColor.DarkRed;
                            else if (ladders.ContainsValue(cellNum))
                                Console.ForegroundColor = ConsoleColor.DarkGreen;
                            else
                                Console.ForegroundColor = ConsoleColor.DarkGray;

                            Console.Write(cellNum.ToString().PadLeft(4) + " ");
                            Console.ResetColor();
                            Console.Write(" ");
                        }
                    }
                }

                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("║");
                Console.ResetColor();

                // Row separator
                if (row > 0)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine("  ║ " + new string('-', 60) + "║");
                    Console.ResetColor();
                }
            }

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("  ╠═══════════════════════════════════════════════════════════════╣");
            Console.ResetColor();

            // Legend
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write("  ║  ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("~S = SNAKE (Down!)  ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("^L = LADDER (Up!)  ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("WIN = Finish!");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("     ║");
            Console.WriteLine("  ╚═══════════════════════════════════════════════════════════════╝");
            Console.ResetColor();
        }

        void PlayGame()
        {
            int currentPlayerIndex = 0;

            while (!gameOver)
            {
                Player currentPlayer = players[currentPlayerIndex];
                DrawBoard();

                Console.WriteLine();
                Console.ForegroundColor = currentPlayer.Color;
                Console.WriteLine("  ┌─────────────────────────────────────────────┐");
                Console.WriteLine("  │  " + currentPlayer.Name + "'s Turn" + new string(' ', Math.Max(0, 40 - currentPlayer.Name.Length - 8)) + "│");
                Console.WriteLine("  │  Current Position: " + currentPlayer.Position + new string(' ', Math.Max(0, 40 - 20 - currentPlayer.Position.ToString().Length)) + "│");
                Console.WriteLine("  └─────────────────────────────────────────────┘");
                Console.ResetColor();

                if (currentPlayer.IsComputer)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.Write("  " + currentPlayer.Name + " (Computer) is rolling");
                    for (int i = 0; i < 3; i++) { Thread.Sleep(400); Console.Write("."); }
                    Console.ResetColor();
                    Console.WriteLine();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("  Press ENTER to roll the dice... ");
                    Console.ResetColor();
                    Console.ReadLine();
                }

                // Roll dice with animation
                int dice = RollDiceWithAnimation(currentPlayer);
                currentPlayer.TotalRolls++;

                // First move rule: need a 6 or 1 to start
                if (currentPlayer.Position == 0 && dice != 6 && dice != 1)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine("  You need a 1 or 6 to start! Better luck next time.");
                    Console.ResetColor();
                    Thread.Sleep(1500);
                    currentPlayerIndex = (currentPlayerIndex + 1) % players.Count;
                    continue;
                }

                // Special: rolled a 6
                bool rolledSix = (dice == 6);
                if (rolledSix)
                {
                    currentPlayer.SixCount++;
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("  CHHAKKA! You rolled a 6! You get another turn!");
                    Console.ResetColor();

                    // Three consecutive 6s = back to start
                    if (currentPlayer.SixCount >= 3)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("  Oh no! Three 6s in a row! Back to START!");
                        Console.ResetColor();
                        currentPlayer.Position = 0;
                        currentPlayer.SixCount = 0;
                        Thread.Sleep(2000);
                        currentPlayerIndex = (currentPlayerIndex + 1) % players.Count;
                        continue;
                    }
                }
                else
                {
                    currentPlayer.SixCount = 0;
                }

                // Calculate new position
                int newPos = currentPlayer.Position + dice;

                // Can't go beyond 100
                if (newPos > 100)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine("  Need exact number to reach 100! Stay at " + currentPlayer.Position);
                    Console.ResetColor();
                    Thread.Sleep(1500);
                    if (!rolledSix)
                        currentPlayerIndex = (currentPlayerIndex + 1) % players.Count;
                    continue;
                }

                // Move player
                Console.ForegroundColor = currentPlayer.Color;
                Console.WriteLine("  Moving from " + currentPlayer.Position + " to " + newPos + "...");
                Console.ResetColor();
                currentPlayer.Position = newPos;
                Thread.Sleep(800);

                // Check for win
                if (currentPlayer.Position == 100)
                {
                    gameOver = true;
                    ShowWinner(currentPlayer);
                    return;
                }

                // Check for snake
                if (snakes.ContainsKey(currentPlayer.Position))
                {
                    int snakeTail = snakes[currentPlayer.Position];
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(@"
     ____
    / . .\
    \  ---<     SAANP NE KAT LIYA! (Snake Bite!)
     \  /
   __/ /
  (____)");
                    Console.WriteLine("  SNAKE at " + currentPlayer.Position + "! Sliding down to " + snakeTail + "!");
                    Console.ResetColor();
                    currentPlayer.Position = snakeTail;
                    currentPlayer.SnakeBites++;
                    Thread.Sleep(2000);
                }
                // Check for ladder
                else if (ladders.ContainsKey(currentPlayer.Position))
                {
                    int ladderTop = ladders[currentPlayer.Position];
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(@"
      |---|
      | | |     SEEDHI MIL GAYI! (Found a Ladder!)
      | | |
      | | |
      |---|
     /     \");
                    Console.WriteLine("  LADDER at " + currentPlayer.Position + "! Climbing up to " + ladderTop + "!");
                    Console.ResetColor();
                    currentPlayer.Position = ladderTop;
                    currentPlayer.LaddersClimbed++;

                    // Check for win after ladder
                    if (currentPlayer.Position == 100)
                    {
                        gameOver = true;
                        ShowWinner(currentPlayer);
                        return;
                    }
                    Thread.Sleep(2000);
                }

                Thread.Sleep(500);

                // If rolled 6, same player goes again
                if (!rolledSix)
                {
                    currentPlayerIndex = (currentPlayerIndex + 1) % players.Count;
                }
            }
        }

        int RollDiceWithAnimation(Player player)
        {
            int finalDice = rng.Next(1, 7);

            // Dice animation
            Console.Write("  Dice: ");
            string[] diceFaces = { "⚀", "⚁", "⚂", "⚃", "⚄", "⚅" };
            for (int i = 0; i < 8; i++)
            {
                int tempDice = rng.Next(1, 7);
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(diceFaces[tempDice - 1] + " ");
                Thread.Sleep(100);
                Console.Write("\b\b");
            }

            // Show final result
            Console.ForegroundColor = finalDice == 6 ? ConsoleColor.Yellow : ConsoleColor.White;
            Console.Write(diceFaces[finalDice - 1] + " ");
            Console.ResetColor();

            // Show dice art
            Console.WriteLine();
            DrawDiceArt(finalDice, player.Color);

            return finalDice;
        }

        void DrawDiceArt(int number, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine("  ┌───────┐");
            switch (number)
            {
                case 1:
                    Console.WriteLine("  │       │");
                    Console.WriteLine("  │   ●   │");
                    Console.WriteLine("  │       │");
                    break;
                case 2:
                    Console.WriteLine("  │ ●     │");
                    Console.WriteLine("  │       │");
                    Console.WriteLine("  │     ● │");
                    break;
                case 3:
                    Console.WriteLine("  │ ●     │");
                    Console.WriteLine("  │   ●   │");
                    Console.WriteLine("  │     ● │");
                    break;
                case 4:
                    Console.WriteLine("  │ ●   ● │");
                    Console.WriteLine("  │       │");
                    Console.WriteLine("  │ ●   ● │");
                    break;
                case 5:
                    Console.WriteLine("  │ ●   ● │");
                    Console.WriteLine("  │   ●   │");
                    Console.WriteLine("  │ ●   ● │");
                    break;
                case 6:
                    Console.WriteLine("  │ ●   ● │");
                    Console.WriteLine("  │ ●   ● │");
                    Console.WriteLine("  │ ●   ● │");
                    break;
            }
            Console.WriteLine("  └───────┘");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("  You rolled: " + number + (number == 6 ? "  CHHAKKA!" : ""));
            Console.ResetColor();
        }

        void ShowWinner(Player winner)
        {
            DrawBoard();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(@"
  ╔═══════════════════════════════════════════════════════════╗
  ║                                                           ║
  ║           ★ ★ ★  BADHAI HO! JEET GAYE!  ★ ★ ★           ║
  ║              (CONGRATULATIONS! YOU WON!)                  ║
  ║                                                           ║
  ╠═══════════════════════════════════════════════════════════╣");
            Console.ResetColor();

            Console.ForegroundColor = winner.Color;
            string winLine = "  ║     WINNER: " + winner.Name;
            Console.Write(winLine);
            Console.Write(new string(' ', Math.Max(0, 60 - winLine.Length + 3)));
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("║");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("  ╠═══════════════════════════════════════════════════════════╣");
            Console.ResetColor();

            // Show all players' stats
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("  ║                                                           ║");
            Console.WriteLine("  ║   FINAL SCOREBOARD:                                       ║");
            Console.WriteLine("  ║                                                           ║");
            Console.ResetColor();

            foreach (var p in players)
            {
                Console.ForegroundColor = p.Color;
                string line = "  ║   " + p.Name;
                Console.Write(line);
                Console.Write(new string(' ', Math.Max(1, 18 - p.Name.Length)));
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write("Pos:" + p.Position.ToString().PadLeft(3));
                Console.Write("  Rolls:" + p.TotalRolls.ToString().PadLeft(3));
                Console.Write("  Snakes:" + p.SnakeBites.ToString().PadLeft(2));
                Console.Write("  Ladders:" + p.LaddersClimbed.ToString().PadLeft(2));
                int remaining = 60 - 18 - 8 - 10 - 11 - 12 + p.Name.Length;
                if (remaining < 0) remaining = 0;
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write(new string(' ', remaining));
                Console.WriteLine("║");
                Console.ResetColor();
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("  ║                                                           ║");
            Console.WriteLine("  ╚═══════════════════════════════════════════════════════════╝");
            Console.ResetColor();

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("  Play again? (Y/N): ");
            Console.ResetColor();
            string again = Console.ReadLine();
            if (again != null && again.Trim().ToUpper().StartsWith("Y"))
            {
                // Reset all players
                foreach (var p in players)
                {
                    p.Position = 0;
                    p.SixCount = 0;
                    p.SnakeBites = 0;
                    p.LaddersClimbed = 0;
                    p.TotalRolls = 0;
                }
                gameOver = false;
                PlayGame();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\n  Thanks for playing Saanp Seedhi! Phir milenge! Bye! ");
                Console.ResetColor();
                Console.Write("  Press any key to exit...");
                Console.ReadKey(true);
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Game game = new Game();
            game.Run();
        }
    }
}
