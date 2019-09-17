using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using System;
using System.Collections.Generic;
namespace Snape
{
    public partial class Form1 : Form
    {
        Node apple;
        bool ISpause;
        Deque<Node> body;
        Random r = new Random();
        Thread drawLoopThread;
        Bitmap bmp;
        Graphics g;
        const int FieldSize = 50;
        const int InitSnakeSize = 4;
        int koef;
        List<Node> walls;
        List<Node> ways;
        List<Node> boda;

        public Form1()
        {
            InitializeComponent();
            InitializeDrawThread();
            body = new Deque<Node>();
            for (int i = 0; i < InitSnakeSize; i++) body.AddFirst(new Node(4, i+1));
            boda = new List<Node>();
            koef = Convert.ToInt32(pictureBox1.Height / FieldSize);

            walls = new List<Node>();
            for (int i = 0; i < FieldSize; i++)
            {
                walls.Add(new Node(0, i));
                walls.Add(new Node(FieldSize - 1, i));
                walls.Add(new Node(i, 0));
                walls.Add(new Node(i, FieldSize - 1));
            }
            ways = new List<Node>();
            putapple();
        }

        private void InitializeDrawThread()
        {
            ThreadStart threadStart = new ThreadStart(DrawLoop);
            drawLoopThread = new Thread(threadStart);
            drawLoopThread.IsBackground = true;
            drawLoopThread.Start();
        }
        public void DrawLoop()
        {
            while (true)
                if (ISpause)
                {
                    Thread.Sleep(33);
                    StepCalc();
                    StepDraw();
                }
        }




        void StepCalc()
        {
            AStar ass = new AStar();
            ass.Size = FieldSize;

            boda = new List<Node>();
            foreach (Node a in body)
                boda.Add(a);
            ways = new List<Node>();
            ways = ass.CalcPath(body.head.Data, apple, walls, boda);
            if (ways == null)
            {

                putapple();
                body.AddFirst(new Node(body.head.Data.X + 1, body.head.Data.Y));

                if (body.head.Data.X == apple.X && body.head.Data.Y == apple.Y)
                    putapple();
                else
                    body.RemoveLast();
            }
            else
            {
                if (ways.Count > 1)
                    body.AddFirst(ways[ways.Count - 2]); //new Node(body.head.Data.X + 1, body.head.Data.Y + dir.Y)

                if (body.head.Data.X == apple.X && body.head.Data.Y == apple.Y)
                    putapple();
                else
                    body.RemoveLast();
            }
        }

        void StepDraw()
        {
            bmp = DrawFilledRectangle(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(bmp);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;


            if (ways != null)
                for (int i = 0; i < ways.Count - 1; i++)
                    g.DrawLine(new Pen(Color.BlueViolet), ways[i].X * koef, ways[i].Y * koef, ways[i + 1].X * koef, ways[i + 1].Y * koef);


            g.FillEllipse(new SolidBrush(Color.Red), apple.X * koef, apple.Y * koef, koef, koef);

            foreach (Node a in body)
                g.FillRectangle(new SolidBrush(Color.Black), a.X * koef, a.Y * koef, koef, koef);

            g.FillRectangle(new SolidBrush(Color.White), body.head.Data.X * koef, body.head.Data.Y * koef, koef, koef);


            for (int i = 0; i < walls.Count; i++)
            {
                g.FillRectangle(new SolidBrush(Color.Cyan), walls[i].X * koef, walls[i].Y * koef, koef, koef);
            }
            pictureBox1.Image = bmp;
        }



        private Bitmap DrawFilledRectangle(int x, int y)
        {
            Bitmap bmp = new Bitmap(x, y);
            using (Graphics graph = Graphics.FromImage(bmp)) graph.FillRectangle(new SolidBrush(Color.WhiteSmoke), new Rectangle(0, 0, x, y));
            return bmp;
        }


        void putapple()
        {
        rep:

            apple = new Node(Convert.ToInt32(r.NextDouble() * (FieldSize - 1)), Convert.ToInt32(r.NextDouble() * (FieldSize - 1)));



            for (int i = 0; i < boda.Count; i++)
                if (apple.X == boda[i].X && apple.Y == boda[i].Y)
                    goto rep;


            for (int i = 0; i < walls.Count; i++)
                if (apple.X == walls[i].X && apple.Y == walls[i].Y)
                    goto rep;
        }
        void pause_state_change()
        {
            ISpause = !ISpause;
        }

        private void MouseClack(object sender, MouseEventArgs e)
        {
            int j = -1;
            bool fail = false;
            Point es = new Point(Convert.ToInt32(e.X / koef), Convert.ToInt32(e.Y / koef));
            for (int i = 0; i < walls.Count; i++)
                if (es.X == walls[i].X && es.Y == walls[i].Y)
                {
                    fail = true;
                    j = i;
                    break;
                }
            if (e.Button == MouseButtons.Left)
            {
                if (!fail) apple = new Node(es.X, es.Y);
            }
            if (e.Button == MouseButtons.Right)
            {
                if (fail) walls.RemoveAt(j);
                else walls.Add(new Node(es.X, es.Y));
            }
            StepDraw();
        }

        private void PPause(object sender, KeyEventArgs e)
        {
            if(e.KeyValue ==32)
            pause_state_change();
        }
    }
}