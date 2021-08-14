

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using System.IO;


using System.Diagnostics;

using System.Runtime.InteropServices;



namespace face_detection
{
    public partial class FACE_DETECTİON_FORM : Form
    {
        Image<Bgr, Byte> currentFrame;
        Capture camera;
        HaarCascade face_1;

        MCvFont font = new MCvFont(FONT.CV_FONT_HERSHEY_TRIPLEX, 0.5d, 0.5d);
        Image<Gray, byte> result, Trainedface_1 = null;
        Image<Gray, byte> gray = null;
        List<Image<Gray, byte>> image_get = new List<Image<Gray, byte>>();
        List<string> face_numbers_1 = new List<string>();
        List<string> users_name = new List<string>();
        int face_c, t;
        string name, users_1 = null;

        [StructLayout(LayoutKind.Sequential)]

        private struct KeyboardDLLStruct
        {
            public Keys key;
            public int scakeyboard_code;
            public int flags;
            public int time;
            public IntPtr extra;
        }

        private delegate IntPtr LowLevelKeyboardProc(int keyboard_code, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]

        private static extern IntPtr SetWindowsHookEx(int id, LowLevelKeyboardProc callback, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]

        private static extern bool UnhookWindowsHookEx(IntPtr hook);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]

        private static extern IntPtr CallNextHookEx(IntPtr hook, int keyboard_code, IntPtr wp, IntPtr lp);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]

        private static extern IntPtr GetModuleHandle(string name);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]

        private static extern short GetAsyncKeyState(Keys key);

        private IntPtr p_t;

        private LowLevelKeyboardProc obj;
        public FACE_DETECTİON_FORM()
        {
            InitializeComponent();
            //MessageBox.Show(Application.StartupPath);

            ProcessModule objCurrentModule = Process.GetCurrentProcess().MainModule;

            obj = new LowLevelKeyboardProc(key_dect);

            p_t = SetWindowsHookEx(13, obj, GetModuleHandle(objCurrentModule.ModuleName), 0);



            face_1 = new HaarCascade("haarcascade_frontalface_default.xml");
            try
            {
                try
                {
                    string file_Path = "C:" + "\\pictures\\";

                    if (Directory.Exists(file_Path) == false)
                    {
                        Directory.CreateDirectory("C:" + "\\pictures\\");
                    }
                }
                catch (Exception)
                { }

                string[] pictures = Directory.GetFiles("C:" + "\\pictures\\");
                DirectoryInfo d = new DirectoryInfo("C:" + "\\pictures\\");
                FileInfo[] file;
                file = d.GetFiles("*.bmp");
                int p = 0;
                List<string> resm = new List<string>();
                foreach (FileInfo f in file)
                {
                    resm.Add(f.Name);
                }
                foreach (string m in pictures)
                {
                    string e = resm[p];
                    p++;
                    e = e.Substring(0, e.Length - 4);
                    image_get.Add(new Image<Gray, byte>(m));
                    face_numbers_1.Add(e);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Lütfen 1 den fazla database kayıdı eklemeyiniz.Yüz kaydı eklemek için camera başlat butonuna basınız.", "Tanımlı Yüz Mevcut", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

        }

        void kontrol_2()
        {
            if (label4.Text != "yuz" && label4.Text != "")
            {
                timer2.Enabled = false;
                timer1.Enabled = false;
                timer3.Enabled = false;
                timer4.Enabled = false;

                MessageBox.Show(label4.Text + " hoşgeldiniz...");
                Environment.Exit(0);
            }
        }

        private IntPtr key_dect(int keyboard_code, IntPtr wp, IntPtr lp)
        {
            if (keyboard_code >= 0)
            {
                KeyboardDLLStruct tus = (KeyboardDLLStruct)Marshal.PtrToStructure(lp, typeof(KeyboardDLLStruct));
                if (tus.key == Keys.RWin || tus.key == Keys.LWin
                    || tus.key == Keys.Alt || tus.key == Keys.Tab || tus.key == Keys.Escape)
                {
                    return (IntPtr)1;
                }
            }
            return CallNextHookEx(p_t, keyboard_code, wp, lp);
        }

        void control()
        {
            textBox1.Location = new Point(Screen.PrimaryScreen.Bounds.Width / 2 - textBox1.Width / 2 - 62,
                Screen.PrimaryScreen.Bounds.Height / 2 - textBox1.Height / 2);
            button3_1.Location = new Point(textBox1.Location.X + 365, textBox1.Location.Y);
            button8.Location = new Point(textBox1.Location.X + 470, textBox1.Location.Y);


            try
            {
                string path = "C:" + "\\pictures\\";
                if (Directory.Exists(path))
                {
                    string[] pictures = Directory.GetFiles("C:" + "\\pictures\\");
                    if (pictures.Length == 0)
                    {
                        DialogResult r = new DialogResult();
                        r = MessageBox.Show("Algılanacak herhangi bir resim bulunamadı.\nProgramdan çıkılsınmı ? ", "Bilgi", MessageBoxButtons.YesNo);
                        if (r == DialogResult.No)
                        {
                            //imageBox1.Visible = true;
                            face_detection_section.Visible = true;
                            diagnosis();
                        }
                        else
                        {
                            MessageBox.Show("Programdan çıkılıyor...", "Bilgi", MessageBoxButtons.OK);
                            Environment.Exit(0);
                        }
                    }
                    else
                    {
                        DialogResult dialog = new DialogResult();
                        dialog = MessageBox.Show("Yeni bir kullanıcı eklemek istermisiniz?", "Seçim", MessageBoxButtons.YesNo);
                        if (dialog == DialogResult.Yes)
                        {
                            face_detection_section.Visible = true;
                            diagnosis();
                        }
                        else
                        {
                            face_detection_section.Visible = true;


                            groupBox2.Visible = false;
                            this.FormBorderStyle = FormBorderStyle.None;
                            face_detection_section.Location = new Point(0, 0);
                            this.Location = new Point(0, 0);
                            this.Width = Screen.PrimaryScreen.Bounds.Width;
                            face_detection_section.Width = Screen.PrimaryScreen.Bounds.Width;
                            face_detection_section.Height = Screen.PrimaryScreen.Bounds.Height;
                            this.Height = Screen.PrimaryScreen.Bounds.Height;
                            // imageBox1.Width = 100;
                            // imageBox1.Height = 100;
                            // imageBox1.Location = new Point(Screen.PrimaryScreen.Bounds.Width - 100, Screen.PrimaryScreen.Bounds.Height - 100);

                            button1_Click(button1, new EventArgs());

                            timer2.Enabled = true;

                        }
                    }

                }

                else
                {
                    Directory.CreateDirectory(path);
                    string[] pictures = Directory.GetFiles("C:" + "\\pictures\\");
                    if (pictures.Length == 0)
                    {
                        DialogResult r = new DialogResult();
                        r = MessageBox.Show("Algılanacak herhangi bir resim bulunamadı.\nProgramdan çıkılsınmı ? ", "Bilgi", MessageBoxButtons.YesNo);
                        if (r == DialogResult.No)
                        {
                            //imageBox1.Visible = true;
                            face_detection_section.Visible = true;
                            diagnosis();
                        }
                        else
                        {
                            MessageBox.Show("Programdan çıkılıyor...", "Bilgi", MessageBoxButtons.OK);
                            Environment.Exit(0);
                        }
                    }
                    else
                    {
                        DialogResult d = new DialogResult();
                        d = MessageBox.Show("Yeni bir kullanıcı eklemek istermisiniz?", "Seçim", MessageBoxButtons.YesNo);
                        if (d == DialogResult.Yes)
                        {
                            face_detection_section.Visible = true;
                            diagnosis();
                        }
                        else
                        {
                            face_detection_section.Visible = true;

                            groupBox2.Visible = false;
                            this.FormBorderStyle = FormBorderStyle.None;
                            face_detection_section.Location = new Point(0, 0);
                            this.Location = new Point(0, 0);
                            this.Width = Screen.PrimaryScreen.Bounds.Width;
                            face_detection_section.Width = Screen.PrimaryScreen.Bounds.Width;
                            face_detection_section.Height = Screen.PrimaryScreen.Bounds.Height;
                            this.Height = Screen.PrimaryScreen.Bounds.Height;
                           
                            // imageBox1.Width = 100;
                            // imageBox1.Height = 100;
                            // imageBox1.Location = new Point(Screen.PrimaryScreen.Bounds.Width - 100, Screen.PrimaryScreen.Bounds.Height - 100);

                            button1_Click(button1, new EventArgs());
                            timer2.Enabled = true;
                        }
                    }
                }
            }
            catch (Exception)
            { }
        }

        void diagnosis()
        {
            groupBox2.Visible = false;
            this.FormBorderStyle = FormBorderStyle.None;
            face_detection_section.Location = new Point(0, 0);
            this.Location = new Point(0, 0);
            this.Width = Screen.PrimaryScreen.Bounds.Width;
            face_detection_section.Width = Screen.PrimaryScreen.Bounds.Width;
            face_detection_section.Height = Screen.PrimaryScreen.Bounds.Height;
            this.Height = Screen.PrimaryScreen.Bounds.Height;
            // imageBox1.Width = 100;
            // imageBox1.Height = 100;
            // imageBox1.Location = new Point(Screen.PrimaryScreen.Bounds.Width - 100, Screen.PrimaryScreen.Bounds.Height - 100);

            button1_Click(button1, new EventArgs());

            timer1.Enabled = true;

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            int q = Convert.ToInt16(label3.Text);
            if (q == 1)
            {
                timer3.Enabled = false;
                timer1.Enabled = false;
                timer4.Enabled = false;
                timer2.Enabled = false;

                face_detection_section.BackColor = Color.White;
                MessageBox.Show("Algılandı.");
                //save();
                //imageBox1.Visible = false;
                face_detection_section.Visible = false;
                this.BackColor = Color.Black;
                textBox1.Visible = true;
                button3_1.Visible = true;
                button8.Visible = true;

            }
        }

        void save()
        {
            try
            {
                face_c = face_c + 1;
                gray = camera.QueryGrayFrame().Resize(320, 240, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
                MCvAvgComp[][] detect_face = gray.DetectHaarCascade(
                face_1,
                1.2,
                10,
                Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.DO_CANNY_PRUNING,
                new Size(20, 20));
                foreach (MCvAvgComp f in detect_face[0])
                {
                    Trainedface_1 = currentFrame.Copy(f.rect).Convert<Gray, byte>();
                    break;
                }
                Trainedface_1 = result.Resize(100, 100, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
                image_get.Add(Trainedface_1);
                face_numbers_1.Add(textBox1.Text);
                int i;
                // isim_al();
                for (i = 1; i <= image_get.ToArray().Length; i++)
                {
                    image_get.ToArray()[i - 1].Save("C:" + "\\pictures\\" + textBox1.Text + ".bmp");
                }
                MessageBox.Show(textBox1.Text + " yüzü tanımlandı.", "Tanımlama başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch
            {
                MessageBox.Show("Öncelikle Yüz tanımlamayı başlatınız.", "Tanımlama Başarısız", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox1.Text == "Lütfen bir isim giriniz...")
            {
                MessageBox.Show("Lütfen bir isim giriniz.");
            }
            else
            {
                save();
                Application.Exit();
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            /*if (textBox1.Text == "")
            {
                textBox1.Text = "Lütfen bir isim giriniz...";
            }*/

        }

        private void textBox1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "Lütfen bir isim giriniz...")
            {
                textBox1.Text = "";
            }
        }

        private void textBox1_MouseLeave(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                textBox1.Text = "Lütfen bir isim giriniz...";
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (textBox1.Text == "Lütfen bir isim giriniz...")
            {
                textBox1.Text = "";
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            kontrol_2();
        }

        private void YUZ_ALGILAMA_FORM_Deactivate(object sender, EventArgs e)
        {

        }

        private void YUZ_ALGILAMA_FORM_Leave(object sender, EventArgs e)
        {

        }

        int color_code = 230;
        bool stage = false;
        private void timer3_Tick(object sender, EventArgs e)
        {
            Color q = new Color();
            q = Color.FromArgb(color_code, color_code, color_code);
            face_detection_section.BackColor = q;
            if (stage == false)
            {
                if (color_code == 255)
                {
                    stage = true;
                }
                else
                {
                    color_code++;
                }
            }
            else
            {
                if (color_code == 200)
                {
                    stage = false;
                }
                else
                {
                    color_code--;
                }
            }


        }

        private void YUZ_ALGILAMA_FORM_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void YUZ_ALGILAMA_FORM_Load(object sender, EventArgs e)
        {
            this.Visible = false;
            timer3.Enabled = true;
            control();
            // timer4.Enabled = true;
        }

        private void YUZ_ALGILAMA_FORM_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            camera = new Capture();

            camera.QueryFrame();
            Application.Idle += new EventHandler(Framecamera);
            button1.Enabled = false;
        }

        private void timer4_Tick(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, System.EventArgs e)
        {
            try
            {
                face_c = face_c + 1;
                gray = camera.QueryGrayFrame().Resize(320, 240, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
                MCvAvgComp[][] detect_face = gray.DetectHaarCascade(
                face_1,
                1.2,
                10,
                Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.DO_CANNY_PRUNING,
                new Size(20, 20));
                foreach (MCvAvgComp f in detect_face[0])
                {
                    Trainedface_1 = currentFrame.Copy(f.rect).Convert<Gray, byte>();
                    break;
                }
                Trainedface_1 = result.Resize(100, 100, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
                image_get.Add(Trainedface_1);
                face_numbers_1.Add(textBox1.Text);
                int i;
                for (i = 2; i < image_get.ToArray().Length; i++)
                {
                    image_get.ToArray()[i - 1].Save("C:/" + "/pictures/" + textBox1.Text + ".bmp");
                }

                MessageBox.Show(textBox1.Text + " yüzü tanımlandı.", "Tanımlama başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);

                try
                {
                    Environment.Exit(0);
                }
                catch (Exception)
                { }


            }
            catch
            {
                MessageBox.Show("Öncelikle Yüz tanımlamayı başlatınız.", "Tanımlama başarısız", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }


        void Framecamera(object sender, EventArgs e)
        {
            label3.Text = "0";
            users_name.Add("");

            currentFrame = camera.QueryFrame().Resize(320, 240, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);

            gray = currentFrame.Convert<Gray, Byte>();
            MCvAvgComp[][] detect_face = gray.DetectHaarCascade(
          face_1,
          1.2,
          10,
          Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.DO_CANNY_PRUNING,
          new Size(20, 20));
            foreach (MCvAvgComp f in detect_face[0])
            {
                t = t + 1;
                result = currentFrame.Copy(f.rect).Convert<Gray, byte>().Resize(100, 100, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
                currentFrame.Draw(f.rect, new Bgr(Color.Red), 2);


                if (image_get.ToArray().Length != 0)
                {
                    MCvTermCriteria termCrit = new MCvTermCriteria(face_c, 0.001);
                    transactions recognizer = new transactions(
                            image_get.ToArray(),
                            face_numbers_1.ToArray(),
                            3000,
                            ref termCrit);

                    name = recognizer.Recognize(result);
                    currentFrame.Draw(name, ref font, new Point(f.rect.X - 2, f.rect.Y - 2), new Bgr(Color.LightGreen));

                }
                users_name[t - 1] = name;
                users_name.Add("");
                label3.Text = detect_face[0].Length.ToString();
            }
            t = 0;
            for (int nnn = 0; nnn < detect_face[0].Length; nnn++)
            {
                users_1 = users_1 + users_name[nnn] + " , ";
            }
            face_detection_section.Image = currentFrame;
            label4.Text = users_1;
            users_1 = "";
            users_name.Clear();

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }












    }
}
