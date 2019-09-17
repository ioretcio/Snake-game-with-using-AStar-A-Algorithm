namespace Snape
{
    class Node
    {
        public int X, Y, G, H, F;
        public Node(int X, int Y)
        {
            this.X = X; this.Y = Y;
        }
        public Node(int X, int Y, int G, int H, int F, Node parent)
        {
            this.X = X; this.Y = Y; this.G = G; this.H = H; this.F = F; this.parent = parent;
        }
        public Node parent;
    }
}
