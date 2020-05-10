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
            ITransporterAndWarehouseManager<object> transporterAndWarehouseManager = new TransporterAndWarehouseManager<object>();
            IMovableMediumCollisionAware<object> pisoOrigen = new ActionCollisionableMediumAware<object>("ActionStreet", null, 0, 0);
            IMovableMediumCollisionAware<object> pisoDestinoAlNorteConPuerta = new ActionCollisionableMediumAware<object>("NorthActionStreet", null, 0, 1);
            pisoOrigen.SetMovableMediumAtNorth(pisoDestinoAlNorteConPuerta);
            ICollisionable<object> movable = new QurentinePlayerModel<object>("ToColideAgainsDoor",null,pisoOrigen, transporterAndWarehouseManager);
            ICollisionable<object> puerta = new SimpleTransporterCollisionable<object>("Obstaculo", null, pisoDestinoAlNorteConPuerta, transporterAndWarehouseManager);
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
            ITransporterAndWarehouseManager<object> transporterAndWarehouseManager = new TransporterAndWarehouseManager<object>();
            IMovableMediumCollisionAware<object> pisoOrigen = new ActionCollisionableMediumAware<object>("ActionStreet", null, 0, 0);
            IMovableMediumCollisionAware<object> pisoDestinoAlSouthConPuerta = new ActionCollisionableMediumAware<object>("SouthActionStreet", null, 0, -1);
            pisoOrigen.SetMovableMediumAtSouth(pisoDestinoAlSouthConPuerta);
            ICollisionable<object> movable = new SimpleTransporterCollisionable<object>("ToColideAgainsDoor", null, pisoOrigen, transporterAndWarehouseManager);
            ICollisionable<object> puerta = new SimpleTransporterCollisionable<object>("Obstaculo", null, pisoDestinoAlSouthConPuerta, transporterAndWarehouseManager);
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
            ITransporterAndWarehouseManager<object> transporterAndWarehouseManager = new TransporterAndWarehouseManager<object>();
            IMovableMediumCollisionAware<object> pisoOrigen = new ActionCollisionableMediumAware<object>("ActionStreet", null, 0, 0);
            IMovableMediumCollisionAware<object> pisoDestinoAlWestConPuerta = new ActionCollisionableMediumAware<object>("WestActionStreet", null, -1, 0);
            pisoOrigen.SetMovableMediumAtWest(pisoDestinoAlWestConPuerta);
            ICollisionable<object> movable = new SimpleTransporterCollisionable<object>("ToColideAgainsDoor", null, pisoOrigen, transporterAndWarehouseManager);
            ICollisionable<object> puerta = new SimpleTransporterCollisionable<object>("Obstaculo", null, pisoDestinoAlWestConPuerta, transporterAndWarehouseManager);
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
            ITransporterAndWarehouseManager<object> transporterAndWarehouseManager = new TransporterAndWarehouseManager<object>();
            IMovableMediumCollisionAware<object> pisoOrigen = new ActionCollisionableMediumAware<object>("ActionStreet", null, 0, 0);
            IMovableMediumCollisionAware<object> pisoDestinoAlEastConPuerta = new ActionCollisionableMediumAware<object>("EastActionStreet", null, -1, 0);
            pisoOrigen.SetMovableMediumAtEast(pisoDestinoAlEastConPuerta);
            ICollisionable<object> movable = new SimpleTransporterCollisionable<object>("ToColideAgainsDoor", null, pisoOrigen, transporterAndWarehouseManager);
            ICollisionable<object> puerta = new SimpleTransporterCollisionable<object>("Obstaculo", null, pisoDestinoAlEastConPuerta, transporterAndWarehouseManager);
            int[] directionWest = new[] { +1, 0 };
            movable.SetDirectionI(directionWest[0]);
            movable.SetDirectionJ(directionWest[1]);
            movable.SetVelocity(10.0f);//It will complete the trip in one tick
            movable.TimeTick(1.0f);
            Assert.AreEqual(pisoDestinoAlEastConPuerta, movable.GetMedium());
            Assert.AreEqual(0, movable.GetVelocity());
            Assert.IsTrue(movable.GetCoordI() < puerta.GetCoordI());
        }
    }
}
