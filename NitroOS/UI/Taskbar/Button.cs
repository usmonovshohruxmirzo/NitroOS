using Cosmos.System;
using Cosmos.System.Graphics;
using Cosmos.System.Graphics.Fonts;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace NitroOS.UI.Taskbar
{
    public class Button
    {
        public int X, Y, Width, Height;
        public string Text;
        public Pen Background, TextColor;
        public Action OnClick;
        public int CornerRadius;

        public Button(int x, int y, int width, int height, string text, Pen bg, Pen fg, Action onClick, double cornerRadius = 0)
        {
            X = x; Y = y; Width = width; Height = height;
            Text = text; Background = bg; TextColor = fg; OnClick = onClick;
            CornerRadius = (int)cornerRadius;
        }

        public void Draw(Canvas canvas)
        {
            bool mouseOver = MouseManager.X >= X && MouseManager.X <= X + Width &&
                             MouseManager.Y >= Y && MouseManager.Y <= Y + Height;

            var bgPen = mouseOver ? new Pen(Color.FromArgb(180, Background.Color)) : Background;

            if (CornerRadius > 0)
            {
                DrawFullRounded(canvas, bgPen, X, Y, Width, Height, CornerRadius);
            }
            else
            {
                canvas.DrawFilledRectangle(bgPen, X, Y, Width, Height);
            }

            canvas.DrawString(Text, PCScreenFont.Default, TextColor,
                new Cosmos.System.Graphics.Point(X + 5, Y + (Height / 2 - 8)));
        }

        private void DrawFullRounded(Canvas canvas, Pen pen, int x, int y, int width, int height, int radius)
        {
            if (radius > height / 2) radius = height / 2;

            canvas.DrawFilledRectangle(pen, x + radius, y, width - 2 * radius, height);

            canvas.DrawFilledCircle(pen, x + radius, y + height / 2, radius);

            canvas.DrawFilledCircle(pen, x + width - radius, y + height / 2, radius);
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
