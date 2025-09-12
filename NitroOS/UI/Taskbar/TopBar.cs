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

        private int margin = 10;

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
            DrawRoundedBar(canvas);

            foreach (var btn in Buttons) btn.Draw(canvas);
            foreach (var menu in Menus) menu.Draw(canvas);

            // Example status indicators
            string timeStr = DateTime.Now.ToString("HH:mm");
            canvas.DrawString(timeStr, PCScreenFont.Default, new Pen(Color.White),
                new Cosmos.System.Graphics.Point((Width - margin) - 80, margin + 5));

            string batteryStr = "100%";
            canvas.DrawString(batteryStr, PCScreenFont.Default, new Pen(Color.White),
                new Cosmos.System.Graphics.Point((Width - margin) - 150, margin + 5));
        }

        private void DrawRoundedBar(Canvas canvas)
        {
            int barX = margin;
            int barY = margin;
            int barWidth = Width - (2 * margin);
            int barHeight = Height;

            int radius = (int)(barHeight / 2 - 0.4);

            canvas.DrawFilledRectangle(Background,
                barX + radius, barY,
                barWidth - (2 * radius), barHeight);

            canvas.DrawFilledCircle(Background, barX + radius, barY + radius, radius);

            canvas.DrawFilledCircle(Background, barX + barWidth - radius, barY + radius, radius);
        }

        public void CheckClicks()
        {
            foreach (var btn in Buttons) btn.CheckClick();
            foreach (var menu in Menus) menu.CheckClicks();
        }
    }
}
