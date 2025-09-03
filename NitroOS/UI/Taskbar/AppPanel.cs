using Cosmos.System.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NitroOS.UI.Taskbar
{
    public class AppsPanel
    {
        public int X, Y, Width, Height;
        public TaskbarButton[] Apps;
        public List<Window> OpenWindows = new List<Window>();

        public AppsPanel(int x, int y, int width, int height, TaskbarButton[] apps)
        {
            X = x; Y = y; Width = width; Height = height;
            Apps = apps;
        }

        public void Draw(Canvas canvas)
        {
            foreach (var app in Apps)
                app.Draw(canvas);

            foreach (var win in OpenWindows)
                win.Draw(canvas);
        }

        public void Update(Canvas canvas)
        {
            foreach (var win in OpenWindows)
            {
                win.Update();
            }

            OpenWindows.RemoveAll(w => !w.IsOpen);
        }


        public void CheckClicks()
        {
            foreach (var app in Apps)
                app.CheckClick();

            OpenWindows.RemoveAll(w => !w.IsOpen);
        }

        public void AddWindow(Window window)
        {
            OpenWindows.Add(window);
        }
    }
}
