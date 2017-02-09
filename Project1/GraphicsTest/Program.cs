using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Input;
using System.Threading;
using System.IO;
using System.Collections.Generic;

namespace GraphicsTest
{
    class Program
    {
        static WriteableBitmap writeableBitmap;
        static Window w;
        static Image image;

        static Screen screen;
        static Sphere sphere;
        static Plane plane;
        static Cylinder cylinder;

        static int antiAliasX;
        static int antiAliasY;
        static Boolean antiAlias;

        static List<Shape> _shapes;
        static bool _includeSpheres;
        static bool _includePlanes;
        static bool _includeCynlinders;

        [STAThread]
        static void Main(string[] args)
        {

            //**************************************************************************************
            //**************************************************************************************
            //**************************************************************************************
            //To create the image, run the program, and click the empty black area, then press enter
            //**************************************************************************************
            //**************************************************************************************
            //**************************************************************************************




            image = new Image();
            RenderOptions.SetBitmapScalingMode(image, BitmapScalingMode.NearestNeighbor);
            RenderOptions.SetEdgeMode(image, EdgeMode.Aliased);

            w = new Window();
            w.Height = 800;
            w.Width = 800;
            w.Content = image;
            //w.WindowState = WindowState.Maximized;
            w.Show();

            writeableBitmap = new WriteableBitmap(
                (int)w.ActualWidth,
                (int)w.ActualHeight,
                96,
                96,
                PixelFormats.Bgr32,
                null);

            image.Source = writeableBitmap;

            image.Stretch = Stretch.None;
            image.HorizontalAlignment = HorizontalAlignment.Left;
            image.VerticalAlignment = VerticalAlignment.Bottom;

            image.MouseMove += new MouseEventHandler(i_MouseMove);
            w.KeyDown += KeyPress;

            

            Point screenPoint = new Point(0, 0, 0);
            Vector vector0 = new Vector(1, 0, 0);
            Vector vector1 = new Vector(0, 1, 0);
            //screen = new Screen(w.ActualWidth, w.ActualHeight, screenPoint, vector0, w.ActualWidth*2, vector1, w.ActualHeight*2);
            screen = new Screen(w.ActualWidth, w.ActualHeight, screenPoint, vector0, w.ActualWidth, vector1, w.ActualHeight);

            _shapes = new List<Shape>();

            //**************************************************************************************
            //**************************************************************************************
            //Determine which shapes to include
            _includeSpheres = true;
            _includePlanes = true;
            _includeCynlinders = true;

            //**************************************************************************************
            //**************************************************************************************
            //antiAlias is a boolean value, for anti alias set to true, for not anti alias, set to false
            antiAlias = false;
            antiAliasX = 4;
            antiAliasY = 2;


            //**************************************************************************************
            //**************************************************************************************
            //Create Simple, leave the following function uncommented
            //CreateSimple();

            //**************************************************************************************
            //**************************************************************************************
            //Create Complex, leave the following function uncommented
            CreateComplex();


            //Render();
            Application app = new Application();
            app.Run();
        }

        static void CreateSimple()
        {

            Point sphereCenter = new Point(300, 300, 0);
            sphere = new Sphere(sphereCenter, 200);
            sphere.SetColor(new Color(0, 1, 0));
            if (_includeSpheres)
                _shapes.Add(sphere);
            
            Point planePoint = new Point(300, 300, 0);
            Vector planeVector = new Vector(1, -1, 0);
            plane = new Plane(planePoint, planeVector);
            plane.SetColor(new Color(0, 0, 1));
            if (_includePlanes)
                _shapes.Add(plane);

            Point cylinderPoint = new Point(400, 500, 0);
            Vector cylinderDirection = new Vector(0 , 1, .5);
            cylinder = new Cylinder(cylinderPoint, cylinderDirection, 100);
            cylinder.SetColor(new Color(1, 0, 0));
            if (_includeCynlinders)
                _shapes.Add(cylinder);

        }

