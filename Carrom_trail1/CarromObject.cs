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
        public int Radius
            {
            get; set;
            }
        protected int origin_X, origin_Y;
        public enum Border
            {
            TOP,
            BOTTOM,
            LEFT,
            RIGHT
            }

        public Point initialPoint;
        
        protected CarromObject (int radius)
            {
            this.carromObject = new Ellipse ();
            this.carromObject.Height = this.carromObject.Width = radius + radius;
            this.Radius = radius;
            initialPoint = new Point ();
            }

        public void SetInitialPoint (Point p)
            {
            initialPoint.X = p.X;
            initialPoint.Y = p.Y;
            SetOrigin (p);
            }

        public Point GetInitialPoint ()
            {
            return initialPoint;
            }

        public void SetOrigin (Point origin)
            {
            this.origin_X = (int)Math.Round (origin.X);
            this.origin_Y = (int)Math.Round (origin.Y);
            }

        public void Update ()
            {
            Canvas.SetLeft (this.carromObject, this.origin_X - this.Radius);
            Canvas.SetTop (this.carromObject, this.origin_Y - this.Radius);
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
