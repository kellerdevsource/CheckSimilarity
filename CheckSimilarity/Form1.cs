using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Numerics;

namespace CheckSimilarity
{
    public partial class Form1 : Form
    {
        public Vector3[] Figure1Data;
        public Vector3[] Figure2Data;
        public Form1()
        {
            InitializeComponent();
            Figure1Data = new Vector3[5];
            Figure1Data = new Vector3[5];
            this.Height = 720;
            this.Width = 720;
           // this.Close();
        }
        private void Form1_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            Pen redPen = new Pen(Color.Red, 1);
            Pen greenPen = new Pen(Color.Green, 1);
            Point p1 = new Point(Top);
            Point p2 = new Point(Top);
            for (int i = 0; i < Figure1Data.Length - 1; i++)
            {
                p1.X = (int)Figure1Data[i].X + 360;
                p1.Y = (int)Figure1Data[i].Y + 360;
                p2.X = (int)Figure1Data[i + 1].X + 360;
                p2.Y = (int)Figure1Data[i + 1].Y+360;
                e.Graphics.DrawLine(redPen, p1, p2);
            }
            p1.X = (int)Figure1Data[Figure1Data.Length - 1].X + 360;
            p1.Y = (int)Figure1Data[Figure1Data.Length - 1].Y + 360;
            p2.X = (int)Figure1Data[0].X + 360;
            p2.Y = (int)Figure1Data[0].Y + 360;
            e.Graphics.DrawLine(redPen, p1, p2);
            for (int i = 0; i < Figure2Data.Length - 1; i++)
            {
                p1.X = (int)Figure2Data[i].X + 360;
                p1.Y = (int)Figure2Data[i].Y + 360;
                p2.X = (int)Figure2Data[i + 1].X + 360;
                p2.Y = (int)Figure2Data[i + 1].Y + 360;
                e.Graphics.DrawLine(greenPen, p1, p2);
            }
            p1.X = (int)Figure2Data[Figure2Data.Length - 1].X + 360;
            p1.Y = (int)Figure2Data[Figure2Data.Length - 1].Y + 360;
            p2.X = (int)Figure2Data[0].X + 360;
            p2.Y = (int)Figure2Data[0].Y + 360;
            e.Graphics.DrawLine(greenPen, p1, p2);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
