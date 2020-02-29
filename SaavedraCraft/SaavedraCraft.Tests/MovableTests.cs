using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SaavedraCraft.Model;
using SaavedraCraft.Model.Constructions;
using SaavedraCraft.Model.Interfaces;

namespace SaavedraCraft.Tests
{
    [TestClass]
    public class MovableTests
    {
        [TestMethod]
        public void MovableSimpleOriginDestinyTest()
        {
            IMovableMedium<object> streetOrigin = new SimpleStreet("CalleOrigin",null,0,-1);
            IMovableMedium<object> streetDestiny = new SimpleStreet("CalleDest", null, 0, 0);
            streetOrigin.SetMovableMediumAtNorth(streetDestiny);
            IMovable<object> simpleMovable = new SimpleMovable("Transporter",null, 0 , 0 ,streetOrigin);
            int[] northDirection = new[] { 0 , 1 };
            simpleMovable.SetDirectionI(northDirection[0]);
            simpleMovable.SetDirectionJ(northDirection[1]);
            simpleMovable.SetVelocity(1.0f);//It will complete the trip in one tick
            simpleMovable.TimeTick(1.0f);
            Assert.AreEqual(streetDestiny, simpleMovable.GetMedium());
        }

        public class SimpleStreet : BasicContruction<object>, IMovableMedium<object>
        {
            private IMovableMedium<object> movableMediumAtNorth;

            public SimpleStreet(string aName, object aComponent, int newI, int newj) : base(aName, aComponent, newI, newj)
            {
            }

            public override IObject<object> CloneMe()
            {
                throw new NotImplementedException();
            }

            public override string GetConstructionInfo()
            {
                throw new NotImplementedException();
            }

            public IMovableMedium<object> GetMovableMediumAtEast()
            {
                throw new NotImplementedException();
            }

            public IMovableMedium<object> GetMovableMediumAtNorth()
            {
                return movableMediumAtNorth;
            }

            public IMovableMedium<object> GetMovableMediumAtSouth()
            {
                throw new NotImplementedException();
            }

            public IMovableMedium<object> GetMovableMediumAtWest()
            {
                throw new NotImplementedException();
            }

            public float MaxSpeed()
            {
                throw new NotImplementedException();
            }

            public void SetMovableMediumAtNorth(IMovableMedium<object> movableMediumAtNorth)
            {
                this.movableMediumAtNorth = movableMediumAtNorth;
            }

            public override void TimeTick(float timedelta)
            {
                throw new NotImplementedException();
            }
        }

        public class SimpleMovable : BasicObject<object>, IMovable<object>
        {
            private IMovableMedium<object> currentMovableMedium;
            private int directionI;
            private int directionJ;
            private float velocity;
            private float distanceI;

            public SimpleMovable(string aName, object aComponent, int newI, int newj, IMovableMedium<object> originMedium) : base(aName, aComponent, newI, newj)
            {
                distanceI = 0.5f;
                currentMovableMedium = originMedium;
            }

            public override int GetCoordI()
            {
                return currentMovableMedium.GetCoordI();
            }

            public override int GetCoordJ()
            {
                return currentMovableMedium.GetCoordJ();
            }

            public override IObject<object> CloneMe()
            {
                throw new NotImplementedException();
            }

            public int GetDirectionI()
            {
                return directionI;
            }
            public int GetDirectionJ()
            {
                return directionJ;
            }

            public IMovableMedium<object> GetMedium()
            {
                return currentMovableMedium;
            }

            public float GetVelocity()
            {
                throw new NotImplementedException();
            }

            public void SetDirectionI(int newDirectionI)
            {
                directionI = newDirectionI;
            }

            public void SetDirectionJ(int newDirectionJ)
            {
                directionJ = newDirectionJ;
            }

            public void SetMedium(IMovableMedium<object> newMedium)
            {
                throw new NotImplementedException();
            }

            public void SetVelocity(float newVelocity)
            {
                this.velocity = newVelocity;
            }

            public override void TimeTick(float timedelta)
            {
                distanceI += this.GetDirectionJ() * timedelta;
                if (distanceI >= 1)
                {
                    traslateNorth();
                }
            }

            private void traslateNorth()
            {
                currentMovableMedium = currentMovableMedium.GetMovableMediumAtNorth();
            }
        }
    }
}
