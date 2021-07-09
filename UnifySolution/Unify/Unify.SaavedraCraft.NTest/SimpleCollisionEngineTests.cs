using System;
using NUnit.Framework;
using QuarentineSurvival.Model;
using QuarentineSurvival.Model.Interface;
using SaavedraCraft.Model.Interface;
using SaavedraCraft.Model.Interfaces;
using SaavedraCraft.Model.Transportation;

namespace Unify.SaavedraCraft.NTest
{
    [TestFixture()]
    public class SimpleCollisionEngineTests
    {
        [Test()]
        public void MovableHitsAgainsActionableDoorAtNorthTest()
        {
            ITransporterAndWarehouseManager<object> transporterAndWarehouseManager = new TransporterAndWarehouseManager<object>();
            IMovableMediumCollisionAware<object> pisoOrigen = new ActionCollisionableMediumAware<object>("ActionStreet", null, 0, 0);
            IMovableMediumCollisionAware<object> pisoDestinoAlNorteConPuerta = new ActionCollisionableMediumAware<object>("NorthActionStreet", null, 0, 1);
            pisoOrigen.SetMovableMediumAtNorth(pisoDestinoAlNorteConPuerta);
            ICollisionable<object> movable = new QurentinePlayerModel<object>("ToColideAgainsDoor", null, pisoOrigen, transporterAndWarehouseManager);
            ICollisionable<object> puerta = new SimpleTransporterCollisionable<object>("Obstaculo", null, pisoDestinoAlNorteConPuerta, transporterAndWarehouseManager);
            int[] directionNorth = new[] { 0, 1 };
            movable.SetDirectionI(directionNorth[0]);
            movable.SetDirectionJ(directionNorth[1]);
            movable.SetVelocity(10.0f);//It will complete the trip in one tick
            movable.TimeTick(1.0f);
            Assert.AreEqual(pisoDestinoAlNorteConPuerta, movable.GetMedium());
            Assert.AreEqual(0, movable.GetVelocity());
            Assert.IsTrue(movable.GetCoordJ() < puerta.GetCoordJ());
        }

        [Test()]
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

        [Test()]
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

        [Test()]
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

        [Test()]
        public void MovableHitsAgainsActionableDoorAtNorthAndItOpensItTest()
        {
            ITransporterAndWarehouseManager<object> transporterAndWarehouseManager = new TransporterAndWarehouseManager<object>();
            IMovableMediumCollisionAware<object> pisoOrigen = new ActionCollisionableMediumAware<object>("ActionStreet", null, 0, 0);
            IMovableMediumCollisionAware<object> pisoDestinoAlNorteConPuerta = new ActionCollisionableMediumAware<object>("NorthActionStreet", null, 0, 1);
            pisoOrigen.SetMovableMediumAtNorth(pisoDestinoAlNorteConPuerta);
            QurentinePlayerModel<object> player = new QurentinePlayerModel<object>("ToColideAgainsDoor", null, pisoOrigen, transporterAndWarehouseManager);
            SimpleDoor<object> puerta = new SimpleDoor<object>("Puerta", null, pisoDestinoAlNorteConPuerta, transporterAndWarehouseManager);
            pisoDestinoAlNorteConPuerta.addActionable(puerta);
            int[] directionNorth = new[] { 0, 1 };
            player.SetDirectionI(directionNorth[0]);
            player.SetDirectionJ(directionNorth[1]);
            player.SetVelocity(10.0f);//It will complete the trip in one tick
            player.TimeTick(1.0f);
            Assert.AreEqual(pisoDestinoAlNorteConPuerta, player.GetMedium());
            Assert.AreEqual(0, player.GetVelocity());
            Assert.IsTrue(player.GetCoordJ() < puerta.GetCoordJ());
            //Open Zezame!
            IAction<object> uniqueAction = player.ShowAvailableActions()[0];
            uniqueAction.execute(player, pisoDestinoAlNorteConPuerta, puerta);
            player.SetVelocity(10.0f);
            player.TimeTick(1.0f);
            Assert.IsTrue(player.GetCoordJ() > puerta.GetCoordJ());
        }

        [Test()]
        public void MovableHitsAgainsMultipleObjectsInTheSameMediumTest()
        {
            ITransporterAndWarehouseManager<object> transporterAndWarehouseManager = new TransporterAndWarehouseManager<object>();
            IMovableMediumCollisionAware<object> piso = new ActionCollisionableMediumAware<object>("ActionStreet", null, 0, 0);
            ICollisionable<object> player = new QurentinePlayerModel<object>("ToCollide", null, piso, transporterAndWarehouseManager);
            player.SetNewIJ(0.5f, -0.4f);
            ICollisionable<object> obstaculo1 = new SimpleTransporterCollisionable<object>("Obstaculo1", null, piso, transporterAndWarehouseManager);
            obstaculo1.SetNewIJ(0.5f, +0.3f);
            ICollisionable<object> obstaculo2 = new SimpleTransporterCollisionable<object>("Obstaculo2", null, piso, transporterAndWarehouseManager);
            obstaculo2.SetNewIJ(0.9f, +0.15f);
            int[] directionNorth = new[] { 0, 1 };
            player.SetDirectionI(directionNorth[0]);
            player.SetDirectionJ(directionNorth[1]);
            player.SetVelocity(10.0f);//It will complete the trip in one tick
            player.TimeTick(1.0f);
            Assert.AreEqual(0, player.GetVelocity());
            Assert.IsTrue(player.GetCoordJ() < obstaculo1.GetCoordJ());
            int[] directionEast = new[] { 1, 0 };
            player.SetDirectionI(directionEast[0]);
            player.SetDirectionJ(directionEast[1]);
            player.SetVelocity(10.0f);//It will complete the trip in one tick
            player.TimeTick(1.0f);
            Assert.AreEqual(0, player.GetVelocity());
            Assert.IsTrue(player.GetCoordI() + player.GetWidh() / 2 < obstaculo2.GetCoordI() - obstaculo2.GetWidh() / 2);
        }
    }
}
