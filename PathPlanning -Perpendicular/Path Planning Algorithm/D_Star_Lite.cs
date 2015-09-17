using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Threading;
using U_Node_List;
using Nodes_Defined;
using System.Diagnostics;
namespace D_Star_Lite_Algorithm
{
    class D_Star_Lite
    {
        private int _Column;
        private int _Row;
        private ArrayList Path_Storage = new ArrayList();
        private ArrayList New_Obs_List = new ArrayList();
        private ArrayList All_New_Obs_List = new ArrayList();
        private ArrayList Pop_id_list = new ArrayList();
        private ArrayList Node_In_U_List = new ArrayList();
        private Node_Define[] Node;
        private int Start_ID;
        private int Goal_ID;
        private int Infinite_Value;
        //private double km;
        private int S_last;
        private ArrayList List_Of_Updated = new ArrayList();
        private U_List U;
        #region Public Methods
        public D_Star_Lite()
        {
            //Do nothing
        }
        public void Initial(int column, int row)
        {
            _Column = column;
            _Row = row;
            Infinite_Value = (_Column + 1) * (_Row + 1) + 1;
            int Number_Of_Node = (_Column + 1) * (_Row + 1);
            int x = 0;
            int y = 0;
            U = new U_List();
            Node = new Node_Define[Number_Of_Node];
            for (int i = 0; i < Node.Length; i++)
            {

                if (x == _Column + 1)
                {
                    y = y + 1;
                    x = 0;
                }
                Node[i].X = x; //Set up coordination for nodes, 0 -> Array_X length
                Node[i].Y = y;//  0 -> Array_Y length
                Node[i].Node_ID = i;
                Node[i].G_Score = Infinite_Value;//infinite
                Node[i].rhs = Infinite_Value;//infinite
                Node[i].Node_Parent_ID = null;//unknown
                Node[i].Node_Children_ID = null;//unknown
                Node[i].Obstacle = false;
                Node[i].Perpendicular_ArcCost = 1;
                Node[i].Diagonal_ArcCost = Math.Round(Math.Sqrt(2), 2, MidpointRounding.AwayFromZero);
                Node[i].IsDiagonal = false;
                Node[i].key_list = new double[2];
                Node[i].Predecessor_Node = new ArrayList();
                Node[i].Successors_List = new ArrayList();
                Node[i].Directed_Node_List = new ArrayList();
                Node[i].Node_Of_Path = false;
                x = x + 1;
            }
        }
        public void Start(int Start_Node_ID, int Goal_Node_ID)
        {
            //Console.WriteLine(".....Start.....");
            Path_Storage.Clear();
            Goal_ID = Goal_Node_ID;
            Start_ID = Start_Node_ID;
            S_last = Start_ID;
            //Initialize();
            //km = 0;
            Node[Goal_ID].rhs = 0;
            CalculateKey(Goal_ID);
            U.Insert(Node[Goal_ID]);
            //Node_In_U_List.Add(Goal_ID);
            //
            Compute_Shortest_Path();
            Reconstruction_Path(Start_ID);
        }
        public void Restart()
        {
            //Console.WriteLine("....Re_Compute_Shortest_Path.....");
            Pop_id_list.Clear();
            Compute_Shortest_Path();
            Reconstruction_Path(Start_ID);
        }
        public void Update_Arc_Cost(int Node_ID)
        {

            for (int i = 0; i < Path_Storage.Count; i++)
            {
                if (Node_ID == (int)Path_Storage[i])
                {
                    {
                        Start_ID = (int)Path_Storage[i - 1];//New start point. It is front of the obstacle on the path.
                    }
                    Path_Storage.Remove(Node_ID);
                }
            }
            //in our case Node_ID becomes an obstacle, so its cost value = infinite
            //km = km + Diagonal_Heuristic_Estimate(S_last, Start_ID);
            Console.WriteLine("update ARc");
            S_last = Start_ID;
            UPDATE_COST(Node_ID, Infinite_Value);
            UpdateVertex(Node_ID);
        }
        public void Obstacle_Setup(ArrayList Obs_List)
        {
            foreach (int element in Obs_List)
            {
                Node[element].Perpendicular_ArcCost = Infinite_Value;
                Node[element].Diagonal_ArcCost = Infinite_Value;
            }
        }
        private void UPDATE_COST(int X, int C_Val)
        {
            Node[X].Diagonal_ArcCost = C_Val;
            Node[X].Perpendicular_ArcCost = C_Val;
        }
        public void Obstacle_Setup(int Node_ID, bool obstacle)
        {
            //Console.WriteLine("......Obstacle_Setup........");
            Node[Node_ID].Diagonal_ArcCost = Infinite_Value;
            Node[Node_ID].Perpendicular_ArcCost = Infinite_Value;
            //Node[Node_ID].Obstacle = obstacle;
        }
       
