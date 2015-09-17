using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Threading;
using Nodes_Defined;

namespace D_Star_Algorithm
{
    class D_Star
    {
        private int _Column;
        private int _Row;
        private ArrayList Path_Storage = new ArrayList();
        private ArrayList OPEN_LIST = new ArrayList();
        public ArrayList Inserted_List = new ArrayList();
        private ArrayList Re_Calculated_Node = new ArrayList();
        private ArrayList Open_Nodes = new ArrayList();
        private ArrayList Closed_Nodes = new ArrayList();
        private Node_Define[] Node;
        private int Start_ID;
        private int Goal_ID;
        private int Infinite_Value;
        private int Number_Of_Node;
        private ArrayList List_Of_Updated = new ArrayList();
        int x = 0;
        int y = 0;
        public D_Star(int column, int row)
        {
            _Column = column;
            _Row = row;
            Infinite_Value = (_Column + 1) * (_Row + 1)*2 + 1;// infinite value must be greater than the total number of nodes in map
            Number_Of_Node = (_Column + 1) * (_Row + 1);
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
                Node[i].G_Score = 0;//infinite
                Node[i].Obstacle = false;
                Node[i].Perpendicular_ArcCost = 1;
                Node[i].Diagonal_ArcCost = Math.Round(Math.Sqrt(2), 2, MidpointRounding.AwayFromZero);
                Node[i].IsDiagonal = false;
                Node[i].Node_Of_Path = false;
                Node[i].Tag = "NEW";
                Node[i].BackPointer = -1;
                x = x + 1;
            }

        }
        public void Reset_D_Star()
        {
            for (int i = 0; i < Node.Length; i++)
            {
                Node[i].G_Score = 0;//
                Node[i].Obstacle = false;
                Node[i].IsDiagonal = false;
                Node[i].Node_Of_Path = false;
                Node[i].Tag = "NEW";
                Node[i].BackPointer = -1;
                Node[i].h_value = -1;
            }
            Path_Storage.Clear();
            OPEN_LIST.Clear();
            Inserted_List.Clear();
            Re_Calculated_Node.Clear();
            Open_Nodes.Clear();
            Closed_Nodes.Clear();
        }
        public void Start(int Start_Node_ID, int Goal_Node_ID)
        {
            //Console.WriteLine(".....Start.....");
            Path_Storage.Clear();
            Goal_ID = Goal_Node_ID;
            Start_ID = Start_Node_ID;
            //Initial
            Node[Goal_ID].h_value = 0;
            INSERT(Goal_ID, Node[Goal_ID].h_value);
            //
            double Process_Result = 0;
            while(Process_Result >-1) //&& Node[Start_ID].Tag != "CLOSED")
            {
                Process_Result = PROCESS_STATE();
            }
            RECONSTRUCTION(Start_ID);
        }
        public void Update_D_Star()
        {
            //Console.WriteLine(".....Restart.....");
            Closed_Nodes.Clear();
            Path_Storage.Clear();
            Re_Calculated_Node.Clear();
            double Process_Result = 0;
            while (Process_Result > -1)// && Node[Start_ID].Tag != "CLOSED")
            {
                Process_Result = PROCESS_STATE();
            }
            RECONSTRUCTION(Start_ID);
        }
        public void Update_Arc_Cost(int Node_ID)
        {
            //Console.WriteLine("Update: " + Node_ID);
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
            int C_Val = Infinite_Value;
            MODIFY_COST(Node_ID, C_Val);
            Node[Node_ID].Obstacle = true;
        }
        private double MODIFY_COST(int X, int C_Val)
        {
            if (Node[X].Tag == "CLOSED")
            {
                //X is changed from a normal Node to an Obstacle node
                double h_value = C_Val;
                INSERT(X, h_value);
            }
            return GET_KMIN();
        }
        public int Get_New_Start_Point
        {
            get
            {
                return Start_ID;
            }
        }
        public void Obstacle_Setup(int Node_ID, bool obstacle)
        {
            Node[Node_ID].Obstacle = obstacle;
        }
        private double PROCESS_STATE()
        {
            //Console.WriteLine("...............PROCESS_STATE.......");
            double K_Old = -1;
            int X = MIN_STATE();
            double h_X = Node[X].h_value;
            if (X == -1)
            {
                return -1;
            }
            K_Old = GET_KMIN();
            //Console.WriteLine("K_Old: " + K_Old);
            //Console.WriteLine("Min_State: " + X);
            DELETE(X);
            if (K_Old < h_X)
            {
                foreach (int Y in NEIGHBOR(X))
                {
                    double h_Y = Node[Y].h_value;
                    int B_Y = Node[Y].BackPointer;
                    int B_X = Node[X].BackPointer;
                    string Tag_Y = Node[Y].Tag;
                    if (h_Y <= K_Old && h_X > h_Y + ARC_COST(Y, X, B_Y))
                    {
                        //if the distance from Y to the current node (X) < the current node distance, then X parrent should be Y
                        Node[X].BackPointer = Y;
                        h_X = h_Y + ARC_COST(Y, X, B_Y);//must do this line, because h_X need to be updated to reuse in the "else" condition
                        Node[X].h_value = h_X;
                        //Console.WriteLine("--Kold<h ---- BPoFX:" + Node[X].BackPointer + " --hx: " + Node[X].h_value);
                    }
                }
            }
            if (K_Old == Node[X].h_value) // At the first search, this condition is used.
            {
                foreach (int Y in NEIGHBOR(X))
                {
                    Console.WriteLine("Y: " + Y + " - X: " + X);
                    Console.WriteLine("Node[Y].X = " + Node[Y].X + "- Node[X].X = " + Node[X].X);
                    double h_Y = Node[Y].h_value;
                    int B_Y = Node[Y].BackPointer;
                    int B_X = Node[X].BackPointer;
                    string Tag_Y = Node[Y].Tag;
                    if ((Tag_Y == "NEW") || (B_Y == X && h_Y != h_X + ARC_COST(X, Y, B_X)) || (B_Y != X && h_Y > h_X + ARC_COST(X, Y, B_X)))// original:(B_Y != X && h_Y > h_X + ARC_COST(X, Y))
                    {
                        double new_h_x;
                        Node[Y].BackPointer = X;
                        new_h_x = h_X + ARC_COST(X, Y, B_X);
                        //Console.WriteLine("--Kold=h ---- BPofInserted:" + Node[Y].BackPointer + " --hx: " + new_h_x);
                        INSERT(Y, new_h_x);
                    }
                }
            }
            else
            { //This condition is used to update info when a obs appears on the path
                foreach (int Y in NEIGHBOR(X))
                {
                    double h_Y = Node[Y].h_value;
                    int B_Y = Node[Y].BackPointer;
                    int B_X = Node[X].BackPointer;
                    string Tag_Y = Node[Y].Tag;
                    if ((Tag_Y == "NEW") || (B_Y == X && h_Y != h_X + ARC_COST(X, Y, B_X)))
                    {
                        double new_h_y;
                        Node[Y].BackPointer = X;
                        new_h_y = h_X + ARC_COST(X, Y, B_X);
                        //Console.WriteLine("--Else Kold < hx1 -- BPofInserted:" + Node[Y].BackPointer + " --- hy: " + new_h_y);
                        INSERT(Y, new_h_y);
                    }
                    else
                    {
                        if (B_Y != X && h_Y > h_X + ARC_COST(X, Y, B_X))
                        {
                            //Console.WriteLine("--Else Kold < hx2  ---hX: " + h_X);
                            INSERT(X, h_X);
                        }
                        else
                        {
                            if (B_Y != X && h_X > h_Y + ARC_COST(Y, X, B_Y) && Tag_Y == "CLOSED" && h_Y > K_Old)
                            {
                                //Console.WriteLine("--Else Kold < hx3  ---hy: " + h_Y);
                                INSERT(Y, h_Y);
                            }
                        }
                    }
                }
            }
            return GET_KMIN();
        }
        
