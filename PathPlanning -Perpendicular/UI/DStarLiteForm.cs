using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Collections;
using GRID_MAP;
using System.Diagnostics;
using System.Windows.Forms.Integration;
using D_Star_Lite_Algorithm;
namespace multiServerTestV01
{
    public partial class DStarLiteForm : Form
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
        private ArrayList Updated_Nodes;
        private ArrayList Team_List;
        private ArrayList Alliance_List;
        private ArrayList Enemy_list;
        private ArrayList Path;
        private D_Star_Lite My_D_Star_Lite;
        private bool Show_Path = false;
        private bool Path_Done = false;
        private const int Map_column = 30;
        private const int Map_row = 30;
        private int Start_Point = 0;
        private int Goal_Point = (Map_column+1) * (Map_row+1)-1;
        private int Next_Start_Point = 0;
        private int count = 0;
        public DStarLiteForm()
        {
            InitializeComponent();
            My_D_Star_Lite = new D_Star_Lite();
            My_D_Star_Lite.Initial(Map_column, Map_row);
            New_Obstacle_List = new ArrayList();
            Team_List = new ArrayList();
            Alliance_List = new ArrayList();
            Enemy_list = new ArrayList();
            Get_Path = new ArrayList();
            Obstacle_List = new ArrayList();
            Updated_Nodes = new ArrayList();
            Path = new ArrayList();
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
            //mymap = new Grid_Map(Map_column, Map_row, 0, 0, Map_Graphic);
            Map_Background = Properties.Resources.Background;
            Map_Graphic.DrawImage(Map_Background, new Point(0, 0));
            Move_Button.Enabled = false;
        }
        private void DStarLiteForm_Paint(object sender, PaintEventArgs e)
        {
            mymap = new Grid_Map(Map_column, Map_row, 20, 20, e.Graphics);
            e.Graphics.DrawImage(Map_Background, 0, 0, 800, 800);
            //Console.WriteLine("Text:" + Option_List.Text);
            mymap.begin();
            if (Path_Done == true && Show_Path == true)
            {
               if (My_D_Star_Lite.Get_U_ID_List.Count > 0)
               {
                   float width = (float)0.30;
                   float heigh = (float)0.30;
                   foreach (int Node in My_D_Star_Lite.Get_U_ID_List)
                   {
                       //mymap.Node_Highlight(Node, Color.Blue);
                       if (Node > -1)
                       {
                           mymap.Add_Object_Ratio(Closed_Nodes, Node, width, heigh);
                       }
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
            mymap.Add_Object(Robot, Start_Point, 20, 20);
            mymap.Add_Object(Target, Goal_Point, 20, 20);
        }
        private void StartButton_Click(object sender, EventArgs e)
        {
            this.Invalidate();
            Thread Search_Path_Thread = new Thread(Begin_Path_Search);
            Search_Path_Thread.Start();
        }
        private void Begin_Path_Search()
        {
            //Console.WriteLine("Obstalces: " + Obstacle_List.Count);
            count = 1;
            Next_Start_Point = 0;
            My_D_Star_Lite.Obstacle_Setup(Obstacle_List);
            My_D_Star_Lite.Start(Start_Point, Goal_Point);
            for (int j = 0; j < My_D_Star_Lite.Get_Path.Count; j++)
            {
                Path.Add(My_D_Star_Lite.Get_Path[j]);
            }// be careful if using: Path = My_PLA_Star.Get_Path[j], because My_PLA_Star.Get_Path[j] is object arraylist, it is
            // clear after calculating and as the result, Path is clear.
            Show_Path = true;
            Path_Done = true;
            this.Invoke((MethodInvoker)delegate
            {
                StartButton.Enabled = false;
                Move_Button.Enabled = true;
                Start_Radio_Button.Enabled = false;
                Goal_Radio_Button.Enabled = false;
                Obs_Radio_Button.Checked = true;
            });
            this.Invalidate();
        }
        private void DStarLiteForm_MouseUp(object sender, MouseEventArgs e)
        {
            this.Invalidate();
            int node = mymap.PixelToNode(e.X, e.Y);
            if (Start_Radio_Button.Checked == true)
            {
                if (node >= 0)//check valid node, node = -1 when there is no node
                {
                    Start_Point = node;
                }
            }
            if(Goal_Radio_Button.Checked == true)
            {
                if (node >= 0)
                {
                    Goal_Point = node;
                }
            }
            if (Obs_Radio_Button.Checked == true)
            {
                //Console.WriteLine("Node: " + node);
                if (node >= 0)
                {
                    //Console.WriteLine("Node: " + node);
                    if (count > 0)//if second search
                    {
                        New_Obstacle_List.Add(node);
                    }
                    else//first search
                        Obstacle_List.Add(node);
                }
            }
        }
        private void Move_Button_Click(object sender, EventArgs e)
        {
            this.Invalidate();
            if (Next_Start_Point < Path.Count - 1)
            {
                Next_Start_Point = Next_Start_Point + 1;
                //Console.WriteLine("Next_Start_Point1: " + Next_Start_Point);
            }
            foreach (int Obs in New_Obstacle_List)
            {
                //Console.WriteLine("Obs: " + Obs);
                if ((int)Path[Next_Start_Point] == Obs)
                {
                    Path.Clear();
                    Next_Start_Point = 0;
                    Console.WriteLine("New search begin");
                    My_D_Star_Lite.Update_Arc_Cost(Obs);
                    My_D_Star_Lite.Restart();
                    break;
                }
            }
            if (Path.Count <= 0)
            {
                foreach (int Path_Element in My_D_Star_Lite.Get_Path)
                {
                    Path.Add(Path_Element);
                    //Console.WriteLine("Path node: " + Path_Element);
                }
            }
            Console.WriteLine("Next_Start_Point: " + Next_Start_Point);
            if (Path.Count > 0)
            {
                Start_Point = (int)Path[Next_Start_Point];
            }
        }
        private void Coordination_Show_CheckedChanged(object sender, EventArgs e)
        {
            this.Invalidate();
        }
    }
}
