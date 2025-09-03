using Cosmos.System.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NitroOS.UI.Taskbar
{
    public class Menu
    {
        public int X, Y, Width;
        public Pen Background;
        public TaskbarButton[] Items;
        public bool IsOpen;

        private int padding = 10;
        private int itemHeight = 35;

        public Menu(int x, int y, int width, Pen bg, TaskbarButton[] items)
        {
            X = x; Y = y; Width = width; Background = bg; Items = items;
        }

        public int Height => Items.Length * itemHeight + padding * 2;

        public void Draw(Canvas canvas)
        {
            if (!IsOpen) return;

            canvas.DrawFilledRectangle(Background, X, Y - Height, Width, Height);

            for (int i = 0; i < Items.Length; i++)
            {
                var item = Items[i];
                item.X = X + padding;
                item.Y = Y - Height + padding + i * itemHeight;
                item.Width = Width - padding * 2;
                item.Height = itemHeight;
                item.Draw(canvas);
            }
        }

        public void CheckClicks()
        {
            if (!IsOpen) return;
            foreach (var item in Items)
                item.CheckClick();
        }
    }
}
