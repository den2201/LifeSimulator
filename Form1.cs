using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace LifeSimulator
{
    public partial class Form1 : Form
    {
        Graphics graphics;
        private int line;
        int fieldRows;
        int fieldColums;
        private bool [,] IsFieldAlive;
        private int resolution;
        public Form1()
        {
            InitializeComponent();
        }

        private void Init()
        {
            
            resolution = (int)nmcResolution.Value;
            fieldRows = pictureBox1.Height / resolution;
            fieldColums = pictureBox1.Width / resolution;
            IsFieldAlive = new bool[fieldColums, fieldRows];
            Random random = new Random();
            for(int x = 0; x < fieldColums; x++)
            {
                for(int y = 0; y < fieldRows; y++)
                {
                    IsFieldAlive[x,y] = random.Next((int)nmcDencity.Value) == 0;
                }
            }
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            graphics = Graphics.FromImage(pictureBox1.Image);
            timer1.Start();
        }

        private void Nextgeneration()
        {
            graphics.Clear(Color.Black);
            var newField = new bool[fieldColums, fieldRows];
            for (int x = 0; x < fieldColums; x++)
            {
                for (int y = 0; y < fieldRows; y++)
                {
                    var countNeighbours = CountNeighbours(x, y);

                    var isAlife = IsFieldAlive[x, y];

                    if (!isAlife && (countNeighbours == 3))
                    {
                        newField[x, y] = true;
                    }
                    else if (isAlife && (countNeighbours < 2 || countNeighbours > 3))
                    {
                        newField[x, y] = false;
                    }
                    else
                    {
                        newField[x, y] = IsFieldAlive[x, y];
                    }

                    if(isAlife)
                    {
                        graphics.FillRectangle(Brushes.Crimson, x * resolution, y * resolution, resolution, resolution);
                    }
                }
            }
            IsFieldAlive = newField;
            pictureBox1.Refresh();
        }

        private int  CountNeighbours(int x, int y)
        {
            int count = 0;
            for( int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    var col = (x + i + fieldColums) % fieldColums;
                    var row = (y + j + fieldRows) % fieldRows;
                    var isSelfChecking = col == x && row == y;
                    var hasLife = IsFieldAlive[col, row];
                    if (hasLife && !isSelfChecking)
                    {
                        count++;
                    }
                }
            }
            return count;
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            Nextgeneration();
        }

        private void BtnStart_Click(object sender, EventArgs e)
        {
            if (timer1.Enabled)
                return;
            nmcResolution.Enabled = false;
            nmcDencity.Enabled = false;
            Init();
            
        }

        private void BtnStop_Click(object sender, EventArgs e)
        {
            if (!timer1.Enabled)
                return;
            timer1.Stop();
            nmcResolution.Enabled = true;
            nmcDencity.Enabled = true;
        }

        private void NmcResolution_ValueChanged(object sender, EventArgs e)
        {
            line = (int)nmcResolution.Value;
        }

        private void PictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
           if(e.Button == MouseButtons.Left)
            {
                var x = e.Location.X / resolution;
                var y = e.Location.Y / resolution;
                IsFieldAlive[x, y] = true;
            }

            if (e.Button == MouseButtons.Right)
            {
                var x = e.Location.X / resolution;
                var y = e.Location.Y / resolution;
                IsFieldAlive[x, y] = false;
            }
        }
    }
}
