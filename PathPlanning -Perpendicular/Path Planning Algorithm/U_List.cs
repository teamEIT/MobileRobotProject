using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
//using LPA_Star_Algorithm;
using System.Diagnostics;
using Nodes_Defined;
namespace U_Node_List
{
    class U_List
    {
        ArrayList Node_List;
        ArrayList ID_List;
        public U_List()
        {
            Node_List = new ArrayList();
            ID_List = new ArrayList();
            
        }
        public void Insert(Node_Define Node)
        {
            Node_Define Inserted_Node = Node;
            double[] key1 = new double[2];
            key1[0] = Inserted_Node.key_list[0];//inserted Node
            key1[1] = Inserted_Node.key_list[1];
            if (Node_List.Count > 0)
            {
                int checksum = 0;
                //int a = Math.Min(Node_List.Count, 4);
                for (int i = 0; i < Node_List.Count; i++)
                //for (int i = 0; i < a; i++)
                {
                    Node_Define Checked_Node = (Node_Define)Node_List[i];
                    double[] key2 = new double[2];
                    key2[0] = Checked_Node.key_list[0]; //Exsisting node
                    key2[1] = Checked_Node.key_list[1];//Existing node
                    //if a <b, a is added before b, if a is the greatest, a is added at the end.
                    if ((key1[0] < key2[0]) || (key1[0] == key2[0] && key1[1] < key2[1]))// Key1< key2
                    {
                        Node_List.Insert(i, Node);//inserted Node has index i, checked node's index is increased automatically to i+1
                        break;
                    }
                    if (key1[0] == key2[0] && key1[1] == key2[1])// if key1 = key2, inserted node is till has index i, checked node's index is the same above
                    {
                        Node_List.Insert(i, Node);
                        break;
                    }
                    checksum = checksum + 1;
                }
                //Add the greatest value to the end of array
                if (checksum == Node_List.Count)
                //if (checksum == a)
                {
                    Node_List.Add(Node);
                }
            }
            else
            {
                Node_List.Add(Node);
            }
           
           // Console.WriteLine("Ulist elapsed:" + tt);
        }
        public void Remove(int node)
        {
            if (Node_List.Count > 0)
            {
                Node_List.Remove(Search_ID(node));
            }
        }
        public int Top_Node()// Return Top_node Id
        {
            int Top_Node_ID = -1;
            if (Node_List.Count > 0)
            {
                Top_Node_ID = ((Node_Define)Node_List[0]).Node_ID;
            }
            return Top_Node_ID;
        }
        public int Pop()// Return Top_Node and remove it
        {
            int Top_Node_ID = -1;
            if (Node_List.Count > 0)
            {
                Top_Node_ID = ((Node_Define)Node_List[0]).Node_ID;
                Node_List.Remove((Node_Define)Node_List[0]);
                //Node_List.Remove(Search_ID(Top_Node_ID));
            }
            return Top_Node_ID;
        }
        public bool Is_In_List(int Node_ID)
        {
            int Input_Node_ID = Node_ID;
            bool result = false;
            if (Node_List.Count > 0)
            {
                for (int i = 0; i < Node_List.Count; i++)
                {
                    if (((Node_Define)Node_List[i]).Node_ID == Input_Node_ID)
                    {
                        result = true;
                        return result;
                    }
                }
            }
            return result;
        }
        public ArrayList Get_U_List
        { 
            get
            {
                return Node_List;
            }
        }

        private Node_Define Search_ID(int Node_ID)
        {
            Node_Define result = new Node_Define();
            result.Node_ID = -1;
            for (int i = 0; i < Node_List.Count; i++)
            {
                if (Node_ID == ((Node_Define)Node_List[i]).Node_ID)
                {
                    result = (Node_Define)Node_List[i];
                    return result;
                }
            }
            return result;
        }
        public void Show_U_List(bool display)
        { 
            foreach (Node_Define element in Node_List)
            {
                ID_List.Add(element.Node_ID);
            }
        }
        public ArrayList Get_ID_List
        {
            get
            {
                return ID_List;
            }
        }
    }
}
