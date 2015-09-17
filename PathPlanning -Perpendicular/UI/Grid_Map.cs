using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Drawing;
using System.Data;
using System.Threading;
using multiServerTestV01;
namespace GRID_MAP
{
    struct Node_Cordination
    {
        internal int X;
        internal int Y;
        internal int Pixel_X;
        internal int Pixel_Y;
        internal int Node_ID;
        internal int G_Score; // distance from start_node to the current node
        internal int Distance_Score;// distance from the parent node to the current node
        internal int F_Score;//distance from the node to the End point
        internal int Node_Parent_ID; //if no initial, by default, Node_Parent_Index = 0 (not good because 0 is in the grid);
        internal bool Obstacle;
    }
    class Grid_Map
    {
        private ArrayList Array_X;
        private ArrayList Array_Y;
        private ArrayList Came_From;
        private Graphics _g;
        private Pen myPen;
        private int Rec_Height = 15;//33;
        private int Rec_Width = 15;//33;
        private int Obstacle_Height;
        private int Obstacle_Width;
        private int Robot_Height;
        private int Robot_Width;
        private int coor_X;
        private int coor_Y;
        private int Col_Cells;
        private int R_Cells;
        private Node_Cordination[] node;
        public Grid_Map(int Column_Cells, int Row_Cells, int Start_point_X, int Start_point_Y, Graphics g)//start point in graphic
        {
            Rec_Height = 750/Column_Cells;//40
            Rec_Width = 750 / Row_Cells;//40
            Obstacle_Height = Rec_Height/3 +2;
            Obstacle_Width = Rec_Width / 3 + 2;
            Robot_Height = Rec_Height;
            Robot_Width = Rec_Width;
            Array_X = new ArrayList();
            Array_Y = new ArrayList();
            Came_From = new ArrayList();
            myPen = new Pen(Color.DimGray);
            coor_X = Start_point_X;
            coor_Y = Start_point_Y;
            Col_Cells = Column_Cells;
            R_Cells = Row_Cells;
            _g = g;
            Initial_Coordination();
        }

