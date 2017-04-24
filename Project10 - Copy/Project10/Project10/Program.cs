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

namespace Project10
{
    class Program
    {
        static WriteableBitmap writeableBitmap;
        static Window w;
        static Image image;

        static int _antiAliasX;
        static int _antiAliasY;
        static int _antiAliasXValues;
        static int _antiAliasYValues;
        static bool _antiAlias;
        static bool _illuminate;
        static bool _multiThread;

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
        static RefractiveSphere _refractiveSphere;

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

            Point cameraPoint = new Point(0, 0, 1500);
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
            _antiAliasXValues = 4;
            _antiAliasYValues = 2;
            SetAntiAlias();

            Tetrahedron tetrahedron = new Tetrahedron(
                new Point(400, 200, -300),
                400
                );
            //foreach (Triangle triangle in tetrahedron.Triangles)
            //    _intersectingShapes.Add(triangle);
            //_shapes.Add(tetrahedron);

            Cube cube = new Cube(
                new Point(300, -200, 400),
                new Vector(0, 1, 0),
                new Vector(1, -1, 3),
                //new Vector(1, 0, 0),
                300);
            //foreach (Triangle triangle in cube.Triangles)
            //    _intersectingShapes.Add(triangle);
            //_shapes.Add(cube);

            List<Triangle> triangles = ObjReader.ReadObjFile("Data.obj");
            foreach (Triangle triangle in triangles)
            {
                //_intersectingShapes.Add(triangle);
                //_shapes.Add(triangle);
            }

            //Triangle triangle = new Triangle(
            //    new Point(0, 0, -650),
            //    new Point(0, 800, -500),
            //    new Point(800, 0, -400)
            //    );
            //_shapes.Add(triangle);
            //_intersectingShapes.Add(triangle);

            //double pokeballRadius = 150;
            //double pokeballY = -450;
            //Point center = new Point(0, 0, -600);

            //Point greatballCenter = new Point(-500, pokeballY, -600);
            //Vector greatballUpVector = new Vector(0, 1, 0);
            //Vector greatballOutVector = new Vector(2, -1, 3);
            //Pokeball greatball = new Pokeball(
            //    greatballCenter, 
            //    pokeballRadius,
            //    greatballUpVector,
            //    greatballOutVector,
            //    new ImageData("GreatBallScratch.png"),
            //    new ImageData("PokeballNormalMapTopScratch.png"),
            //    new ImageData("PokeballNormalMapBottom.png")
            //    );
            //_pokeball = greatball;
            ////_shapes.Add(greatball);
            ////_intersectingShapes.Add(greatball.Sphere);
            ////_intersectingShapes.Add(greatball.Dot);

            //Vector pokeballPerpendicularVector = Function.CrossProduct(greatball.OutVector, greatball.UpVector).UnitVector();


            //Point ultraballCenter = new Point(700, pokeballY, -800);
            //Vector ultraballUpVector = new Vector(0, 1, 3);
            //Vector ultraballOutVector = new Vector(-7, 2, 6);
            //Pokeball ultraball = new Pokeball(
            //    ultraballCenter,
            //    pokeballRadius,
            //    ultraballUpVector,
            //    ultraballOutVector,
            //    new ImageData("UltraBall.png"),
            //    new ImageData("PokeballNormalMapTop.png"),
            //    new ImageData("PokeballNormalMapBottom.png")
            //    );
            ////_shapes.Add(ultraball);
            ////_intersectingShapes.Add(ultraball.Sphere);
            ////_intersectingShapes.Add(ultraball.Dot);


            //Point masterballCenter = new Point(-400, pokeballY, -1200);
            //Vector masterballUpVector = new Vector(1, 4, 0);
            //Vector masterballOutVector = new Vector(3, -2, 6);
            //Pokeball masterball = new Pokeball(
            //    masterballCenter,
            //    pokeballRadius,
            //    masterballUpVector,
            //    masterballOutVector,
            //    new ImageData("MasterBall.png"),
            //    new ImageData("PokeballNormalMapTop.png"),
            //    new ImageData("PokeballNormalMapBottom.png")
            //    );
            ////_shapes.Add(masterball);
            ////_intersectingShapes.Add(masterball.Sphere);
            ////_intersectingShapes.Add(masterball.Dot);

            Sphere sphere = new Sphere(
                new Point(-300, 0, 100),
                300
                );
            //_shapes.Add(sphere);
            //_intersectingShapes.Add(sphere);

            ReflectiveSphere reflectiveSphere = new ReflectiveSphere(
                new Point(400, 0, 0),
                300,
                _shapes
                );
            //_shapes.Add(reflectiveSphere);
            //_intersectingShapes.Add(reflectiveSphere);

            IrridescentSphere irridescentSphere = new IrridescentSphere(
                new Point(600, 500, 0),
                300
                );
            //_shapes.Add(irridescentSphere);
            //_intersectingShapes.Add(irridescentSphere);

