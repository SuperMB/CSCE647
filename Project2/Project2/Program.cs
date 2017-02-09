using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Input;
using System.Threading;
using System.IO;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Project2
{
    class Program
    {
        static WriteableBitmap writeableBitmap;
        static Window w;
        static Image image;

        //static Screen screen;
        //static Sphere sphere;
        //static Plane plane;
        //static Cylinder cylinder;

        static int antiAliasX;
        static int antiAliasY;
        static Boolean antiAlias;

        static List<Shape> _shapes;
        static bool _includeSpheres;
        static bool _includePlanes;
        static bool _includeCynlinders;

        static Camera _camera;

        static double _moveAmount;

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
            
            //Point screenPoint = new Point(0, 0, 0);
            //Vector vector0 = new Vector(1, 0, 0);
            //Vector vector1 = new Vector(0, 1, 0);
            //screen = new Screen(w.ActualWidth, w.ActualHeight, screenPoint, vector0, w.ActualWidth, vector1, w.ActualHeight);

            Point cameraPoint = new Point(400, 400, 200);
            Vector viewVector = new Vector(0, 0, -1);
            Vector upVector = new Vector(0, 1, 0);
            double screenDistance = 400;
            _camera = new Camera(cameraPoint, viewVector, upVector, screenDistance, w.ActualWidth, w.ActualHeight, w.ActualWidth, w.ActualHeight);

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
            //CreateComplex();

            Point centerPoint = new Point(500, 200, -600);
            //Point sphereCenter = new Point(500, 200, -600);
            Sphere sphere = new Sphere(centerPoint, 300);
            sphere.SetColor(new Color(0, 1, 0));
            _shapes.Add(sphere);

            //Point planePoint = new Point(500, 200, -600);
            Vector planeVector = new Vector(1, 0, 0);
            Plane plane = new Plane(centerPoint, planeVector);
            plane.SetColor(new Color(0, 1, 1));
            _shapes.Add(plane);

            //planePoint = new Point(500, 400, -600);
            planeVector = new Vector(0, 1, 0);
            plane = new Plane(centerPoint, planeVector);
            plane.SetColor(new Color(1, 0, 1));
            _shapes.Add(plane);

            //planePoint = new Point(500, 400, -600);
            planeVector = new Vector(0, 0, 1);
            plane = new Plane(centerPoint, planeVector);
            plane.SetColor(new Color(1, 1, 0));
            _shapes.Add(plane);

            //int numberOfSpheres = 3;
            //Random random = new Random();
            //for(int i = 0; i < numberOfSpheres; i++)
            //{
            //    Point sphereCenter = new Point(-900 + random.Next(0,1900), -900 + random.Next(0, 1900), -1500 + random.Next(0,1100));
            //    Sphere sphere = new Sphere(sphereCenter, random.Next(50,300));
            //    sphere.SetColor(new Color(random.NextDouble(), random.NextDouble(), random.NextDouble()));
            //    _shapes.Add(sphere);

            //}

            _moveAmount = 100;
            //Render();
            Application app = new Application();
            app.Run();
        }



        static void CreateSimple()
        {

            Point sphereCenter = new Point(300, 300, 0);
            Sphere sphere = new Sphere(sphereCenter, 200);
            sphere.SetColor(new Color(0, 1, 0));
            if (_includeSpheres)
                _shapes.Add(sphere);
            
            Point planePoint = new Point(300, 300, 0);
            Vector planeVector = new Vector(1, -1, 0);
            Plane plane = new Plane(planePoint, planeVector);
            plane.SetColor(new Color(0, 0, 1));
            if (_includePlanes)
                _shapes.Add(plane);

            Point cylinderPoint = new Point(400, 500, 0);
            Vector cylinderDirection = new Vector(0 , 1, .5);
            Cylinder cylinder = new Cylinder(cylinderPoint, cylinderDirection, 100);
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
                Sphere sphere = new Sphere(sphereCenter, radius);
                if (_includeSpheres)
                    _shapes.Add(sphere);
            }
            radius = 0;
            for (int i = 0; i < numberOfSpheres; i++)
            {
                radius += radiusStep;
                Point sphereCenter = new Point(radius, radiusStep, 0);
                Sphere sphere = new Sphere(sphereCenter, radius);
                if (_includeSpheres)
                    _shapes.Add(sphere);
            }
            radius = 0;
            for (int i = 0; i < numberOfSpheres; i++)
            {
                radius += radiusStep;
                Point sphereCenter = new Point(radiusStep + (radiusStep * i) / Math.Sqrt(2), radiusStep + (radiusStep * i) / Math.Sqrt(2), 0);
                Sphere sphere = new Sphere(sphereCenter, radius);
                if (_includeSpheres)
                    _shapes.Add(sphere);
            }

            double planeStep = .2;
            double planeStartX = _camera.Screen.Width / 2;
            double planeStartY = _camera.Screen.Height;
            int numberOfPlanes = 5;
            for (int i = 1; i <= numberOfPlanes; i++)
            {
                Point planePoint = new Point(planeStartX, planeStartY, 0);
                Vector planeVector = new Vector(i * planeStep, -1 + i * planeStep, 0);
                Plane plane = new Plane(planePoint, planeVector);
                if (_includePlanes)
                    _shapes.Add(plane);
            }

            int numberOfCylinders = 20;
            double cylinderStartX = 3 * _camera.Screen.Width / 4;
            double cylinderStartY = 3 * _camera.Screen.Height / 4;
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
                Cylinder cylinder = new Cylinder(cylinderPoint, cylinderDirection, radius);
                if (_includeCynlinders)
                    _shapes.Add(cylinder);
            }
            for (int i = 0; i < numberOfCylinders; i++)
            {
                radius = radiusStep;
                twist = twistStep * i;
                Point cylinderPoint = new Point(cylinderStartX, cylinderStartY, 0);
                Vector cylinderDirection = new Vector(1 + twist, 1 - twist, .5);
                Cylinder cylinder = new Cylinder(cylinderPoint, cylinderDirection, radius);
                if (_includeCynlinders)
                    _shapes.Add(cylinder);
            }
            for (int i = 0; i < numberOfCylinders; i++)
            {
                radius = radiusStep;
                twist = twistStep * i;
                Point cylinderPoint = new Point(cylinderStartX, cylinderStartY, 0);
                Vector cylinderDirection = new Vector(-1 + twist, 1 + twist, .5);
                Cylinder cylinder = new Cylinder(cylinderPoint, cylinderDirection, radius);
                if (_includeCynlinders)
                    _shapes.Add(cylinder);
            }
            for (int i = 0; i < numberOfCylinders; i++)
            {
                radius = radiusStep;
                twist = twistStep * i;
                Point cylinderPoint = new Point(cylinderStartX, cylinderStartY, 0);
                Vector cylinderDirection = new Vector(-1 - twist, 1 - twist, .5);
                Cylinder cylinder = new Cylinder(cylinderPoint, cylinderDirection, radius);
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
            else if (e.Key == Key.Q)
            {
                _camera.MoveBack(_moveAmount);
                Render();
            }
            else if (e.Key == Key.E)
            {
                _camera.MoveForward(_moveAmount);
                Render();
            }
            else if (e.Key == Key.W)
            {
                _camera.MoveUp(_moveAmount);
                Render();
            }
            else if (e.Key == Key.S)
            {
                _camera.MoveDown(_moveAmount);
                Render();
            }
            else if (e.Key == Key.A)
            {
                _camera.MoveLeft(_moveAmount);
                Render();
            }
            else if (e.Key == Key.D)
            {
                _camera.MoveRight(_moveAmount);
                Render();
            }
            else if (e.Key == Key.Up)
            {
                _camera.TiltUp(15);
                Render();
            }
            else if (e.Key == Key.Down)
            {
                _camera.TiltDown(15);
                Render();
            }
            else if (e.Key == Key.Right)
            {
                _camera.TiltRight(15);
                Render();
            }
            else if (e.Key == Key.Left)
            {
                _camera.TiltLeft(15);
                Render();
            }
        }

        static void RenderOld()
        {
            //Point origin = screen.GetPoint(1, 1); 
            //Point nexPointX = screen.GetPoint(2, 1);
            //Point nexPointY = screen.GetPoint(1, 0);

            //Vector vectorX = Function.GetVector(origin, nexPointX);
            //vectorX.Scale(1 / (double)antiAliasX);
            //Vector vectorY = Function.GetVector(origin, nexPointY);
            //vectorY.Scale(1 / (double)antiAliasY);

            Color white = new Color(1, 1, 1);
            for (int i = 1; i <= w.ActualWidth; i++)
            {
                for (int j = 1; j <= w.ActualHeight; j++)
                {
                    Dictionary<Point, Shape> intersectionPoints = new Dictionary<Point, Shape>();
                    foreach (Shape shape in _shapes)
                    {
                        Point point = shape.Intersection(_camera.CameraPoint, _camera.GetRay(i, j));
                        if (point != null)
                            intersectionPoints.Add(point, shape);
                    }
                    if (intersectionPoints.Count > 0)
                    {
                        Point closest = null;
                        double closestDistance = 100000000;
                        foreach(Point point in intersectionPoints.Keys)
                        {
                            double distance = (point - _camera.CameraPoint).Length();
                            if(distance < closestDistance)
                            {
                                closest = point;
                                closestDistance = distance;
                            }
                        }
                        _camera.Screen.Pixels[i - 1][j - 1].Set(intersectionPoints[closest].Color());
                    }
                    else
                        _camera.Screen.Pixels[i - 1][j - 1].None();

                    //Point point = screen.GetPoint(i, j);

                    /*
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
                    */
                    //}
                }
            }

            //_camera.Screen.Normalize();
           
            UpdateImage();            
        }

        static void Render()
        {
            Thread thread = new Thread(new ThreadStart(() =>
            {
                List<Thread> threads = new List<Thread>();
                int xSubSections = 2;
                int ySubSections = 2;
                int xWidth = _camera.Screen.Pixels.Length / xSubSections;
                int yWidth = _camera.Screen.Pixels[0].Length / ySubSections;
                for (int i = 0; i < xSubSections; i++)
                    for (int j = 0; j < ySubSections; j++)
                    {
                        threads.Add(new Thread(new ParameterizedThreadStart(RenderSection)));
                        threads[threads.Count - 1].Start(new int[] { 1 + i * xWidth, 1 + j * yWidth, xWidth, yWidth });
                    }

                for (int i = 0; i < threads.Count; i++)
                    threads[i].Join();

                Application.Current.Dispatcher.Invoke(UpdateImage);
            }));
            thread.Start();
        }

        static void RenderSection(Object obj)
        {
            int[] parameters = (int[])obj;

            int xStart = parameters[0];
            int yStart = parameters[1];
            int xWidth = parameters[2];
            int yWidth = parameters[3];

            for (int i = xStart; i < xStart + xWidth; i++)
            {
                for (int j = yStart; j < yStart + yWidth; j++)
                {
                    Dictionary<Point, Shape> intersectionPoints = new Dictionary<Point, Shape>();
                    foreach (Shape shape in _shapes)
                    {
                        Point point = shape.Intersection(_camera.CameraPoint, _camera.GetRay(i, j));
                        if (point != null)
                            intersectionPoints.Add(point, shape);
                    }
                    if (intersectionPoints.Count > 0)
                    {
                        Point closest = null;
                        double closestDistance = 100000000;
                        foreach (Point point in intersectionPoints.Keys)
                        {
                            double distance = (point - _camera.CameraPoint).Length();
                            if (distance < closestDistance)
                            {
                                closest = point;
                                closestDistance = distance;
                            }
                        }
                        if(closest != null)
                            _camera.Screen.SetPixel( i, j, intersectionPoints[closest].Color());
                        else
                            _camera.Screen.ErasePixel(i, j);
                    }
                    else
                        _camera.Screen.ErasePixel(i, j);
                }
            }
        }

        /*
        static void Render()
        {

            writeableBitmap.Lock();
            int pBackBuffer = (int)writeableBitmap.BackBuffer;

            Color black = new Color();
            for (int i = 1; i <= w.ActualWidth; i++)
            {
                for (int j = 1; j <= w.ActualHeight; j++)
                {
                    Dictionary<Point, Shape> intersectionPoints = new Dictionary<Point, Shape>();
                    foreach (Shape shape in _shapes)
                    {
                        Point point = shape.Intersection(_camera.CameraPoint, _camera.GetRay(i, j));
                        if (point != null)
                            intersectionPoints.Add(point, shape);
                    }
                    if (intersectionPoints.Count > 0)
                    {
                        Point closest = null;
                        double closestDistance = 100000000;
                        foreach (Point point in intersectionPoints.Keys)
                        {
                            double distance = (point - _camera.CameraPoint).Length();
                            if (distance < closestDistance)
                            {
                                closest = point;
                                closestDistance = distance;
                            }
                        }
                        //_camera.Screen.Pixels[i - 1][j - 1].Set(intersectionPoints[closest].Color());
                        int color_data = intersectionPoints[closest].Color().GetColorInt();
                        unsafe
                        {
                            *((int*)pBackBuffer) = color_data;
                        }
                    }
                    else
                        unsafe
                        {
                            *((int*)pBackBuffer) = black.GetColorInt();
                        }
                    //_camera.Screen.Pixels[i - 1][j - 1].None();


                    pBackBuffer += 4;
                }
            }

            //_camera.Screen.Normalize();

            //UpdateImage();
            writeableBitmap.AddDirtyRect(new Int32Rect(0, 0, (int)_camera.Screen.Width, (int)_camera.Screen.Height));

            writeableBitmap.Unlock();
        }
        */
        static public void UpdateImage()
        {
            writeableBitmap.Lock();
            
            int pBackBuffer = (int)writeableBitmap.BackBuffer;

            /*
            for (int i = 1; i <= _camera.Screen.Pixels.Length; i++)
                for (int j = 1; j <= _camera.Screen.Pixels[i - 1].Length; j++)
                {
                    unsafe
                    {
                        int color_data = _camera.Screen.Pixels[j - 1][i - 1].GetColorInt();
                        *((int*)pBackBuffer) = color_data;
                        pBackBuffer += 4;
                    }
                }
            */

            Marshal.Copy(_camera.Screen.PixelArray, 0, writeableBitmap.BackBuffer, (int)(_camera.Screen.Width * _camera.Screen.Height));
            
            writeableBitmap.AddDirtyRect(new Int32Rect(0, 0, (int)_camera.Screen.Width, (int)_camera.Screen.Height));

            writeableBitmap.Unlock();
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
            Point point = _camera.Screen.GetPoint((int)e.GetPosition(image).X, (int)e.GetPosition(image).Y);
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