        public void begin()
        {
            Rectangle_Object();
        }
        public void Write_Text(int Node_ID, string Text, Color mycolor)
        {
            Font drawFont = new Font("Arial", 12);
            SolidBrush drawBrush = new SolidBrush(mycolor);
            _g.DrawString(Text, drawFont, drawBrush, node[Node_ID].Pixel_X-10, node[Node_ID].Pixel_Y - 25);
        }
        private void Initial_Coordination()//Create Coordination,  array index = coordinate
        {
            for (int i = 0; i <= Col_Cells; i++)//9 collumn but 10 angles
            {
                Array_X.Add(coor_X);
                coor_X = coor_X + Rec_Width;//Array_X: contains X coordinates, each Array_X element is different : Rec_width value
            }
            for (int j = 0; j <= R_Cells; j++)
            {
                Array_Y.Add(coor_Y);
                coor_Y = coor_Y + Rec_Height;//Array_Y: contains Y coordinates,
            }
            node = new Node_Cordination[(Array_X.Count) * (Array_Y.Count)];
            int x = 0;
            int y = 0;
            for (int i = 0; i < node.Length; i++)
            {
                if (x == Array_X.Count)
                {
                    y = y + 1;
                    x = 0;
                }
                node[i].X = x; //Set up coordination for nodes, 0 -> Array_X length
                node[i].Y = y;//  0 -> Array_Y length
                node[i].Node_ID = i;
                node[i].F_Score = 0;
                node[i].G_Score = 0;
                node[i].Node_Parent_ID = -1;//unknown parent, so set -1
                node[i].Obstacle = false;
                node[i].Distance_Score = 1;
                node[i].Pixel_X = (int)Array_X[x];//convert coordination to pixel coordination
                node[i].Pixel_Y = (int)Array_Y[y];
                x = x + 1;
            }
        }
        private void Rectangle_Object()//Draw Grid by multiply a square 
        {

            for (int i = 0; i <= Col_Cells; i++)
            {
                _g.DrawLine(new Pen(Color.DimGray, 2), new Point((int)Array_X[i], (int)Array_Y[0]), new Point((int)Array_X[i], (int)Array_Y[R_Cells]));
            }
            for (int i = 0; i <= R_Cells; i++)
            {
                _g.DrawLine(new Pen(Color.DimGray, 2), new Point((int)Array_X[0], (int)Array_Y[i]), new Point((int)Array_X[Col_Cells], (int)Array_Y[i]));
            }
        }
        public void MovingObject(int x, int y)// Current position of robot is highlighted by a red square
        {
            //_g.FillRectangle(new SolidBrush(Color.Red), new Rectangle((int)Array_X[x] + (Rec_Height / 2)-5, (int)Array_Y[y] + (Rec_Width/2)-5, 10, 10));
            _g.FillRectangle(new SolidBrush(Color.Green), new Rectangle((int)Array_X[x] - Robot_Width / 2, (int)Array_Y[y] - Robot_Height / 2, Robot_Width, Robot_Height));// this draw robot at cross point
        }
        public void Obstacle_Position(int NumberOfObstacle)
        {
            Random Random_Position = new Random();
            int X = Random_Position.Next(0, Array_X.Count - 1);
            int Y = Random_Position.Next(0, Array_Y.Count - 1);
            for (int i = 1; i < Array_X.Count; i = i + 1)
            {
                for (int j = 1; j < Array_Y.Count; j = j + 1)
                {
                    //_g.FillRectangle(new SolidBrush(Color.Blue), new Rectangle((int)Array_X[i] + (Rec_Height / 2) - 5, (int)Array_Y[j] + (Rec_Width / 2) - 5, 10, 10));
                    _g.FillRectangle(new SolidBrush(Color.Blue), new Rectangle((int)Array_X[i] - Obstacle_Width / 2, (int)Array_Y[j] - Obstacle_Height / 2, Obstacle_Width, Obstacle_Height));//this draw obstacles at cross points
                }
            }
        }
        public void Node_Highlight(int Node_ID, Color MyColor)
        {

            _g.FillRectangle(new SolidBrush(MyColor), new Rectangle(node[Node_ID].Pixel_X - Obstacle_Width / 2, node[Node_ID].Pixel_Y - Obstacle_Height / 2, Obstacle_Width, Obstacle_Height));

        }
        public void Connect_Node(int Node_ID1, int Node_ID2, Color MyColor)
        {
            Point node1= new Point();
            Point node2= new Point();
            node1.X = node[Node_ID1].Pixel_X;
            node1.Y = node[Node_ID1].Pixel_Y;
            node2.X = node[Node_ID2].Pixel_X;
            node2.Y = node[Node_ID2].Pixel_Y;
            _g.DrawLine(new Pen(MyColor, 3), node1, node2);
        }
        public void Show_Path(ArrayList Node_List, bool Closed_Node)
        {
            for (int i = 0; i < Node_List.Count - 1; i++)
            {
                int id = (int)Node_List[i];
                Color my_color = Color.Green;
                if (Closed_Node == false)
                {
                    my_color = Color.Green;
                }
                else
                {
                    my_color = Color.Gray;
                }
                Connect_Node((int)Node_List[i], (int)Node_List[i + 1], my_color);
            }
        }
        //public void
        public void Add_Object(Image Obstacle,int X, int Y, int width, int height)
        {

            _g.DrawImage(Obstacle, (int)Array_X[X], (int)Array_Y[Y], width*Robot_Width, height*Rec_Height);
        }
        public void Add_Object_Ratio(Image Obstacle, int Node_ID, float width_factor, float height_factor)
        {
            int x;
            int y;
            x = node[Node_ID].Pixel_X - (int)(width_factor * (Rec_Width+4)/2);
            y = node[Node_ID].Pixel_Y - (int)(height_factor * (Rec_Height+4)/2);
            _g.DrawImage(Obstacle, x, y, width_factor * (Rec_Width+4), height_factor * (Rec_Height+4));//
        }
        public void Add_Object(Image Obstacle, int Node_ID, float width, float height)
        {
            int x;
            int y;
            x = node[Node_ID].Pixel_X - (int)(width / 2);
            y = node[Node_ID].Pixel_Y - (int)(height/ 2);
            _g.DrawImage(Obstacle, x, y, width, height);//
        }
        public int CoordinationToNode(int x, int y)
        {
            int node_ID = -1;
            for (int i = 0; i <= node.Length-1; i++)
            {
                if ((node[i].X == x) && (node[i].Y == y))
                {
                    return node[i].Node_ID;
                }
            }
            return node_ID;
        }
        public int PixelToNode(int pixel_X, int pixel_Y)
        {
            int node_ID = -1;
            for (int i = 0; i <= node.Length - 1; i++)
            {
                if ((Math.Abs((node[i].Pixel_X - pixel_X)) <= Rec_Width / 2) && (Math.Abs(node[i].Pixel_Y - pixel_Y) <= Rec_Height/2))
                {
                    return node[i].Node_ID;
                }
            }
            return node_ID;
        
        }
    }
}
