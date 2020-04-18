﻿using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuarentineSurvival.Model;
using SaavedraCraft.Model;
using SaavedraCraft.Model.Interfaces;
using SaavedraCraft.Model.Interfaces.Transportation;
using SaavedraCraft.Model.Resources;
using SaavedraCraft.Model.Transportation;

namespace QuarentineSurvivalTest
{
    [TestClass]
    public class ResourceStorateAndTransportationTets
    {
        [TestMethod]
        public void ResourcePickupTest()
        {
            ITransporterAndWarehouseManager<object> transporterAndWarehouseManager = new TransporterAndWarehouseManager<object>();
            IWarehouse<object> warehouse = new SimpleWareHouse<object>("Warehouse1", null, 0, 0, transporterAndWarehouseManager);
            ICargoTransporter<object> player = new QurentinePlayerModel<object>("Player", null, warehouse, transporterAndWarehouseManager);
            ICargo<object> simpleCargo = new SimpleCargo<object>();
            IResource resource = new SimpleResource(1, "Encendedor", 0);
            IMovableMedium<object> destinyOfResources = null; // The resouce has no fixed destination
            simpleCargo.addResources(resource, destinyOfResources);
            warehouse.addCargo(simpleCargo);
            Assert.AreEqual(1, warehouse.ShowAllCargo().Count);
            warehouse.addCargoTransporter(player);
            warehouse.UnloadCargoToCurrentTransporters(simpleCargo);
            Assert.AreEqual(0, warehouse.ShowAllCargo().Count);
            Assert.AreEqual(simpleCargo, player.showCargo()[0]);
        }

        [TestMethod]
        public void OnMovableArriveToWarehouseGetCargoAvailableNotificationTest()
        {
            ITransporterAndWarehouseManager<object> transporterAndWarehouseManager = new TransporterAndWarehouseManager<object>();
            IMovableMedium<object> streetNextToWarehouse = new SimpleStreet<object>("StreetNextToWareHouse", null, 0, 1);
            IWarehouse<object> warehouse = new SimpleWareHouse<object>("Warehouse1", null, 0, 0, transporterAndWarehouseManager);
            warehouse.SetMovableMediumAtEast(streetNextToWarehouse);
            streetNextToWarehouse.SetMovableMediumAtWest(warehouse);            
            ICargoTransporter<object> player = new QurentinePlayerModel<object>("Player", null, streetNextToWarehouse, transporterAndWarehouseManager);
            player.SetDirectionI(-1);
            player.SetDirectionJ(0);
            player.SetVelocity(1);
            bool parkinSpaceNotificationReceived = false;
            Action<IWarehouse<object>> handlerOnParkinSpaceAvailableFromWarehouse = x => 
            {
                parkinSpaceNotificationReceived = true;
            };
            player.OnParkinSpaceAvailableFromWarehouse(handlerOnParkinSpaceAvailableFromWarehouse);
            Assert.AreEqual(0, warehouse.GetMovablesOnMedium().Count);
            player.TimeTick(1);
            Assert.AreEqual(1, warehouse.GetMovablesOnMedium().Count);
            Assert.IsTrue(parkinSpaceNotificationReceived);
        }

        [TestMethod]
        public void OnMovablePartFromWarehouseGetCargoAvailableNotificationTest()
        {
            ITransporterAndWarehouseManager<object> transporterAndWarehouseManager = new TransporterAndWarehouseManager<object>();
            IMovableMedium<object> streetNextToWarehouse = new SimpleStreet<object>("StreetNextToWareHouse", null, 0, 1);
            IWarehouse<object> warehouse = new SimpleWareHouse<object>("Warehouse1", null, 0, 0, transporterAndWarehouseManager);
            warehouse.SetMovableMediumAtEast(streetNextToWarehouse);
            streetNextToWarehouse.SetMovableMediumAtWest(warehouse);
            ICargoTransporter<object> player = new QurentinePlayerModel<object>("Player", null, warehouse, transporterAndWarehouseManager);
            player.SetDirectionI(+1);
            player.SetDirectionJ(0);
            player.SetVelocity(1);
            bool playerLeftWarehouseNotificationReceived = false;
            Action<IWarehouse<object>> handlerOnPlayerPartFromWarehouse = x =>
            {
                playerLeftWarehouseNotificationReceived = true;
            };
            player.OnTransportPartFromWarehouse(handlerOnPlayerPartFromWarehouse);
            Assert.AreEqual(1, warehouse.GetMovablesOnMedium().Count);
            player.TimeTick(1);
            Assert.AreEqual(0, warehouse.GetMovablesOnMedium().Count);
            Assert.IsTrue(playerLeftWarehouseNotificationReceived);
        }
    }
}
