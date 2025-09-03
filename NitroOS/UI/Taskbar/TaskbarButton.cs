using Cosmos.System;
using Cosmos.System.Graphics;
using Cosmos.System.Graphics.Fonts;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NitroOS.UI.Taskbar
{
    public class TaskbarButton
    {
        public int X, Y, Width, Height;
        public string Text;
        public Pen Background, TextColor;
        public Action OnClick;

        public TaskbarButton(int x, int y, int width, int height, string text, Pen bg, Pen fg, Action onClick)
        {
            X = x; Y = y; Width = width; Height = height;
            Text = text; Background = bg; TextColor = fg; OnClick = onClick;
        }

        public void Draw(Canvas canvas)
        {
            bool mouseOver = MouseManager.X >= X && MouseManager.X <= X + Width &&
                             MouseManager.Y >= Y && MouseManager.Y <= Y + Height;

            var bgPen = mouseOver ? new Pen(Color.FromArgb(180, Background.Color)) : Background;

            canvas.DrawFilledRectangle(bgPen, X, Y, Width, Height);
            canvas.DrawString(Text, PCScreenFont.Default, TextColor, new Cosmos.System.Graphics.Point(X + 5, Y + 10));
        }

        public void CheckClick()
        {
            if (MouseManager.MouseState == MouseState.Left &&
                MouseManager.X >= X && MouseManager.X <= X + Width &&
                MouseManager.Y >= Y && MouseManager.Y <= Y + Height)
            {
                OnClick?.Invoke();
            }
        }
    }

}
