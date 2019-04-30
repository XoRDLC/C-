using System;
using System.Collections.Generic;

namespace Project1
{
    class C04_Switch
    {
        public void Main()
        {
            Test_ShapesSwitchCase();
        }

        #region "shapes Test case, Test_ShapesSwitchCase()"
        public static void Test_ShapesSwitchCase() {

            var t1 = new Square();
            t1.Side = 4;
            var t2 = new Square();
            var t3 = t2;
            t2.Side = 4;
            Statics.Log(t1.Equals(t2));
            Statics.Log(t2.Equals(t3));
            Statics.Log(t3.Equals(t2));
            t3 = null;
            Statics.Log(t2.Equals(t3));
            t1 = null;
            t2 = null;

            var shapes = new List<SimpleShape>();

            Statics.Log(shapes.GetType());

            shapes.Add(new Triangle() { SideA = 6, SideB = 3, SideC = Math.Sqrt(6 * 6 - 3 * 3) });
            shapes.Add(new Circle() { Radius = Math.PI });
            shapes.Add(new Rect() { SideA = 3, SideB = 5 });
            shapes.Add(new Square() { Side = 10 });
            shapes.Add((Shape)null);

            foreach (var shape in shapes)
            {
                switch (shape)
                {
                    case Square sq when !sq.IsValidSides():
                    case Rect rect when !rect.IsValidSides():
                    case Circle circle when !circle.IsValidSides():
                    case Triangle tri when !tri.IsValidSides():
                        Statics.Log("Shape sides must be > 0, " + shape.GetType());
                        break;
                    case Triangle tri when tri.AngleAB == 90 || tri.AngleAC == 90 || tri.AngleBC == 90:
                        Statics.Log("Right triangle, Area: " + tri.Area);
                        break;
                    case Triangle tri:
                        Statics.Log("Triangle, Area: " + tri.Area);
                        break;
                    case Rect rect when rect.SideA == rect.SideB:
                    case Square sq:
                        Statics.Log("Square, Area: " + (shape is Rect ? ((Rect)shape).Area : ((Square)shape).Area));
                        break;
                    case Rect rect:
                        Statics.Log("Rectangle, Area: " + rect.Area);
                        break;
                    case Circle circle when circle.Radius == Math.PI:
                        Statics.Log("PI radius circle, Area: " + circle.Area);
                        break;
                    case Circle circle:
                        Statics.Log("Circle, Area: " + circle.Area);
                        break;
                    case null:
                        Statics.Log("null shape");
                        break;
                    default:
                        throw new ArgumentException(message: "No such Shape", paramName: nameof(Shape));
                }
            }
        }
        #endregion
    }

    #region "shapes"

    abstract class SimpleShape
    {
        abstract new public bool Equals(object obj);
    }

    abstract class Shape : SimpleShape
    {
        abstract public bool IsValidSides();
    }

    class Triangle : Shape {
        public double SideA { get; set; }
        public double SideB { get; set; }
        public double SideC { get; set; }
        public double AngleAB { get => !IsValidSides() ? 0 : Math.Acos((SideA * SideA + SideB * SideB - SideC * SideC) / (2 * SideA * SideB)) * 180 / Math.PI; }
        public double AngleAC { get => !IsValidSides() ? 0 : Math.Acos((SideA * SideA - SideB * SideB + SideC * SideC) / (2 * SideA * SideC)) * 180 / Math.PI; }
        public double AngleBC { get => !IsValidSides() ? 0 : Math.Acos((-SideA * SideA + SideB * SideB + SideC * SideC) / (2 * SideC * SideB)) * 180 / Math.PI; }
        public double Area
        {
            get
            {
                if (!IsValidSides()) return 0;
                return SideA * SideB * Math.Sin(AngleAB / 180 * Math.PI)  / 2;
            }
        }
        public override bool IsValidSides() {
            return 
                SideA > 0 && 
                SideB > 0 && 
                SideC > 0 &&
                SideA + SideB > SideC && 
                SideA + SideC > SideB && 
                SideB + SideC > SideA;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (this == obj) return true;
            if (!(obj is Triangle)) return false;
            return 
                this.SideA == ((Triangle)obj).SideA &&
                this.SideB == ((Triangle)obj).SideB &&
                this.SideC == ((Triangle)obj).SideC;
        }
    }

    class Circle : Shape {
        public Circle() { }
        public double Radius { get; set; }
        public double Area { get => !IsValidSides()? 0 : Math.PI * Radius * Radius; }
        public override bool IsValidSides() => Radius > 0;
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (this == obj) return true;
            if (!(obj is Circle)) return false;
            return this.Radius == ((Circle)obj).Radius;
        }
    }
    class Rect : Shape {
        public double SideA { get; set; }
        public double SideB { get; set; }
        public double Area { get => !IsValidSides() ? 0 : SideA * SideB; }
        public override bool IsValidSides() => SideA > 0 && SideB > 0;
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (this == obj) return true;
            if (!(obj is Rect)) return false;
            return this.SideA == ((Rect)obj).SideA && this.SideB == ((Rect)obj).SideB;
        }
    }
    class Square : Shape {
        public double Side { get; set; }
        public double Area { get => !IsValidSides() ? 0 : Side * Side; }
        public override bool IsValidSides() => Side > 0;
        public override bool Equals(object obj)
        {
            Statics.Log("\tnull check");
            if (obj == null) return false;
            Statics.Log("\tref check");
            if (this == obj) return true;
            Statics.Log("\ttype check");
            if (!(obj is Square)) return false;
            Statics.Log("\tside check");
            return this.Side == ((Square)obj).Side;
        }
    }
    #endregion
}
