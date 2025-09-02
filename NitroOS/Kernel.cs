using Cosmos.System;
using Cosmos.System.Graphics;
using Cosmos.System.Graphics.Fonts;
using System;
using System.Drawing;
using Sys = Cosmos.System;

namespace NitroOS
{
    public class Kernel : Sys.Kernel
    {
        private Canvas canvas;
        private Pen redPen;
        private Pen cursor;
        private Pen buttonPen;
        private bool isRed = true;
        private const int buttonX = 10;
        private const int buttonY = 10;
        private const int buttonWidth = 100;
        private const int buttonHeight = 40;

        protected override void BeforeRun()
        {
            canvas = FullScreenCanvas.GetFullScreenCanvas(new Mode(800, 600, ColorDepth.ColorDepth32));
            canvas.Clear(Color.White);

            redPen = new Pen(Color.Red);
            cursor = new Pen(Color.Black);
            buttonPen = new Pen(Color.Green);

            MouseManager.ScreenWidth = 800;
            MouseManager.ScreenHeight = 600;
        }

        protected override void Run()
        {
            canvas.DrawFilledRectangle(new Pen(Color.White), 0, 0, 800, 600);

            canvas.DrawFilledRectangle(buttonPen, buttonX, buttonY, buttonWidth, buttonHeight);
            canvas.DrawString(
                "Click Me",
                PCScreenFont.Default,
                new Pen(Color.Black),
                buttonX + 5,
                buttonY + 10
            );

            if (MouseManager.MouseState == MouseState.Left &&
                MouseManager.X >= buttonX && MouseManager.X <= buttonX + buttonWidth &&
                MouseManager.Y >= buttonY && MouseManager.Y <= buttonY + buttonHeight)
            {
                isRed = !isRed;
                System.Threading.Thread.Sleep(200);
            }

            canvas.DrawFilledRectangle(new Pen(isRed ? Color.Red : Color.Blue), 200, 150, 400, 300);

            canvas.DrawFilledRectangle(cursor, (int)MouseManager.X, (int)MouseManager.Y, 15, 15);

            canvas.Display();
        }
    }
}