        static void CreateComplex()
        {
            int numberOfSpheres = 20;
            double radiusStep = 25;
            double radius = 0;
            for (int i = 0; i < numberOfSpheres; i++)
            {
                radius += radiusStep;
                Point sphereCenter = new Point(radiusStep, radius, 0);
                sphere = new Sphere(sphereCenter, radius);
                if (_includeSpheres)
                    _shapes.Add(sphere);
            }
            radius = 0;
            for (int i = 0; i < numberOfSpheres; i++)
            {
                radius += radiusStep;
                Point sphereCenter = new Point(radius, radiusStep, 0);
                sphere = new Sphere(sphereCenter, radius);
                if (_includeSpheres)
                    _shapes.Add(sphere);
            }
            radius = 0;
            for (int i = 0; i < numberOfSpheres; i++)
            {
                radius += radiusStep;
                Point sphereCenter = new Point(radiusStep + (radiusStep * i) / Math.Sqrt(2), radiusStep + (radiusStep * i) / Math.Sqrt(2), 0);
                sphere = new Sphere(sphereCenter, radius);
                if (_includeSpheres)
                    _shapes.Add(sphere);
            }

            double planeStep = .2;
            double planeStartX = screen.Width / 2;
            double planeStartY = screen.Height;
            int numberOfPlanes = 5;
            for (int i = 1; i <= numberOfPlanes; i++)
            {
                Point planePoint = new Point(planeStartX, planeStartY, 0);
                Vector planeVector = new Vector(i * planeStep, -1 + i * planeStep, 0);
                plane = new Plane(planePoint, planeVector);
                if (_includePlanes)
                    _shapes.Add(plane);
            }

            int numberOfCylinders = 20;
            double cylinderStartX = 3 * screen.Width / 4;
            double cylinderStartY = 3 * screen.Height / 4;
            radiusStep = 50;
            radius = 0;
            double twist = 0;
            double twistStep = 2.0 / numberOfCylinders;
            for (int i = 0; i < numberOfCylinders; i++)
            {
                radius = radiusStep;
                twist = twistStep * i;
                Point cylinderPoint = new Point(cylinderStartX, cylinderStartY, 0);
                Vector cylinderDirection = new Vector(1 - twist, 1 + twist, .5);
                //Vector cylinderDirection = new Vector(-1, 1, .5);
                cylinder = new Cylinder(cylinderPoint, cylinderDirection, radius);
                if (_includeCynlinders)
                    _shapes.Add(cylinder);
            }
            for (int i = 0; i < numberOfCylinders; i++)
            {
                radius = radiusStep;
                twist = twistStep * i;
                Point cylinderPoint = new Point(cylinderStartX, cylinderStartY, 0);
                Vector cylinderDirection = new Vector(1 + twist, 1 - twist, .5);
                cylinder = new Cylinder(cylinderPoint, cylinderDirection, radius);
                if (_includeCynlinders)
                    _shapes.Add(cylinder);
            }
            for (int i = 0; i < numberOfCylinders; i++)
            {
                radius = radiusStep;
                twist = twistStep * i;
                Point cylinderPoint = new Point(cylinderStartX, cylinderStartY, 0);
                Vector cylinderDirection = new Vector(-1 + twist, 1 + twist, .5);
                cylinder = new Cylinder(cylinderPoint, cylinderDirection, radius);
                if (_includeCynlinders)
                    _shapes.Add(cylinder);
            }
            for (int i = 0; i < numberOfCylinders; i++)
            {
                radius = radiusStep;
                twist = twistStep * i;
                Point cylinderPoint = new Point(cylinderStartX, cylinderStartY, 0);
                Vector cylinderDirection = new Vector(-1 - twist, 1 - twist, .5);
                cylinder = new Cylinder(cylinderPoint, cylinderDirection, radius);
                if (_includeCynlinders)
                    _shapes.Add(cylinder);
            }

        }

        static void KeyPress(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                //Thread thread = new Thread(new ThreadStart(Render));
                //thread.Start();
                Render();
                using (FileStream file = new FileStream("image.png", FileMode.Create))
                {
                    PngBitmapEncoder pngBitmapEncoder = new PngBitmapEncoder();
                    pngBitmapEncoder.Frames.Add(BitmapFrame.Create(writeableBitmap));
                    pngBitmapEncoder.Save(file);
                }
            }
            else if(e.Key == Key.LeftShift)
            {
                antiAlias = !antiAlias;
            }
        }