        private int MIN_STATE()
        {            
            if (OPEN_LIST.Count > 0)
            {
                //Console.WriteLine("MIN_STATE: " + OPEN_LIST[0]);
                return (int)OPEN_LIST[0];
            }
            else
                return -1;
        }
        private double GET_KMIN()
        {
            //Console.WriteLine("Open_List in Kmin:" + OPEN_LIST.Count);
            if (OPEN_LIST.Count > 0)
            {
                int Opened_Node = (int)OPEN_LIST[0];
                //Console.WriteLine("Kmin:" + Node[Opened_Node].Key);
                return Node[Opened_Node].Key;
            }
            else
                return -1;
        }
        private void INSERT(int Node_ID, double h_new)
        {
            //Console.WriteLine("Insert: " + Node_ID);
            double kx = -1;
            Inserted_List.Add(Node_ID);
            if (Node[Node_ID].Tag == "NEW")
            {
                Node[Node_ID].Tag = "OPEN";
                //Console.WriteLine("NEW: " + Node_ID);
                kx = h_new;
                Node[Node_ID].Key = kx;
                //Console.WriteLine("Kx: " + kx);
            }
            if (Node[Node_ID].Tag == "OPEN")
            {
                //Console.WriteLine("OPEN: " + Node_ID);
                kx = Math.Min(Node[Node_ID].Key, h_new);
                Node[Node_ID].Key = kx;
                //Console.WriteLine("Kx: " + kx);
            }
            if (Node[Node_ID].Tag == "CLOSED")
            {
                //Console.WriteLine("CLOSED: " + Node_ID);
                //Node[Node_ID].h_value = h_new;
                kx = Math.Min(Node[Node_ID].h_value, h_new);
                Node[Node_ID].Key = kx;
                Node[Node_ID].h_value = h_new;
                Node[Node_ID].Tag = "OPEN";
                if (Node[Node_ID].Obstacle == false)
                {
                    Re_Calculated_Node.Add(Node_ID);
                }
                //Console.WriteLine("Kx: " + kx);
            }
            Node[Node_ID].h_value = h_new;
            if (OPEN_LIST.Count > 0)
            {
                int checksum = 0;
                for (int i = 0; i < OPEN_LIST.Count; i++)
                {
                    int Open_Node = (int)OPEN_LIST[i];
                    //if a <=b, a is added before b, if a is the greatest, a is added at the end.
                    if (Node[Node_ID].Key <= Node[Open_Node].Key)// Key1<= key2
                    {
                        OPEN_LIST.Insert(i, Node_ID);
                        Open_Nodes.Add(Node_ID);
                        //Node[Node_ID].Tag = "OPEN";
                        //Console.WriteLine("Insert: " + Node_ID);
                        break;
                    }
                    else
                        checksum = checksum + 1;
                }
                //Add the greatest value to the end of array
                if (checksum == OPEN_LIST.Count)
                {
                    OPEN_LIST.Add(Node_ID);
                    //Console.WriteLine("Insert: " + Node_ID);
                }
            }
            else
            {
                OPEN_LIST.Add(Node_ID);
                //Console.WriteLine("Insert: " + Node_ID);
            }
            //Console.WriteLine("Open_List in Insert:" + OPEN_LIST.Count);
        }
        private void DELETE(int Node_ID)
        {
            OPEN_LIST.Remove(Node_ID);
            Node[Node_ID].Tag = "CLOSED";
            if (Node[Node_ID].Obstacle == false)
            {
                Closed_Nodes.Add(Node_ID);
            }
            //Console.WriteLine("Remove: " + Node_ID);
        }
        private void RECONSTRUCTION(int X)
        {
            int Tmp = X;
            Path_Storage.Clear();
            //Console.WriteLine("Start: " + Tmp);
            while (Tmp != Goal_ID)
            {
                if (Tmp > -1)
                {
                    //Console.WriteLine("Tmp: " + Tmp);
                    Path_Storage.Add(Tmp);
                    Node[Tmp].Node_Of_Path = true;
                    Tmp = Node[Tmp].BackPointer;
                    //Console.WriteLine("Backpointer: " + Tmp);
                }
            }
            Path_Storage.Add(Tmp);
        }
        private ArrayList NEIGHBOR(int Current_Node_ID)
        {
            ArrayList Nodes = new ArrayList();
            Nodes.Add(-1);
            if (Current_Node_ID - (_Column + 1) >= 0)//upper
            {
                int UpperNode_ID;
                UpperNode_ID = Current_Node_ID - (_Column + 1);
                Nodes.Add(UpperNode_ID);
            }
            if (Current_Node_ID + (_Column + 1) < Node.Length)//lower
            {
                int LowerNode_ID;
                LowerNode_ID = Current_Node_ID + (_Column + 1);
                Nodes.Add(LowerNode_ID);
            }
            if (Node[Current_Node_ID].X + 1 < (_Column + 1))//right
            {
                int RightNode_ID;
                RightNode_ID = Current_Node_ID + 1;
                Nodes.Add(RightNode_ID);
            }
            //Console.WriteLine("Current Node:" + Current_Node_ID);
            if (Node[Current_Node_ID].X - 1 >= 0)//left
            {
                int LeftNode_ID;
                LeftNode_ID = Current_Node_ID - 1;
                Nodes.Add(LeftNode_ID);
            }
            //------------------------

            //-----------------------
            if (Nodes.Count > 1)
            {
                Nodes.Remove(-1);
            }
            return Nodes;
        }
        private double ARC_COST(int X, int Y, int Z)
        {
            //Console.WriteLine("Z: " + Z);
            if (Node[Y].Obstacle == false )
            {
                if (Z > 0)
                {
                    if (((Node[X].X == Node[Y].X) && (Node[Y].X == Node[Z].X)) || ((Node[X].Y == Node[Y].Y) && (Node[Z].Y == Node[X].Y)))
                        return 1;
                    else
                        return 1.5;
                }
                else
                    return 1;
            }
            else
            {
                //Console.WriteLine(Y + "is Obstacle");
                return Infinite_Value;
            }
        }
        public ArrayList Get_Path
        {
            get
            {
                return Path_Storage;
            }
        }
        public ArrayList Get_Open_All_Nodes
        {
            get
            {
                return Open_Nodes;
            }
        }
        public ArrayList Get_Closed_Nodes
        {
            get
            {
                return Closed_Nodes;
            }
        }
        public ArrayList Get_ReOpen_Nodes
        {
            get
            {
                return Re_Calculated_Node;
            }
        }
    }
}
