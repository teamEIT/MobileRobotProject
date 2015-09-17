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
using System.Windows.Forms.Integration;
using D_Star_Lite_Algorithm;
using D_Star_Algorithm;
using System.Diagnostics;
namespace multiServerTestV01
{
    public partial class DStarForm : Form
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
        private ArrayList New_Obstacle_List;
        private ArrayList Obstacle_List;
        private ArrayList Path;
        private D_Star My_D_Star;
        private bool Show_Path = false;
        private bool Path_Done = false;
        private int Path_Planning_Type = -1;
        private const int Map_column = 30;
        private const int Map_row = 30;
        private int Start_Point = 0;
        private int Goal_Point = 3;
        private int Next_Start_Point = 0;
        private int count = 0;
        public DStarForm()
        {
            InitializeComponent();
            My_D_Star = new D_Star(Map_column, Map_row);
            Obstacle_List = new ArrayList();
            Path = new ArrayList();
            New_Obstacle_List = new ArrayList();
            Map_Graphic = this.CreateGraphics();
            Re_Open_Nodes = Properties.Resources.Circle;
            Closed_Nodes = Properties.Resources.Circle_Cross;
            Robot = Properties.Resources.Robot2;
            New_Obs = Properties.Resources.RectangleWithCross;
            Path_Node = Properties.Resources.Empty_Rec;
            Round_Obstacle = Properties.Resources.Round;
            Triangle_Obstacle = Properties.Resources.Triangle;
            Target = Properties.Resources.Target;
            Rect_Obstacle = Properties.Resources.Rec_Obs;
            //mymap = new Grid_Map(Map_column, Map_row, 0, 0, Map_Graphic);
            Map_Background = Properties.Resources.Background;
            Map_Graphic.DrawImage(Map_Background, new Point(0, 0));
            Move_Button.Enabled = false;
            Start.Checked = true;
        }
        private void SimulationAlgorithm_Paint(object sender, PaintEventArgs e)
        {            
            mymap = new Grid_Map(Map_column, Map_row, 20, 20, e.Graphics);  
            e.Graphics.DrawImage(Map_Background, 0,0,780,780);
            mymap.begin();
            if (Show_Path == true)
            {
                //mymap.Show_Path(My_A_Star.Closed_Nodes, true);  
            }
            if (Path_Done == true && Show_Path == true)
            {
                if (My_D_Star.Get_Closed_Nodes.Count > 0 && My_D_Star.Get_ReOpen_Nodes.Count == 0)//First search, so there is no reopen nodes
                {
                    float width = (float)0.30;
                    float heigh = (float)0.30;
                    foreach (int Node in My_D_Star.Get_Closed_Nodes)
                    {
                        //mymap.Node_Highlight(Node, Color.Blue);
                        mymap.Add_Object_Ratio(Closed_Nodes, Node, width, heigh);
                    }
                }
                
                if (My_D_Star.Get_ReOpen_Nodes.Count > 0)// afeter detecting an obstacle, update node. Only display nodes which are reopened to calculate again
                {
                    float width = (float)0.3;
                    float heigh = (float)0.3;
                    foreach (int Node in My_D_Star.Get_ReOpen_Nodes)
                    {
                        //mymap.Node_Highlight(Node, Color.Green);
                        mymap.Add_Object_Ratio(Closed_Nodes, Node, width, heigh);
                    }
                }
                for (int i = 0; i < Path.Count; i++)
                {
                    Console.WriteLine("Node in Path: " + Path[i]);
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
            mymap.Add_Object(Robot, Start_Point, 25, 25);
            mymap.Add_Object(Target, Goal_Point, 35, 35);
        }

        private void Start_Click(object sender, EventArgs e)
        {
            this.Invalidate();
            Thread Search_Path_Thread = new Thread(Begin_Path_Search);
            Search_Path_Thread.Start();
        }
        private void Begin_Path_Search()
        {
            Console.WriteLine("Obstacles: " + Obstacle_List.Count);
            count = 1;
            foreach (int element in Obstacle_List)
            {
                My_D_Star.Obstacle_Setup(element,true);
            }
            My_D_Star.Start(Start_Point, Goal_Point);
            Path = My_D_Star.Get_Path;
            this.Invoke((MethodInvoker)delegate
            {
                Move_Button.Enabled = true;
                StartButton.Enabled = false;
                Start.Enabled = false;
                Goal.Enabled = false;
                Point_Obs.Checked = true;
            });
            Show_Path = true;
            Path_Done = true;
            this.Invalidate();
        }
        private void Undo_Click(object sender, EventArgs e)
        {
            this.Invalidate();
            if (Start.Checked == true)
            {
                Start_Point = -1;
            }
            if (Goal.Checked == true)
            {
                Goal_Point = -1;
            }
        }
        private void SimulationAlgorithm_MouseUp(object sender, MouseEventArgs e)
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
                    else
                    {
                        Obstacle_List.Add(node);
                        My_D_Star.Obstacle_Setup(node, true);
                    }
                }
            }
        }
        
        private void Coordination_Show_CheckedChanged(object sender, EventArgs e)
        {
            this.Invalidate();
        }
        public int Set_Path_Planning
        {
            set
            {
                Path_Planning_Type = value;
            }
        }

        private void Move_Button_Click(object sender, EventArgs e)
        {
            this.Invalidate();
            if (Next_Start_Point < Path.Count-1)
            {
                
                Next_Start_Point = Next_Start_Point + 1;
                //Console.WriteLine("Next_Start_Point1: " + Next_Start_Point);
            }

            foreach (int Obs in Obstacle_List)
            {
                //Console.WriteLine("Obs: " + Obs);
                if ((int)Path[Next_Start_Point] == Obs)
                {
                    Next_Start_Point = 0;
                    My_D_Star.Update_Arc_Cost(Obs);//Update arc cost must be before clearing Path! (related to Reference Type characteristic)
                    //Path_Storage in D_Star_Lite is referenced by Path, so if Path is cleared, Path_Storage is also cleared, then My_D_Star will update incorectly!
                    Path.Clear();
                    My_D_Star.Update_D_Star();
                    //Console.WriteLine("Next_Start_Point2: " + Next_Start_Point);
                    break;
                }
            }
            Start_Point = (int)Path[Next_Start_Point];
        }
        private void ReStart_Button_Click(object sender, EventArgs e)
        {
            Move_Button.Enabled = false;
            StartButton.Enabled = true;
            Start.Enabled = true;
            Goal.Enabled = true;
            Start_Point = 0;
            Goal_Point = 3;
            Path.Clear();
            New_Obstacle_List.Clear();
            Obstacle_List.Clear();
            count = 0;
            My_D_Star.Reset_D_Star();
            this.Invalidate();
        }
    }
}
