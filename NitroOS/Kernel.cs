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
using System.Globalization;
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
        private CalendarApp calendar;

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
            var calcButton = new Button(400, (int)screenHeight - 70 - 10, 50, 50, "Calc",
                new Pen(Color.White), primaryPen,
                () => appsCenter.AddWindow(new CalculatorApp(250, 200, 250, 250).CalcWindow), 25);

            var paintButton = new Button(460, (int)screenHeight - 70 - 10, 50, 50, "Paint",
                new Pen(Color.White), primaryPen,
                () => appsCenter.AddWindow(new PaintApp(250, 200, 400, 300).PaintWindow), 25);

            var numberGuesserButton = new Button(520, (int)screenHeight - 70 - 10, 50, 50, "Guess",
                new Pen(Color.White), primaryPen,
                () => appsCenter.AddWindow(new NumberGuesserGame(250, 200, 400, 300).GameWindow), 25);

            var sysInfoButton = new Button(580, (int)screenHeight - 70 - 10, 50, 50, "Info",
                new Pen(Color.White), primaryPen,
                () => appsCenter.AddWindow(new SystemInfoApp(250, 200, 400, 200).InfoWindow), 25);

            appsCenter = new AppsPanel(400, (int)screenHeight - 70 + 20, 120, 50, new Button[] { calcButton, paintButton, numberGuesserButton, sysInfoButton });

            var shutdownItem = new Button(0, 0, 180, 35, "Shutdown", new Pen(Color.Gray), new Pen(Color.White), () =>
            {
                Power.Shutdown();
                menu.IsOpen = false;
            });

            var settingsItem = new Button(0, 0, 180, 35, "Settings", new Pen(Color.Gray), new Pen(Color.White), () =>
            {
                appsCenter.AddWindow(new Window(200, 150, 400, 300, new Pen(Color.DarkCyan), "Settings"));
                menu.IsOpen = false;
            });

            var wallpaperItem = new Button(0, 0, 180, 35, "Wallpapers", new Pen(Color.Gray), new Pen(Color.White), () =>
            {
                var wallpaperApp = new Wallpapers(250, 200, 500, 400);
                appsCenter.AddWindow(wallpaperApp.WallpapersWindow);
                menu.IsOpen = false;
            });

            menu = new Menu(20, (int)screenHeight - 100, 200, primaryPen, new Button[] { shutdownItem, settingsItem, wallpaperItem });

            var menuButton = new Button(20, (int)screenHeight - 90, 100, 70, "Nitro OS", primaryPen, new Pen(Color.White),
                () => { menu.IsOpen = !menu.IsOpen; }, 33.8);

            calendar = new CalendarApp((int)screenWidth - 250, (int)screenHeight - 300, primaryPen, new Pen(Color.White));

            var calendarButton = new Button(
                (int)screenWidth - 120,
                (int)screenHeight - 90,
                100,
                70,
                DateTime.Now.ToString("M/d/yyyy"),
                primaryPen,
                new Pen(Color.White),
                () =>
                {
                    calendar.IsOpen = !calendar.IsOpen;
                    menu.IsOpen = false;
                },
                33.8
            );

            topBar = new TopBar((int)screenWidth, 40, primaryPen);

            int dockWidth = 1500;
            int dockHeight = 70;
            int dockX = (int)((screenWidth - dockWidth) / 2);

            bottomDock = new Dock(dockX, dockWidth, dockHeight, primaryPen, new Button[] { menuButton, calendarButton }, appsCenter);

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

            calendar.Draw(canvas);

            int mouseX = (int)MouseManager.X;
            int mouseY = (int)MouseManager.Y;
            canvas.DrawImageAlpha(curs, mouseX, mouseY);

            canvas.Display();
        }
    }
}