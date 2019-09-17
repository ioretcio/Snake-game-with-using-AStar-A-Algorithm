using System.Collections.Generic;
using static System.Math;
using System;
namespace Snape
{
    class AStar
    {
        public int Size;
        public List<Node> CalcPath(Node start, Node goal, List<Node> Walls, List<Node> body)
        {
            List<Node> ClosedList = new List<Node>();
            List<Node> OpenList = new List<Node>();

            start.G = 0;
            start.F = 1;
            start.H = Piffa(start, goal);
            start.parent = null;
            Node current;
            OpenList.Add(start);

            int failure = 0;

            do
            {
                failure++;
                if (failure > Size*Size)
                    return null;
                current = MinFNode(OpenList, goal);
                OpenList.RemoveAt(OpenList.IndexOf(current));
                ClosedList.Add(current);

                if (current.X == goal.X && current.Y == goal.Y)
                {
                    Node curcur = ClosedList[ClosedList.Count - 1];
                    List<Node> answer = new List<Node>();
                    while (curcur != null)
                    {
                        answer.Add(curcur);
                        curcur = curcur.parent;
                    }
                    return answer;
                }


                List<Node> Nei = Neigh(current, ClosedList, Walls, body);

                for (int i = 0; i < Nei.Count; i++)
                {
                    int index = OpenList.IndexOf(Nei[i]);
                    if (index == -1 || Nei[i].F > current.G + 1 + Piffa(Nei[i], goal))
                    {
                        Nei[i] = new Node(Nei[i].X, Nei[i].Y, current.G + 1, Piffa(Nei[i], goal), current.G + 1 + Piffa(Nei[i], goal), current);
                        if (index == -1)
                            OpenList.Add(Nei[i]);
                    }
                }
            }
            while (OpenList.Count > 0);


            return ClosedList;
        }







        int Piffa(Node a, Node b)
        {
            return Convert.ToInt32(10 * Sqrt(Pow(b.X - a.X, 2) + Pow(b.Y - a.Y, 2)));
        }
        Node MinFNode(List<Node> nodes, Node goal)
        {
            int result = 0;
            for (int i = 0; i < nodes.Count; i++)
                if (Piffa(nodes[result], goal) > Piffa(nodes[i], goal))
                    result = i;
            return nodes[result];
        }
        List<Node> Neigh(Node node, List<Node> closed, List<Node> walls, List<Node> body)
        {
            List<Node> result = new List<Node>();

            Node tmp = new Node(node.X + 1, node.Y);
            if (IsWallOrNeigh(tmp, closed, walls, body))
                result.Add(tmp);


            tmp = new Node(node.X - 1, node.Y);
            if (IsWallOrNeigh(tmp, closed, walls, body))
                result.Add(tmp);


            tmp = new Node(node.X, node.Y + 1);
            if (IsWallOrNeigh(tmp, closed, walls, body))
                result.Add(tmp);


            tmp = new Node(node.X, node.Y - 1);
            if (IsWallOrNeigh(tmp, closed, walls, body))
                result.Add(tmp);


            return result;
        }
        bool IsWallOrNeigh(Node node, List<Node> closed, List<Node> walls, List<Node> body)
        {
            if (node.X > Size || node.X < 0 || node.Y < 0 || node.Y > Size)
                return false;

            for (int i = 0; i < body.Count; i++)
                if (node.X == body[i].X && node.Y == body[i].Y)
                    return false;

            for (int i = 0; i < closed.Count; i++)
                if (node.X == closed[i].X && node.Y == closed[i].Y)
                    return false;

            for (int i = 0; i < walls.Count; i++)
                if (node.X == walls[i].X && node.Y == walls[i].Y)
                    return false;

            return true;
        }
    }
}