        #endregion
        #region Private methods
        private void UpdateVertex(int Successor_Node_ID)
        {
            if (Successor_Node_ID != Goal_ID)
            {
                //Console.WriteLine("......UpdateVertex.....");
                Node[Successor_Node_ID].rhs = Min_Rhs_of_Current_Node(Successor_Node_ID);
            }
            //--------------------------------
            if (U.Is_In_List(Successor_Node_ID) == true)
            {
                U.Remove(Successor_Node_ID);
            }
            //-------------------------------
            if (Node[Successor_Node_ID].G_Score != Node[Successor_Node_ID].rhs)
            {
                Console.WriteLine("G : "+ Node[Successor_Node_ID].G_Score);
                Console.WriteLine("new Rhs: " + Node[Successor_Node_ID].rhs); 
                CalculateKey(Successor_Node_ID);
                U.Insert(Node[Successor_Node_ID]);
                Node_In_U_List.Add(Successor_Node_ID);
            }
        }
        private double Min_Rhs_of_Current_Node(int Current_Node)
        {
            double Rhs_Tmp = -1;
            ArrayList Rhs_List = new ArrayList();
            //Console.WriteLine("Current: " + Current_Node);
            foreach (int parrent in Node[Current_Node].Successors_List)
            {
                Rhs_Tmp = Math.Min((double)Node[parrent].G_Score + Node[Current_Node].Perpendicular_ArcCost, Infinite_Value);//Rhs_tmp may > infinite
                Rhs_List.Add(Rhs_Tmp);
            }
            Rhs_List.Sort();
            Node[Current_Node].rhs = (double)Rhs_List[0];
            return Node[Current_Node].rhs;
        }
        private double[] CalculateKey(int Node_ID)
        {
            double Min1 = Math.Min(Node[Node_ID].G_Score, Node[Node_ID].rhs) + Manhattan_Heuristic(Node_ID, Start_ID);// + km;
            double Min2 = Math.Min(Node[Node_ID].G_Score, Node[Node_ID].rhs);
            Node[Node_ID].key_list[0] = Min1;
            Node[Node_ID].key_list[1] = Min2;
            if (Node_ID == Start_ID)
            {
                Console.WriteLine("Min1_Start: " + Min1);
                Console.WriteLine("Min2_Start: " + Min2);
            }
            return Node[Node_ID].key_list;
        }
        private void Compute_Shortest_Path()
        {
            //Console.WriteLine(".....Compute_Shortest_Path....");
            Console.WriteLine("Start ID: " + Start_ID);
            Node_Define TopNode = new Node_Define();
            int Top_id = U.Top_Node();
            if (Top_id >= 0)
            {
                TopNode = (Node_Define)Node[Top_id];
                Console.WriteLine("TopNode :" + Top_id+ ": " + Node[Top_id].key_list[0] + "-" + Node[Top_id].key_list[1]);
                while ((Smaller_Key(TopNode.key_list, CalculateKey(Start_ID)) == true) || (Node[Start_ID].rhs != Node[Start_ID].G_Score))
                {
                    int Pop_id = U.Pop(); //be careful, if U_list is not Synced with Node in Main file, the G and rhs of a node in U_list
                    Pop_id_list.Add(Pop_id);
                    Console.WriteLine("Pop ID:" + Pop_id);
                    if (Pop_id > -1)
                    {
                        Console.WriteLine("Pop_id :" + Pop_id);
                        if (Smaller_Key(TopNode.key_list, CalculateKey(Pop_id)) == true)
                        {
                            Console.WriteLine("Kold < K new");
                            U.Insert(Node[Pop_id]);
                            //Node_In_U_List.Add(Pop_id);
                        }
                        else 
                        if (Node[Pop_id].G_Score > Node[Pop_id].rhs)
                        {
                            //Console.WriteLine("else if G_Score > rhs");
                            Node[Pop_id].G_Score = Node[Pop_id].rhs;
                            Predecessors(Pop_id);//In here, to be easy to understand for me, I use successors_nodes to impply Predecessors, and parrent node as Successors
                            foreach (int Node_Element in Node[Pop_id].Predecessor_Node)
                            {
                                if (Node_Element > -1)
                                {
                                    Node[Node_Element].Successors_List.Add(Pop_id);
                                    UpdateVertex(Node_Element);
                                }
                            }
                        }
                        else
                        {
                            //Console.WriteLine("else if G_Score <= rhs");
                            Node[Pop_id].G_Score = Infinite_Value;
                            UpdateVertex(Pop_id);
                            foreach (int Predecessor in Node[Pop_id].Predecessor_Node)
                            {
                                if (Predecessor > -1)
                                {
                                    UpdateVertex(Predecessor);
                                }
                            }
                        }
                    }
                    else
                        break;
                    Top_id = U.Top_Node();
                    //Console.WriteLine("Top_id:" + Top_id);
                    if (Top_id > -1)
                    {
                        TopNode = (Node_Define)Node[Top_id];
                    }
                    else
                        Console.WriteLine("no more nodes");
                }
            }
        }
        private void Reconstruction_Path(int Current_Node_ID)
        {
            //Console.WriteLine("....Reconstruction_Path....");
            Path_Storage.Clear();
            int Tmp_ID = Current_Node_ID;
            if (Node[Tmp_ID].G_Score != Infinite_Value)
            {
                while (Tmp_ID != Goal_ID)
                {
                    if (Tmp_ID > -1)
                    {
                        //Console.WriteLine("TMD: " + Tmp_ID);
                        Path_Storage.Add(Tmp_ID);
                        Tmp_ID = Smallest_Rhs_Node(Tmp_ID);
                        //Console.WriteLine("Rhs: " + Node[Tmp_ID].rhs);
                    }
                }
                Path_Storage.Add(Tmp_ID);
            }
            else
                Console.WriteLine("no Path Found");
        }

