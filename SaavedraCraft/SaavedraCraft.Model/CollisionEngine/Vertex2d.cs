using SaavedraCraft.Model.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace SaavedraCraft.Model.CollisionEngine
{
    public class Vertex2d<T>
    {
        public float I { get { return parent.GetCoordI() + deltaI; } }
        public float J { get { return parent.GetCoordJ() + deltaJ; } }

        private float deltaI, deltaJ;
        private ICollisionable<T> parent;

        public Vertex2d(float deltaI, float deltaJ, ICollisionable<T> parent)
        {
            this.deltaI = deltaI;
            this.deltaJ = deltaJ;
            this.parent = parent;
        }

        public float[] GetVectorVelocity()
        {
            double[] vector = new double[2] { parent.GetDirectionI(), parent.GetDirectionJ() };
            double module = Math.Sqrt(vector[0] * vector[0] + vector[1] * vector[1]);
            vector[0] = parent.GetVelocity() * vector[0] / module;
            vector[1] = parent.GetVelocity() * vector[1] / module;
            return new float[] { (float)vector[0], (float)vector[1] };
        }

        public override bool Equals(object other)
        {
            Vertex2d<T> otherCasted = other as Vertex2d<T>;
            if (otherCasted == null)
            {
                return false;
            }
            float distance = (this.I - otherCasted.I) * (this.I - otherCasted.I) + (this.J - otherCasted.J) * (this.J - otherCasted.J);
            return distance < 0.0001;
        }

        public override string ToString()
        {
            return "(" + I + "," + J + ")";
        }

        public override int GetHashCode()
        {
            return ((I * J) + (I + J)).GetHashCode(); //Maybe this is very WRONG!
        }
    }
}
