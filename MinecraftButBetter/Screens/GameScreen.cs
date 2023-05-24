using MinecraftButBetter.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MinecraftButBetter.Datatypes;
using MinecraftButBetter.WorldStuff;

namespace MinecraftButBetter.Screens
{
    public partial class GameScreen : UserControl
    {
        Camera camera;
        List<PointD3> pointD3s = new List<PointD3>();
        List<Block> blocks = new List<Block>();
        bool[] pressedKeys = new bool[6];
        public GameScreen()
        {
            InitializeComponent();
            camera = new Camera(-3, 2, 4, 0, 90, 90, 0, 0);
            //for (int x = 0; x < 10; x++)
            //{
            //    for (int z = 0; z < 10; z++)
            //    {
            //        Block b = new Block(x, 0, z);

            //        blocks.Add(b);

            //    }
            //}

            Block b = new Block(0, 0, 0);
            blocks.Add(b);

            pointD3s.Add(new PointD3(0, 0, 0));
            pointD3s.Add(new PointD3(1, 0, 0));

            gameTimer.Start();
        }

        private void GameScreen_Paint(object sender, PaintEventArgs e)
        {
            SolidBrush black = new SolidBrush(Color.Black);
            Pen blackPen = new Pen(Color.Black);
            foreach (Block b in blocks)
            {
                PointF[] pointsFs = new PointF[8];
                for (int i = 0; i < 8; i++)
                {
                    PointF p = camera.pointToScreen(b.points[i]);
                    pointsFs[i] = p;
                    Rectangle r = new Rectangle(multiplierToCoords(p), new Size(5, 5));
                    e.Graphics.FillEllipse(black, r);
                }

                int[,] edges = b.getFace(FaceIndex.BACK).getEdges();

                for(int i = 0; i < 4; i++)
                {
                    e.Graphics.DrawLine(blackPen, multiplierToCoords(pointsFs[edges[i,0]]), multiplierToCoords(pointsFs[edges[i, 1]]));
                }
                #region 
                //e.Graphics.DrawLine(blackPen, multiplierToCoords(pointsFs[0]), multiplierToCoords(pointsFs[1]));
                //e.Graphics.DrawLine(blackPen, multiplierToCoords(pointsFs[0]), multiplierToCoords(pointsFs[2]));
                //e.Graphics.DrawLine(blackPen, multiplierToCoords(pointsFs[1]), multiplierToCoords(pointsFs[3]));
                //e.Graphics.DrawLine(blackPen, multiplierToCoords(pointsFs[2]), multiplierToCoords(pointsFs[3])); // z-

                //e.Graphics.DrawLine(blackPen, multiplierToCoords(pointsFs[4]), multiplierToCoords(pointsFs[5]));
                //e.Graphics.DrawLine(blackPen, multiplierToCoords(pointsFs[4]), multiplierToCoords(pointsFs[6]));
                //e.Graphics.DrawLine(blackPen, multiplierToCoords(pointsFs[5]), multiplierToCoords(pointsFs[7]));
                //e.Graphics.DrawLine(blackPen, multiplierToCoords(pointsFs[6]), multiplierToCoords(pointsFs[7])); // z+

                //e.Graphics.DrawLine(blackPen, multiplierToCoords(pointsFs[0]), multiplierToCoords(pointsFs[2]));
                //e.Graphics.DrawLine(blackPen, multiplierToCoords(pointsFs[0]), multiplierToCoords(pointsFs[4]));
                //e.Graphics.DrawLine(blackPen, multiplierToCoords(pointsFs[2]), multiplierToCoords(pointsFs[6]));
                //e.Graphics.DrawLine(blackPen, multiplierToCoords(pointsFs[4]), multiplierToCoords(pointsFs[6])); // x-

                //e.Graphics.DrawLine(blackPen, multiplierToCoords(pointsFs[1]), multiplierToCoords(pointsFs[3]));
                //e.Graphics.DrawLine(blackPen, multiplierToCoords(pointsFs[1]), multiplierToCoords(pointsFs[5]));
                //e.Graphics.DrawLine(blackPen, multiplierToCoords(pointsFs[3]), multiplierToCoords(pointsFs[7]));
                //e.Graphics.DrawLine(blackPen, multiplierToCoords(pointsFs[5]), multiplierToCoords(pointsFs[7])); // x+

                //e.Graphics.DrawLine(blackPen, multiplierToCoords(pointsFs[0]), multiplierToCoords(pointsFs[1]));
                //e.Graphics.DrawLine(blackPen, multiplierToCoords(pointsFs[0]), multiplierToCoords(pointsFs[4]));
                //e.Graphics.DrawLine(blackPen, multiplierToCoords(pointsFs[1]), multiplierToCoords(pointsFs[5]));
                //e.Graphics.DrawLine(blackPen, multiplierToCoords(pointsFs[4]), multiplierToCoords(pointsFs[5])); // y-

                //e.Graphics.DrawLine(blackPen, multiplierToCoords(pointsFs[2]), multiplierToCoords(pointsFs[3]));
                //e.Graphics.DrawLine(blackPen, multiplierToCoords(pointsFs[2]), multiplierToCoords(pointsFs[6]));
                //e.Graphics.DrawLine(blackPen, multiplierToCoords(pointsFs[3]), multiplierToCoords(pointsFs[7]));
                //e.Graphics.DrawLine(blackPen, multiplierToCoords(pointsFs[6]), multiplierToCoords(pointsFs[7])); // y+
                #endregion //determined indexes of face edges 
                //determined indexes of face edges


            }


        }
        private Point multiplierToCoords(PointF m)
        {
            return new Point((int)(m.X * this.Width), (int)(m.Y * this.Height));
        }

