using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CS332_Lab3
{
    public partial class Task3 : Form
    {

        Bitmap bmp;

        public Task3()
        {
            InitializeComponent();
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);        
        }

        void DrawBrezenhaim(int x_1, int y_1, int x_2, int y_2)
        {
            Graphics g = Graphics.FromImage(pictureBox1.Image);
 
            if (x_1 > x_2)
            {
                int t = y_1;
                y_1 = y_2;
                y_2 = t;
                t = x_1;
                x_1 = x_2;
                x_2 = t;
            }
            int dx = x_2 - x_1;
            int dy = y_2 - y_1;
            int xi = x_1;
            int yi = y_1;
            int step = 1;
            int di = 2 * dy - dx;
            //1 и 4 четверти
            if (dx == 0 || Math.Abs(dy / (double)dx) > 1)
            {
                //4 четверть
                if (dy / (double)dx < 0)
                {
                    xi = x_2;
                    step = -1;
                    dy = -dy;
                    int t = y_1;
                    y_1 = y_2;
                    y_2 = t;
                }
                for (yi = y_1; yi <= y_2; yi++)
                {
                    bmp.SetPixel(xi, yi, Color.Black);
                    if (di >= 0)
                    {
                        xi += step;
                        di += 2 * (dx - dy);
                    }
                    else
                    {
                        di += 2 * dx;
                    }
                }
            }
            else
            {
                if (dy / (double)dx < 0)
                {
                    step = -1;
                    dy = -dy;
                }
                for (xi = x_1; xi <= x_2; xi++)
                {
                    bmp.SetPixel(xi, yi, Color.Black);
                    if (di >= 0)
                    {
                        yi += step;
                        di += 2 * (dy - dx);
                    }
                    else
                    {
                        di += 2 * dy;
                    }
                }
            }
        }

        void DrawWu(int x1, int y1, int x2, int y2)
        {
            if (x1 > x2)
            {
                int t = y1;
                y1 = y2;
                y2 = t;
                t = x1;
                x1 = x2;
                x2 = t;
            }
            int dx = x2 - x1;
            int dy = y2 - y1;
            double gradient;
            if (dx == 0)
            {
                gradient = 1;
            }
            else
            {
                gradient = dy / (double)dx;
            }
            int step = 1;
            double xi = x1;
            double yi = y1;

            //1 и 4 четверти
            if (Math.Abs(gradient) > 1)
            {
                gradient = 1 / gradient;
                //4 четверть
                if (gradient < 0)
                {
                    xi = x2;
                    step = -1;
                    int t = y1;
                    y1 = y2;
                    y2 = t;
                }

                for (yi = y1; yi <= y2; yi += 1)
                {
                    int help;
                    if ( gradient < 0)
                    {
                        help = (int)(255 * (xi - (int)xi));
                    }
                    else
                    {
                        help = 255 - (int)(255 * (xi - (int)xi));
                    }
                    bmp.SetPixel((int)xi, (int)yi, Color.FromArgb(255 - help, 255 - help, 255 - help));
                    bmp.SetPixel((int)xi + step, (int)yi, Color.FromArgb(help, help, help));
                    xi += gradient;
                }
            }
            else
            {
                //3 четверть
                if (gradient < 0)
                {
                    step = -1;
                }
                for (xi = x1; xi <= x2; xi += 1)
                {
                    int help;
                    if (gradient < 0)
                    {
                        help = (int)(255 * (yi - (int)yi));
                    }
                    else
                    {
                        help = 255 - (int)(255 * (yi - (int)yi));
                    }
                    bmp.SetPixel((int)xi, (int)yi, Color.FromArgb(255 - help, 255 - help, 255 - help));
                    bmp.SetPixel((int)xi, (int)yi + step, Color.FromArgb(help, help, help));
                    yi += gradient;
                }
            }
        }

        private void Task3_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            int wdth = pictureBox1.Width;
            int hght = pictureBox1.Height;
            bmp = new Bitmap(wdth, hght);
            pictureBox1.Image = bmp;
            var x_1 = int.Parse(textBox1.Text);
            var y_1 = int.Parse(textBox2.Text);

            var x_2 = int.Parse(textBox3.Text);
            var y_2 = int.Parse(textBox4.Text);
            DrawBrezenhaim(x_1, y_1, x_2, y_2);
            DrawWu(x_1 + 10, y_1 + 10, x_2 + 10, y_2 + 10);
            //DrawBrezenhaim(93, 185, 156, 20);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
