using Cosmos.System;
using Cosmos.System.Graphics;
using Cosmos.System.Graphics.Fonts;
using NitroOS.App;
using NitroOS.App.Games;
using NitroOS.UI;
using NitroOS.UI.Taskbar;
using System;
using System.Drawing;
using Sys = Cosmos.System;

namespace NitroOS
{
    public class Kernel : Sys.Kernel
    {
        private Canvas canvas;
        private Pen primaryPen;
        private Pen cursor;

        private TopBar topBar;
        private Dock bottomDock;
        private Menu menu;

        protected override void BeforeRun()
        {
            canvas = FullScreenCanvas.GetFullScreenCanvas(new Mode(1024, 768, ColorDepth.ColorDepth32));
            canvas.Clear(Color.White);

            primaryPen = new Pen(Color.FromArgb(0xFF, 0xFC, 0x48, 0x50));
            cursor = new Pen(Color.Black);

            AppsPanel appsCenter = null;

            var calcButton = new TaskbarButton(400, 768 - 70 - 10, 50, 50, "Calc", new Pen(Color.White), primaryPen,
                            () => appsCenter.AddWindow(new CalculatorApp(250, 200, 250, 250).CalcWindow));

            var paintButton = new TaskbarButton(460, 768 - 70 - 10, 50, 50, "Paint", new Pen(Color.White), primaryPen,
                () => appsCenter.AddWindow(new PaintApp(250, 200, 400, 300).PaintWindow));

            var numberGuesserButton = new TaskbarButton(520, 768 - 70 - 10, 50, 50, "Guess", new Pen(Color.White), primaryPen,
                () => appsCenter.AddWindow(new NumberGuesserGame(250, 200, 400, 300).GameWindow));

            appsCenter = new AppsPanel(400, 768 - 70 + 20, 120, 50, new TaskbarButton[] { calcButton, paintButton, numberGuesserButton });

            var shutdownItem = new TaskbarButton(0, 0, 180, 35, "Shutdown", new Pen(Color.Gray), new Pen(Color.White), () =>
            {
                Power.Shutdown();
                menu.IsOpen = false;
            });

            var settingsItem = new TaskbarButton(0, 0, 180, 35, "Settings", new Pen(Color.Gray), new Pen(Color.White), () =>
            {
                appsCenter.AddWindow(new Window(200, 150, 400, 300, new Pen(Color.DarkCyan), "Settings"));
                menu.IsOpen = false;
            });

            var wallpaperItem = new TaskbarButton(0, 0, 180, 35, "Wallpapers", new Pen(Color.Gray), new Pen(Color.White), () =>
            {
                var wallpaperApp = new Wallpapers(250, 200, 500, 400);
                appsCenter.AddWindow(wallpaperApp.WallpapersWindow);
                menu.IsOpen = false;
            });

            menu = new Menu(10, 768 - 70, 200, primaryPen, new TaskbarButton[] { shutdownItem, settingsItem, wallpaperItem });

            var menuButton = new TaskbarButton(10, 768 - 70 + 15, 100, 40, "Nitro OS", new Pen(Color.White), primaryPen,
                () => { menu.IsOpen = !menu.IsOpen; });

            topBar = new TopBar(1024, 40, primaryPen);

            int dockWidth = 400;
            int dockHeight = 70;
            int dockX = (1024 - dockWidth) / 2;

            bottomDock = new Dock(dockX, dockWidth, dockHeight, primaryPen, new TaskbarButton[] { menuButton }, appsCenter);

            MouseManager.ScreenWidth = 1024;
            MouseManager.ScreenHeight = 768;
        }

        protected override void Run()
        {
            canvas.DrawFilledRectangle(new Pen(Color.White), 0, 0, 1024, 768);

            topBar.Draw(canvas);
            topBar.CheckClicks();

            int dockY = 768 - 20;
            bottomDock.Draw(canvas, dockY);
            bottomDock.CheckClicks();

            if (bottomDock.AppsCenter != null)
            {
                bottomDock.AppsCenter.Update(canvas);
                bottomDock.AppsCenter.Draw(canvas);
            }

            if (menu.IsOpen)
            {
                int menuHeight = 120;
                canvas.DrawFilledRectangle(new Pen(Color.FromArgb(200, 0xFC, 0x48, 0x50)), menu.X, menu.Y - menuHeight, menu.Width, menuHeight);

                menu.Draw(canvas);

                menu.CheckClicks();
            }

            int mouseX = (int)MouseManager.X;
            int mouseY = (int)MouseManager.Y;
            canvas.DrawFilledRectangle(cursor, mouseX, mouseY, 15, 15);

            canvas.Display();
        }
    }
}