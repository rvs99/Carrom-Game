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

namespace Carrom_trail1
    {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
        {
        private DispatcherTimer timer,valueChanger; // timer object
        private Vector speed; // movement in pixels/second, initially zero
        int speed1=0;
        Ellipse el;
        Coin c;

        public MainWindow ()
            {
            InitializeComponent ();
            }

        private void Window_Loaded (object sender, RoutedEventArgs e)
            {
                        c = new Coin (31, 50, 100, Colors.Black);

            canvas.Children.Add (c.coin);
            Canvas.SetLeft (c.coin, 50);
            Canvas.SetTop (c.coin, 100);

            

            timer = new DispatcherTimer ();
            valueChanger = new DispatcherTimer ();
            speed = new Vector (0, 0);

            timer.Interval = TimeSpan.FromMilliseconds (10); // update 20 times/second
            timer.Tick += TimerTick;
            timer.Start ();

            valueChanger.Interval = TimeSpan.FromMilliseconds (1);
            valueChanger.Tick += ValueChanger;
            valueChanger.Start ();
            }

        private void ValueChanger (object sender, EventArgs e)
            {
            if (Canvas.GetLeft (c.coin) == 200)
                speed1 -= 10;
            else
                speed1 += 10;
            }

        private void TimerTick (object sender, EventArgs e)
            {
            //el.RenderTransform = new ScaleTransform (1.25, 1.25, el.Width / 2, el.Height / 2);
            Canvas.SetLeft (c.coin, Canvas.GetLeft (c.coin) + speed1);
            //Canvas.SetTop (el, Canvas.GetTop (el) + 20);
            if (canvas.IsMouseOver)
                {
                canvas.MouseMove +=  (sender1, e1) =>
                {
                    labelX.Content = "X = " + e1.GetPosition (canvas).X;
                    labelY.Content = "Y = " + e1.GetPosition (canvas).Y;
                };
                }
            //DoubleAnimation
            }
        }
    }