            ImageData Smooth = new ImageData("Smooth.png");
            ImageData MasterBallTexture = new ImageData("MasterBall.png");
            ImageData PokeBallTop = new ImageData("PokeballNormalMapTop.png");


            Point irridescentCenter = new Point(400, -700, 0);
            Vector irridescentUpVector = new Vector(0, 1, 0);
            Vector irridescentOutVector = new Vector(3, -1, 4);
            IrridescentReflectivePokeball irridescentPokeball = new IrridescentReflectivePokeball(
                irridescentCenter,
                300,
                irridescentUpVector,
                irridescentOutVector,
                MasterBallTexture,
                PokeBallTop,
                Smooth,
                _shapes
                );
            //_shapes.Add(irridescentPokeball);
            //_intersectingShapes.Add(irridescentPokeball.Sphere);
            //_intersectingShapes.Add(irridescentPokeball.Dot);

            //Point reflectiveCenter = new Point(0, -700, 0);
            Point glossyCenter = new Point(400, -700, 0);
            Vector glossyUpVector = new Vector(4, -1, 8);
            Vector glossyOutVector = new Vector(3, -1, 4).UnitVector();
            //Vector reflectiveOutVector = (cameraPoint - reflectiveCenter).UnitVector();
            GlossyPokeball glossyPokeball = new GlossyPokeball(
                glossyCenter,
                300,
                glossyUpVector,
                glossyOutVector,
                MasterBallTexture,
                Smooth,
                Smooth,
                _shapes
                );
            _shapes.Add(glossyPokeball);
            _intersectingShapes.Add(glossyPokeball.Sphere);
            _intersectingShapes.Add(glossyPokeball.Dot);


            Point pokeballCenter = new Point(-700, -700, -1275);
            Vector pokeballUpVector = new Vector(0, 1, 0);
            Pokeball pokeball = new Pokeball(
                pokeballCenter,
                300,
                pokeballUpVector,
                (cameraPoint - pokeballCenter).UnitVector() + new Vector(2, 0, 0),
                MasterBallTexture,
                Smooth,
                Smooth,
                true
                );
            _shapes.Add(pokeball);
            _intersectingShapes.Add(pokeball.Sphere);
            _intersectingShapes.Add(pokeball.Dot);
            _pokeball = pokeball;

            Point pokeball2Center = new Point(700, -700, -1275);
            Vector pokeball2UpVector = new Vector(0, 1, 0);
            Pokeball pokeball2 = new Pokeball(
                pokeball2Center,
                300,
                pokeball2UpVector,
                new Vector(-1,-1,1),
                MasterBallTexture,
                Smooth,
                Smooth,
                true
                );
            //_shapes.Add(pokeball2);
            //_intersectingShapes.Add(pokeball2.Sphere);
            //_intersectingShapes.Add(pokeball2.Dot);

            RefractiveSphere refractiveSphere = new RefractiveSphere(
                new Point(400, 0, -300),
                500,
                new Vector(0, 1, 0),
                new Vector(0, 0, 1),
                PokeBallTop,
                PokeBallTop,
                null, //Smooth, //new ImageData("RefractiveIndex.png"),
                _shapes
                );
            //_shapes.Add(refractiveSphere);
            //_intersectingShapes.Add(refractiveSphere);

            TranslucentSquare translucentSquare = new TranslucentSquare(
                new Point(-1600, 1000, -900),
                new Point(-1600, -1000, -950),
                new Point(-500, -1000, -950),
                new Point(-500, 1000, -900),
                _shapes
                );
            _shapes.Add(translucentSquare);
            _intersectingShapes.Add(translucentSquare);
            //_refractiveSphere = refractiveSphere;

            ImageData SkyTexture = new ImageData();
            SkyTexture.MakeSky();
            EnvironmentSphere environmentSPhere = new EnvironmentSphere(
                new Point(0, 0, -30000),
                50000,
                new Vector(0, 1, 0),
                new Vector(0,0,1),
                SkyTexture
                );
            environmentSPhere.SetColor(Color.BlueColor);
            _shapes.Add(environmentSPhere);



            double diffusedLightIntensity = 6;
            DiffusedLight diffusedLight = new DiffusedLight
            {
                Point = new Point(0, 100, 100),
                LightColor = new Color(diffusedLightIntensity)
            };
            _lights.Add(diffusedLight);




            Plane backPlane = new Plane(
                new Point(0, 0, -3000),
                new Vector(0, 0, 1)
                );
            backPlane.SetColor(Color.WhiteColor);
            //_shapes.Add(backPlane);
            //_intersectingShapes.Add(backPlane);

            Plane topPlane = new Plane(
                new Point(0, 3000, 0),
                new Vector(0, -1, 0)
                );
            topPlane.SetColor(new Color(1, .5, 0));
            //_shapes.Add(topPlane);
            //_intersectingShapes.Add(topPlane);

            Plane bottomPlane = new Plane(
                new Point(0, -1000, 0),
                new Vector(0, 1, 0),
                new Vector(0, 0, -1),
                new ImageData("Grass.png"),
                Smooth
                );
            bottomPlane.SetColor(new Color(.5, 1, 0));
            _shapes.Add(bottomPlane);
            _intersectingShapes.Add(bottomPlane);