        private void GameScreen_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    pressedKeys[0] = true;
                    break;
                case Keys.S:
                    pressedKeys[1] = true;
                    break;
                case Keys.A:
                    pressedKeys[2] = true;
                    break;
                case Keys.D:
                    pressedKeys[3] = true;
                    break;
                case Keys.Space:
                    pressedKeys[4] = true;
                    break;
                case Keys.Q:
                    pressedKeys[5] = true;
                    break;
            }
        }
        private void GameScreen_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    pressedKeys[0] = false;
                    break;
                case Keys.S:
                    pressedKeys[1] = false;
                    break;
                case Keys.A:
                    pressedKeys[2] = false;
                    break;
                case Keys.D:
                    pressedKeys[3] = false;
                    break;
                case Keys.Space:
                    pressedKeys[4] = false;
                    break;
                case Keys.Q:
                    pressedKeys[5] = false;
                    break;
                case Keys.B:
                    Cursor.Position = this.PointToScreen(new Point(this.Width / 2, this.Height / 2));
                    break;
            }
        }
        private void gameTimer_Tick(object sender, EventArgs e)
        {
            double z = (pressedKeys[0] == true ? 1d * (gameTimer.Interval / 1000d) : 0)
                - (pressedKeys[1] == true ? 1d * (gameTimer.Interval / 1000d) : 0);

            double x = (pressedKeys[2] == true ? 1d * (gameTimer.Interval / 1000d) : 0)
                - (pressedKeys[3] == true ? 1d * (gameTimer.Interval / 1000d) : 0);
            double y = (pressedKeys[4] == true ? 1d * (gameTimer.Interval / 1000d) : 0)
                - (pressedKeys[5] == true ? 1d * (gameTimer.Interval / 1000d) : 0);

            camera.move(new PointD3(x, y, z));

            Refresh();
        }

        private void GameScreen_MouseMove(object sender, MouseEventArgs e)
        {
            camera.rotate(e.X - (this.Width / 2), e.Y - (this.Height / 2), this.Width, this.Height);
            label1.Text = camera.headingY.ToString();
        }
    }
}
