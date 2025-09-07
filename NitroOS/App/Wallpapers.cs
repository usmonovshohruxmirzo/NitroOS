using Cosmos.System;
using Cosmos.System.Graphics;
using Cosmos.System.Graphics.Fonts;
using NitroOS.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NitroOS.App
{
    internal class Wallpapers
    {
        public Window WallpapersWindow;

        public Wallpapers(int startX, int startY, int width, int height)
        {
            WallpapersWindow = new Window(
                startX, startY, width, height,
                new Pen(Color.LightGray),
                "Wallpapers",
                DrawCalcContent
            );
        }

        private void DrawCalcContent(Canvas canvas, Window win)
        {
            string[] buttons = { "Red", "Green", "Blue" };
            int btnWidth = 100, btnHeight = 100;
            int startX = win.X + 10, startY = win.Y + 70;

            for (int i = 0; i < buttons.Length; i++)
            {
                int col = i % 4;
                int row = i / 4;

                int btnX = startX + col * (btnWidth + 5);
                int btnY = startY + row * (btnHeight + 5);

                Color btnColor = Color.FromName(buttons[i]);

                canvas.DrawFilledRectangle(new Pen(btnColor), btnX, btnY, btnWidth, btnHeight);

                canvas.DrawString(buttons[i], PCScreenFont.Default, new Pen(Color.White), new Cosmos.System.Graphics.Point(btnX + 15, btnY + 5));
            }

        }

    }
}
