using QuarentineSurvival.Model.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuarentineSurvival.Model.CollisionEngine
{
    public class Edge<T>
    {
        private Vertex2d<T> p0;
        private Vertex2d<T> p1;

        public Vertex2d<T> P0 { get { return p0; } }
        public Vertex2d<T> P1 { get { return p1; } }
        public int[] Normal { get; set; }

        private ICollisionable<T> parent;

        public Edge(ICollisionable<T> parent, int[] normal, float width, float heigh)
        {
            p0 = new Vertex2d<T>(0,0,parent);
            p1 = new Vertex2d<T>(0, 0, parent);
            this.parent = parent;
            this.Normal = normal;
            if (Normal[0] == 0 && Normal[1] == 1)//North
            {
                p0 = new Vertex2d<T>(-width / 2, heigh / 2, parent);
                p1 = new Vertex2d<T>(+width / 2, heigh / 2, parent);
            }
            else if (Normal[0] == 1 && Normal[1] == 0)//East
            {
                p0 = new Vertex2d<T>(+width / 2, heigh / 2, parent);
                p1 = new Vertex2d<T>(+width / 2, -heigh / 2, parent);
            }
            else if (Normal[0] == 0 && Normal[1] == -1)//South
            {
                p0 = new Vertex2d<T>(-width / 2, -heigh / 2, parent);
                p1 = new Vertex2d<T>(+width / 2, -heigh / 2, parent);
            }
            else if (Normal[0] == -1 && Normal[1] == 0)//West
            {
                p0 = new Vertex2d<T>(-width / 2, +heigh / 2, parent);
                p1 = new Vertex2d<T>(-width / 2, -heigh / 2, parent);
            }
            else
            {
                throw new Exception("Normal but be modulus 1!");
            }
        }

        public override string ToString()
        {
            return P0.ToString() + "--" + P1.ToString() + "Parent=" + parent.GetName();
        }
    }
}
