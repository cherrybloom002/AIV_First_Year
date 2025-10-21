using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace Heads_23
{
    class Node
    {
        public int X { get; }
        public int Y { get; }
        public int Cost { get; private set; }
        public List<Node> Neighbours { get; }

        public Vector4 Color;

        public Node(int x, int y, int cost)
        {
            X = x;
            Y = y;
            Cost = cost;
            Neighbours = new List<Node>();
            Color = new Vector4(0.0f, 1.0f, 0.0f, 0.01f);
        }

        public void AddNeighbour(Node node)
        {
            Neighbours.Add(node);
        }

        public void RemoveNeighbour(Node node)
        {
            Neighbours.Remove(node);
        }

        public void SetCost(int cost)
        {
            Cost = cost;
        }
    }
}
