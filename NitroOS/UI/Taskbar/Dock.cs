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
        public TaskbarButton[] Buttons;
        public AppsPanel AppsCenter;

        public Dock(int x, int width, int height, Pen bg, TaskbarButton[] buttons, AppsPanel appsCenter = null)
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
            canvas.DrawFilledRectangle(Background, X, y, Width, Height);

            foreach (var btn in Buttons) btn.Draw(canvas);
            AppsCenter?.Draw(canvas);
        }

        public void CheckClicks()
        {
            foreach (var btn in Buttons) btn.CheckClick();
            AppsCenter?.CheckClicks();
        }
    }
}
