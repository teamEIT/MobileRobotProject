using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Nodes_Defined
{
    struct Node_Define
    {
        internal int X;// Coordinate in the grid map
        internal int Y;// Coordiante in the grid map
        internal int Node_ID;
        internal double G_Score; // distance from start_node to the current node
        internal double rhs;
        internal double Perpendicular_ArcCost;// distance from the parent node to the current node, vertical and horizon
        internal double Diagonal_ArcCost;//distance from the parent node to the current node, diagonal
        internal int? Node_Parent_ID;//if no initial, by default, Node_Parent_Index = -1 (not good because 0 is in the grid);
        internal int? Node_Children_ID;
        internal bool Obstacle;//Should not use it
        internal bool? IsDiagonal;
        internal double[] key_list;
        internal bool Node_Of_Path;
        internal ArrayList Predecessor_Node;
        internal ArrayList Successors_List;
        internal ArrayList Directed_Node_List;
        internal string Tag; //NEW, OPEN, CLOSED
        internal int BackPointer;
        internal double Key; // = min(g_Score)
        internal double h_value; // the same as G_Score, use this name is for being easy to remember in D* algorithm.
    }
    class Path_Planning_Node
    {
        public Path_Planning_Node()
        {
            Console.WriteLine("Nothing here");
        }
    }
}
