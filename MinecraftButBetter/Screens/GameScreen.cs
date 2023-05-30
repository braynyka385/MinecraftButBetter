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
using MinecraftButBetter.WorldStuff.Blocks;

namespace MinecraftButBetter.Screens
{
    public partial class GameScreen : UserControl
    {
        Random random;
        Camera camera;
        //List<Block> blocks = new List<Block>();
        bool[] pressedKeys = new bool[6];
        Ray ray;
        World world = new World(6, 48);
        public GameScreen()
        {
            random = new Random();
            InitializeComponent();
            camera = new Camera(-3, 2, 4, 90, 90, 0, 0);

            world.generateChunks(camera.pos());






            gameTimer.Start();
        }

        private void GameScreen_Paint(object sender, PaintEventArgs e)
        {
            SolidBrush blackBrush = new SolidBrush(Color.Black);
            foreach (Chunk c in world.getLoadedChunks())
            {
                foreach (Block b in c.blocks)
                {
                    PointF[] pointsFs = new PointF[8];
                    for (int i = 0; i < 8; i++)
                    {
                        PointF p = camera.pointToScreen(b.points[i]);
                        pointsFs[i] = p;
                    }

                    int[] corners = new int[4];
                    b.selfObstructFaces(camera.pos());
                    for (int j = 5; j >= 0; j--)
                    {
                        Face f = b.getFace((FaceIndex)j);
                        if (f.isVisible && !f.obstructedByOwnBlock)
                        {
                            corners = f.getCorners();

                            Point[] converted =
                            {
                           multiplierToCoords(pointsFs[corners[0]]),
                           multiplierToCoords(pointsFs[corners[1]]),
                           multiplierToCoords(pointsFs[corners[2]]),
                           multiplierToCoords(pointsFs[corners[3]])

                        };

                            if (pointsFs[corners[0]].X != -1 && pointsFs[corners[1]].X != -1 && pointsFs[corners[2]].X != -1 && pointsFs[corners[3]].X != -1)
                                e.Graphics.FillPolygon(b.faceBrush[j], converted);
                        }


                    }





                }
            }
            
            int halfWidth = this.Width / 2;
            int halfHeight = this.Height / 2;
            int crosshairScale = 10;
            Pen blackPen = new Pen(Color.Black, 3);

            e.Graphics.DrawLine(blackPen, halfWidth, halfHeight - crosshairScale, halfWidth, halfHeight + crosshairScale);
            e.Graphics.DrawLine(blackPen, halfWidth - crosshairScale, halfHeight, halfWidth + crosshairScale, halfHeight);

            if(ray != null)
            {
                e.Graphics.FillEllipse(blackBrush, new Rectangle(multiplierToCoords(camera.pointToScreen(ray.end)).X, multiplierToCoords(camera.pointToScreen(ray.end)).Y, 5, 5));
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
            double movementScale = 2;
            double z = (pressedKeys[0] == true ? movementScale * (gameTimer.Interval / 1000d) : 0)
                - (pressedKeys[1] == true ? movementScale * (gameTimer.Interval / 1000d) : 0);
            double x = (pressedKeys[2] == true ? movementScale * (gameTimer.Interval / 1000d) : 0)
                - (pressedKeys[3] == true ? movementScale * (gameTimer.Interval / 1000d) : 0);
            double y = (pressedKeys[4] == true ? movementScale * (gameTimer.Interval / 1000d) : 0)
                - (pressedKeys[5] == true ? movementScale * (gameTimer.Interval / 1000d) : 0);

            camera.move(new PointD3(x, y, z));

            foreach (Chunk c in world.loadChunks(camera.pos()))
            {
                foreach (Block b in c.blocks)
                {
                    double xFactor = b.points[0].X - camera.pos().X;
                    double yFactor = b.points[0].Y - camera.pos().Y;
                    double zFactor = b.points[0].Z - camera.pos().Z;
                    b.distSq = (xFactor * xFactor) + (yFactor * yFactor) + (zFactor * zFactor);
                }
                c.blocks = c.blocks.OrderByDescending(b => b.distSq).ToList();

            }


            Refresh();
        }

        private void GameScreen_MouseMove(object sender, MouseEventArgs e)
        {
            camera.rotate(e.X - (this.Width / 2), e.Y - (this.Height / 2), this.Width, this.Height);
        }

        private void GameScreen_MouseClick(object sender, MouseEventArgs e)
        {
            ray = new Ray(camera.pos(), camera.headingX, camera.headingY, 5);
        }
    }
}
