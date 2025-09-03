using Cosmos.System;
using Cosmos.System.Graphics;
using Cosmos.System.Graphics.Fonts;
using NitroOS.UI;
using NitroOS.UI.Taskbar;
using System;
using System.Drawing;
using Sys = Cosmos.System;
using NitroOS.App;

namespace NitroOS
{
    public class Kernel : Sys.Kernel
    {
        private Canvas canvas;
        private Pen primaryPen;
        private Pen cursor;
        private Taskbar taskbar;

        protected override void BeforeRun()
        {
            canvas = FullScreenCanvas.GetFullScreenCanvas(new Mode(1024, 768, ColorDepth.ColorDepth32));
            canvas.Clear(Color.White);

            primaryPen = new Pen(Color.FromArgb(0xFF, 0xFC, 0x48, 0x50));
            cursor = new Pen(Color.Black);

            // -------------------
            // Menu items
            // --------------------------
            AppsPanel appsCenter = null;

            var settingsItem = new TaskbarButton(0, 0, 180, 35, "Settings", new Pen(Color.Gray), new Pen(Color.White),
                () =>
                {
                    appsCenter.AddWindow(new Window(200, 150, 400, 300, new Pen(Color.DarkCyan), "Settings"));
                });

            var wallpaperItem = new TaskbarButton(0, 0, 180, 35, "Wallpapers", new Pen(Color.Gray), new Pen(Color.White),
              () =>
              {
                  appsCenter.AddWindow(new Window(200, 150, 400, 300, new Pen(Color.DarkCyan), "Wallpapers"));
              });

            var shutdownItem = new TaskbarButton(0, 0, 180, 35, "Shutdown", new Pen(Color.Gray), new Pen(Color.White),
              () => Power.Shutdown());

            // --------------------------
            // Menu (opens above taskbar)
            // --------------------------
            var menu = new Menu(10, 768 - 70, 200, primaryPen, new TaskbarButton[] { shutdownItem, settingsItem, wallpaperItem });

            // --------------------------
            // Left taskbar button
            // --------------------------
            var menuButton = new TaskbarButton(10, 768 - 70 + 15, 100, 40, "Nitro OS", new Pen(Color.White), primaryPen,
                () => { menu.IsOpen = !menu.IsOpen; });

            // --------------------------
            // Center apps (optional)
            // --------------------------
            var calcButton = new TaskbarButton(400, 768 - 70 + 10, 50, 50, "Calc",
                new Pen(Color.Green), new Pen(Color.White),
                () =>
                {
                    var calcApp = new CalculatorApp(250, 200, 250, 250);
                    appsCenter.AddWindow(calcApp.CalcWindow);
                });
            var paintButton = new TaskbarButton(460, 768 - 70 + 10, 50, 50, "Paint",
                new Pen(Color.Blue), new Pen(Color.White),
                () =>
                {
                    var paintApp = new PaintApp(250, 200, 400, 300);
                    appsCenter.AddWindow(paintApp.PaintWindow);
                });

            appsCenter = new AppsPanel(400, 768 - 70 + 10, 120, 50, new TaskbarButton[] { calcButton, paintButton });

            // --------------------------
            // Create taskbar
            // --------------------------
            taskbar = new Taskbar(1024, 768, 70, primaryPen, new TaskbarButton[] { menuButton }, new Menu[] { menu }, appsCenter);

            MouseManager.ScreenWidth = 1024;
            MouseManager.ScreenHeight = 768;
        }

        protected override void Run()
        {
            canvas.DrawFilledRectangle(new Pen(Color.White), 0, 0, 1024, 768);

            taskbar.Draw(canvas);
            taskbar.CheckClicks();

            taskbar.AppsCenter.Update(canvas);

            canvas.DrawFilledRectangle(cursor, (int)MouseManager.X, (int)MouseManager.Y, 15, 15);

            canvas.Display();
        }
    }
}
