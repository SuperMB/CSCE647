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

namespace Project4
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

        static int _antiAliasX;
        static int _antiAliasY;
        static int _antiAliasXValues;
        static int _antiAliasYValues;
        static bool _antiAlias;
        static bool _illuminate;

        static List<Shape> _shapes;
        static List<Shape> _intersectingShapes;
        static List<Light> _lights;

        static Camera _camera;

        static double _moveAmount;

        static Random _random;
        static readonly double _alpha = 2;

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

            Point cameraPoint = new Point(0, 0, 0);
            Vector viewVector = new Vector(0, 0, -1);
            Vector upVector = new Vector(0, 1, 0);
            double screenDistance = 400;
            _camera = new Camera(cameraPoint, viewVector, upVector, screenDistance, w.ActualWidth, w.ActualHeight, w.ActualWidth, w.ActualHeight);

            _shapes = new List<Shape>();
            _intersectingShapes = new List<Shape>();
            _lights = new List<Light>();
            _random = new Random();
            _illuminate = true;

            //**************************************************************************************
            //**************************************************************************************
            //antiAlias is a boolean value, for anti alias set to true, for not anti alias, set to false
            _antiAlias = false;
            _antiAliasXValues = 1;// 4;
            _antiAliasYValues = 1;// 2;
            SetAntiAlias();

            Point pokeballCenter = new Point(0, 0, -600);
            Vector pokeballUpVector = new Vector(0, 1, 0);
            Vector pokeballOutVector = new Vector(1, 1, 7);
            double pokeballRadius = 300;
            Pokeball pokeball = new Pokeball(
                pokeballCenter, 
                pokeballRadius, 
                pokeballUpVector, 
                pokeballOutVector);
            _shapes.Add(pokeball);
            _intersectingShapes.Add(pokeball.Sphere);
            _intersectingShapes.Add(pokeball.Dot);

            Vector pokeballPerpendicularVector = Function.CrossProduct(pokeball.OutVector, pokeball.UpVector).UnitVector();

            double diffusedLightIntensity = 3;
            DiffusedLight diffusedLight = new DiffusedLight
            {
                Point = new Point(500, 200, -100),
                LightColor = new Color(diffusedLightIntensity)
            };
            _lights.Add(diffusedLight);

            Point specularHighlightPoint = pokeball.Sphere.Center + (pokeballPerpendicularVector + pokeball.OutVector / 3 + 2 * pokeball.UpVector).UnitVector() * pokeball.Sphere.Radius * 4;
            //specularHighlightPoint.Z += 700;
            //Point specularHighlightPoint = new Point(500, 200, 100); // pokeball.Sphere.Center + (pokeballPerpendicularVector + pokeball.OutVector / 3 + 2 * pokeball.UpVector).UnitVector() * pokeball.Sphere.Radius * 4;
            double specularHighlightIntensity = 30;
            SpecularHighlight specularHighlight = new SpecularHighlight
            {
                Point = specularHighlightPoint,
                LightColor = new Color(specularHighlightIntensity),
                EyePoint = _camera.CameraPoint,
                Minimum = Math.Cos(Function.Degrees(25)),
                Maximum = Math.Cos(Function.Degrees(2))
            };
            _lights.Add(specularHighlight);

            //Point spotlightPoint = pokeballCenter + (pokeballPerpendicularVector + pokeball.UpVector * 2 + pokeball.OutVector * 2).UnitVector() * pokeballRadius * 1.5;
            Point spotlightPoint = pokeballCenter + (pokeball.OutVector).UnitVector() * pokeballRadius * 1.5;
            double spotLightIntensity = 20;
            SpotLight spotLight = new SpotLight
            {
                Point = spotlightPoint,
                Direction = (pokeballCenter - spotlightPoint).UnitVector(),
                LightColor = new Color(spotLightIntensity),
                Angle = Function.Degrees(35),
                Buffer = Function.Degrees(15)
            };
            _lights.Add(spotLight);


            Point planePoint = new Point(0, 0, -700);
            Vector planeVector = new Vector(0, 0, 1);
            Plane plane = new Plane(planePoint, planeVector);
            plane.SetColorMatrix(ColorMatrix.White());
            _shapes.Add(plane);
            _intersectingShapes.Add(plane);



            _moveAmount = 100;
            Application app = new Application();
            app.Run();
        }

        static void SetAntiAlias()
        {
            if (_antiAlias)
            {
                _antiAliasX = _antiAliasXValues;
                _antiAliasY = _antiAliasYValues;
            }
            else
            {
                _antiAliasX = 1;
                _antiAliasY = 1;
            }
        }
        
        static void KeyPress(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Render();
                Thread.Sleep(1000);
                using (FileStream file = new FileStream("image.png", FileMode.Create))
                {
                    PngBitmapEncoder pngBitmapEncoder = new PngBitmapEncoder();
                    pngBitmapEncoder.Frames.Add(BitmapFrame.Create(writeableBitmap));
                    pngBitmapEncoder.Save(file);
                }
            }
            else if(e.Key == Key.LeftShift)
            {
                _antiAlias = !_antiAlias;
                SetAntiAlias();
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

        static void Render()
        {
            Thread thread = new Thread(new ThreadStart(() =>
            {
                List<Thread> threads = new List<Thread>();
                int xSubSections = 4;
                int ySubSections = 4;
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
        
        static int Closest(List<ReturnData> intersectionPoints)
        {
            int index = -1;

            if (intersectionPoints.Count < 1)
                return index;

            double closestDistance = 100000000;
            for (int k = 0; k < intersectionPoints.Count; k++)
            {
                ReturnData pointColor = intersectionPoints[k];
                double distance = (pointColor.Point - _camera.CameraPoint).Length();
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    index = k;
                }
            }

            return index;
        }

        static Color Illuminate(ReturnData returnData)
        {
            if (!_illuminate)
                return returnData.ColorMatrix.Flatten();

            Color color = new Color(0, 0, 0, 0);
            foreach (Light light in _lights)
                color += light.ShineOnShape(returnData, _alpha, _intersectingShapes);

            return color;
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
                    double randomX = _random.NextDouble() / _antiAliasX;
                    double randomY = _random.NextDouble() / _antiAliasY;
                    Color color = new Color(0, 0, 0, 0);
                    for (int k = 0; k < _antiAliasX; k++)
                    {
                        for (int n = 0; n < _antiAliasY; n++)
                        {
                            List<ReturnData> intersectionPoints = Function.IntersectionPoints(_camera.CameraPoint, _camera.GetRay(i + randomX * k, j + randomY * n), _shapes);

                            int closestIndex = Closest(intersectionPoints);
                            if (closestIndex >= 0)
                            {
                                if (intersectionPoints[closestIndex].ColorMatrix.IsHighlight)
                                    color += intersectionPoints[closestIndex].ColorMatrix.Flatten();
                                else
                                    color += Illuminate(intersectionPoints[closestIndex]);
                            }
                        }
                    }

                    color /= _antiAliasX * _antiAliasY;
                    color.Omega /= _antiAliasX * _antiAliasY;
                    if (color.Omega > 0)
                        _camera.Screen.SetPixel(i, j, color);
                    else
                        _camera.Screen.ErasePixel(i, j);
                }
            }
        }

     
        static public void UpdateImage()
        {
            writeableBitmap.Lock();
            
            int pBackBuffer = (int)writeableBitmap.BackBuffer;

            Marshal.Copy(_camera.Screen.PixelArray, 0, writeableBitmap.BackBuffer, (int)(_camera.Screen.Width * _camera.Screen.Height));
            
            writeableBitmap.AddDirtyRect(new Int32Rect(0, 0, (int)_camera.Screen.Width, (int)_camera.Screen.Height));

            writeableBitmap.Unlock();
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