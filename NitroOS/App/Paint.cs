using Cosmos.System;
using Cosmos.System.Graphics;
using NitroOS.UI;
using System.Drawing;

namespace NitroOS.App
{
    internal class PaintApp
    {
        public Window PaintWindow;

        public PaintApp(int startX, int startY, int width, int height)
        {
            PaintWindow = new Window(
                startX, startY, width, height,
                new Pen(Color.LightGray),
                "Paint",
                DrawPaintContent
            );
        }

        private void DrawPaintContent(Canvas canvas, Window win)
        {
            if (MouseManager.MouseState == MouseState.Left)
            {
                int mouseX = (int)MouseManager.X;
                int mouseY = (int)MouseManager.Y;

                if (mouseX > win.X && mouseX < win.X + win.Width &&
                    mouseY > win.Y + 20 && mouseY < win.Y + win.Height)
                {
                    int relX = mouseX - win.X;
                    int relY = mouseY - win.Y - 20;

                    win.PaintPixels.Add(new Cosmos.System.Graphics.Point(relX, relY));
                }
            }
        }
    }
}
