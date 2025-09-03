using Cosmos.System;
using Cosmos.System.Graphics;
using Cosmos.System.Graphics.Fonts;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace NitroOS.UI
{
    public class Window
    {
        public int X, Y, Width, Height;
        public Pen Background;
        public string Title;
        public bool IsOpen;
        public List<Cosmos.System.Graphics.Point> PaintPixels = new List<Cosmos.System.Graphics.Point>(); // store relative positions

        public Action<Canvas, Window> DrawContent;

        private bool isDragging = false;
        private int dragOffsetX, dragOffsetY;
        private int closeSize = 15;

        public Window(int x, int y, int width, int height, Pen bg, string title, Action<Canvas, Window> drawContent = null)
        {
            X = x; Y = y; Width = width; Height = height;
            Background = bg; Title = title; IsOpen = true;
            DrawContent = drawContent;
        }

        public void Draw(Canvas canvas)
        {
            if (!IsOpen) return;

            canvas.DrawFilledRectangle(Background, X, Y, Width, Height);

            canvas.DrawFilledRectangle(new Pen(Color.DarkGray), X, Y, Width, 20);
            canvas.DrawString(Title, PCScreenFont.Default, new Pen(Color.White), new Cosmos.System.Graphics.Point(X + 5, Y + 3));

            canvas.DrawFilledRectangle(new Pen(Color.Red), X + Width - closeSize - 5, Y + 2, closeSize, closeSize);
            canvas.DrawString("X", PCScreenFont.Default, new Pen(Color.White), new Cosmos.System.Graphics.Point(X + Width - closeSize, Y + 2));

            foreach (var p in PaintPixels)
                canvas.DrawFilledRectangle(new Pen(Color.Black), X + p.X, Y + 20 + p.Y, 4, 4);

            DrawContent?.Invoke(canvas, this);
        }

        public void Update()
        {
            if (!IsOpen) return;

            int mouseX = (int)MouseManager.X;
            int mouseY = (int)MouseManager.Y;

            if (MouseManager.MouseState == MouseState.Left &&
                mouseX >= X + Width - closeSize - 5 && mouseX <= X + Width - 5 &&
                mouseY >= Y + 2 && mouseY <= Y + 2 + closeSize)
            {
                IsOpen = false;
                return;
            }

            bool mouseOverTitle = mouseX >= X && mouseX <= X + Width && mouseY >= Y && mouseY <= Y + 20;
            if (MouseManager.MouseState == MouseState.Left && mouseOverTitle && !isDragging)
            {
                isDragging = true;
                dragOffsetX = mouseX - X;
                dragOffsetY = mouseY - Y;
            }
            if (isDragging)
            {
                X = mouseX - dragOffsetX;
                Y = mouseY - dragOffsetY;
            }
            if (MouseManager.MouseState != MouseState.Left)
                isDragging = false;
        }
    }
}
