using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuarentineSurvival.Model;
using QuarentineSurvival.Model.Interface;
using SaavedraCraft.Model.Interfaces;
using SaavedraCraft.Model.Transportation;

namespace QuarentineSurvivalTest
{
    [TestClass]
    public class SimpleCollisionEngineTests
    {
        [TestMethod]
        public void MovableHitsAgainsActionableDoorAtNorthTest()
        {
            IMovableMediumCollisionAware<object> pisoOrigen = new CollisionableMediumAware<object>("ActionStreet", null, 0, 0);
            IMovableMediumCollisionAware<object> pisoDestinoAlNorteConPuerta = new CollisionableMediumAware<object>("NorthActionStreet", null, 0, 1);
            pisoOrigen.SetMovableMediumAtNorth(pisoDestinoAlNorteConPuerta);
            ICollisionable<object> movable = new SimpleMovableCollisionable<object>("ToColideAgainsDoor",null,pisoOrigen);
            ICollisionable<object> puerta = new SimpleMovableCollisionable<object>("Obstaculo", null, pisoDestinoAlNorteConPuerta);
            int[] directionNorth = new[] { 0, 1 };
            movable.SetDirectionI(directionNorth[0]);
            movable.SetDirectionJ(directionNorth[1]);
            movable.SetVelocity(10.0f);//It will complete the trip in one tick
            movable.TimeTick(1.0f);
            Assert.AreEqual(pisoDestinoAlNorteConPuerta,movable.GetMedium());
            Assert.AreEqual(0, movable.GetVelocity());
            Assert.IsTrue(movable.GetCoordJ() < puerta.GetCoordJ());
        }

        [TestMethod]
        public void MovableHitsAgainsActionableDoorAtSouthTest()
        {
            IMovableMediumCollisionAware<object> pisoOrigen = new CollisionableMediumAware<object>("ActionStreet", null, 0, 0);
            IMovableMediumCollisionAware<object> pisoDestinoAlSouthConPuerta = new CollisionableMediumAware<object>("SouthActionStreet", null, 0, -1);
            pisoOrigen.SetMovableMediumAtSouth(pisoDestinoAlSouthConPuerta);
            ICollisionable<object> movable = new SimpleMovableCollisionable<object>("ToColideAgainsDoor", null, pisoOrigen);
            ICollisionable<object> puerta = new SimpleMovableCollisionable<object>("Obstaculo", null, pisoDestinoAlSouthConPuerta);
            int[] directionNorth = new[] { 0, -1 };
            movable.SetDirectionI(directionNorth[0]);
            movable.SetDirectionJ(directionNorth[1]);
            movable.SetVelocity(10.0f);//It will complete the trip in one tick
            movable.TimeTick(1.0f);
            Assert.AreEqual(pisoDestinoAlSouthConPuerta, movable.GetMedium());
            Assert.AreEqual(0, movable.GetVelocity());
            Assert.IsTrue(movable.GetCoordJ() > puerta.GetCoordJ());
        }

        [TestMethod]
        public void MovableHitsAgainsActionableDoorAtWestTest()
        {
            IMovableMediumCollisionAware<object> pisoOrigen = new CollisionableMediumAware<object>("ActionStreet", null, 0, 0);
            IMovableMediumCollisionAware<object> pisoDestinoAlWestConPuerta = new CollisionableMediumAware<object>("WestActionStreet", null, -1, 0);
            pisoOrigen.SetMovableMediumAtWest(pisoDestinoAlWestConPuerta);
            ICollisionable<object> movable = new SimpleMovableCollisionable<object>("ToColideAgainsDoor", null, pisoOrigen);
            ICollisionable<object> puerta = new SimpleMovableCollisionable<object>("Obstaculo", null, pisoDestinoAlWestConPuerta);
            int[] directionWest = new[] { -1, 0 };
            movable.SetDirectionI(directionWest[0]);
            movable.SetDirectionJ(directionWest[1]);
            movable.SetVelocity(10.0f);//It will complete the trip in one tick
            movable.TimeTick(1.0f);
            Assert.AreEqual(pisoDestinoAlWestConPuerta, movable.GetMedium());
            Assert.AreEqual(0, movable.GetVelocity());
            Assert.IsTrue(movable.GetCoordI() > puerta.GetCoordI());
        }

