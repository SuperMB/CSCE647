using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project10
{
    class IrridescentSphere : Shape
    {
        public IrridescentSphere(Point center, double radius)
        {
            Sphere = new Sphere(
                center,
                radius
                );
        }

        public bool Inside(Point point)
        {
            return Sphere.Inside(point);
        }

        public Color GetColor(Point point)
        {
            return Sphere.Color;
        }
        public void SetColor(Color color)
        {
            Sphere.Color = color;
        }

        public Color GetIrridescent(Vector normalVector, Vector ray)
        {
            normalVector = normalVector.UnitVector();
            ray = ray.UnitVector() * -1;
            double angle = Math.Acos(Function.DotProduct(normalVector, ray));

            double red = 0;
            double green = 0;
            double blue = 0;
            if(angle < Function.Degrees(30))
            {
                red = angle / Function.Degrees(30);
                green = .5 - (angle) / (2 * Function.Degrees(30));
                blue = 1 - (angle) / (2 * Function.Degrees(30));
            }
            else if(angle < Function.Degrees(60))
            {
                red = 1 - (angle - Function.Degrees(30))/ (2 * Function.Degrees(30));
                green = (angle - Function.Degrees(30)) / Function.Degrees(30);
                blue = .5 - (angle - Function.Degrees(30)) / (2 * Function.Degrees(30));
            }
            else if(angle < Function.Degrees(90))
            {
                red = .5 - (angle - Function.Degrees(60)) / (2 * Function.Degrees(30));
                green = 1 - (angle - Function.Degrees(60)) / (2 * Function.Degrees(30));
                blue = (angle - Function.Degrees(60)) / Function.Degrees(30);
            }

            double max = Math.Max(red, Math.Max(green, blue));
            return new Color(red / max, green / max, blue / max);

            //Vector normalX = new Vector(normalVector.X, 0, normalVector.Z);
            //Vector normalY = new Vector(normalVector.X, normalVector.Y, 0);
            //Vector normalZ = new Vector(0, normalVector.Y, normalVector.Z);

            //Vector rayX = new Vector(ray.X, 0, ray.Z);
            //Vector rayY = new Vector(ray.X, ray.Y, 0);
            //Vector rayZ = new Vector(0, ray.Y, ray.Z);

            //double red = Math.Abs(Function.DotProduct(normalX, rayX));
            //double green = Math.Abs(Function.DotProduct(normalY, rayY));
            //double blue = Math.Abs(Function.DotProduct(normalZ, rayZ));

            //double max = Math.Max(red, Math.Max(green, blue));

            //return new Color(red / max, green / max, blue / max);

        }

        public ReturnData Outline(Point point, Vector ray)
        {
            return Sphere.Outline(point, ray);
        }

        public ReturnData Intersection(Point point, Vector ray)
        {
            ReturnData sphereReturnData = Sphere.Intersection(point, ray);
            if (sphereReturnData == null)
                return null;

            sphereReturnData.Color = GetIrridescent(sphereReturnData.NormalVector, ray);
            sphereReturnData.NonIntersectingShapes = new List<Shape>{ this };
            return sphereReturnData;
        }

        public Sphere Sphere { get; set; }
    }
}

