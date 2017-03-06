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

namespace Project5
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

        static Pokeball _pokeball;
        static ImageData _texture;
        static Plane _grass;
        static Sky _sky;

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

            Point cameraPoint = new Point(0, 0, 500);
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

            double pokeballRadius = 150;
            double pokeballY = -450;
            Point center = new Point(0, 0, -600);

            Point greatballCenter = new Point(-500, pokeballY, -600);
            Vector greatballUpVector = new Vector(0, 1, 0);
            Vector greatballOutVector = new Vector(2, -1, 3);
            Pokeball greatball = new Pokeball(
                greatballCenter, 
                pokeballRadius,
                greatballUpVector,
                greatballOutVector,
                new ImageData("GreatBallScratch.png"),
                new ImageData("PokeballNormalMapTopScratch.png"),
                new ImageData("PokeballNormalMapBottom.png")
                );
            _pokeball = greatball;
            _shapes.Add(greatball);
            _intersectingShapes.Add(greatball.Sphere);
            _intersectingShapes.Add(greatball.Dot);

            Vector pokeballPerpendicularVector = Function.CrossProduct(greatball.OutVector, greatball.UpVector).UnitVector();


            Point ultraballCenter = new Point(700, pokeballY, -800);
            Vector ultraballUpVector = new Vector(0, 1, 3);
            Vector ultraballOutVector = new Vector(-7, 2, 6);
            Pokeball ultraball = new Pokeball(
                ultraballCenter,
                pokeballRadius,
                ultraballUpVector,
                ultraballOutVector,
                new ImageData("UltraBall.png"),
                new ImageData("PokeballNormalMapTop.png"),
                new ImageData("PokeballNormalMapBottom.png")
                );
            _shapes.Add(ultraball);
            _intersectingShapes.Add(ultraball.Sphere);
            _intersectingShapes.Add(ultraball.Dot);


            Point masterballCenter = new Point(-400, pokeballY, -1200);
            Vector masterballUpVector = new Vector(1, 4, 0);
            Vector masterballOutVector = new Vector(3, -2, 6);
            Pokeball masterball = new Pokeball(
                masterballCenter,
                pokeballRadius,
                masterballUpVector,
                masterballOutVector,
                new ImageData("MasterBall.png"),
                new ImageData("PokeballNormalMapTop.png"),
                new ImageData("PokeballNormalMapBottom.png")
                );
            _shapes.Add(masterball);
            _intersectingShapes.Add(masterball.Sphere);
            _intersectingShapes.Add(masterball.Dot);

            Point pokeballCenter = new Point(200, pokeballY, -400);
            Vector pokeballUpVector = new Vector(-5, 1, 0);
            Vector pokeballOutVector = new Vector(3, 0, 10);
            Pokeball pokeball = new Pokeball(
                pokeballCenter,
                pokeballRadius,
                pokeballUpVector,
                pokeballOutVector,
                new ImageData("MasterBall.png"),
                new ImageData("PokeballNormalMapTop.png"),
                new ImageData("PokeballNormalMapBottom.png"),
                true
                );
            _shapes.Add(pokeball);
            _intersectingShapes.Add(pokeball.Sphere);
            _intersectingShapes.Add(pokeball.Dot);












            double diffusedLightIntensity = 3;
            DiffusedLight diffusedLight = new DiffusedLight
            {
                Point = new Point(500, 200, -100),
                LightColor = new Color(diffusedLightIntensity)
            };
            _lights.Add(diffusedLight);















            Point specularHighlightPoint = greatball.Sphere.Center + (pokeballPerpendicularVector + greatball.OutVector / 3 + 2 * greatball.UpVector).UnitVector() * greatball.Sphere.Radius * 4;
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
            //_lights.Add(specularHighlight);

            //Point spotlightPoint = pokeballCenter + (pokeballPerpendicularVector + pokeball.UpVector * 2 + pokeball.OutVector * 2).UnitVector() * pokeballRadius * 1.5;
            Point spotlightPoint = center + (greatball.OutVector).UnitVector() * pokeballRadius * 1.5;
            double spotLightIntensity = 20;
            SpotLight spotLight = new SpotLight
            {
                Point = spotlightPoint,
                Direction = (center - spotlightPoint).UnitVector(),
                LightColor = new Color(spotLightIntensity),
                Angle = Function.Degrees(35),
                Buffer = Function.Degrees(15)
            };
            //_lights.Add(spotLight);

            
            Point planePoint = new Point(0, -600, -600);
            Vector planeNormalVector = new Vector(0, 1, 0);
            Vector planeUpVector = new Vector(0, 0, -1);
            Plane plane = new Plane(
                planePoint, 
                planeNormalVector, 
                planeUpVector, 
                new ImageData("Wallpaper.png"),
                new ImageData("NormalMap.png")
                );
            plane.SetColorMatrix(ColorMatrix.White());
            _grass = plane;
            _shapes.Add(plane);
            _intersectingShapes.Add(plane);

            _sky = new Sky(
                new Point(0, 0, -2000),
                new Vector(0, 0, 1)
                );
            _shapes.Add(_sky);


            _moveAmount = 300;
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

                _pokeball.Evaluate();
            }
            else if(e.Key == Key.R)
            {
                _pokeball.Texture.ReloadTexture();
                _pokeball.NormalMapTop.ReloadTexture();
                _pokeball.NormalMapBottom.ReloadTexture();
                Render();
            }
            else if(e.Key == Key.Space)
            {
                _grass.ToggleNormalMap();
                _sky.MakeSky();
                Render();
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