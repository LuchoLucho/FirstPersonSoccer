using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuarentineSurvival.Model;
using QuarentineSurvival.Model.Interface;
using SaavedraCraft.Model;
using SaavedraCraft.Model.Constructions;
using SaavedraCraft.Model.Interfaces;
using SaavedraCraft.Model.Interfaces.Transportation;
using SaavedraCraft.Model.Resources;
using SaavedraCraft.Model.Transportation;

namespace SaavedraCraft.Tests
{
    [TestClass]
    public class MovableTests
    {
        [TestMethod] 
        public void MovableSimpleMoveNorthOriginDestinyTest()
        {
            IMovableMediumCollisionAware<object> streetOrigin = new ActionCollisionableMediumAware<object>("CalleOrigin",null,0,-1);
            IMovableMediumCollisionAware<object> streetDestinyAtNorth = new ActionCollisionableMediumAware<object>("CalleDest", null, 0, 0);
            streetOrigin.SetMovableMediumAtNorth(streetDestinyAtNorth);
            IMovable<object> simpleMovable = new SimpleMovable<object>("Simple",null, streetOrigin);
            int[] northDirection = new[] { 0 , 1 };
            simpleMovable.SetDirectionI(northDirection[0]);
            simpleMovable.SetDirectionJ(northDirection[1]);
            simpleMovable.SetVelocity(1.0f);//It will complete the trip in one tick
            simpleMovable.TimeTick(1.0f);
            Assert.AreEqual(streetDestinyAtNorth, simpleMovable.GetMedium());
            Assert.AreEqual(streetDestinyAtNorth.GetCoordI() + SimpleStreet<object>.MOVABLE_MEDIUM_EDGE_LIMIT / 2, simpleMovable.GetCoordI());
            Assert.AreEqual(streetDestinyAtNorth.GetCoordJ() + SimpleStreet<object>.MOVABLE_MEDIUM_EDGE_LIMIT / 2, simpleMovable.GetCoordJ());
        }

        [TestMethod]
        public void MovableSimpleMoveSouthOriginDestinyTest()
        {
            IMovableMedium<object> streetOrigin = new SimpleStreet<object>("CalleOrigin", null, 0, 0);
            IMovableMedium<object> streetDestinyAtSouth = new SimpleStreet<object>("CalleDest", null, 0, -1);
            streetOrigin.SetMovableMediumAtSouth(streetDestinyAtSouth);
            IMovable<object> simpleMovable = new SimpleMovable<object>("Simple", null, streetOrigin);
            int[] direction = new[] { 0, -1 };
            simpleMovable.SetDirectionI(direction[0]);
            simpleMovable.SetDirectionJ(direction[1]);
            simpleMovable.SetVelocity(1.0f);//It will complete the trip in one tick
            simpleMovable.TimeTick(1.0f);
            Assert.AreEqual(streetDestinyAtSouth, simpleMovable.GetMedium());
            Assert.AreEqual(streetDestinyAtSouth.GetCoordI() + SimpleStreet<object>.MOVABLE_MEDIUM_EDGE_LIMIT / 2, simpleMovable.GetCoordI());
            Assert.AreEqual(streetDestinyAtSouth.GetCoordJ() + SimpleStreet<object>.MOVABLE_MEDIUM_EDGE_LIMIT / 2, simpleMovable.GetCoordJ());
        }

