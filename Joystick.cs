using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.DirectX.DirectInput;

namespace JoyMouse
{
    public class Joystick : Form
    {
        //Initialize objects
        Device dev;
        DeviceInstance devi;
        DeviceList devlist;
        JoystickState joy;
        //Initialize data types
        Object thisLock = new Object();
        bool detected = false;
        bool active = true;
        bool ck = true;
        //bool viewfinder = false;
        int axis = 0;
        int button = 0;
        int[] buttons = new int[4];
        int nbuttons = 0;
        int pressbutton = 0;
        //User32 DLL
        private const int MOUSEEVENTF_LEFTDOWN = 0X02;
        private const int MOUSEEVENTF_LEFTUP = 0X04;
        private const int MOUSEEVENTF_RIGHTDOWN = 0X08;
        private const int MOUSEEVENTF_RIGHTUP = 0X10;
        [DllImport("User32")]
        public static extern void mouse_event(long dwFlags, long dx, long dy, long cButtons, long dwExtraInfo);

        public Joystick()
        {
            //Constructor
        }

        /*
        public Device Joystick()
        {
            //Constructor that return device object
        }
        */
        public bool JoyConfigure()
        {
            bool request = false;
            joy = new JoystickState();
            devlist = Manager.GetDevices(DeviceClass.GameControl, EnumDevicesFlags.AttachedOnly);
            if (devlist.Count >= 1)
            {
                try
                {
                    devlist.MoveNext();
                    devi = (DeviceInstance)devlist.Current;
                    dev = new Device(devi.ProductGuid);
                    dev.SetCooperativeLevel(new JoyMouse(), CooperativeLevelFlags.Background | CooperativeLevelFlags.NonExclusive);
                    dev.SetDataFormat(DeviceDataFormat.Joystick);
                    dev.Acquire();
                    this.nbuttons = dev.Caps.NumberButtons;
                    request = true;
                }
                catch (Exception ex)
                {
                    request = false;
                }
            }
            return request;
        }

        public void JoyCapabilities()
        {
            axis = dev.Caps.NumberAxes;
            button = dev.Caps.NumberButtons;
        }
        public bool DetectJoy()
        {
            bool request = false;
            if (detected ? request = true : request = false) { }
            return request;
        }

