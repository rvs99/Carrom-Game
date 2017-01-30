using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Carrom
    {
    //Will handle the life cycle of a single carrom game
    class Game
        {
        public static Canvas carromBoard;
        public static Pocket[] pockets;
        public static Player playerOne;
        public static Player playerTwo;
        public static Coin[] coins;
        public static Striker striker;
        static Line directionPointer;
        static int clickNumber = 0;
        static Point anglePoint;

        #region Methods
        //Reset game stage here, all coins must be at initial position
        //striker must be in Player 1's hand
        public void BeginGame ()
            {
            //Start PhysicsEngine
            PhysicsEngine.Start ();

            #region Players
            playerOne = new Player ("Rahul");
            playerTwo = new Player ("Ajinkya");
            #endregion

            #region Pockets
            pockets = new Pocket[4];
            pockets[0] = new Pocket (22, new Point (22, 22));
            pockets[1] = new Pocket (22, new Point (718, 22));
            pockets[2] = new Pocket (22, new Point (22, 718));
            pockets[3] = new Pocket (22, new Point (718, 718));

            pockets[0].Update ();
            pockets[0].AddToGame ();
            pockets[1].Update ();
            pockets[1].AddToGame ();
            pockets[2].Update ();
            pockets[2].AddToGame ();
            pockets[3].Update ();
            pockets[3].AddToGame ();
            #endregion

            #region All Coins
            //Create all coins
            coins = new Coin[19];

            Random r = new Random ();
            //for player one
            for (int i = 0; i <= 8; i++)
                {
                coins[i] = new Coin (16, Colors.Black);
                //coins[i].SetInitialPoint (new Point (r.Next (5, 730), r.Next (5, 730)));
                coins[i].GetBaseElement ().Stroke = new SolidColorBrush (Colors.White);
                coins[i].AssignPlayer (playerOne);
                coins[i].CoinNumber = i + 1;
                coins[i].Update ();
                coins[i].AddToGame ();
                }


            //for player two
            for (int i = 9; i <= 17; i++)
                {
                coins[i] = new Coin (16, Colors.WhiteSmoke);
                //coins[i].SetInitialPoint (new Point (r.Next (730), r.Next (730)));
                coins[i].AssignPlayer (playerTwo);
                coins[i].GetBaseElement ().Stroke = new SolidColorBrush (Colors.Black);
                coins[i].CoinNumber = i + 1;
                coins[i].Update ();
                coins[i].AddToGame ();
                }

            //Create Queen
            coins[18] = new Coin (16, Colors.DarkGreen);
            coins[18].SetInitialPoint (new Point (370, 370));
            coins[18].Update ();
            coins[18].AddToGame ();
            coins[18].GetBaseElement ().Stroke = new SolidColorBrush (Colors.Black);
            coins[18].IsQueen = true;
            coins[18].CoinNumber = 19;

            coins[0].SetInitialPoint (new Point (370, 403));

            #endregion

            #region Striker
            //Create Striker
            striker = new Striker (20, Colors.White);
            striker.SetInitialPoint (new Point (370, 633));
            striker.Update ();
            striker.AddToGame ();
            striker.GetBaseElement ().Stroke = new SolidColorBrush (Colors.Black);
            striker.GetBaseElement ().MouseLeftButtonDown += TestMethod;
            #endregion

            #region Set Player
            //Give Striker in Player One's hand
            Game.striker.Player = playerOne;
            #endregion

            enableDrag (Game.striker.GetBaseElement ());
            directionPointer = new Line ();
            Game.carromBoard.Children.Add (directionPointer);

            anglePoint = new Point ();
            }

        private void TestMethod (object sender, MouseButtonEventArgs e)
            {
            //Use this statement for Striker to Coin detection and Coin to Pocket detection
            PhysicsEngine engine = new PhysicsEngine ();
            //engine.HitStriker (10, 4.45059);
            //engine.HitStriker (10, 5.49779); //315
            //engine.HitStriker (10, 4.79966);  //275
            //engine.HitStriker (10, 4.7473); //272
            //engine.HitStriker (7.5, 4.62512); //265
            //Coin c = new Coin (15, Colors.AliceBlue);
            //engine.HitCoin (ref c, 1.5, 4.71239);
            //engine.HitCoin (ref Game.coins[18], 1.5, 4.71239);

            //Task.Run (() => {
            //    engine.HitStriker (10, 4.79966);
            //    } );

            //Use this line for Striker to Edge detection
            //PhysicsEngine.HitStriker (ref striker, 70, 5.41052);
            }

        //All standard rules that applies in the turn must apply here
        //PhysicsEngine's HitCarromObject will be called here
        public void FireTurn ()
            {

            }

        //Set variables for next turn
        // e.g. 1. Switch Player
        //      2. Striker must be in another player's hand or 
        //         in the same player's hand depending on the rules
        public void NextTurn ()
            {
            }

        //Game will end here and it shuld return winner Player of the game
        public Player EndGame ()
            {
            return this.GetWinner ();
            }

        // Find max score and return respective Player
        public Player GetWinner ()
            {
            if (playerOne.Score > playerTwo.Score)
                {
                return playerOne;
                }
            else if (playerTwo.Score > playerOne.Score)
                {
                return playerTwo;
                }
            else
                {
                return null;
                }
            }
        #endregion

        #region New Code
        //Reference : http://mark-dot-net.blogspot.in/2012/11/how-to-drag-shapes-on-canvas-in-wpf.html
        static Nullable<Point> dragStart = null;

        static MouseButtonEventHandler mouseDown = (sender, args) =>
        {
            var element = (UIElement)sender;
            dragStart = args.GetPosition (element);
            element.CaptureMouse ();
            clickNumber = clickNumber == 0 ? 1 : clickNumber;

            if (clickNumber == 2)
                {
                directionPointer.X1 = Game.striker.GetOrigin ().X;
                directionPointer.Y1 = Game.striker.GetOrigin ().Y;
                directionPointer.X2 = Game.striker.GetOrigin ().X;
                directionPointer.Y2 = Game.striker.GetOrigin ().Y;
                directionPointer.Stroke = System.Windows.Media.Brushes.Black;
                directionPointer.StrokeThickness = 3;
                }
        };
        static MouseEventHandler mouseMove = (sender, args) =>
        {
            if (dragStart != null && args.LeftButton == MouseButtonState.Pressed)
                {
                switch (clickNumber)
                    {
                    //Set Striker position
                    case 1:
                        var element = (UIElement)sender;
                        var p2 = args.GetPosition (Game.carromBoard);
                        double left = p2.X - dragStart.Value.X;
                        double top = p2.Y - dragStart.Value.Y;
                        if (590 > left && left > 123)
                            {
                            Game.striker.SetOrigin (new Point (left + 22, 633));
                            }
                        break;

                    //Get final point
                    case 2:
                        anglePoint = args.GetPosition (Game.striker.GetBaseElement ());
                        //anglePoint = args.MouseDevice.GetPosition (Game.striker.GetBaseElement ());
                        //clickNumber = 3;
                        directionPointer.X2 = anglePoint.X + Game.striker.GetOrigin ().X - striker.Radius;
                        directionPointer.Y2 = anglePoint.Y + Game.striker.GetOrigin ().Y - striker.Radius;
                        //Canvas.SetLeft (directionPointer, striker.GetOrigin ().X);
                        //Canvas.SetTop (directionPointer, striker.GetOrigin ().Y);
                        break;
                    }
                }
        };
        static MouseButtonEventHandler mouseUp = (sender, args) =>
        {
            var element = (UIElement)sender;
            dragStart = null;
            element.ReleaseMouseCapture ();
             
            if (clickNumber == 1)
                {
                clickNumber = 2;
                }
            //Hit striker with calculated angle and force
            else if (clickNumber == 2)
                {
                double strikerAngle = PhysicsEngine.AngleBetweenTwoLines (new Point(0,740) , new Point (740, 740), striker.GetOrigin (), new Point(directionPointer.X2, directionPointer.Y2));
                if(directionPointer.Y2 < 613)
                    strikerAngle = -(strikerAngle);
                double strkerForce = Math.Sqrt (((anglePoint.X - striker.GetOrigin ().X) * (anglePoint.X - striker.GetOrigin ().X) + (anglePoint.Y - striker.GetOrigin ().Y) * (anglePoint.Y - striker.GetOrigin ().Y)));
                new PhysicsEngine ().HitStriker (strkerForce/100, strikerAngle);
                clickNumber = 0;
                carromBoard.Children.Remove (directionPointer);
                }
        };
        Action<UIElement> enableDrag = (element) =>
        {
            element.MouseDown += mouseDown;
            element.MouseMove += mouseMove;
            element.MouseUp += mouseUp;
        };

        #endregion

        #region Testing Code
        //private Point StartPoint, EndPoint;

        //private void panel1_MouseDown (object sender, MouseEventArgs e)
        //    {
        //    if (e.RightButton == MouseButtonState.Pressed)
        //        {
        //        Point pt = e.GetPosition (Game.striker.GetBaseElement ());
        //        StartPoint = pt;
        //        EndPoint = pt;
        //        ControlPaint.DrawReversibleLine (StartPoint, EndPoint, Color.Black);
        //        }
        //    }

        //private void panel1_MouseMove (object sender, MouseEventArgs e)
        //    {
        //    if (e.Button == System.Windows.Forms.MouseButtons.Right)
        //        {
        //        ControlPaint.DrawReversibleLine (StartPoint, EndPoint, Color.Black); // erase previous line
        //        EndPoint = Cursor.Position;
        //        ControlPaint.DrawReversibleLine (StartPoint, EndPoint, Color.Black); // draw new line
        //        }
        //    }

        //private void panel1_MouseUp (object sender, MouseEventArgs e)
        //    {
        //    if (e.Button == System.Windows.Forms.MouseButtons.Right)
        //        {
        //        ControlPaint.DrawReversibleLine (StartPoint, EndPoint, Color.Black); // erase previous line

        //        // ... do something with StartPont and EndPoint in here ...

        //        // possibly add them to a class level structure to use in the Paint() event to make it persistent?

        //        }
        //    }
        #endregion
        }
    }