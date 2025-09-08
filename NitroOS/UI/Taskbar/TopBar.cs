using Cosmos.System;
using Cosmos.System.Graphics;
using Cosmos.System.Graphics.Fonts;
using System;
using System.Drawing;

namespace NitroOS.UI.Taskbar
{
    public class TopBar
    {
        public int Width;
        public int Height;
        public Pen Background;
        public TaskbarButton[] Buttons;
        public Menu[] Menus;

        public TopBar(int width, int height, Pen bg, TaskbarButton[] buttons = null, Menu[] menus = null)
        {
            Width = width;
            Height = height;
            Background = bg;
            Buttons = buttons ?? new TaskbarButton[0];
            Menus = menus ?? new Menu[0];
        }

        public void Draw(Canvas canvas)
        {
            // Draw full-width top bar
            canvas.DrawFilledRectangle(Background, 0, 0, Width, Height);

            // Draw buttons and menus
            foreach (var btn in Buttons) btn.Draw(canvas);
            foreach (var menu in Menus) menu.Draw(canvas);

            // Clock (top-right)
            string timeStr = DateTime.Now.ToString("HH:mm");
            canvas.DrawString(timeStr, PCScreenFont.Default, new Pen(Color.White),
                new Cosmos.System.Graphics.Point(Width - 80, 5));

            // Battery status placeholder (top-right, left of clock)
            string batteryStr = "🔋 100%"; // TODO: integrate actual battery if supported
            canvas.DrawString(batteryStr, PCScreenFont.Default, new Pen(Color.White),
                new Cosmos.System.Graphics.Point(Width - 150, 5));
        }

        public void CheckClicks()
        {
            foreach (var btn in Buttons) btn.CheckClick();
            foreach (var menu in Menus) menu.CheckClicks();
        }
    }
}
