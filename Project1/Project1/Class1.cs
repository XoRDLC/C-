using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1
{
    class Class1
    {
        //public void Dummy()
        public static void Main()
        {
            var shapes = new List<Shape>();
            shapes.Add(new Triangle() { SideA = 6, SideB = 3, SideC = Math.Sqrt(6 * 6 - 3 * 3) });
            shapes.Add(new Circle() { Radius = Math.PI });
            shapes.Add(new Rect() { SideA = 3, SideB = 3 });
            shapes.Add(new Square() { Side = 10 });
            shapes.Add((Shape)null);

            foreach (var shape in shapes) {
                switch (shape) {
                    case Triangle tri when tri.AngleAB == 0:
                        throw new ArgumentException(message: "sum of any two sides of Triangle must be more then third one");
                    case Triangle tri when tri.AngleAB == 90||tri.AngleAC==90||tri.AngleBC ==90: 
                        Statics.Log("Right triangle");
                        break;
                    case Triangle tri:
                        Statics.Log("Triangle");
                        break;
                    case Rect rect when rect.SideA == rect.SideB:
                    case Square sq:
                        Statics.Log("Square");
                        break;
                    case Rect rect:
                        Statics.Log("Rectangle");
                        break;
                    case Circle circle when circle.Radius == Math.PI:
                        Statics.Log("PI radius circle");
                        break;
                    case Circle circle:
                        Statics.Log("Circle");
                        break;
                    case null:
                        Statics.Log("null shape");
                        break;
                    default:
                        throw new ArgumentException(message: "No such Shape", paramName: nameof(Shape));
                }
            }
        }
    }
    #region "shapes"
    abstract class Shape { }

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
        private bool IsValidSides() {
            return SideA > 0 && 
                SideB > 0 && 
                SideC > 0 &&
                SideA + SideB > SideC && 
                SideA + SideC > SideB && 
                SideB + SideC > SideA;
        }
    }

    class Circle : Shape {
        public double Radius { get; set; }
        public double Area() => Math.PI * Radius * Radius;
    }
    class Rect : Shape {
        public double SideA { get; set; }
        public double SideB { get; set; }
        public double Area { get => !IsValidSides() ? 0 : SideA * SideB; }
        private bool IsValidSides() => SideA > 0 && SideB > 0;
    }
    class Square : Shape {
        public double Side { get; set; }
        public double Area { get => Side < 0 ? 0 : Side * Side; }
    }
    #endregion
}
