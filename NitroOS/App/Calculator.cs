using Cosmos.System;
using Cosmos.System.Graphics;
using Cosmos.System.Graphics.Fonts;
using NitroOS.UI;
using System;
using System.Drawing;

namespace NitroOS.App
{
    internal class CalculatorApp
    {
        public Window CalcWindow;
        private string input = "";
        private string result = "";

        public CalculatorApp(int startX, int startY, int width, int height)
        {
            CalcWindow = new Window(
                startX, startY, width, height,
                new Pen(Color.LightGray),
                "Calculator",
                DrawCalcContent
            );
        }

        private void DrawCalcContent(Canvas canvas, Window win)
        {
            canvas.DrawFilledRectangle(new Pen(Color.White), win.X + 10, win.Y + 30, win.Width - 20, 30);
            canvas.DrawRectangle(new Pen(Color.Black), win.X + 10, win.Y + 30, win.Width - 20, 30);
            canvas.DrawString(input + result, PCScreenFont.Default, new Pen(Color.Black), new Cosmos.System.Graphics.Point(win.X + 15, win.Y + 35));

            string[] buttons = { "7", "8", "9", "/", "4", "5", "6", "*", "1", "2", "3", "-", "0", "C", "=", "+" };
            int btnWidth = 50, btnHeight = 30;
            int startX = win.X + 10, startY = win.Y + 70;

            for (int i = 0; i < buttons.Length; i++)
            {
                int col = i % 4;
                int row = i / 4;

                int btnX = startX + col * (btnWidth + 5);
                int btnY = startY + row * (btnHeight + 5);

                canvas.DrawFilledRectangle(new Pen(Color.Gray), btnX, btnY, btnWidth, btnHeight);
                canvas.DrawString(buttons[i], PCScreenFont.Default, new Pen(Color.White), new Cosmos.System.Graphics.Point(btnX + 15, btnY + 5));

                if (MouseManager.MouseState == MouseState.Left &&
                    MouseManager.X >= btnX && MouseManager.X <= btnX + btnWidth &&
                    MouseManager.Y >= btnY && MouseManager.Y <= btnY + btnHeight)
                {
                    HandleButton(buttons[i]);
                }
            }
        }

        private void HandleButton(string btn)
        {
            if (btn == "C")
            {
                input = "";
                result = "";
            }
            else if (btn == "=")
            {
                try
                {
                    result = " = " + Evaluate(input);
                }
                catch
                {
                    result = " Error";
                }
            }
            else
            {
                input += btn;
                result = "";
            }
        }

        private string Evaluate(string expression)
        {
            double current = 0;
            double lastNumber = 0;
            char lastOperator = '+';

            for (int i = 0; i < expression.Length; i++)
            {
                char c = expression[i];

                if (char.IsDigit(c))
                {
                    int numStart = i;
                    while (i < expression.Length && (char.IsDigit(expression[i]) || expression[i] == '.')) i++;
                    double number = double.Parse(expression.Substring(numStart, i - numStart));
                    i--;

                    switch (lastOperator)
                    {
                        case '+': current += number; break;
                        case '-': current -= number; break;
                        case '*': current *= number; break;
                        case '/': current /= number; break;
                    }
                }
                else
                {
                    lastOperator = c;
                }
            }
            return current.ToString();
        }
    }
}