        [TestMethod]
        public void MovableHitsAgainsActionableDoorAtEastTest()
        {
            IMovableMediumCollisionAware<object> pisoOrigen = new CollisionableMediumAware<object>("ActionStreet", null, 0, 0);
            IMovableMediumCollisionAware<object> pisoDestinoAlEastConPuerta = new CollisionableMediumAware<object>("EastActionStreet", null, -1, 0);
            pisoOrigen.SetMovableMediumAtEast(pisoDestinoAlEastConPuerta);
            ICollisionable<object> movable = new SimpleMovableCollisionable<object>("ToColideAgainsDoor", null, pisoOrigen);
            ICollisionable<object> puerta = new SimpleMovableCollisionable<object>("Obstaculo", null, pisoDestinoAlEastConPuerta);
            int[] directionWest = new[] { +1, 0 };
            movable.SetDirectionI(directionWest[0]);
            movable.SetDirectionJ(directionWest[1]);
            movable.SetVelocity(10.0f);//It will complete the trip in one tick
            movable.TimeTick(1.0f);
            Assert.AreEqual(pisoDestinoAlEastConPuerta, movable.GetMedium());
            Assert.AreEqual(0, movable.GetVelocity());
            Assert.IsTrue(movable.GetCoordI() < puerta.GetCoordI());
        }

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
                return ((I * J)+(I+J)).GetHashCode(); //Maybe this is very WRONG!
            }
        }

        public class Edge<T>
        {
            private Vertex2d<T> p0;
            private Vertex2d<T> p1;

            public Vertex2d<T> P0 { get { return p0; } }
            public Vertex2d<T> P1 { get { return p1; } }
            public int[] Normal { get; set; }

            private ICollisionable<T> parent;

            public Edge (ICollisionable<T> parent, int[] normal,float size)
            {
                this.parent = parent;
                this.Normal = normal;
                if (Normal[0] == 0 && Normal[1]==1)//North
                {
                    p0 = new Vertex2d<T>(-size / 2, size / 2, parent);
                    p1 = new Vertex2d<T>(+size / 2, size / 2, parent);
                }
                else if (Normal[0] == 1 && Normal[1] == 0)//East
                {
                    p0 = new Vertex2d<T>(+size / 2, size / 2, parent);
                    p1 = new Vertex2d<T>(+size / 2, -size / 2, parent);
                }
                else if (Normal[0] == 0 && Normal[1] == -1)//South
                {
                    p0 = new Vertex2d<T>(-size / 2, -size / 2, parent);
                    p1 = new Vertex2d<T>(+size / 2, -size / 2, parent);
                }
                else if (Normal[0] == -1 && Normal[1] == 0)//West
                {
                    p0 = new Vertex2d<T>(-size / 2, +size / 2, parent);
                    p1 = new Vertex2d<T>(-size / 2, -size / 2, parent);
                }
                else
                {
                    throw new Exception("Normal but be modulus 1!");
                }
            }
        }

        public class CageBox<T>
        {
            private ICollisionable<T> parent;
            private Edge<T>[] edges;

            public CageBox(ICollisionable<T> parent, float size)
            {
                this.parent = parent;
                edges = new Edge<T>[4] {
                    new Edge<T>(parent,new[] { 0,1 },size),
                    new Edge<T>(parent,new[] { 1,0 },size),
                    new Edge<T>(parent,new[] { 0,-1 },size),
                    new Edge<T>(parent,new[] { -1,0 },size)
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

        public interface ICollisionable<T> : IMovable<T>
        {
            Edge<T>[] ShowEdges();
            Vertex2d<T>[] ShowCorners();
            float GetCollisionTime(ICollisionable<T> other);
        }

        public interface IMovableMediumCollisionAware<T> : IMovableMedium<T>
        {
            float GetCollisionTime(ICollisionable<T> other);
        }

        public class SimpleMovableCollisionable<T> : SimpleMovable<T>, ICollisionable<T>
        {
            private CageBox<T> cageBox;

            public SimpleMovableCollisionable(string aName, T aComponent, IMovableMediumCollisionAware<T> originMedium) : base(aName, aComponent, originMedium)
            {
                cageBox = new CageBox<T>(this, this.GetWidh());//I'm just reading the width to calculate size of CAGE!
            }

            public float GetCollisionTime(ICollisionable<T> other)
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
                else
                {
                    throw new NotImplementedException();
                }
            }

            private float GetCollisionBetweenMovableAndFix(ICollisionable<T> movableCol, ICollisionable<T> fixedCol)
            {
                double[] velocityInverseVector = new[] { (double)-movableCol.GetDirectionI() * movableCol.GetVelocity(), (double)-movableCol.GetDirectionJ() * movableCol.GetVelocity() };
                double module = Math.Sqrt(velocityInverseVector[0] * velocityInverseVector[0] + velocityInverseVector[1] * velocityInverseVector[1]);
                velocityInverseVector[0] = velocityInverseVector[0] / module;
                velocityInverseVector[1] = velocityInverseVector[1] / module;
                List<Edge<T>> edgesThatMayCollide = GetEdgesWhoseNormalsAngleBetweenVectorAreLessThan90(fixedCol, velocityInverseVector);
                List<float> collissionTime = new List<float>();
                for (int i = 0; i < movableCol.ShowCorners().Length; i++)
                {
                    Vertex2d<T> currentVertex = movableCol.ShowCorners()[i];
                    edgesThatMayCollide.ForEach(e => collissionTime.Add(GetCollisionTimeBetweenEdgeAndPoint(e, currentVertex)));
                }
                collissionTime.Sort();
                return collissionTime[0];
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
                if (!(0<=lambda && lambda<=1))
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

            private float GetCollisionFromBothMovable(SimpleMovableCollisionable<T> simpleMovableCollisionable, ICollisionable<T> other)
            {
                throw new NotImplementedException();
            }

            public Vertex2d<T>[] ShowCorners()
            {                
                return cageBox.ShowCorners();
            }

            public Edge<T>[] ShowEdges()
            {
                return cageBox.ShowEdges();
            }
        }

        public class CollisionableMediumAware<T> : ActionStreet<T>, IMovableMediumCollisionAware<T>
        {
            public CollisionableMediumAware(string aName, T aComponent, float newI, float newj) : base(aName, aComponent, newI, newj)
            {
            }

            private List<ICollisionable<T>> ShowAllCollisionables()
            {
                List<ICollisionable<T>> ret = new List<ICollisionable<T>>();
                foreach (IMovable<T> currentMovable in this.GetMovablesOnMedium())
                {
                    ICollisionable<T> collisionableCandidate = currentMovable as ICollisionable<T>;
                    ret.Add(collisionableCandidate);
                }
                return ret;
            }

            public float GetCollisionTime(ICollisionable<T> other)
            {
                List<ICollisionable<T>> otherToCollideWith = ShowAllCollisionables();
                float nextCollision = float.MaxValue;
                foreach (ICollisionable<T> currentCollisionable in otherToCollideWith)
                {
                    if (currentCollisionable != other)
                    {
                        float currentPossibleCollision = currentCollisionable.GetCollisionTime(other);
                        if (nextCollision > currentPossibleCollision)
                        {
                            nextCollision = currentPossibleCollision;
                        }
                    }
                }
                return nextCollision;
            }

            public override void OnMovableMoving(IMovable<T> simpleMovable, float timedelta)
            {
                ICollisionable<T> simpleMovableAsCollisionable = simpleMovable as ICollisionable<T>;
                if (simpleMovableAsCollisionable != null)
                {
                    float nextCollisionTime = GetCollisionTime(simpleMovableAsCollisionable);
                    if (nextCollisionTime<=timedelta)
                    {
                        //TODO;
                        //throw new NotImplementedException("Seguir!");
                        base.OnMovableMoving(simpleMovable, nextCollisionTime);//ProcessNormally with a delta = to the time of collission
                        timedelta -= nextCollisionTime;
                        float movableDeltaI;
                        float movableDeltaJ;
                        movableDeltaI = simpleMovable.GetCoordI() - (this.GetCoordI() + MOVABLE_MEDIUM_EDGE_LIMIT / 2);
                        movableDeltaJ = simpleMovable.GetCoordJ() - (this.GetCoordJ() + MOVABLE_MEDIUM_EDGE_LIMIT / 2);
                        simpleMovable.OnColissionAt(movableDeltaI,movableDeltaJ);
                        return;
                    }
                }
                base.OnMovableMoving(simpleMovable, timedelta);
            }
        }
    }
}