        static void Render()
        {
            Point origin = screen.GetPoint(1, 1); 
            Point nexPointX = screen.GetPoint(2, 1);
            Point nexPointY = screen.GetPoint(1, 0);

            Vector vectorX = Function.GetVector(origin, nexPointX);
            vectorX.Scale(1 / (double)antiAliasX);
            Vector vectorY = Function.GetVector(origin, nexPointY);
            vectorY.Scale(1 / (double)antiAliasY);

            Color white = new Color(1, 1, 1);
            for (int i = 1; i <= w.ActualWidth; i++)
            {
                for (int j = 1; j <= w.ActualHeight; j++)
                {

                    Point point = screen.GetPoint(i, j);

                    if (antiAlias)
                    {
                        Random random = new Random();
                        double randomX = random.NextDouble();
                        double randomY = random.NextDouble();
                        Vector moveVectorX = vectorX.Clone();
                        moveVectorX.Scale(randomX);
                        Vector moveVectorY = vectorY.Clone();
                        moveVectorY.Scale(randomY);
                        Vector moveVector = Function.Add(moveVectorX, moveVectorY);

                        //Color color = new Color(1, 1, 1);
                        for (double k = 0; k < antiAliasX; k++)
                        {
                            for (double p = 0; p < antiAliasY; p++)
                            {
                                Vector shiftX = vectorX.Clone();
                                shiftX.Scale(k);
                                Vector shiftY = vectorY.Clone();
                                shiftY.Scale(p);

                                Point newPoint = Function.Add(point, shiftX);
                                newPoint = Function.Add(newPoint, shiftY);
                                newPoint = Function.Add(newPoint, moveVector);
                                foreach (Shape shape in _shapes)
                                {
                                    if (shape.Inside(newPoint))
                                    {
                                        Color color = shape.Color();
                                        color /= (antiAliasX * antiAliasY);
                                        screen.Pixels[i - 1][j - 1] += color;
                                        //screen.Pixels[i - 1][j - 1] += shape.Color() / (antiAliasX * antiAliasY);
                                    }
                                }

                                ////Boolean inSphere = sphere.Inside(newPoint);
                                //if (sphere.Inside(newPoint))
                                //    color += sphere.Color();
                                //    //color.Red += 1.0 / (antiAliasX * antiAliasY);
                                ////Boolean onPlane = plane.OnPlane(newPoint);
                                //if (plane.Inside(newPoint))
                                //    color += plane.Color();
                                ////color.Green += 1.0 / (antiAliasX * antiAliasY);
                                ////Boolean inCylinder = cylinder.InsideCylinder(newPoint);
                                //if (cylinder.Inside(newPoint))
                                //    color += cylinder.Color();
                                ////color.Blue += 1.0 / (antiAliasX * antiAliasY);
                            }
                        }
                        //if (color.Red > 0 | color.Green > 0 | color.Blue > 0)
                        //    ColorPixel(color, i, j);
                    }
                    else
                    {
                        foreach(Shape shape in _shapes)
                        {
                            if (shape.Inside(point))
                                screen.Pixels[i - 1][j - 1] += shape.Color();
                        }
                    }
                }
            }

            screen.Normalize();
            for (int i = 1; i < screen.Pixels.Length; i++)
                for (int j = 1; j < screen.Pixels[i].Length; j++)
                    ColorPixel(screen.Pixels[i - 1][j - 1], i, j);
        }

        // The DrawPixel method updates the WriteableBitmap by using
        // unsafe code to write a pixel into the back buffer.
        static void ColorPixel(Color color, int pixelX, int pixelY)
        {
            byte[] ColorData = color.GetColor(); // B G R

            Int32Rect rect = new Int32Rect(pixelX, pixelY, 1, 1);
            try
            {
                writeableBitmap.WritePixels(rect, ColorData, 4, 0);
            }
            catch
            {

            }
        }

        //static void i_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        //{
        //    ErasePixel(e);
        //}

        //static void i_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        //{
        //    DrawPixel(e);
        //}

        static void i_MouseMove(object sender, MouseEventArgs e)
        {
            Point point = screen.GetPoint((int)e.GetPosition(image).X, (int)e.GetPosition(image).Y);
            Console.Out.WriteLine("pixelX: " + e.GetPosition(image).X.ToString() + ", pixelY: " + e.GetPosition(image).Y.ToString());
            Console.Out.WriteLine("X: " + point.X.ToString() + ", Y: " + point.Y.ToString() + ", Z " + point.Z.ToString());
            Console.Out.WriteLine();
        }

        //static void w_MouseWheel(object sender, MouseWheelEventArgs e)
        //{
        //    System.Windows.Media.Matrix m = i.RenderTransform.Value;

        //    if (e.Delta > 0)
        //    {
        //        m.ScaleAt(
        //            1.5,
        //            1.5,
        //            e.GetPosition(w).X,
        //            e.GetPosition(w).Y);
        //    }
        //    else
        //    {
        //        m.ScaleAt(
        //            1.0 / 1.5,
        //            1.0 / 1.5,
        //            e.GetPosition(w).X,
        //            e.GetPosition(w).Y);
        //    }

        //    i.RenderTransform = new MatrixTransform(m);
        //}
    }
}