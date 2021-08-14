using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;

using System.Windows.Forms;
using AForge.Video;
using AForge.Video.DirectShow; 

namespace face_detection_1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        bool form_move = false;
        Point starting_move = new Point(0, 0);

        public static FilterInfoCollection camera;

        public static int m;
        private void Form1_Load(object sender, EventArgs e)
        {
            camera_refresh();
        }

       
        void camera_refresh()
        {
            comboBox1.Items.Clear();
            camera = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo devices in camera)
            {
                comboBox1.Items.Add(devices.Name);
            }
            if (comboBox1.Items.Count > 0)
            {
                comboBox1.SelectedIndex = 0; 
            }
                
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            form_move = true;
            starting_move = new Point(e.X, e.Y);
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            form_move = false;
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (form_move)
            {
                Point p = PointToScreen(e.Location);
                Location = new Point(p.X - this.starting_move.X, p.Y - this.starting_move.Y);
            }
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            form_move = true;
            starting_move = new Point(e.X, e.Y);
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (form_move)
            {
                Point p = PointToScreen(e.Location);
                Location = new Point(p.X - this.starting_move.X, p.Y - this.starting_move.Y);
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            form_move = false;
        }

        private void label1_MouseDown(object sender, MouseEventArgs e)
        {
            form_move = true;
            starting_move = new Point(e.X, e.Y);
        }

        private void label1_MouseMove(object sender, MouseEventArgs e)
        {
            if (form_move)
            {
                Point p = PointToScreen(e.Location);
                Location = new Point(p.X - this.starting_move.X, p.Y - this.starting_move.Y);
            }
        }

        private void label1_MouseUp(object sender, MouseEventArgs e)
        {
            form_move = false;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            try
            {
                Environment.Exit(0);
            }
            catch (Exception)
            {
                
            }
           
        }

        private void label2_MouseDown(object sender, MouseEventArgs e)
        {
            label2.ForeColor = Color.Lime;
        }

        private void label2_MouseLeave(object sender, EventArgs e)
        {
            label2.ForeColor = Color.White;
        }

        private void label2_MouseMove(object sender, MouseEventArgs e)
        {
            label2.ForeColor = Color.Tan;
        }

        private void label2_Click(object sender, EventArgs e)
        {
            if (comboBox1.Items.Count == 0 || comboBox1.SelectedItem == null)
            {
                MessageBox.Show("Lütfen kullanılacak bir kamera seçiniz.");
            }
            else
            {
                //m = comboBox1.SelectedIndex;
                
                this.Hide();

                face_detection.FACE_DETECTİON_FORM y = new face_detection.FACE_DETECTİON_FORM();
                y.Show();
            }
        }

        private void pictureBox2_MouseMove(object sender, MouseEventArgs e)
        {
            
        }

        private void pictureBox2_MouseLeave(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
           
        }

        private void label3_Click(object sender, EventArgs e)
        {
            camera_refresh();
        }

        private void label3_MouseDown(object sender, MouseEventArgs e)
        {
            label3.ForeColor = Color.Lime;
        }

        private void label3_MouseLeave(object sender, EventArgs e)
        {
            label3.ForeColor = Color.White;
        }

        private void label3_MouseMove(object sender, MouseEventArgs e)
        {
            label3.ForeColor = Color.Tan;
        }




    }
}
