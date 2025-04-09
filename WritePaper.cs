using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
namespace JoyMouse
{
    public class WritePaper
    {
        StreamWriter w;
        string file = "";
        public WritePaper(string file)
        {
            this.file = file;
        }

        public bool WriteButtonsConfig(int[] btn)
        {
            bool request = false;
            try
            {
                w = new StreamWriter(file, false, Encoding.UTF8);
                int tcount = 0;
                for(int count = 0; count <= (btn.Length-1); count++)
                {
                    w.Write(btn[count].ToString() + "&");
                }
                w.Close();
                request = true;
            }
            catch (Exception ex)
            {
                request = false;
            }
            return request;
        }

        public bool WriteScreen(int[] resolution)
        {
            bool request = false;
            try
            {
                int width = resolution[0];
                int height = resolution[1];
                w = new StreamWriter(file, false, Encoding.UTF8);
                w.WriteLine(width + "&" + height);
                w.Close();
                request = true;
            }
            catch(Exception ex)
            {
                request = false;
            }
            return request;
        }

        public bool WriteJoyType(int type)
        {
            bool request = false;
            try
            {
                w = new StreamWriter(this.file, false, Encoding.UTF8);
                w.WriteLine(type);
                w.Close();
                request = true;
            }
            catch (Exception ex)
            {
                request = false;
            }
            return request;
        }
    }
}
