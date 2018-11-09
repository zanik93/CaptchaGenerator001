using System;
using System.Drawing;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.Collections.Generic;
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
            Image[] images = GenerateCaptchas(Convert.ToInt32(textBox1.Text));
            int g = 0;
            foreach (Image i in images)
            {
                i.Save(label1.Text + "\\" + Strings[g] + ".png");

                g++;
            }
        }

        Image[] GenerateCaptchas(int amount)
        {
            Image[] images = new Image[amount];
            Random ran = new Random();
            for (int z = 0; z < amount; z++)
            {
                Bitmap bitmap = new Bitmap(panel1.Width, panel1.Height);
                Graphics g = Graphics.FromImage(bitmap);
                g.Clear(panel1.BackColor); // Clears the panel after each captcha
                SolidBrush b = new SolidBrush(Color.FromArgb(255, ran.Next(0, 255), ran.Next(0, 255), ran.Next(0, 255)));  // Generating a random color for the brush each time
                Pen p = new Pen(Color.FromArgb(255, ran.Next(0, 255), ran.Next(0, 255), ran.Next(0, 255)));
                char[] chars = "qwertyuiopasdfghjklzxcvbnm1234567890".ToCharArray();
                string randomString = "";
                for (int i = 0; i < 6; i++) // We put 6 random letters/numbers from the char array onto the string
                {
                    randomString += chars[(ran.Next(0, 35))];
                }
                byte[] buffer = new byte[randomString.Length];
                int y = 0;
                foreach (char c in randomString.ToCharArray())
                {
                    buffer[y] = (byte)c;
                    y++;
                }
                MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
                string md5string = BitConverter.ToString(md5.ComputeHash(buffer)).Replace("-", "");
                Strings.Add(md5string); // The picture name will be the captchas name in Md5 hash
                FontFamily ff = new FontFamily("Arial");
                Font f = new Font(ff, 16);
                g.DrawString(randomString, f, b, 80, 80);
                for (int i = 0; i < 6; i++) // Creating random rectangles, ellipses onto the captcha so its harder to read.
                {
                    int j = ran.Next(0, 2);
                    if (j == 0) g.DrawRectangle(p, ran.Next(0, 150), ran.Next(0, 155), ran.Next(0, 150), ran.Next(0, 155));
                    else if (j == 1) g.DrawEllipse(p, ran.Next(0, 150), ran.Next(0, 155), ran.Next(0, 150), ran.Next(0, 155));
                    p.Color = Color.FromArgb(255, ran.Next(0, 255), ran.Next(0, 255), ran.Next(0, 255)); // Resseting the color of the pen each time so it is not the same
                }
                panel1.BackgroundImage = bitmap;
                images[z] = bitmap;
            }
            return images;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if(fbd.ShowDialog() == DialogResult.OK)
            {
                label1.Text = fbd.SelectedPath;
            }
        }

        string md5HashesName = "";

        private void button4_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if(ofd.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.ImageLocation = ofd.FileName;
                md5HashesName = Path.GetFileNameWithoutExtension(ofd.FileName);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            int y = 0;
            byte[] buffer = new byte[textBox2.Text.Length];
            foreach (char c in textBox2.Text.ToCharArray())
            {
                buffer[y] = (byte)c;
                y++;
            }
            string md5HashCheck = BitConverter.ToString(buffer).Replace("-", "");
            if(md5HashCheck != md5HashesName)
            {
                MessageBox.Show("Wrong Captcha!");
            }
            else
            {
                MessageBox.Show("Captcha Correct!");
            }
        }
    }
}
