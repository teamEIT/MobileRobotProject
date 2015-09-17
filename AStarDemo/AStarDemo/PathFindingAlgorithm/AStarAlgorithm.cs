/*  A star algorithm simulation
 *  17/09/2015
 *  Class: ClassName
 *  Struct: StrucName
 *  Variable: variableName
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace AStarAlgorithm
{
    //Define Node in A* Algorithm
    struct AStarNodeDefine
    {
        internal int X; //x coordinate in the grid map
        internal int Y; //y coordinate in the grid map
        internal int nodeID;//ID node for searching
        internal int parentNodeID;//If no initial, by default, Node_Parent_Index = 0 (not good because 0 is in the grid, it should be -1);
        internal int distanceScore;
        internal double gScore;
        internal double fScore;
        internal double diagonalScore;
        internal bool isObstacle;
        internal bool isDiagonal;
    }
    class AStarAlgorithm
    {
        //Properties for class AStarAlgorithm
        //Map size
        private int columnForAlgorithm;
        private int rowForAlgorithm;

        private ArrayList path;//
        public ArrayList closedNodes;//

        private AStarNodeDefine[] node;//Create an array of nodes for map
        private int startID;//ID for start

        //Methods for class AStarAlgorithm
        //Constructor
        public AStarAlgorithm(int column, int row)
        {
            path = new ArrayList();// The map of navigated nodes. 
            closedNodes = new ArrayList();//The set of nodes already evaluated.
            columnForAlgorithm = column;
            rowForAlgorithm = row;
            int numberNode = (column + 1) * (row + 1);//because index from 0
            int x = 0;
            int y = 0;//coordinate for start
            node = new AStarNodeDefine[numberNode];//create array of nodes

            for (int i = 0; i < node.Length; i++)
            {
                if (x == column + 1)
                {
                    y = y + 1;
                    x = 0;
                }
                node[i].X = x;//set up coordination for node, from 0 - Array_X length
                node[i].Y = y;//set up coordination for node, from 0 - Array_Y length
                node[i].nodeID = i;
                node[i].fScore = 0;
                node[i].gScore = 0;
                node[i].parentNodeID = -1;//unknow parent, so set -1
                node[i].isObstacle = false;
                node[i].distanceScore = 1;
                node[i].isDiagonal = false;
                node[i].diagonalScore = Math.Round(Math.Sqrt(2), 2, MidpointRounding.AwayFromZero);
                x = x + 1;
            }
        }
        //Change a list of nodes to a list of obstacles
        public void ObstacleSetup(ArrayList nodeIDList)
        {
            for (int i = 0; i < nodeIDList.Count; i++)
            {
                int nodeID = (int)nodeIDList[i];
                node[nodeID].isObstacle = true;
            }
        }
        //Change a node to an obstacle
        public void SingleObstacleSetup(int nodeID)
        {
            node[nodeID].isObstacle = true;
        }
        //Start Algorithm
        public void StartAlgorithm(int startNodeID, int goalNodeID)
        {
            startID = startNodeID;
            ArrayList closedSet = new ArrayList();
            ArrayList openSet = new ArrayList();
            ArrayList fList = new ArrayList();
            int currentNodeID;
            int gScore = 0;//distance from current node to start node;
            int heuristicScore = (int)heuristicEstimate(startNodeID, goalNodeID);
            double fScore = node[startNodeID].gScore + heuristicScore;
        }

        //
        private int GetIDFromFScore(ArrayList nodeList)//This method is used to find a node that has the smallest F in a list of nodes
        {
            int nodeID = -1;
            ArrayList flist = new ArrayList();
            foreach (int openNode in nodeList)
            {
                flist.Add(node[openNode].fScore);
                //Console.WriteLine("Node: " + Open_Node + "---F_score: " + Node[Open_Node].F_Score);
            }
            flist.Sort();
            foreach (int openNode in nodeList)
            {
                if (node[openNode].fScore == (double)flist[0])
                {
                    nodeID = openNode;
                    //Console.WriteLine("Choosen Node: " + Node_ID + "---F_score: " + Node[Node_ID].F_Score);
                    break;
                }
            }
            return nodeID;
        }

        //Diagonal heuricstic
        private double heuristicEstimate(int currentNodeID, int goalNodeID)//Diagonal heuricstic 
        {
            double dx = Math.Abs(node[goalNodeID].X - node[currentNodeID].X);
            double dy = Math.Abs(node[goalNodeID].Y - node[currentNodeID].Y);
            double remaining = Math.Abs(dx - dy);
            double minValue;
            if (dx > dy)
            {
                minValue = dy;
            }
            else
            {
                minValue = dx;
            }
            return (Math.Sqrt(2 * minValue * minValue)) + remaining;
        }
        //
        private void ReconstructionPath(int currentNodeID)
        {
            path.Clear();
            int tmpID = currentNodeID;
            while (tmpID != startID)
            {
                //Console.WriteLine("TMD: " + Tmp_ID);
                path.Add(tmpID);
                tmpID = node[tmpID].parentNodeID;
            }
            //Console.WriteLine("path: " + Tmp_ID);
            //Console.WriteLine("--------------------------------------");
            path.Add(tmpID);
        }
        private bool Compare_Node(int indexNode1, int indexNode2)
        {
            bool result;
            if (indexNode1 == indexNode2)
            {
                result = true;
            }
            else
            {
                result = false;
            }
            return result;
        }
        private bool MatchedID(int item, ArrayList List)//=false if item not in list
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
        public ArrayList GetPath
        {
            get
            {
                return path;
            }
        }
        public ArrayList GetClosedList
        {
            get
            {
                return closedNodes;
            }
        }
    }
}