        public void JoyStart()
        {
            JoyConfigure();
            JoyCapabilities();
            ReadPapers r = new ReadPapers();
            bool calc = true;
            string[] tempbuttons = r.ReadConfigureFile("c:\\debgbtn.txt");
            string[] screen = r.ReadConfigureFile("c:\\debgscreen.txt");
            string[] joytype = r.ReadConfigureFile("c:\\debgjtype.txt");
            int wx = (Convert.ToInt32(screen[0].ToString()));
            int hy = Convert.ToInt32(screen[1].ToString());
            //Coeficient Points for mouse movements in resolution
            int jx = wx;
            int jy = hy;
            //Actual points for joystick
            int px = 0;
            int py = 0;
            int pjx = 0;
            int pjy = 0;
            int pju = 0;
            int pjd = 0;
            int pjl = 0;
            int pjr = 0;
            //Coeficient Points for slow mouse movement
            int tempx = 0;
            int tempy = 0;
            //Decrapted
            int wjx = 0;
            int hjy = 0;
            //Other variables
            int[] buttons = new int[4];
            int pressbutton = 0;
            for (int count = 0; count <= 3; count++)
            {
                try
                {
                    buttons[count] = Convert.ToInt32(tempbuttons[count]);
                }
                catch (Exception ex)
                {
                }
            }
            this.buttons = buttons;
            int ax = joy.X, bx = joy.X / 2, cx = joy.X * 2;
            while (active)
            {
                try
                {
                    if (calc)
                    {
                        dev.Poll();
                        joy = dev.CurrentJoystickState;
                        pjx = joy.X;
                        pjy = joy.Y;
                        px = joy.X;
                        py = joy.Y;
                        jx = joy.X / wx;
                        jy = joy.Y / hy;
                        //jx = wx;
                        //jy = hy;
                        tempx = joy.X / jx;
                        wjx = jx;
                        hjy = jy;
                    }
                }
                catch (Exception ex)
                {
                }
                JoyDetectClick();
                if( this.pressbutton == buttons[0])
                {
                    lock (thisLock)
                    {
                        if (calc)
                        {
                            for (int count = 0; count <= 4; count++)
                            {
                                dev.Poll();
                                joy = dev.CurrentJoystickState;
                                switch (count)
                                {
                                    case 0:
                                        MessageBox.Show("Please mainten direcional" +
                                            "controler centralized and press OK", "Atention", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                                        pjy = joy.Y;
                                        pjx = joy.X;
                                        break;
                                    case 1:
                                        MessageBox.Show("Please mainten direcional" +
                                            "controler pressed to up and press OK", "Atention", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                                        pju = joy.Y;
                                        break;
                                    case 2:
                                        MessageBox.Show("Please mainten direcional" +
                                            "controler pressed to down and press OK", "Atention", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                                        pjd = joy.Y;
                                        break;
                                    case 3:
                                        MessageBox.Show("Please mainten direcional" +
                                            "controler pressed to left and press OK", "Atention", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                                        pjl = joy.X;
                                        break;
                                    case 4:
                                        MessageBox.Show("Please mainten direcional" +
                                            "controler pressed to right and press OK", "Atention", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                                        pjr = joy.X;
                                        break;
                                }
                            }
                             calc = false;
                        }
                        if (joytype[0] == "3")
                        {
                            Cursor.Position = new Point((joy.X / jx)-300, (joy.Y / jy)-300);
                        }
                        else
                        {
                            if (joy.X == pjl || joy.X <= (pjl + 200))
                            {
                                tempx += 5;
                            }
                            else if (joy.X == pjr || joy.X <= pjl - 200) ;
                            {
                                tempx -= 5;
                            }
                            if (joy.Y < pjy)
                            {
                                //tempy += 5;
                            }
                            if (joy.Y > pjy)
                            {
                                //tempy -= 5;

                            }
                            Cursor.Position = new Point(tempx, tempy);
                        }
                        Thread.Sleep(10);
                    }
                }
                lock (thisLock)
                {
                    if (this.pressbutton == buttons[1])
                    {
                                mouse_event(MOUSEEVENTF_RIGHTDOWN | MOUSEEVENTF_RIGHTUP, 0, 0, 0, 0);
                                Thread.Sleep(300);
                    }
                }
                lock(thisLock)
                {
                    if (this.pressbutton == buttons[2])
                    {
                            mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
                            Thread.Sleep(300);
                    }
                }
                lock (thisLock)
                {
                    if (this.pressbutton == buttons[3])
                    {
                        mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
                        mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
                        Thread.Sleep(300);
                    }
                }
            }
        }

        public void JoyDetectClick()
        {
            bool active = true;
            byte[] buttons;
            while (active)
            {
                try
                {
                    dev.Poll();
                    joy = dev.CurrentJoystickState;
                }
                catch (Exception ex)
                {
                }
                    buttons = joy.GetButtons();
                    for (int count = 0; count <= (JoyGetButtons()-1); count++)
                    {
                        if (buttons[count] >= 128)
                        {
                            this.pressbutton = count;
                            count = JoyGetButtons();
                        }
                        else
                            this.pressbutton = -1;
                        active = false;
                    }
            }
        }

        public void JoyPressButton(int btn)
        {

        }
        //Set and Get
        public int JoyGetButtons()
        {
            return this.nbuttons;
        }

        public void JoySetPressButton(int button)
        {
            this.pressbutton = button;
        }

        public int JoyGetPressButton()
        {
            return this.pressbutton;
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // Joystick
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(292, 273);
            this.Name = "Joystick";
            this.Load += new System.EventHandler(this.Joystick_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void Joystick_Load(object sender, EventArgs e)
        {

        }
    }
}
