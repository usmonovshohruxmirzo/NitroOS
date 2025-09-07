using Cosmos.System;
using Cosmos.System.Graphics;
using Cosmos.System.Graphics.Fonts;
using NitroOS.UI;
using System;
using System.Drawing;

namespace NitroOS.App.Games
{
internal class NumberGuesserGame
{
    public Window GameWindow;
    private int secretNumber;
    private Random random = new Random();
    private string lastMessage = "";

    public NumberGuesserGame(int startX, int startY, int width, int height)
    {
        secretNumber = random.Next(1, 11);

        GameWindow = new Window(
            startX,
            startY,
            width,
            height,
            new Pen(Color.Gray),
            "Number Guesser",
            DrawGameContent
        );
    }

    private void DrawGameContent(Canvas canvas, Window win)
    {
        int buttonWidth = 50;
        int buttonHeight = 50;
        int startX = win.X + 20;
        int startY = win.Y + 50;

        for (int i = 1; i <= 10; i++)
        {
            int col = (i - 1) % 5;
            int row = (i - 1) / 5;

            int btnX = startX + col * (buttonWidth + 10);
            int btnY = startY + row * (buttonHeight + 10);

            canvas.DrawFilledRectangle(new Pen(Color.Blue), btnX, btnY, buttonWidth, buttonHeight);
            canvas.DrawString(i.ToString(), PCScreenFont.Default, new Pen(Color.White),
                new Cosmos.System.Graphics.Point(btnX + 15, btnY + 15));

            // Detect click
            if (MouseManager.MouseState == MouseState.Left &&
                MouseManager.X >= btnX && MouseManager.X <= btnX + buttonWidth &&
                MouseManager.Y >= btnY && MouseManager.Y <= btnY + buttonHeight)
            {
                CheckGuess(i);
            }
        }

        canvas.DrawString(lastMessage, PCScreenFont.Default, new Pen(Color.Black),
            new Cosmos.System.Graphics.Point(win.X + 20, win.Y + win.Height - 30));
    }

    private void CheckGuess(int guess)
    {
        if (guess == secretNumber)
        {
            lastMessage = $"Correct! Number was {secretNumber}";
            secretNumber = random.Next(1, 11);
        }
        else
        {
            lastMessage = guess < secretNumber ? "Too low!" : "Too high!";
        }
    }
}
}
