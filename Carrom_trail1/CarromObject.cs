using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace Carrom
    {
    public class CarromObject
        {
        protected Ellipse carromObject;
        protected int radius;
        protected int origin_X, origin_Y;


        protected CarromObject (int radius)
            {
            this.carromObject = new Ellipse ();
            this.carromObject.Height = this.carromObject.Width = radius + radius;
            this.radius = radius;
            }

        protected void SetOrigin (Point origin)
            {
            this.origin_X = (int)Math.Round (origin.X);
            this.origin_Y = (int)Math.Round (origin.Y);
            Canvas.SetLeft (this.carromObject, this.origin_X - this.radius);
            Canvas.SetTop (this.carromObject, this.origin_Y - this.radius);
            }

        public void AddToGame ()
            {
            Game.carromBoard.Children.Add (this.carromObject);
            }
        
        public Point GetOrigin ()
            {
            return new Point (this.origin_X, this.origin_Y);
            }

        public Ellipse GetBaseElement ()
            {
            return this.carromObject;
            }
        }
    }
