using Cosmos.System;
using Cosmos.System.Graphics;
using Cosmos.System.Graphics.Fonts;
using System;
using System.Drawing;

namespace NitroOS.UI.Taskbar
{
    public class Dock
    {
        public int X, Width, Height;
        public Pen Background;
        public Button[] Buttons;
        public AppsPanel AppsCenter;

        public int CornerRadius = 20;

        public Dock(int x, int width, int height, Pen bg, Button[] buttons, AppsPanel appsCenter = null)
        {
            X = x;
            Width = width;
            Height = height;
            Background = bg;
            Buttons = buttons;
            AppsCenter = appsCenter;
        }

        public void Draw(Canvas canvas, int screenHeight)
        {
            int y = screenHeight - Height;

            DrawRoundedDock(canvas, Background, X, y, Width, Height, CornerRadius);

            foreach (var btn in Buttons)
                btn.Draw(canvas);

            AppsCenter?.Draw(canvas);
        }

        private void DrawRoundedDock(Canvas canvas, Pen pen, int x, int y, int width, int height, int cornerRadius)
        {
            int circleRadius = 34;

            canvas.DrawFilledRectangle(pen, x + circleRadius, y, width - 2 * circleRadius, height);
            canvas.DrawFilledCircle(pen, x + circleRadius, y + height / 2, circleRadius);
            canvas.DrawFilledCircle(pen, x + width - circleRadius, y + height / 2, circleRadius);
        }


        public void CheckClicks()
        {
            foreach (var btn in Buttons)
                btn.CheckClick();

            AppsCenter?.CheckClicks();
        }
    }
}
