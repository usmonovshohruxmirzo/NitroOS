using Cosmos.System;
using Cosmos.System.Graphics;
using Cosmos.System.Graphics.Fonts;
using NitroOS.UI;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace NitroOS.App
{
    public class SnakeGame
    {
        public Window GameWindow;

        // grid settings
        private readonly int cols;
        private readonly int rows;
        private readonly int cellSize = 12; // size of each cell in pixels
        private readonly int boardX = 10;   // padding inside window
        private readonly int boardY = 30;

        // game state
        private List<Cosmos.System.Graphics.Point> snake = new List<Cosmos.System.Graphics.Point>();
        private Cosmos.System.Graphics.Point food;
        private Direction currentDir = Direction.Right;
        private bool growNext = false;
        private DateTime lastMove = DateTime.MinValue;
        private int moveMs = 150; // milliseconds per move (speed)
        private bool isGameOver = false;
        private int score = 0;
        private Random rnd = new Random();

        private Pen bgPen = new Pen(Color.Black);
        private Pen gridPen = new Pen(Color.FromArgb(40, 40, 40));
        private Pen snakePen = new Pen(Color.Lime);
        private Pen foodPen = new Pen(Color.Red);
        private Pen textPen = new Pen(Color.White);

        public SnakeGame(int x, int y, int width, int height)
        {
            // compute grid size from window size
            cols = Math.Max(10, (width - boardX * 2) / cellSize);
            rows = Math.Max(10, (height - boardY - 10) / cellSize);

            // create window and pass draw delegate
            GameWindow = new Window(x, y, width, height, new Pen(Color.DarkSlateGray), "Snake", DrawContent);

            ResetGame();
        }

        private void ResetGame()
        {
            snake.Clear();
            // start centered-ish
            var start = new Cosmos.System.Graphics.Point(cols / 2, rows / 2);
            snake.Add(start);
            // make initial length 4
            for (int i = 1; i < 4; i++)
                snake.Add(new Cosmos.System.Graphics.Point(start.X - i, start.Y));

            currentDir = Direction.Right;
            PlaceFood();
            lastMove = DateTime.Now;
            isGameOver = false;
            score = 0;
            growNext = false;
        }

        private void PlaceFood()
        {
            // place food on random empty cell
            while (true)
            {
                int fx = rnd.Next(0, cols);
                int fy = rnd.Next(0, rows);
                bool used = false;
                foreach (var s in snake)
                    if (s.X == fx && s.Y == fy) { used = true; break; }
                if (!used) { food = new Cosmos.System.Graphics.Point(fx, fy); break; }
            }
        }

        private void DrawBoard(Canvas canvas, Window win)
        {
            int w = win.Width;
            int h = win.Height;

            // background of board area
            int boardPxW = cols * cellSize;
            int boardPxH = rows * cellSize;
            int bx = win.X + boardX;
            int by = win.Y + boardY;

            canvas.DrawFilledRectangle(bgPen, bx, by, boardPxW, boardPxH);

            // optional grid (light)
            for (int gx = 0; gx <= cols; gx++)
            {
                int x = bx + gx * cellSize;
                canvas.DrawLine(gridPen, x, by, x, by + boardPxH);
            }
            for (int gy = 0; gy <= rows; gy++)
            {
                int y = by + gy * cellSize;
                canvas.DrawLine(gridPen, bx, y, bx + boardPxW, y);
            }

            // draw food (filled rect)
            int fx = bx + food.X * cellSize;
            int fy = by + food.Y * cellSize;
            canvas.DrawFilledRectangle(foodPen, fx + 2, fy + 2, cellSize - 4, cellSize - 4);

            // draw snake as connected lines between cell centers
            if (snake.Count > 0)
            {
                for (int i = 0; i < snake.Count - 1; i++)
                {
                    var a = snake[i];
                    var b = snake[i + 1];
                    int ax = bx + a.X * cellSize + cellSize / 2;
                    int ay = by + a.Y * cellSize + cellSize / 2;
                    int bx2 = bx + b.X * cellSize + cellSize / 2;
                    int by2 = by + b.Y * cellSize + cellSize / 2;
                    canvas.DrawLine(snakePen, ax, ay, bx2, by2);
                }
                // draw head as small filled rectangle
                var head = snake[0];
                int hx = bx + head.X * cellSize;
                int hy = by + head.Y * cellSize;
                canvas.DrawFilledRectangle(snakePen, hx + 2, hy + 2, cellSize - 4, cellSize - 4);
            }

            // score & info
            canvas.DrawString($"Score: {score}", PCScreenFont.Default, textPen, new Cosmos.System.Graphics.Point(win.X + 8, win.Y + 8));
            if (isGameOver)
            {
                canvas.DrawString("GAME OVER - Press R to restart", PCScreenFont.Default, new Pen(Color.Yellow),
                    new Cosmos.System.Graphics.Point(win.X + 8, win.Y + win.Height - 18));
            }
        }

        private void UpdateGame()
        {
            // handle input (keyboard)
            var k = KeyboardManager.ReadKey();
            if (k != null)
            {
                if (k.Key == ConsoleKeyEx.LeftArrow || k.KeyChar == 'a' || k.KeyChar == 'A')
                    TryChangeDir(Direction.Left);
                else if (k.Key == ConsoleKeyEx.RightArrow || k.KeyChar == 'd' || k.KeyChar == 'D')
                    TryChangeDir(Direction.Right);
                else if (k.Key == ConsoleKeyEx.UpArrow || k.KeyChar == 'w' || k.KeyChar == 'W')
                    TryChangeDir(Direction.Up);
                else if (k.Key == ConsoleKeyEx.DownArrow || k.KeyChar == 's' || k.KeyChar == 'S')
                    TryChangeDir(Direction.Down);
                else if (k.Key == ConsoleKeyEx.R)
                { ResetGame(); return; }
            }


            // time-based movement
            var now = DateTime.Now;
            if ((now - lastMove).TotalMilliseconds >= moveMs && !isGameOver)
            {
                lastMove = now;
                Step();
            }
        }

        private void TryChangeDir(Direction newDir)
        {
            if (newDir == Direction.Left && currentDir == Direction.Right) return;
            if (newDir == Direction.Right && currentDir == Direction.Left) return;
            if (newDir == Direction.Up && currentDir == Direction.Down) return;
            if (newDir == Direction.Down && currentDir == Direction.Up) return;
            currentDir = newDir;
        }

        private void Step()
        {
            var head = snake[0];
            int nx = head.X;
            int ny = head.Y;
            switch (currentDir)
            {
                case Direction.Left: nx--; break;
                case Direction.Right: nx++; break;
                case Direction.Up: ny--; break;
                case Direction.Down: ny++; break;
            }

            // wrap around (or you can set game over at bounds)
            if (nx < 0) nx = cols - 1;
            if (nx >= cols) nx = 0;
            if (ny < 0) ny = rows - 1;
            if (ny >= rows) ny = 0;

            var newHead = new Cosmos.System.Graphics.Point(nx, ny);

            // collision with self?
            foreach (var s in snake)
            {
                if (s.X == newHead.X && s.Y == newHead.Y)
                {
                    isGameOver = true;
                    return;
                }
            }

            // insert new head
            snake.Insert(0, newHead);

            // eat food?
            if (newHead.X == food.X && newHead.Y == food.Y)
            {
                score += 1;
                growNext = true;
                PlaceFood();
            }

            if (!growNext)
            {
                // remove tail
                snake.RemoveAt(snake.Count - 1);
            }
            else
            {
                // keep tail (grow)
                growNext = false;
            }
        }

        private void DrawContent(Canvas canvas, Window win)
        {
            // run update (input + step) on each draw call
            try
            {
                UpdateGame();
            }
            catch { /* ignore runtime input differences */ }

            // draw the board and snake
            DrawBoard(canvas, win);
        }

        private enum Direction { Left, Right, Up, Down }
    }
}