            //Plane leftPlane = new Plane(
            //    new Point(-3000, 0, 0),
            //    new Vector(1, 0, 0)
            //    );
            //leftPlane.SetColor(new Color(1, 0, .5));
            //_shapes.Add(leftPlane);
            //_intersectingShapes.Add(leftPlane);

            //Plane rightPlane = new Plane(
            //    new Point(3000, 0, 0),
            //    new Vector(-1, 0, 0)
            //    );
            //rightPlane.SetColor(new Color(.5, 0, 1));
            //_shapes.Add(rightPlane);
            //_intersectingShapes.Add(rightPlane);

            _multiThread = true;

            _moveAmount = 300;


            Function._illuminate = _illuminate;
            Function._lights = _lights;
            Function._alpha = _alpha;
            Function._intersectingShapes = _intersectingShapes;

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

        static void Animate()
        {
            Thread thread = new Thread(new ThreadStart(() =>
            {
                _pokeball.Direction = new Vector(10, 0, 0);
                int numberOfFrames = 1;
                for (int i = 0; i < numberOfFrames; i++)
                {
                    if (i > 0)
                    {
                        _camera.NextScreen();
                        _pokeball.Move();
                    }

                    Render();
                    //UpdateImage();
                    //Thread.Sleep(1000);
                }
                Application.Current.Dispatcher.Invoke(UpdateImage);
            }));
            //Thread.Sleep(2000);
            thread.Start();
        }
        
        static void KeyPress(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                //_refractiveSphere.RefractiveIndexData = new ImageData("RefractiveIndex.png");
                //Render();
                Animate();
                //Thread.Sleep(1000);
                //using (FileStream file = new FileStream("image.png", FileMode.Create))
                //{
                //    PngBitmapEncoder pngBitmapEncoder = new PngBitmapEncoder();
                //    pngBitmapEncoder.Frames.Add(BitmapFrame.Create(writeableBitmap));
                //    pngBitmapEncoder.Save(file);
                //}

                //_pokeball.Evaluate();
            }
            else if(e.Key == Key.R)
            {
                //_pokeball.Texture.ReloadTexture();
                //_pokeball.NormalMapTop.ReloadTexture();
                //_pokeball.NormalMapBottom.ReloadTexture();
                Render();
            }
            else if(e.Key == Key.Space)
            {
                //_grass.ToggleNormalMap();
                //_sky.ToggleSky();
                //_sky.MakeSky();
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
            //Thread thread = new Thread(new ThreadStart(() =>
            //{
                List<Thread> threads = new List<Thread>();
                int xSubSections = 1;
                int ySubSections = 1;
                if (_multiThread)
                {
                    xSubSections = 4;
                    ySubSections = 2;
                }
                int xWidth = _camera.CurrentScreen.Pixels.Length / xSubSections;
                int yWidth = _camera.CurrentScreen.Pixels[0].Length / ySubSections;
                for (int i = 0; i < xSubSections; i++)
                    for (int j = 0; j < ySubSections; j++)
                    {
                        threads.Add(new Thread(new ParameterizedThreadStart(RenderSection)));
                        threads[threads.Count - 1].Start(new int[] { 1 + i * xWidth, 1 + j * yWidth, xWidth, yWidth });
                    }

                for (int i = 0; i < threads.Count; i++)
                    threads[i].Join();

                //Application.Current.Dispatcher.Invoke(UpdateImage);
            //}));
            //thread.Start();
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

                            int closestIndex = Function.Closest(intersectionPoints, _camera.CameraPoint);
                            if (closestIndex >= 0)
                            {
                                //if (intersectionPoints[closestIndex].ColorMatrix.IsHighlight)
                                //    color += intersectionPoints[closestIndex].ColorMatrix.Flatten();
                                //else
                                    color += Function.Illuminate(intersectionPoints[closestIndex]);
                            }
                        }
                    }

                    color /= _antiAliasX * _antiAliasY;
                    color.Omega /= _antiAliasX * _antiAliasY;
                    if (color.Omega > 0)
                        _camera.CurrentScreen.SetPixel(i, j, color);
                    else
                        _camera.CurrentScreen.ErasePixel(i, j);
                }
            }
        }

     
        static public void UpdateImage()
        {
            _camera.MakeScreen();
            writeableBitmap.Lock();
            
            int pBackBuffer = (int)writeableBitmap.BackBuffer;

            Marshal.Copy(_camera.VisibileScreen.PixelArray, 0, writeableBitmap.BackBuffer, (int)(_camera.VisibileScreen.Width * _camera.VisibileScreen.Height));
            
            writeableBitmap.AddDirtyRect(new Int32Rect(0, 0, (int)_camera.VisibileScreen.Width, (int)_camera.VisibileScreen.Height));

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
            Point point = _camera.CurrentScreen.GetPoint((int)e.GetPosition(image).X, (int)e.GetPosition(image).Y);
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








