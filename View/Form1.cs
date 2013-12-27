using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace View
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void queryButton_Click(object sender, EventArgs e)
        {
            Image image = Image.FromFile(@"D:\stock\store\chart\sh600749_2000_Weekly-BuySellShare.jpg");
            // Set the PictureBox image property to this image.
            // ... Then, adjust its height and width properties.
            pictureBox1.Image = image;
            //pictureBox1.Height = 200;
            //pictureBox1.Width = 300;
        }
    }
}
