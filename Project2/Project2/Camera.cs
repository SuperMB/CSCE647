using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project2
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
            Screen = new Screen(screenWidth, screenHeight, screenOrigin, _xVector, scaleX, _yVector, scaleY);

        }

        private void ConfigureScreenVectors()
        {
            _xVector = Function.CrossProduct(_viewVector, _upVector).UnitVector();
            _yVector = Function.CrossProduct(_xVector, _viewVector).UnitVector();
        }

        public Vector GetRay(int pixelX, int pixelY)
        {
            Point screenPoint = Screen.GetPoint(pixelX, pixelY);
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

            //_upVector = (_upVector - _xVector * scaling).UnitVector();
            ConfigureScreenVectors();
            ResetScreen();
        }

        public void TiltRight(double angle)
        {
            double angleRadians = angle * Math.PI / 180;
            double scaling = Math.Tan(angleRadians);
            _viewVector = (_viewVector + _xVector * scaling).UnitVector();

            //_upVector = (_upVector + _xVector * scaling).UnitVector();
            ConfigureScreenVectors();
            ResetScreen();
        }

        public void ResetScreen()
        {
            Point screenCenter = _cameraPoint + _viewVector * _screenDistance;
            Point screenOrigin = screenCenter - (_xVector * Screen.XScale / 2) - (_yVector * Screen.YScale / 2);
            Screen.Set(screenOrigin, _xVector, _yVector);
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

        public Screen Screen { get; set; }
    }
}
