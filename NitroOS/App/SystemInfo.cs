using Cosmos.Core;
using Cosmos.System;
using Cosmos.System.Graphics;
using Cosmos.System.Graphics.Fonts;
using NitroOS.UI;
using System;
using System.Drawing;

namespace NitroOS.App
{
    public class SystemInfoApp
    {
        public Window InfoWindow;

        public SystemInfoApp(int x, int y, int w, int h)
        {
            InfoWindow = new Window(x, y, w, h,
                new Pen(Color.LightBlue), "System Info", DrawContent);
        }

        private void DrawContent(Canvas canvas, Window win)
        {
            int offsetY = 40;
            var black = new Pen(Color.Black);

            canvas.DrawString("NitroOS System Information", PCScreenFont.Default, black,
                new Cosmos.System.Graphics.Point(win.X + 10, win.Y + offsetY));
            offsetY += 20;

            var uptime = DateTime.Now - Kernel.BootTime;
            canvas.DrawString($"Uptime: {uptime:hh\\:mm\\:ss}",
                PCScreenFont.Default, black,
                new Cosmos.System.Graphics.Point(win.X + 10, win.Y + offsetY));
            offsetY += 20;

            canvas.DrawString($"Total RAM: {CPU.GetAmountOfRAM()} MB",
                PCScreenFont.Default, black,
                new Cosmos.System.Graphics.Point(win.X + 10, win.Y + offsetY));
            offsetY += 20;

            canvas.DrawString("CPU: x86 (CosmosVM)", PCScreenFont.Default, black,
                new Cosmos.System.Graphics.Point(win.X + 10, win.Y + offsetY));
            offsetY += 20;

            canvas.DrawString("Used RAM: Not supported", PCScreenFont.Default, black,
                new Cosmos.System.Graphics.Point(win.X + 10, win.Y + offsetY));
        }
    }
}
