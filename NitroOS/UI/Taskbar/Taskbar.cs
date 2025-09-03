using Cosmos.System;
using Cosmos.System.Graphics;
using Cosmos.System.Graphics.Fonts;
using System;
using System.Drawing;
using System.Collections.Generic;

namespace NitroOS.UI.Taskbar
{
    public class Taskbar
    {
        public int ScreenWidth, ScreenHeight, Height;
        public Pen Background;
        public TaskbarButton[] Buttons;
        public Menu[] Menus;
        public AppsPanel AppsCenter;

        public Taskbar(int screenWidth, int screenHeight, int height, Pen bg,
            TaskbarButton[] buttons, Menu[] menus, AppsPanel appsCenter = null)
        {
            ScreenWidth = screenWidth;
            ScreenHeight = screenHeight;
            Height = height;
            Background = bg;
            Buttons = buttons;
            Menus = menus;
            AppsCenter = appsCenter;
        }

        public void Draw(Canvas canvas)
        {
            canvas.DrawFilledRectangle(Background, 0, ScreenHeight - Height, ScreenWidth, Height);

            foreach (var btn in Buttons)
                btn.Draw(canvas);

            AppsCenter?.Draw(canvas);

            foreach (var menu in Menus)
                menu.Draw(canvas);

            string dateStr = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
            canvas.DrawString(dateStr, PCScreenFont.Default, new Pen(Color.White),
                new Cosmos.System.Graphics.Point(ScreenWidth - 200, ScreenHeight - Height + 25));
        }

        public void CheckClicks()
        {
            foreach (var btn in Buttons)
                btn.CheckClick();

            AppsCenter?.CheckClicks();

            foreach (var menu in Menus)
                menu.CheckClicks();
        }
    }
}
