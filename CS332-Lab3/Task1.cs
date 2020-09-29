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
    public partial class Task1 : Form
    {
        Color borderColor = Color.Black;
        Color fillColor = Color.Red;
        bool isPressed = false;
        Point currentPoint;
        Point prevPoint;
        Graphics g;

        int clickX = 0, clickY = 0;

        bool isFilling = false;

        Bitmap drawArea;
        Bitmap pictureFillArea;

        bool isPictureFilling;
        public Task1()
        {
            InitializeComponent();

            pictureFillArea = (Bitmap)pictureBox1.Image;

            panel1.Image = null;
            panel1.Refresh();

            drawArea = new Bitmap(panel1.Size.Width, panel1.Size.Height);
            panel1.Image = drawArea;
            g = Graphics.FromImage(drawArea);
            SetBorders();
            isPictureFilling = false;
        }

        private void SetBorders()
        {
            Pen blackPen = new Pen(borderColor, 1);
            Rectangle rect = new Rectangle(1, 1, panel1.Width - 3, panel1.Height - 3);
            g.DrawRectangle(blackPen, rect);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult d = colorDialog1.ShowDialog();
            if (d == DialogResult.OK)
                fillColor = colorDialog1.Color;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            panel1.Image = null;
            panel1.Refresh();

            drawArea = new Bitmap(panel1.Size.Width, panel1.Size.Height);
            panel1.Image = drawArea;
            g = Graphics.FromImage(drawArea);

            SetBorders();

            isFilling = false;
            button3.Text = "Выбрать точку для заливки";

            isPictureFilling = false;
            button4.Text = "Заливка изображением";
        }

        private void Panel1_MouseClick(object sender, MouseEventArgs e)
        {
            if (isFilling)
            {
                clickX = Convert.ToInt32(e.X);
                clickY = Convert.ToInt32(e.Y);
                fill(Convert.ToInt32(e.X), Convert.ToInt32(e.Y));
                panel1.Image = drawArea;
            }
        }

        private void Panel1_MouseDown(object sender, MouseEventArgs e)
        {
            if (!isFilling)
            {
                isPressed = true;
                currentPoint = e.Location;
            }
        }

        private void Panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (!isFilling)
            {
                if (isPressed)
                {
                    prevPoint = currentPoint;
                    currentPoint = e.Location;
                    paintSimple();
                }
            }
        }

        private void paintSimple()
        {
            Pen p = new Pen(borderColor, 4);
            g.DrawLine(p, prevPoint, currentPoint);
            panel1.Image = drawArea;
        }

        private void Panel1_MouseUp(object sender, MouseEventArgs e)
        {
            isPressed = false;
        }

        private int lineFill(int x, int y, int dir, int prevXl, int prevXr)
        {
            int xl = x;
            int xr = x;
            Color c;

            if (!isPictureFilling)
            {
                do
                {
                    c = drawArea.GetPixel(--xl, y);
                } while ((c.ToArgb() != borderColor.ToArgb()) && (c.ToArgb() != fillColor.ToArgb()) && (xl > 0));

                do
                {
                    c = drawArea.GetPixel(++xr, y);
                } while ((c.ToArgb() != borderColor.ToArgb()) && (c.ToArgb() != fillColor.ToArgb()) && (xr < panel1.Width - 1));

            }
            else
            {
                do
                {
                    c = drawArea.GetPixel(--xl, y);
                } while ((c.ToArgb() != borderColor.ToArgb()) && (c.ToArgb() != pictureFillArea.GetPixel((xl - clickX + pictureBox1.Width) % pictureBox1.Width, (y - clickY + pictureBox1.Height) % pictureBox1.Height).ToArgb()) && (xl > 0));

                do
                {
                    c = drawArea.GetPixel(++xr, y);
                } while ((c.ToArgb() != borderColor.ToArgb()) && (c.ToArgb() != pictureFillArea.GetPixel((xr - clickX + pictureBox1.Width) % pictureBox1.Width, (y - clickY + pictureBox1.Height) % pictureBox1.Height).ToArgb()) && (xr < panel1.Width - 1));
            }

            xl++;
            xr--;

            if (!isPictureFilling)
            {
                Pen p = new Pen(fillColor);
                g.DrawLine(p, xl, y, xr, y);
            }
            else
            {
                for (int i = xl; i <= xr; i++)
                {

                    drawArea.SetPixel(i, y, pictureFillArea.GetPixel((i - clickX + pictureBox1.Width) % pictureBox1.Width, (y - clickY + pictureBox1.Height) % pictureBox1.Height));
                }
            }

            if (!isPictureFilling)
            {
                for (x = xl; x <= xr; x++)
                {
                    c = drawArea.GetPixel(x, y + dir);
                    if ((c.ToArgb() != borderColor.ToArgb()) && (c.ToArgb() != fillColor.ToArgb()) && (y + dir < panel1.Height - 1))
                    {
                        x = lineFill(x, y + dir, dir, xl, xr);
                    }
                }

                for (x = xl; x < prevXl; x++)
                {
                    c = drawArea.GetPixel(x, y - dir);
                    if ((c.ToArgb() != borderColor.ToArgb()) && (c.ToArgb() != fillColor.ToArgb()) && (y - dir > 0))
                    {
                        x = lineFill(x, y - dir, -dir, xl, xr);
                    }
                }

                for (x = prevXr; x < xr; x++)
                {
                    c = drawArea.GetPixel(x, y - dir);
                    if ((c.ToArgb() != borderColor.ToArgb()) && (c.ToArgb() != fillColor.ToArgb()) && (y - dir > 0))
                    {
                        x = lineFill(x, y - dir, -dir, xl, xr);
                    }
                }
            }
            else
            {
                for (x = xl; x <= xr; x++)
                {
                    c = drawArea.GetPixel(x, y + dir);
                    if ((c.ToArgb() != borderColor.ToArgb()) && (c.ToArgb() != pictureFillArea.GetPixel((x - clickX + pictureBox1.Width) % pictureBox1.Width, (y - clickY + pictureBox1.Height + dir) % pictureBox1.Height).ToArgb()) && (y + dir < panel1.Height - 1))
                    {
                        x = lineFill(x, y + dir, dir, xl, xr);
                    }
                }

                for (x = xl; x < prevXl; x++)
                {
                    c = drawArea.GetPixel(x, y - dir);
                    if ((c.ToArgb() != borderColor.ToArgb()) && (c.ToArgb() != pictureFillArea.GetPixel((x - clickX + pictureBox1.Width) % pictureBox1.Width, (y - clickY + pictureBox1.Height - dir) % pictureBox1.Height).ToArgb()) && (y - dir > 0))
                    {
                        x = lineFill(x, y - dir, -dir, xl, xr);
                    }
                }

                for (x = prevXr; x < xr; x++)
                {
                    c = drawArea.GetPixel(x, y - dir);
                    if ((c.ToArgb() != borderColor.ToArgb()) && (c.ToArgb() != pictureFillArea.GetPixel((x - clickX + pictureBox1.Width) % pictureBox1.Width, (y - clickY + pictureBox1.Height + dir) % pictureBox1.Height).ToArgb()) && (y - dir > 0))
                    {
                        x = lineFill(x, y - dir, -dir, xl, xr);
                    }
                }
            }
            return xr;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            isFilling = !isFilling;
            if (isFilling)
                button3.Text = "Рисовать далее";
            else
                button3.Text = "Выбрать точку для заливки";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            isPictureFilling = !isPictureFilling;
            if (isPictureFilling)
                button4.Text = "Заливка цветом";
            else
                button4.Text = "Заливка изображением";
        }
        private void fill(int x, int y)
        {
            lineFill(x, y, 1, x, x);
        }
    }
}
