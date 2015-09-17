/*
 * This algorithm was developed based on the pseudo code on wikipedia.
 * https://en.wikipedia.org/wiki/A*_search_algorithm
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Threading;
namespace A_Start
{
    struct A_Star_Node_Define
    {
        internal int X;
        internal int Y;
        internal int Node_ID;
        internal double G_Score; // distance from start_node to the current node
        internal int Distance_Score;// distance from the parent node to the current node, vertical and horizon
        internal double Diagonal_Score;//distance from the parent node to the current node, diagonal
        internal double F_Score;//distance from the node to the End point
        internal int Node_Parent_ID; //if no initial, by default, Node_Parent_Index = 0 (not good because 0 is in the grid, it should be -1);
        internal bool Obstacle;
        internal bool IsDiagonal;
    }

    public class A_Star_Algorithm
    {
        private int _Column;
        private int _Row;
        private ArrayList Path;
        public ArrayList Closed_Nodes;
        private A_Star_Node_Define[] Node; 
        private int Start_ID;

//---------------------------------------------------------------------------
        public A_Star_Algorithm(int Column, int Row)
        {
            Path = new ArrayList();
            Closed_Nodes = new ArrayList();
            _Column = Column;
            _Row = Row;
            int Number_Of_Node = (Column + 1) * (Row + 1);
            int x = 0;
            int y = 0;
            Node = new A_Star_Node_Define[Number_Of_Node];
            for (int i = 0; i < Node.Length; i++)
            {

                if (x == Column + 1)
                {
                    y = y + 1;
                    x = 0;
                }
                Node[i].X = x; //Set up coordination for nodes, 0 -> Array_X length
                Node[i].Y = y;//  0 -> Array_Y length
                Node[i].Node_ID = i;
                Node[i].F_Score = 0;
                Node[i].G_Score = 0;
                Node[i].Node_Parent_ID = -1;//unknown parent, so set -1
                Node[i].Obstacle = false;
                Node[i].Distance_Score = 1;
                Node[i].Diagonal_Score = Math.Round(Math.Sqrt(2), 2, MidpointRounding.AwayFromZero);
                Node[i].IsDiagonal = false;
                x = x + 1;
            }
        }
        public void Obstacle_Setup(ArrayList Node_ID_List)
        {
            for (int i = 0; i < Node_ID_List.Count; i++)
            {
                int Node_ID = (int)Node_ID_List[i];
                Node[Node_ID].Obstacle = true;
            }
        }
        public void Single_Obstacle_Setup(int Node_ID)
        {
               Node[Node_ID].Obstacle = true;
        }
        public void Start(int Start_Node_ID, int Goal_Node_ID)//two adjacent nodes distance  = 1;
        {
            Start_ID = Start_Node_ID;
            ArrayList Closed_Set = new ArrayList();
            ArrayList Open_Set = new ArrayList();
            ArrayList F_List = new ArrayList();
            int Current_Node_ID;
            int g_score = 0; //distance from current Node to Start_Node
            int Heuristic = (int)Heuristic_Estimate(Start_Node_ID,Goal_Node_ID);
            double f_score = Node[Start_Node_ID].G_Score + Heuristic;
            Node[Start_Node_ID].F_Score = f_score;
            Open_Set.Add(Start_Node_ID);
            while(Open_Set.Count >0)
            {
                //Thread.Sleep(100);
                int UpperNode_ID;
                int LowerNode_ID;
                int RightNode_ID;
                int LeftNode_ID;
                ArrayList Neighbor_ID_Set = new ArrayList(); //Create neighbor list
                Current_Node_ID = Get_ID_From_F_Score(Open_Set); //Current Node is the node has smallest f
                //Console.WriteLine("Node Lowest F: " + Current_Node_ID);
                if (Compare_Node(Current_Node_ID, Goal_Node_ID))
                {
                    Reconstruction_Path(Current_Node_ID);
                    break;
                }
                int Open_Set_Length = Open_Set.Count;
                for (int i = 0; i<Open_Set_Length;i++) //Remove current node from Open_Set and add current node to Closed_Set
                {
                    if (Compare_Node(Current_Node_ID, (int)Open_Set[i]) == true)
                    {
                        //Console.WriteLine("Removed node: " + Node[(int)Open_Set[i]].Node_ID);
                        Open_Set.Remove(Open_Set[i]);
                        //Console.WriteLine("Open Set length " + Open_Set.Count);
                        Closed_Set.Add(Current_Node_ID);
                        Closed_Nodes = Closed_Set;
                        break;
                    }
                }
                /*
                 * Check neighbors and add them to Neighbor list:
                 * 
                 *                         neighbor3       
                 *                             | 
                 *              neighbor2 <-current-> neighbor1
                 *                             | 
                 *                         neighbor4       
                 */
                if (Current_Node_ID - (_Column+1) >= 0)//upper
                {
                    UpperNode_ID = Current_Node_ID - (_Column + 1);
                    Node[UpperNode_ID].IsDiagonal = false;
                    if (Node[UpperNode_ID].Obstacle == false)
                    {
                        Neighbor_ID_Set.Add(UpperNode_ID);
                        //Console.WriteLine("UpperID " + UpperNode_ID);
                    }
                }
                if (Current_Node_ID + (_Column + 1) < Node.Length)//lower
                {
                    LowerNode_ID = Current_Node_ID + (_Column + 1);
                    Node[LowerNode_ID].IsDiagonal = false;
                    if (Node[LowerNode_ID].Obstacle == false)
                    {
                        Neighbor_ID_Set.Add(LowerNode_ID);
                        //Console.WriteLine("lowerID " + LowerNode_ID);
                    } 
                }
                if (Node[Current_Node_ID].X + 1 < (_Column + 1))//right
                {
                    RightNode_ID = Current_Node_ID+ 1;
                    Node[RightNode_ID].IsDiagonal = false;
                    if (Node[RightNode_ID].Obstacle == false)
                    {
                        Neighbor_ID_Set.Add(RightNode_ID);
                        //Console.WriteLine("rightID " + RightNode_ID);
                    }
                }
                //Console.WriteLine("Current Node:" + Current_Node_ID);
                if (Node[Current_Node_ID].X - 1 >= 0)//left
                {
                    LeftNode_ID = Current_Node_ID - 1;
                    Node[LeftNode_ID].IsDiagonal = false;
                    if (Node[LeftNode_ID].Obstacle == false)
                    {
                        Neighbor_ID_Set.Add(LeftNode_ID);
                        //Console.WriteLine("left  " + LeftNode_ID);
                    }
                }
                if (Neighbor_ID_Set.Count == 0)
                {
                    //Console.WriteLine("There is no path");
                    break;
                }
                ArrayList Neighbor_Removed_List = new ArrayList();
                foreach (int Closed_Node_ID in Closed_Set)// If neighbors have already been in Closed_Set, remove them from neighbor list
                {
                    int Neighbor_List_Length = Neighbor_ID_Set.Count;
                    for (int i = 0; i < Neighbor_List_Length; i++)
                    {
                        if (Compare_Node((int)Neighbor_ID_Set[i], Closed_Node_ID) == true)
                        {
                            //Console.WriteLine("Removed ID: " + Neighbor_ID_Set[i]);
                            Neighbor_ID_Set.Remove(Closed_Node_ID);
                            break;
                        }
                    }
                }

                ArrayList Open_List_Tmp = new ArrayList();
                for (int i = 0; i < Neighbor_ID_Set.Count; i++) // Calculate distance from each neighbor node to the start node
                {
                    int Neighbor_ID = ((int)Neighbor_ID_Set[i]);
                    int New_Open_Set_Length = Open_Set.Count;
                    if (Open_Set.Count > 0)
                    {
                        double tentative_g_score;
                        if (Node[Neighbor_ID].IsDiagonal == false)
                        {
                            tentative_g_score = Node[Current_Node_ID].G_Score + Node[Neighbor_ID].Distance_Score;
                        }
                        else
                        {
                            tentative_g_score = Node[Current_Node_ID].G_Score + Node[Neighbor_ID].Diagonal_Score;
                        }

                        //Check nodes which are not in open_list. If they are in open_list, check G_score again, because new G_score from a new neighbor, 
                        //maybe smaller than the G_score from the previous neighbor.
                        if ((Matched_ID(Neighbor_ID, Open_Set) == false) || (tentative_g_score < Node[Neighbor_ID].G_Score))
                        {
                            Node[Neighbor_ID].Node_Parent_ID = Current_Node_ID;
                            Node[Neighbor_ID].G_Score = tentative_g_score;
                            Node[Neighbor_ID].F_Score = Node[Neighbor_ID].G_Score + (int)Manhattan_Heuristic(Neighbor_ID, Goal_Node_ID);
                            if (Matched_ID(Neighbor_ID, Open_Set) == false)
                            {
                                Open_List_Tmp.Add(Neighbor_ID);
                            }
                        } 
                    }
                    if (Open_Set.Count == 0)
                    {
                        int tentative_g_score = g_score + Node[Neighbor_ID].Distance_Score;
                        Node[Neighbor_ID].Node_Parent_ID = Current_Node_ID;
                        Node[Neighbor_ID].G_Score = tentative_g_score;
                        Node[Neighbor_ID].F_Score = Node[Neighbor_ID].G_Score + (int)Manhattan_Heuristic(Neighbor_ID, Goal_Node_ID);
                        Open_List_Tmp.Add(Neighbor_ID);
                    }
                }
                foreach (int tmp_open in Open_List_Tmp)
                {
                    Open_Set.Add(tmp_open);
                }
            }
        }
        private int Get_ID_From_F_Score(ArrayList Node_List)//This method is used to find a node that has the smallest F in a list of nodes
        {
            int Node_ID = -1;
            ArrayList F_list = new ArrayList();
            foreach (int Open_Node in Node_List)
            {
                F_list.Add(Node[Open_Node].F_Score);
                //Console.WriteLine("Node: " + Open_Node + "---F_score: " + Node[Open_Node].F_Score);
            }
            F_list.Sort();
            foreach (int Open_Node in Node_List)
            {
                if (Node[Open_Node].F_Score == (double)F_list[0])
                {
                    Node_ID = Open_Node;
                    //Console.WriteLine("Choosen Node: " + Node_ID + "---F_score: " + Node[Node_ID].F_Score);
                    break;
                }
            }
            return Node_ID;
        }
        private double Heuristic_Estimate(int Current_Node_ID, int Goal_Node_ID)//Diagonal heuricstic 
        {
            double Dx = Math.Abs(Node[Goal_Node_ID].X - Node[Current_Node_ID].X);            
            double Dy = Math.Abs(Node[Goal_Node_ID].Y - Node[Current_Node_ID].Y);
            double Remaining = Math.Abs(Dx - Dy);
            double Min_Value;
            if (Dx > Dy)
            {
                Min_Value = Dy;
            }
            else
            {
                Min_Value = Dx;
            }
            return (Math.Sqrt(2*Min_Value*Min_Value))+Remaining;
        }
        private double Manhattan_Heuristic(int Current_Node_ID, int Goal_Node_ID)//Please read wikipedia to get the idea of Manhattan distance (or Taxicap distance) or read here:
        //http://theory.stanford.edu/~amitp/GameProgramming/Heuristics.html
        {
            double Dx = Math.Abs(Node[Goal_Node_ID].X - Node[Current_Node_ID].X);
            double Dy = Math.Abs(Node[Goal_Node_ID].Y - Node[Current_Node_ID].Y);
            return (Dx + Dy);
        }
        private void Reconstruction_Path(int Current_Node_ID)
        {
            Path.Clear();
            int Tmp_ID = Current_Node_ID;
            while (Tmp_ID != Start_ID)
            {
                //Console.WriteLine("TMD: " + Tmp_ID);
                Path.Add(Tmp_ID);
                Tmp_ID = Node[Tmp_ID].Node_Parent_ID;
            }
            //Console.WriteLine("path: " + Tmp_ID);
            //Console.WriteLine("--------------------------------------");
            Path.Add(Tmp_ID);
        }
        private bool Compare_Node(int Index_Node1, int Index_Node2)
        {
            bool result;
            if (Index_Node1 == Index_Node2)
            {
                result = true;
            }
            else
            {
                result = false;
            }
            return result;
        }
        private bool Matched_ID(int item, ArrayList List)//=false if item not in list
        {
            bool final = false;
            foreach (int ID in List)
            {
                if (item == ID)
                {
                    final = true;
                    break;
                }
            }
            return final;
        }
        public ArrayList Get_Path
        {
            get
            {
                return Path;
            }
        }
        public ArrayList Get_Closed_List
        {
            get
            {
                return Closed_Nodes;
            }
        }
    }
}