        [TestMethod]
        public void MovableSimpleMoveWestOriginDestinyTest()
        {
            IMovableMedium<object> streetOrigin = new SimpleStreet<object>("CalleOrigin", null, 0, 0);
            IMovableMedium<object> streetDestinyAtWest = new SimpleStreet<object>("CalleDest", null, -1, 0);
            streetOrigin.SetMovableMediumAtWest(streetDestinyAtWest);
            IMovable<object> simpleMovable = new SimpleMovable<object>("Simple", null, streetOrigin);
            int[] direction = new[] { -1, 0 };
            simpleMovable.SetDirectionI(direction[0]);
            simpleMovable.SetDirectionJ(direction[1]);
            simpleMovable.SetVelocity(1.0f);//It will complete the trip in one tick
            simpleMovable.TimeTick(1.0f);
            Assert.AreEqual(streetDestinyAtWest, simpleMovable.GetMedium());
            Assert.AreEqual(streetDestinyAtWest.GetCoordI() + SimpleStreet<object>.MOVABLE_MEDIUM_EDGE_LIMIT / 2, simpleMovable.GetCoordI());
            Assert.AreEqual(streetDestinyAtWest.GetCoordJ() + SimpleStreet<object>.MOVABLE_MEDIUM_EDGE_LIMIT / 2, simpleMovable.GetCoordJ());
        }

        [TestMethod]
        public void MovableSimpleMoveEastOriginDestinyTest()
        {
            IMovableMedium<object> streetOrigin = new SimpleStreet<object>("CalleOrigin", null, 0, 0);
            IMovableMedium<object> streetDestinyAtEast = new SimpleStreet<object>("CalleDest", null, +1, 0);
            streetOrigin.SetMovableMediumAtEast(streetDestinyAtEast);
            IMovable<object> simpleMovable = new SimpleMovable<object>("Simple", null, streetOrigin);
            int[] direction = new[] { +1, 0 };
            simpleMovable.SetDirectionI(direction[0]);
            simpleMovable.SetDirectionJ(direction[1]);
            simpleMovable.SetVelocity(1.0f);//It will complete the trip in one tick
            simpleMovable.TimeTick(1.0f);
            Assert.AreEqual(streetDestinyAtEast, simpleMovable.GetMedium());
            Assert.AreEqual(streetDestinyAtEast.GetCoordI() + SimpleStreet<object>.MOVABLE_MEDIUM_EDGE_LIMIT / 2, simpleMovable.GetCoordI());
            Assert.AreEqual(streetDestinyAtEast.GetCoordJ() + SimpleStreet<object>.MOVABLE_MEDIUM_EDGE_LIMIT / 2, simpleMovable.GetCoordJ());
        }

        [TestMethod]
        public void MovableSimpleCollisionAtNorthTest()
        {
            IMovableMedium<object> streetOrigin = new SimpleStreet<object>("CalleOrigin", null, 0, 0);
            IMovable<object> simpleMovable = new SimpleMovable<object>("Simple", null, streetOrigin);
            int[] direction = new[] { +0, 1 };
            simpleMovable.SetDirectionI(direction[0]);
            simpleMovable.SetDirectionJ(direction[1]);
            simpleMovable.SetVelocity(1.0f);//It will complete the trip in one tick
            simpleMovable.TimeTick(1.0f);
            Assert.AreEqual(SimpleStreet<object>.MOVABLE_MEDIUM_EDGE_LIMIT/2, simpleMovable.GetCoordI());
            Assert.AreEqual(SimpleStreet<object>.MOVABLE_MEDIUM_EDGE_LIMIT,simpleMovable.GetCoordJ());
        }

        [TestMethod]
        public void MovableSimpleSmallMoveNorthInsideMediumTest()
        {
            IMovableMedium<object> streetOrigin = new SimpleStreet<object>("CalleOrigin", null, 0, 0);
            IMovable<object> simpleMovable = new SimpleMovable<object>("Simple", null, streetOrigin);
            int[] direction = new[] { +0, 1 };
            simpleMovable.SetDirectionI(direction[0]);
            simpleMovable.SetDirectionJ(direction[1]);
            simpleMovable.SetVelocity(1.0f);//It will complete the trip in one tick
            simpleMovable.TimeTick(0.25f);
            Assert.AreEqual(SimpleStreet<object>.MOVABLE_MEDIUM_EDGE_LIMIT / 2, simpleMovable.GetCoordI());
            Assert.AreEqual(0.25f+SimpleStreet<object>.MOVABLE_MEDIUM_EDGE_LIMIT/2, simpleMovable.GetCoordJ());
        }
    }
}
