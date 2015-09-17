using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using A_Start;
using System.Threading;
using System.Collections;
using GRID_MAP;
using System.Windows.Forms.Integration;
using D_Star_Lite_Algorithm;
using D_Star_Algorithm;
using System.Diagnostics;
namespace multiServerTestV01
{
    public partial class A_Star_Form : Form
    {
        Graphics Map_Graphic;
        private Grid_Map mymap;
        private System.Drawing.Image Robot;
        private System.Drawing.Image Path_Node;
        private System.Drawing.Image New_Obs;
        private System.Drawing.Image Closed_Nodes;
        private System.Drawing.Image Re_Open_Nodes;
        private System.Drawing.Image Round_Obstacle;
        private System.Drawing.Image Triangle_Obstacle;
        private System.Drawing.Image Target;
        private System.Drawing.Image Rect_Obstacle;
        private System.Drawing.Image Map_Background;
        private ArrayList Get_Path;
        private ArrayList New_Obstacle_List;
        private ArrayList Obstacle_List;
        private ArrayList Obstacle_Round_List;
        private ArrayList Obstacle_TriAngle_List;
        private ArrayList Obstacle_Rectangle_List;
        private ArrayList Path;
        private ArrayList CLosed_List;
        private bool Show_Path = false;
        private bool Path_Done = false;
        private const int Map_column = 40;
        private const int Map_row = 40;
        private int Start_Point = 0;
        private int Goal_Point = (Map_column + 1) * (Map_row + 1) - 1;
        private int Next_Start_Point = 0;
        private int count = 0;
        Thread Search_Path_Thread;
        A_Star_Algorithm My_A_Star = new A_Star_Algorithm(Map_column, Map_row);
        public A_Star_Form()
        {
            InitializeComponent();
            Get_Path = new ArrayList();
            Obstacle_List = new ArrayList();
            Obstacle_Round_List = new ArrayList();
            Obstacle_TriAngle_List = new ArrayList();
            Obstacle_Rectangle_List = new ArrayList();
            Path = new ArrayList();
            CLosed_List = new ArrayList();
            New_Obstacle_List = new ArrayList();
            Map_Graphic = this.CreateGraphics();
            Re_Open_Nodes = Properties.Resources.Circle;
            Closed_Nodes = Properties.Resources.Circle_Cross;
            Robot = Properties.Resources.Robot1;
            New_Obs = Properties.Resources.RectangleWithCross;
            Path_Node = Properties.Resources.Empty_Rec;
            Round_Obstacle = Properties.Resources.Round;
            Triangle_Obstacle = Properties.Resources.Triangle;
            Target = Properties.Resources._5edge_Star;
            Rect_Obstacle = Properties.Resources.Rec_Obs;
            Map_Background = Properties.Resources.Background;
            Map_Graphic.DrawImage(Map_Background, new Point(0, 0));
            Move_Button.Enabled = false;

        }
        private void A_Star_Form_Paint(object sender, PaintEventArgs e)
        {
            mymap = new Grid_Map(Map_column, Map_row, 10, 10, e.Graphics);
            e.Graphics.DrawImage(Map_Background, 0, 0, 750, 750);
            mymap.begin();
            if (Path_Done == true && Show_Path == true)
            {
                if (CLosed_List.Count > 0)
                {
                    float width = (float)0.30;
                    float heigh = (float)0.30;
                    foreach (int Node in CLosed_List)
                    {
                        //mymap.Node_Highlight(Node, Color.Blue);
                        mymap.Add_Object_Ratio(Closed_Nodes, Node, width, heigh);
                    }
                }
                for (int i = 0; i < Path.Count; i++)
                {
                    //Console.WriteLine("Node in Path: " + Path[i]);
                    float width = (float)0.3;
                    float heigh = (float)0.3;
                    mymap.Add_Object_Ratio(Path_Node, (int)Path[i], width, heigh);
                }
                for (int i = 0; i < Path.Count; i++)
                {
                    mymap.Show_Path(Path, false);
                }

            }
            for (int i = 0; i < Obstacle_List.Count; i++)
            {
                mymap.Node_Highlight((int)Obstacle_List[i], Color.Black);
            }
            for (int i = 0; i < New_Obstacle_List.Count; i++)
            {
                float width = (float)0.5;
                float heigh = (float)0.5;
                mymap.Add_Object_Ratio(New_Obs, (int)New_Obstacle_List[i], width, heigh);
            }
            mymap.Add_Object_Ratio(Robot, Start_Point, 1, 1);
            mymap.Add_Object_Ratio(Target, Goal_Point, 1, 1);
        }
        private void StartButton_Click(object sender, EventArgs e)
        {
            this.Invalidate();
            Search_Path_Thread = new Thread(Begin_Path_Search);
            My_A_Star.Obstacle_Setup(Obstacle_List);
            Search_Path_Thread.Start();
        }
        private void Begin_Path_Search()
        {
            Path.Clear();
            CLosed_List.Clear();
            count = 1;
            My_A_Star.Start(Start_Point, Goal_Point);
            for (int i = 0; i < My_A_Star.Get_Closed_List.Count; i++)
            {
                CLosed_List.Add(My_A_Star.Get_Closed_List[i]);
            }
            for (int j = 0; j < My_A_Star.Get_Path.Count; j++)
            {
                Path.Add(My_A_Star.Get_Path[j]);
            }
            this.Invoke((MethodInvoker)delegate
            {
                Start.Enabled = false;
                Goal.Enabled = false;
                Point_Obs.Checked = true;
                StartButton.Enabled = false;
                Move_Button.Enabled = true;
            });
            Show_Path = true;
            Path_Done = true;

            this.Invalidate();
        }
        private void Move_Button_Click(object sender, EventArgs e)//Robot moves to the next coordinate, if that coordinate is obstacle, it should be updated
        {
            this.Invalidate();
            Console.WriteLine("Thread state:" + Search_Path_Thread.ThreadState);
            if (Next_Start_Point < Path.Count - 1)
            {

                Next_Start_Point = Next_Start_Point + 1;
                Console.WriteLine("Next_Start_Point1: " + Next_Start_Point);
            }
            foreach (int Obs in New_Obstacle_List)
            {
                Console.WriteLine("Obs: " + Obs);
                if ((int)Path[Path.Count - Next_Start_Point - 1] == Obs)
                {
                    Next_Start_Point = 0;
                    My_A_Star.Single_Obstacle_Setup(Obs);
                    Begin_Path_Search();
                    Console.WriteLine("Next_Start_Point2: " + Next_Start_Point);
                    break;
                }
            }
            Start_Point = (int)Path[Path.Count - Next_Start_Point - 1];
        }
        private void A_Star_Form_MouseUp(object sender, MouseEventArgs e)
        {
            this.Invalidate();
            int node = mymap.PixelToNode(e.X, e.Y);
            if (Start.Checked == true)
            {
                if (node >= 0)//check valid node, node = -1 when there is no node
                {
                    Start_Point = node;
                }
            }
            if (Goal.Checked == true)
            {
                if (node >= 0)
                {
                    Goal_Point = node;
                }
            }
           
            if (Point_Obs.Checked == true)
            {
                if (node >= 0)
                {
                    Obstacle_List.Add(node);
                    if (count > 0)
                    {
                        New_Obstacle_List.Add(node);
                    }
                        Obstacle_List.Add(node);
                }
            }
        }

        private void Restart_Click(object sender, EventArgs e)
        {
            Path.Clear();
            count = 0;
            CLosed_List.Clear();
            Obstacle_List.Clear();
            New_Obstacle_List.Clear();
            Start_Point = 0;
            Goal_Point = 3;
            Next_Start_Point = 0;
            this.Invoke((MethodInvoker)delegate
            {
                StartButton.Enabled = true;
                Goal.Enabled = true;
                Start.Enabled = true;
                Move_Button.Enabled = false;
            });
            this.Invalidate();
        }
    }
}