        private int Smallest_Rhs_Node(int Current_Node)//From Goal, check the parent  nodes having smallest rhs, then return it
        {
            //Console.WriteLine("......Smallest_Rhs_Node.....");
            int Tmp_node = -1;
            double Rhs_Tmp;
            ArrayList Rhs_List = new ArrayList();
            ArrayList Parrent_List = new ArrayList();
            ArrayList Selected_Nodes = new ArrayList();
            //Direct_List = Directed_Nodes(Current_Node);
            Parrent_List = Node[Current_Node].Successors_List;
            //Console.WriteLine("Current Node: " + Current_Node);
            foreach (int element in Parrent_List)
            {
                if (element >= 0)
                {
                    //Console.WriteLine("Parrent of the Current: " + element);
                    Rhs_Tmp = (double)Node[element].rhs;
                    //Console.WriteLine("RHS of the Parrent: " + Rhs_Tmp);
                    Rhs_List.Add(Rhs_Tmp);
                }
            }
            Rhs_List.Sort();
         
            if (Rhs_List.Count > 0)
            {
                foreach (int element in Parrent_List)
                {
                    if (Node[element].rhs == (double)Rhs_List[0])
                    {
                        Tmp_node = element;
                            //break;
                        Selected_Nodes.Add(element);// add all nodes having smallest rhs to an array (selected_node)
                        //Console.WriteLine("Node added: " + element);
                    }
                }
            }
           
            
            // Optimize path, to minimize the number of turns
            if (Rhs_List.Count > 0)
            {
                foreach (int element in Parrent_List)
                {
                    if (Node[element].rhs == (double)Rhs_List[0])
                    {
                        Selected_Nodes.Add(element);// add all nodes having smallest rhs to an array (selected_node)
                        //Console.WriteLine("Node added: " + element);
                    }
                }
                foreach (int Selector in Selected_Nodes)
                {
                    if (Path_Storage.Count >= 2)//Check if the future node is on the traight line with the two previous nodes in path_storage
                    {
                        //Console.WriteLine("Path_Storage.Count " + Path_Storage.Count);
                        int Node_0 = (int)Path_Storage[Path_Storage.Count - 2];
                        int Node_1 = (int)Path_Storage[Path_Storage.Count - 1];
                        if (((Node[Node_0].X == Node[Node_1].X) && (Node[Selector].X == Node[Node_1].X)) || ((Node[Node_0].Y == Node[Node_1].Y) && (Node[Selector].Y == Node[Node_1].Y)))
                        {
                            //if the three nodes is on the same line
                            //Console.WriteLine("Node added 1: " + Selector);
                            Tmp_node = Selector;
                            break;
                        }
                        if (Selector == (int)Selected_Nodes[Selected_Nodes.Count - 1])
                        {
                            //if the future node is not in the same line with the two previous nodes
                            //Console.WriteLine("Node added 2: " + Selector);
                            if (Node[Node_0].X != Node[Selector].X)
                            {
                                Tmp_node = Selector;
                                break;
                            }
                        }
                    }
                    else
                    {
                        //Console.WriteLine("Node added 3: " + Selector);
                        Tmp_node = Selector;
                        break;
                    }
                }
            }
            
            //Console.WriteLine("Next Node: " + Tmp_node);
            return Tmp_node;
        }
        private bool IsDiagonal(int Node1, int Node2)
        { 
            if ((Node[Node1].X != Node[Node2].X)&&(Node[Node1].Y != Node[Node2].Y))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private bool Smaller_Key(double[] key1, double[] key2)
        {
            //Console.WriteLine("....Smaller_Key....");
            bool result = false;
         
            if ((key1[0] < key2[0]) || (key1[0] == key2[0] && key1[1] < key2[1]))
            {
                result = true;
            }
            return result;
        }
        private void Predecessors(int Current_Node_ID)
        {
            //Console.WriteLine("....Successor_Node....");
            int UpperNode_ID;
            int LowerNode_ID;
            int RightNode_ID;
            int LeftNode_ID;
            int Check_Successor = 0;
            if (Current_Node_ID - (_Column + 1) >= 0)//upper
            {
                UpperNode_ID = Current_Node_ID - (_Column + 1);//Checking_Node(x,y), Enter coordination (x,y) will return the node Index having this coordination
                Node[Current_Node_ID].Predecessor_Node.Add(UpperNode_ID);
                Check_Successor = Check_Successor + 1;
            }
            if (Current_Node_ID + (_Column + 1) < Node.Length)//lower
            {
                LowerNode_ID = Current_Node_ID + (_Column + 1);
                Node[Current_Node_ID].Predecessor_Node.Add(LowerNode_ID);
                Check_Successor = Check_Successor + 1;
            }
            if (Node[Current_Node_ID].X + 1 < (_Column + 1))//right
            {
                RightNode_ID = Current_Node_ID + 1;
                Node[Current_Node_ID].Predecessor_Node.Add(RightNode_ID);
                Check_Successor = Check_Successor + 1;
            }
            if (Node[Current_Node_ID].X - 1 >= 0)//left
            {
                LeftNode_ID = Current_Node_ID - 1;
                Node[Current_Node_ID].Predecessor_Node.Add(LeftNode_ID);
                Check_Successor = Check_Successor + 1;
            }
            if (Check_Successor == 0)
            {
                Node[Current_Node_ID].Predecessor_Node.Add(-1);//if a node has no neighbor, we add (-1) to the array list. Because we cannot return with a null array.
            }
        }
        private double Diagonal_Heuristic_Estimate(int Current_Node_ID, int Goal_Node_ID)//Diagonal heuricstic 
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
            return Math.Round((Math.Sqrt(2 * Min_Value * Min_Value)),2,MidpointRounding.AwayFromZero) + Remaining;
        }
        private double Manhattan_Heuristic(int Current_Node_ID, int Goal_Node_ID)
        {
            double Dx = Math.Abs(Node[Goal_Node_ID].X - Node[Current_Node_ID].X);
            double Dy = Math.Abs(Node[Goal_Node_ID].Y - Node[Current_Node_ID].Y);
            return (Dx + Dy);
        }
        #endregion
        public ArrayList Get_Path
        {
            get
            {
                return Path_Storage;
            }
        }
        public ArrayList Get_Updated_node
        {
            get
            {
                return List_Of_Updated;
            }
        }
 
        public ArrayList Get_U_ID_List
        {
            get
            {
                return Node_In_U_List;
            }
        }
        public ArrayList Get_Pop_id_node
        {
            get
            {
                return Pop_id_list;
            }
        }
      
    }
}
