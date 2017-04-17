using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project10
{
    class Camera
    {
        public Camera(Point cameraPoint, Vector viewVector, Vector upVector, double screenDistance, double screenWidth, double screenHeight, double scaleX, double scaleY)
        {
            _cameraPoint = cameraPoint;
            _viewVector = viewVector.UnitVector();
            _upVector = upVector.UnitVector();

            _screenDistance = screenDistance;

            ConfigureScreenVectors();

            Point screenCenter = _cameraPoint + _viewVector * _screenDistance;
            Point screenOrigin = screenCenter - (_xVector * scaleX / 2) - (_yVector * scaleY / 2);
            VisibileScreen = new Screen(screenWidth, screenHeight, screenOrigin, _xVector, scaleX, _yVector, scaleY);
            CurrentScreen = new Screen(screenWidth, screenHeight, screenOrigin, _xVector, scaleX, _yVector, scaleY);
            Screens = new List<Screen>();
            Screens.Add(CurrentScreen);

        }

        private void ConfigureScreenVectors()
        {
            _xVector = Function.CrossProduct(_viewVector, _upVector).UnitVector();
            _yVector = Function.CrossProduct(_xVector, _viewVector).UnitVector();
        }

        public Vector GetRay(double pixelX, double pixelY)
        {
            Point screenPoint = CurrentScreen.GetPoint(pixelX, pixelY);
            return (screenPoint - _cameraPoint).UnitVector();
        }

        public void MoveBack(double distance)
        {
            _cameraPoint -= distance * _viewVector;
            ResetScreen();
        }
        public void MoveForward(double distance)
        {
            _cameraPoint += distance * _viewVector;
            ResetScreen();
        }
        public void MoveRight(double distance)
        {
            _cameraPoint += distance * _xVector;
            ResetScreen();
        }
        public void MoveLeft(double distance)
        {
            _cameraPoint -= distance * _xVector;
            ResetScreen();
        }
        public void MoveUp(double distance)
        {
            _cameraPoint += distance * _yVector;
            ResetScreen();
        }
        public void MoveDown(double distance)
        {
            _cameraPoint -= distance * _yVector;
            ResetScreen();
        }


        public void TiltDown(double angle)
        {
            double angleRadians = angle * Math.PI / 180;
            double scaling = Math.Tan(angleRadians);
            _viewVector = (_viewVector - _yVector * scaling).UnitVector();

            Vector perpendicularToView = Function.CrossProduct(_xVector, _upVector);
            _upVector = (_upVector - perpendicularToView * scaling).UnitVector();
            ConfigureScreenVectors();
            ResetScreen();
        }

        public void TiltUp(double angle)
        {
            double angleRadians = angle * Math.PI / 180;
            double scaling = Math.Tan(angleRadians);
            _viewVector = (_viewVector + _yVector * scaling).UnitVector();

            Vector perpendicularToView = Function.CrossProduct(_xVector, _upVector);
            _upVector = (_upVector + perpendicularToView * scaling).UnitVector();
            ConfigureScreenVectors();
            ResetScreen();
        }
        public void TiltLeft(double angle)
        {
            double angleRadians = angle * Math.PI / 180;
            double scaling = Math.Tan(angleRadians);
            _viewVector = (_viewVector - _xVector * scaling).UnitVector();

            double upScaling = Function.DotProduct(_upVector, scaling * _xVector);
            _upVector = (_upVector - _xVector * upScaling).UnitVector();
            ConfigureScreenVectors();
            ResetScreen();
        }

        public void TiltRight(double angle)
        {
            double angleRadians = angle * Math.PI / 180;
            double scaling = Math.Tan(angleRadians);
            _viewVector = (_viewVector + _xVector * scaling).UnitVector();

            double upScaling = Function.DotProduct(_upVector, scaling * _xVector);
            _upVector = (_upVector + _xVector * upScaling).UnitVector();
            ConfigureScreenVectors();
            ResetScreen();
        }

        public void ResetScreen()
        {
            Point screenCenter = _cameraPoint + _viewVector * _screenDistance;
            Point screenOrigin = screenCenter - (_xVector * CurrentScreen.XScale / 2) - (_yVector * CurrentScreen.YScale / 2);
            CurrentScreen.Set(screenOrigin, _xVector, _yVector);
        }

        private double _screenDistance;
        private Vector _viewVector;
        private Vector _upVector;
        private Vector _xVector;
        private Vector _yVector;
        private Point _cameraPoint;
        public Point CameraPoint
        {
            get
            {
                return _cameraPoint;
            }
        }

        public void NextScreen()
        {
            Screens.Add(new Screen(
                CurrentScreen.Width, 
                CurrentScreen.Height,
                CurrentScreen.Point,
                CurrentScreen.XVector,
                CurrentScreen.XScale,
                CurrentScreen.YVector,
                CurrentScreen.YScale
                ));
            CurrentScreen = Screens[Screens.Count - 1];
        }

        public void MakeScreen()
        {
            double[] weights = new double[Screens.Count];
            double sum = Screens.Count * (Screens.Count + 1) /2;
            for (int i = 1; i <= weights.Count(); i++)
                weights[i - 1] = i / sum;

            for (int i = 0; i < CurrentScreen.Width; i++)
            {
                for (int j = 0; j < CurrentScreen.Height; j++)
                {
                    Color color = new Color();
                    for (int k = 0; k < Screens.Count; k++)
                    {
                        Color addColor = Screens[k].Pixels[i][j] * weights[k];
                        addColor.Omega *= weights[k];
                        color = color + addColor;
                    }

                    VisibileScreen.SetPixel(i + 1, j + 1, color);
                }
            }            
        }

        public Screen CurrentScreen { get; set; }
        public List<Screen> Screens { get; set; }
        public Screen VisibileScreen { get; set; }
    }
}
