using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace JoyMouse
{
    public class Screens : JoyMouse
    {
        //Datatypes
        private int width;
        private int height;
        
        public Screens(int width, int height)
        {
            this.width = width;
            this.height = height;
        }

        public void SetScreen(int width, int height)
        {
            this.width = width;
            this.height = height;
        }

        public int[] GetScreen()
        {
            int[] screen = {this.width, this.height};
            return screen;
        }

        public int[] Resolution()
        {
            WritePaper w = new WritePaper("c:\\debgscreen.txt");
            Screen[] screen = Screen.AllScreens;
            SetScreen( screen[0].Bounds.Width, height = screen[0].Bounds.Height);
            w.WriteScreen(this.GetScreen());
            return GetScreen();
        }
    }
}
