using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
namespace MovingObject
{
    class Moving_Object
    {
        Pen BlackPen = new Pen(Color.Black);
        Pen GreenPen = new Pen(Color.Green);
        Matrix Moving_Matrix;
        SolidBrush GreenBrush = new SolidBrush(Color.Green);
        Graphics G_moving;
        Point point1;
        Point point2;
        Point point3;
        Point[] curvePoints;
        PointF FloatPoint;
        FillMode newFillMode;
        private int _Init_X;
        private int _x2;
        private int _Init_Y;
        private int _y2;
        private int _angle;
        const int Robot_Height = 20;
        const int Robot_Width = 40;
        private float Robot_Center_Point_X;
        private float Robot_Center_Point_Y;
        public Moving_Object(Graphics _Gmoving)
        {
            G_moving = _Gmoving;
            Moving_Matrix = new Matrix(1, 0, 0, 1, 0, 0);
            Robot_Center_Point_X = Robot_Width/2 + _Init_X;
            
            point1 = new Point(10, 10);
            point2 = new Point(10, 20);
            point3 = new Point(30, 15);
            curvePoints = new Point[3];
            curvePoints[0] = point1;
            curvePoints[1] = point2;
            curvePoints[2] = point3;
            newFillMode = FillMode.Winding;
        }
        public void Moving(int Angle, int x, int y)
        {
            if (Angle != 0)
            {
                FloatPoint = new PointF(Robot_Width / 2, Robot_Height / 2);
                Moving_Matrix.RotateAt(Angle, FloatPoint, MatrixOrder.Append);
                G_moving.Transform = Moving_Matrix;
                G_moving.FillPolygon(GreenBrush, curvePoints, newFillMode);
            }
            else
            {
                Moving_Matrix.Translate(x, y, MatrixOrder.Append);
                G_moving.Transform = Moving_Matrix;
                G_moving.FillPolygon(GreenBrush, curvePoints, newFillMode);
            }
        }
    }
}
