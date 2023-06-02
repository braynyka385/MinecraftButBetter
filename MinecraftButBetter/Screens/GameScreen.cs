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
        Ray ray;
        Random random;
        Camera camera;
        //List<Block> blocks = new List<Block>();
        bool[] pressedKeys = new bool[6];
        //Ray ray;
        World world = new World(6, 12);
        public GameScreen()
        {
            random = new Random();
            InitializeComponent();
            camera = new Camera(0, 10, 0, 90, 90, 0, 0);
            world.generateChunks(camera.pos());
            gameTimer.Start();
        }
        private void GameScreen_Paint(object sender, PaintEventArgs e)
        {
            foreach (Chunk c in world.getLoadedChunks())
            {
                foreach (Block b in c.blocks)
                {
                    drawBlock(b, e.Graphics);





                }
            }

            int halfWidth = this.Width / 2;
            int halfHeight = this.Height / 2;
            int crosshairScale = 10;
            Pen blackPen = new Pen(Color.Black, 3);

            e.Graphics.DrawLine(blackPen, halfWidth, halfHeight - crosshairScale, halfWidth, halfHeight + crosshairScale);
            e.Graphics.DrawLine(blackPen, halfWidth - crosshairScale, halfHeight, halfWidth + crosshairScale, halfHeight);

        }
        void drawBlock(Block b, Graphics g)
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



                    if (pointsFs[corners[0]].X != -1 && pointsFs[corners[1]].X != -1 && pointsFs[corners[2]].X != -1 && pointsFs[corners[3]].X != -1)
                    {
                        if (!b.hasTexture)
                        {
                            Point[] converted =
                            {
                                multiplierToCoords(pointsFs[corners[0]]),
                                multiplierToCoords(pointsFs[corners[1]]),
                                multiplierToCoords(pointsFs[corners[2]]),
                                multiplierToCoords(pointsFs[corners[3]])

                            };
                            g.FillPolygon(b.faceBrush[j], converted);

                        }
                        else
                        {
                            //The 4 midpoints in any quad form a parrallelogram... therefore:
                            PointF p1 = calculateMidpoint(pointsFs[corners[0]], pointsFs[corners[1]]);
                            PointF p2 = calculateMidpoint(pointsFs[corners[1]], pointsFs[corners[2]]);
                            PointF p12 = calculateMidpoint(p1, p2);



                            PointF p3 = calculateMidpoint(pointsFs[corners[2]], pointsFs[corners[3]]);
                            PointF p4 = calculateMidpoint(pointsFs[corners[3]], pointsFs[corners[0]]);
                            PointF p34 = calculateMidpoint(p3, p4);


                            PointF middleOfFace = calculateMidpoint(p12, p34);


                            int[,] fill = new int[4, 2]
                            {
                                {0,0},
                                {1,0},
                                {0,1},
                                {1,1},
                            };
                            for (int i = 0; i < 4; i++)
                            {

                                //This only works well if the face appears to be a square... fix
                                int[] nums = new int[]
                                {
                                    3,0,1,2
                                }
                                ;

                                if(i == 1 || i == 3)
                                {
                                    nums = new int[] 
                                     {
                                        1,2,3,0 //I have no idea why this works
                                    };
                                }
                                PointF[] midpoints =
                                {
                                        calculateMidpoint(pointsFs[corners[i]], pointsFs[corners[nums[0]]]),
                                        calculateMidpoint(pointsFs[corners[i]], pointsFs[corners[nums[1]]]),
                                        calculateMidpoint(pointsFs[corners[i]], pointsFs[corners[nums[2]]]),
                                        calculateMidpoint(pointsFs[corners[i]], pointsFs[corners[nums[3]]])
                                };
                                midpoints[3 - i] = middleOfFace;
                                Point[] converted =
                                {
                                    multiplierToCoords(midpoints[0]),
                                    multiplierToCoords(midpoints[1]),
                                    multiplierToCoords(midpoints[2]),
                                    multiplierToCoords(midpoints[3])

                                };
                                if (converted[0].X > -10000 && converted[1].X > -10000 && converted[2].X > -10000 && converted[3].X > -10000)
                                {
                                    g.FillPolygon(b.faceTextureBrush[j, fill[i, 0], fill[i, 1]], converted);
                                }
                            }

                        }
                    }
                }


            }
        }

        private PointF calculateMidpoint(PointF p1, PointF p2)
        {
            return new PointF((p1.X + p2.X) / 2, (p1.Y + p2.Y) / 2);
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
            double movementScale = 10;
            double z = (pressedKeys[0] == true ? movementScale * (gameTimer.Interval / 1000d) : 0)
                - (pressedKeys[1] == true ? movementScale * (gameTimer.Interval / 1000d) : 0);
            double x = (pressedKeys[2] == true ? movementScale * (gameTimer.Interval / 1000d) : 0)
                - (pressedKeys[3] == true ? movementScale * (gameTimer.Interval / 1000d) : 0);
            double y = (pressedKeys[4] == true ? movementScale * (gameTimer.Interval / 1000d) : 0)
                - (pressedKeys[5] == true ? movementScale * (gameTimer.Interval / 1000d) : 0);

            camera.move(new PointD3(x, y, z));
            world.generateChunks(camera.pos());
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

            ray = new Ray(5, camera);
            List<Block> blocks = new List<Block>();
            List<Chunk> chunks = world.getLoadedChunks();
            List<int> cI = new List<int>();
            List<int> bIC = new List<int>();
            for (int i = chunks.Count - 1; i >= chunks.Count - 10; i--)
            {
                for (int bb = 0; bb < chunks[i].blocks.Count; bb++)
                {
                    blocks.Add(chunks[i].blocks[bb]);
                    cI.Add(i);
                    bIC.Add(bb);
                }

            }
            int j = ray.indexOfIntersectedBlock(blocks);
            if (j >= 0)
            {
                if (e.Button == MouseButtons.Left)
                {
                    world.removeBlock(cI[j], bIC[j]);

                }
                else if (e.Button == MouseButtons.Right)
                {
                    FaceIndex clickedSide = ray.clickedFace(blocks[j]);
                    PointD3 delta = new PointD3(0, 0, 0);
                    switch (clickedSide)
                    {
                        case FaceIndex.TOP:
                            delta.Y += 1;
                            break;
                        case FaceIndex.BOTTOM:
                            delta.Y -= 1;
                            break;
                        case FaceIndex.LEFT:
                            delta.X += 1;
                            break;
                        case FaceIndex.RIGHT:
                            delta.X -= 1;
                            break;
                        case FaceIndex.FRONT:
                            delta.Z += 1;
                            break;
                        case FaceIndex.BACK:
                            delta.Z -= 1;
                            break;

                    }
                    if(clickedSide != FaceIndex.NONE)
                    {
                        Block clicked = blocks[j];
                        PointD3 newPos = clicked.points[0].added(delta);
                        Block newBlock = new BlockCobblestone((int)newPos.X, (int)newPos.Y, (int)newPos.Z);

                        world.addBlock(newBlock);
                    }
                    
                }
                world.optimizeChunks();
            }

        }
    }
}
