
using SaavedraCraft.Model.Interface;
using System.Collections.Generic;
using System.Text;

namespace SaavedraCraft.Model.CollisionEngine
{
    public class CageBox<T>
    {
        private ICollisionable<T> parent;
        private Edge<T>[] edges;

        public CageBox(ICollisionable<T> parent, float width, float heigh)
        {
            this.parent = parent;
            edges = new Edge<T>[4] {
                    new Edge<T>(parent,new[] { 0,1 },width,heigh),
                    new Edge<T>(parent,new[] { 1,0 },width,heigh),
                    new Edge<T>(parent,new[] { 0,-1 },width,heigh),
                    new Edge<T>(parent,new[] { -1,0 },width,heigh)
                };
        }

        public Vertex2d<T>[] ShowCorners()
        {
            List<Vertex2d<T>> retList = new List<Vertex2d<T>>();
            for (int i = 0; i < edges.Length; i++)
            {
                if (!retList.Contains(edges[i].P0))
                {
                    retList.Add(edges[i].P0);
                }
                if (!retList.Contains(edges[i].P1))
                {
                    retList.Add(edges[i].P1);
                }
            }
            return retList.ToArray();
        }

        public Edge<T>[] ShowEdges()
        {
            return this.edges;
        }
    }

}
