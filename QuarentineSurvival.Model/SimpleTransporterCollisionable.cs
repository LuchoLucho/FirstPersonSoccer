using SaavedraCraft.Model.CollisionEngine;
using SaavedraCraft.Model.Interface;
using SaavedraCraft.Model.Interfaces;
using SaavedraCraft.Model.Transportation;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuarentineSurvival.Model
{
    public class SimpleTransporterCollisionable<T> : SimpleTransporter<T>, ICollisionable<T>
    {
        private CageBox<T> cageBox;

        public SimpleTransporterCollisionable(string aName, T aComponent, IMovableMedium<T> originMedium, ITransporterAndWarehouseManager<T> transporterAndWarehouseManager) : base(aName, aComponent, originMedium, transporterAndWarehouseManager)
        {            
        }

        private CageBox<T> getCageBox()
        {
            if (cageBox == null)
            {
                cageBox = new CageBox<T>(this, this.GetWidh(), this.GetHeigh());//I'm just reading the width to calculate size of CAGE!
            }
            return cageBox;
        }

        #region SimpleMovableCollisionable
        public virtual QuarentineCollision<T> GetCollision(ICollisionable<T> other)
        {
            if (GetVelocity() > 0 && other.GetVelocity() > 0)
            {
                return GetCollisionFromBothMovable(this, other);
            }
            else if (GetVelocity() > 0 && other.GetVelocity() == 0)
            {
                return GetCollisionBetweenMovableAndFix(this, other);
            }
            else if (GetVelocity() == 0 && other.GetVelocity() > 0)
            {
                return GetCollisionBetweenMovableAndFix(other, this);
            }
            throw new NotImplementedException();
        }

        private QuarentineCollision<T> GetCollisionBetweenMovableAndFix(ICollisionable<T> movableCol, ICollisionable<T> fixedCol)
        {            
            double[] velocityInverseVector = new[] { (double)-movableCol.GetDirectionI() * movableCol.GetVelocity(), (double)-movableCol.GetDirectionJ() * movableCol.GetVelocity() };
            double module = Math.Sqrt(velocityInverseVector[0] * velocityInverseVector[0] + velocityInverseVector[1] * velocityInverseVector[1]);
            velocityInverseVector[0] = velocityInverseVector[0] / module;
            velocityInverseVector[1] = velocityInverseVector[1] / module;
            List<Edge<T>> edgesThatMayCollide = GetEdgesWhoseNormalsAngleBetweenVectorAreLessThan90(fixedCol, velocityInverseVector);
            List<float> collissionTime = new List<float>();
            //Log("GetCollisionBetweenMovableAndFix----INIT"+movableCol + " & " + fixedCol);
            for (int i = 0; i < movableCol.ShowCorners().Length; i++)
            {
                Vertex2d<T> currentVertex = movableCol.ShowCorners()[i];
                edgesThatMayCollide.ForEach(e => collissionTime.Add(GetCollisionTimeBetweenEdgeAndPoint(e, currentVertex)));
            }
            if (collissionTime.Count == 0)
            {
                QuarentineCollision<T> nullColission = movableCol.GetCustomCollision(new List<ICollisionable<T>> { fixedCol }, float.MaxValue);
                return nullColission;
            }
            collissionTime.Sort();
            /*QuarentineCollision<T> customCollision = movableCol.GetCustomCollision(new List<ICollisionable<T>> { fixedCol }, collissionTime[0]);
            if (customCollision != null)
            {
                return customCollision;
            }
            QuarentineCollision<T> customCollision = fixedCol.GetCustomCollision(new List<ICollisionable<T>> { movableCol }, collissionTime[0]);
            if (customCollision != null)
            {
                return customCollision;
            }
            return new HardCollision<T>(new List<ICollisionable<T>> { movableCol, fixedCol }, collissionTime[0]);*/
            return fixedCol.GetCustomCollision(new List<ICollisionable<T>> { movableCol, fixedCol }, collissionTime[0]);
        }

        private float GetCollisionTimeBetweenEdgeAndPoint(Edge<T> e, Vertex2d<T> currentVertex)
        {
            float[] u = new float[] { e.P1.I - e.P0.I, e.P1.J - e.P0.J };
            float[] v = currentVertex.GetVectorVelocity();
            float vxvy = v[0] / v[1];
            float vyvx = v[1] / v[0];
            float p0x = e.P0.I;
            float p0y = e.P0.J;
            float p1x = e.P1.I;
            float p1y = e.P1.J;
            float q0x = currentVertex.I;
            float q0y = currentVertex.J;
            float lambda1 = 0;
            float lambda2 = 0;
            if (v[1] == 0)
            {
                lambda1 = vyvx * q0x + p0y - q0y - p0x * vyvx;
                lambda2 = (p1x - p0x) * vyvx + (p0y - p1y);
            }
            else
            {
                lambda1 = vxvy * q0y + p0x - q0x - p0y * vxvy;
                lambda2 = (p1y - p0y) * vxvy + (p0x - p1x);
            }
            float lambda = lambda1 / lambda2;
            if (!(0 <= lambda && lambda <= 1))
            {
                return float.MaxValue;
            }
            float t = 0;
            if (v[0] == 0)
            {
                if (v[1] == 0)
                {
                    return float.MaxValue;
                }                
                t = (lambda * (p1y - p0y) + p0y - q0y) / v[1];
            }
            else
            {
                t = (lambda * (p1x - p0x) + p0x - q0x) / v[0];
            }
            if (t<0)
            {
                return float.MaxValue;
            }
            //Log("Lambda = " + lambda + " Time = " + t + " Edge " + e + " and Vertex " + currentVertex);
            return t;
        }

        private List<Edge<T>> GetEdgesWhoseNormalsAngleBetweenVectorAreLessThan90(ICollisionable<T> other, double[] vector)
        {
            List<Edge<T>> edgesThatMayCollide = new List<Edge<T>>();
            for (int i = 0; i < other.ShowEdges().Length; i++)
            {
                Edge<T> currentFixedEdge = other.ShowEdges()[i];
                double[] edgeNormal = { currentFixedEdge.Normal[0], currentFixedEdge.Normal[1] };
                double scalarProduct = vector[0] * edgeNormal[0] + vector[1] * edgeNormal[1];
                double angleBetweenInverseVelocityAndEdgeNormal = Math.Acos(scalarProduct);
                if (Math.Abs(angleBetweenInverseVelocityAndEdgeNormal) < Math.PI / 2)
                {
                    edgesThatMayCollide.Add(currentFixedEdge);
                }
            }
            return edgesThatMayCollide;
        }

        private QuarentineCollision<T> GetCollisionFromBothMovable(ICollisionable<T> simpleMovableCollisionable, ICollisionable<T> other)
        {
            throw new NotImplementedException();
        }

        public Vertex2d<T>[] ShowCorners()
        {
            return getCageBox().ShowCorners();
        }

        public Edge<T>[] ShowEdges()
        {
            return getCageBox().ShowEdges();
        }
        #endregion

        public override string ToString()
        {
            string ret = this.GetName() + "Pos = (" + GetCoordI() + "," + GetCoordJ() + ")";
            ret += "Delta(" + GetDeltaI() + "," + GetDeltaJ() + ")";
            ret += "-Cage(";
            for (int i = 0; i < this.getCageBox().ShowCorners().Length;i++)
            {
                ret += getCageBox().ShowCorners()[i].ToString() + ",";
            }
            ret += ") Width = " + this.GetWidh() + " Heigh = " + this.GetHeigh();
            return ret;
        }

        public virtual QuarentineCollision<T> GetCustomCollision(List<ICollisionable<T>> list, float collisionTime)
        {
            return new HardCollision<T>(list, collisionTime);
        }
    }
}
