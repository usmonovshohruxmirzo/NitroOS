using Cosmos.System.Graphics;
using Cosmos.System.Graphics.Fonts;
using System;
using System.Drawing;

namespace NitroOS.UI.Taskbar
{
    public class CalendarApp
    {
        public int X, Y, Width, Height;
        public Pen Background;
        public Pen TextColor;
        public bool IsOpen;

        private int cellSize = 30;
        private int padding = 10;
        private DateTime currentMonth;

        public CalendarApp(int x, int y, Pen bg, Pen textColor)
        {
            X = x;
            Y = y;
            Background = bg;
            TextColor = textColor;
            IsOpen = false;
            currentMonth = DateTime.Now;
            Width = 7 * cellSize + padding * 2;
            Height = 6 * cellSize + padding * 2;
        }

        public void Draw(Canvas canvas)
        {
            if (!IsOpen) return;

            canvas.DrawFilledRectangle(Background, X, Y, Width, Height);

            Font font = PCScreenFont.Default;

            int dayCounter = 1;
            int daysInMonth = DateTime.DaysInMonth(currentMonth.Year, currentMonth.Month);
            int startDay = (int)(new DateTime(currentMonth.Year, currentMonth.Month, 1).DayOfWeek);

            for (int week = 0; week < 6; week++)
            {
                for (int day = 0; day < 7; day++)
                {
                    int cellX = X + padding + day * cellSize;
                    int cellY = Y + padding + week * cellSize;

                    if (week == 0 && day < startDay) continue;
                    if (dayCounter > daysInMonth) break;

                    DateTime today = DateTime.Now;
                    if (currentMonth.Year == today.Year &&
                        currentMonth.Month == today.Month &&
                        dayCounter == today.Day)
                    {
                        canvas.DrawFilledRectangle(new Pen(Color.Black), cellX, cellY, cellSize, cellSize);
                    }

                    canvas.DrawString(dayCounter.ToString(), font, TextColor, cellX + 5, cellY + 5);

                    dayCounter++;
                }
            }
        }
    }
}
