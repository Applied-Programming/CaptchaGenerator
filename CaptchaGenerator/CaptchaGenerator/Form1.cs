using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.IO;

namespace CaptchaGenerator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        List<string> Strings = new List<string>();

        private void button2_Click(object sender, EventArgs e)
        {
            Image[] captcha_images = GenerateCaptchas(Convert.ToInt32(textBox1.Text));
            int g = 0;
            foreach (Image i in captcha_images )
            {
                i.Save(label1.Text + "\\" + Strings[g] + ".png");
                g++;
            }
        }

        Image[] GenerateCaptchas(int amount)
        {
            Image[] captcha_images = new Image[amount];
            Random rand = new Random();
            for (int z = 0; z < amount; z++)
            {
                Bitmap b = new Bitmap(panel1.Width, panel1.Height);
                Graphics g = Graphics.FromImage(b);
                //Graphics g = panel1.CreateGraphics();
                g.Clear(panel1.BackColor);
                SolidBrush sb = new SolidBrush(Color.FromArgb(0xFF, rand.Next(0, 255), rand.Next(0, 255), rand.Next(0, 255)));
                Pen p = new Pen(Color.FromArgb(0xFF, rand.Next(0, 255), rand.Next(0, 255), rand.Next(0, 255)));
                char[] chars = "qwertyuiopasdfghjklzxcvbnm1234567890".ToCharArray();
                string randomString = "";
                for (int i = 0; i < 6; i++)
                {
                    randomString += chars[rand.Next(0, 35)];
                }

                byte[] buff = new byte[randomString.Length];
                int y = 0;
                foreach (char ch in randomString.ToCharArray())
                {
                    buff[y] = (byte)ch;
                    y++;
                }
                //Hashing
                MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
                string md5str = BitConverter.ToString(md5.ComputeHash(buff)).Replace("-", "");
                Strings.Add(md5str);

                FontFamily ff = new FontFamily("Arial");
                Font f = new System.Drawing.Font(ff, 14);
                g.DrawString(randomString, f, sb, 30, 60);
                for (int i = 0; i < 6; i++)
                {
                    int j = rand.Next(0, 2);
                    if (j == 0)
                        g.DrawRectangle(p, rand.Next(0, 111), rand.Next(0, 60), rand.Next(0, 111), rand.Next(0, 60));
                    else if (j == 1)
                        g.DrawEllipse(p, rand.Next(0, 111), rand.Next(0, 60), rand.Next(0, 111), rand.Next(0, 60));
                    p.Color = Color.FromArgb(0xFF, rand.Next(0, 255), rand.Next(0, 255), rand.Next(0, 255));
                }
                panel1.BackgroundImage = b;
                captcha_images[z] = b;
            }
            return captcha_images;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd_obj = new FolderBrowserDialog();
            if (fbd_obj.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                label1.Text = fbd_obj.SelectedPath;
            }
        }

        string md5_Hashes_Name = "";

        private void button4_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd_obj = new OpenFileDialog();
            if (ofd_obj.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                pictureBox1.ImageLocation = ofd_obj.FileName;
                md5_Hashes_Name = Path.GetFileNameWithoutExtension(ofd_obj.FileName);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MD5CryptoServiceProvider md5_obj = new MD5CryptoServiceProvider();
            int y = 0;
            byte[] buff = new byte[textBox2.Text.Length];
            foreach (char ch in textBox2.Text.ToCharArray())
            {
                buff[y] = (byte)ch;
                y++;
            }
            string str = BitConverter.ToString(md5_obj.ComputeHash(buff)).Replace("-", "");
            if (str != md5_Hashes_Name)
            {
                MessageBox.Show("You got it wrong! :| ");
            }
            else
            {
                MessageBox.Show("You got it right! :D ");
            }
        }
    }
}
