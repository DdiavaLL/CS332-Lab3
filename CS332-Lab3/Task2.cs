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
    public partial class Task2 : Form
    {
        public Task2()
        {
            InitializeComponent();
            bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(bmp);
            pictureBox1.Image = bmp;
        }

        Graphics g;
        int sizePen = 1;
        Point mouse;
        Bitmap bmp;

        //рисуем
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            mouse = e.Location;
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                g.DrawLine(new Pen(Color.Black, sizePen), mouse, e.Location);
                mouse = e.Location;
                pictureBox1.Refresh();
            }
        }
        
        //Начать
        private void button2_Click(object sender, EventArgs e)
        {
            foreach (var x in GetBorderPoints(bmp))
                bmp.SetPixel(x.X, x.Y, Color.Red);
            pictureBox1.Refresh();
        }

        //Очистить
        private void button1_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
                pictureBox1.Image = null;
            bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(bmp);
            pictureBox1.Image = bmp;
        }

        //Поиск начальной точки
        private Point FindStartPoint(Bitmap sourceImage)
        {
            Color back_color = bmp.GetPixel(bmp.Width-1,0);
            Color cur_color = back_color;
            
            for (var x = bmp.Width - 1; x >= 0  ; x--) //x
                for (var y = 0; y < bmp.Height - 1; y++) //y
                    if (cur_color == back_color)
                    {
                        cur_color = bmp.GetPixel(x,y++);
                    }
                    else return new Point(x,y); 

            return new Point(0,0);
        }

        //Функция формирования списка граничных точек
        private List<Point> GetBorderPoints(Bitmap bmp_img)
        {
            List<Point> border = new List<Point>();
            Point cur = FindStartPoint(bmp_img);
            border.Add(cur);
            Point start = cur;
            Point next = cur;
            Color borderColor = bmp_img.GetPixel(cur.X, cur.Y);

            //Будем идти против часовой стрелки и ходить внутри области
            int dir = 8;
            do
            {
                dir += dir - 1 < 0 ? 7 : -2; // поворот на 90 градусов
                int t = dir;
                do
                {
                    next = cur;
                    switch (dir)
                    {
                        case 0: next.X++; break;
                        case 1: next.X++; next.Y--; break;
                        case 2: next.Y--;  break;
                        case 3: next.X--; next.Y--; break;
                        case 4: next.X--;  break;
                        case 5: next.X--; next.Y++; break;
                        case 6: next.Y++; break;
                        case 7: next.X++; next.Y++; break;
                    }
                    //Если не нашли - останавливаемся
                    if (next == start)
                        break;
                    if (bmp_img.GetPixel(next.X, next.Y) == borderColor)
                    {
                        //Кладем в список
                        border.Add(next);
                        cur = next;
                        //cur_dir = pred_Dir;
                        break;
                    }
                    dir = (dir + 1) % 8;
                } while (dir != t);
            } while (next != start);

            return border;
        }

    }
}
