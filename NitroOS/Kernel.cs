using Cosmos.System;
using Cosmos.System.Graphics;
using Cosmos.System.Graphics.Fonts;
using IL2CPU.API.Attribs;
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

        private TopBar topBar;
        private Dock bottomDock;
        private Menu menu;

        public static DateTime BootTime;
        private readonly uint screenWidth = 1920;
        private readonly uint screenHeight = 1080;

        [ManifestResourceStream(ResourceName = "NitroOS.Assets.background.bmp")]
        public static byte[] background_bmp;
        public static Bitmap background = new Bitmap(background_bmp);

        [ManifestResourceStream(ResourceName = "NitroOS.Assets.cursor.bmp")]
        public static byte[] cursor_bmp;
        public static Bitmap curs = new Bitmap(cursor_bmp);

        [ManifestResourceStream(ResourceName = "NitroOS.Assets.start.bmp")]
        public static byte[] start_bmp;
        public static Bitmap start = new Bitmap(start_bmp);

        protected override void BeforeRun()
        {
            canvas = FullScreenCanvas.GetFullScreenCanvas(new Mode((int)screenWidth, (int)screenHeight, ColorDepth.ColorDepth32));

            canvas.Clear(Color.White);

            primaryPen = new Pen(Color.FromArgb(0xFF, 0xFC, 0x48, 0x50));

            AppsPanel appsCenter = null;
            var calcButton = new TaskbarButton(400, (int)screenHeight - 70 - 10, 50, 50, "Calc",
                new Pen(Color.White), primaryPen,
                () => appsCenter.AddWindow(new CalculatorApp(250, 200, 250, 250).CalcWindow));

            var paintButton = new TaskbarButton(460, (int)screenHeight - 70 - 10, 50, 50, "Paint",
                new Pen(Color.White), primaryPen,
                () => appsCenter.AddWindow(new PaintApp(250, 200, 400, 300).PaintWindow));

            var numberGuesserButton = new TaskbarButton(520, (int)screenHeight - 70 - 10, 50, 50, "Guess",
                new Pen(Color.White), primaryPen,
                () => appsCenter.AddWindow(new NumberGuesserGame(250, 200, 400, 300).GameWindow));

            var sysInfoButton = new TaskbarButton(50, 60, 100, 100, "MY NITRO",
                primaryPen, new Pen(Color.White),
                () => appsCenter.AddWindow(new SystemInfoApp(250, 200, 400, 200).InfoWindow));

            appsCenter = new AppsPanel(400, (int)screenHeight - 70 + 20, 120, 50, new TaskbarButton[] { calcButton, paintButton, numberGuesserButton, sysInfoButton });

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

            menu = new Menu(10, (int)screenHeight - 70, 200, primaryPen, new TaskbarButton[] { shutdownItem, settingsItem, wallpaperItem });

            var menuButton = new TaskbarButton(10, (int)screenHeight - 70 + 15, 100, 40, "Nitro OS", new Pen(Color.White), primaryPen,
                () => { menu.IsOpen = !menu.IsOpen; });

            topBar = new TopBar((int)screenWidth, 40, primaryPen);

            int dockWidth = 1500;
            int dockHeight = 70;
            int dockX = (int)((screenWidth - dockWidth) / 2);

            bottomDock = new Dock(dockX, dockWidth, dockHeight, primaryPen, new TaskbarButton[] { menuButton }, appsCenter);

            MouseManager.ScreenWidth = screenWidth;
            MouseManager.ScreenHeight = screenHeight;
        }

        protected override void Run()
        {
            canvas.DrawFilledRectangle(new Pen(Color.White), 0, 0, (int)screenWidth, (int)screenHeight);
            canvas.DrawImage(background, 0, 0);
            topBar.Draw(canvas);
            topBar.CheckClicks();

            int dockY = (int)(screenHeight - 20);
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
            canvas.DrawImageAlpha(curs, mouseX, mouseY);

            canvas.Display();
        }
    }
}