using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Carrom
    {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
        {
        private DispatcherTimer timer, valueChanger; // timer object
        private Vector speed; // movement in pixels/second, initially zero
        int speed1 = 0;
        public MainWindow ()
            {
            InitializeComponent ();
            Game.carromBoard = canvas;
            }

        private void Window_Loaded (object sender, RoutedEventArgs e)
            {
            Game newGame = new Game ();
            newGame.BeginGame ();
            newGame.FireTurn ();
            newGame.NextTurn ();



            //timer = new DispatcherTimer ();
            //valueChanger = new DispatcherTimer ();
            //speed = new Vector (0, 0);

            //timer.Interval = TimeSpan.FromMilliseconds (10); // update 20 times/second
            //timer.Tick += TimerTick;
            //timer.Start ();

            //valueChanger.Interval = TimeSpan.FromMilliseconds (1);
            //valueChanger.Tick += ValueChanger;
            //valueChanger.Start ();
            }

        private void ValueChanger (object sender, EventArgs e)
            {
            //if (Canvas.GetLeft (c) == 200)
            //    speed1 -= 10;
            //else
            //    speed1 += 10;
            }

        //private void canvas_MouseDown (object sender, MouseButtonEventArgs e)
        //    {
        //    if (Game.striker.GetBaseElement().IsMouseOver)
        //        {
        //        Point p = e.GetPosition (this);
        //        Game.striker.SetStrikerOrigin (p);
        //        }
        //    }

        //private void canvas_MouseLeftButtonDown (object sender, MouseButtonEventArgs e)
        //    {
        //    if (Game.striker.GetBaseElement ().IsMouseOver)
        //        {
        //        Point p = e.GetPosition (this);
        //        Game.striker.SetStrikerOrigin (p);
        //        }
        //    }

        private void TimerTick (object sender, EventArgs e)
            {
            }
        }
    }
