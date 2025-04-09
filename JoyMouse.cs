using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime;
using System.Threading;
using Microsoft.VisualBasic;

namespace JoyMouse
{
    public partial class JoyMouse : Form
    {
        Joystick joy;
        Thread t;
        bool request = false;
        bool[] steps = new bool[4];
        int[] screen = new int[2];
        //int[] buttons;
        public JoyMouse()
        {
            InitializeComponent();
        }

        private void JoyMouse_Load(object sender, EventArgs e)
        {
            ReadPapers r = new ReadPapers();
            if (r.FileExists("c:\\debgbtn.txt") & r.FileExists("c:\\debgscreen.txt") &
                r.FileExists("c:\\debgjtype.txt"))
            {
                btnOn.Enabled = true;
                joy = new Joystick();
            }
        }

        private void btnDtJ_Click(object sender, EventArgs e)
        {
            tabMain.Hide();
            tabConf.Show();
            tabControlMain.SelectedTab = tabConf;
        }

        private void btnS1_Click(object sender, EventArgs e)
        {
            joy = new Joystick();
            OpenProgram open = new OpenProgram(false, true, "C:\\WINDOWS\\system32\\joy.cpl", "", 60, "c:\\debgjoy.txt");
            if (open.GetOutput() == "false")
            {
                lblStatus.Text = "Failed : Erro=0";
                btnS2.Enabled = false;
            }
            else
            {
                picOkS1.Image = Properties.Resources.ok;
                request = joy.JoyConfigure();
                if (request)
                    request = joy.DetectJoy();
                if (request)
                    lblStatus.Text = "Sucess Joystick Attached";                    
                btnS2.Enabled = true;
            }
        }

        private void btnS2_Click(object sender, EventArgs e)
        {
            int[] resolution;
            int width = 0;
            int height = 0;
            DialogResult request = new DialogResult();
            Screens screen = new Screens( width , height );
            resolution = screen.Resolution();
            width = resolution[0];
            height = resolution[1];
            request = MessageBox.Show("Your resolution is " +
                width.ToString() + "x" + height.ToString() + ".",
                "Screen Capture", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if(request==DialogResult.OK){
                lblStatus.Text = "Success: " + width + "x" + height;
                picOkS2.Image = Properties.Resources.ok;
                btnS3.Enabled = true;
            }
            else
            {
                lblStatus.Text = "Capture failed, set manualy!";
                picOkS2.Image = Properties.Resources.no;
                btnS3.Enabled = false;
            }              
        }

        private void btnS3_Click(object sender, EventArgs e)
        {
            ConfigureButtons config = new ConfigureButtons(joy.JoyGetButtons());
            config.Wizzard();
            lblStatus.Text = "Success: Joystick buttons configured";
            picOkS3.Image = Properties.Resources.ok;
            btnS4.Enabled = true;
        }

        private void btnS4_Click(object sender, EventArgs e)
        {
            WritePaper w = new WritePaper("c:\\debgjtype.txt");
            DialogResult request = new DialogResult();
            while(request != DialogResult.Yes)
            {
                request = MessageBox.Show( "If your joystick is 2D select Yes !", "Select your joystick type", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2, MessageBoxOptions.DefaultDesktopOnly );
                if (request == DialogResult.No)
                {
                    request = MessageBox.Show("If your joystick is 3D selecte Yes!", "Select your joystick type", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2, MessageBoxOptions.DefaultDesktopOnly);
                    if (request == DialogResult.Yes)
                        w.WriteJoyType(3);
                }
                else
                {
                    w.WriteJoyType(2);
                }
            }
            request = MessageBox.Show("Complete, your joystick is now configured to run with mouse for this application!", "Complete", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            request = MessageBox.Show("How you do start use your joystick for movement your mouse?", "Start Application", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            lblStatus.Text = "Success: Joystick type configured";
            picOkS4.Image = Properties.Resources.ok;
            if (request == DialogResult.Yes)
            {
                tabConf.Hide();
                tabMain.Show();
                tabControlMain.SelectedTab = tabMain;
                btnOn.Enabled = false;
                btnOff.Enabled = true;
                t = new Thread(joy.JoyStart);
                t.Start();
            }
            else
            {
                tabConf.Hide();
                tabMain.Show();
                tabControlMain.SelectedTab = tabMain;
                btnOn.Enabled = true;
                btnOff.Enabled = false;
            }
        }

        private void btnOn_Click(object sender, EventArgs e)
        {
            t = new Thread(joy.JoyStart);
            t.Start();
            btnOn.Enabled = false;
            btnOff.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
        }

        private void timer1_Tick_1(object sender, EventArgs e)
        {

        }

        private void btnOff_Click(object sender, EventArgs e)
        {
            t.Abort();
            btnOff.Enabled = false;
            btnOn.Enabled = true;
        }
    }
}