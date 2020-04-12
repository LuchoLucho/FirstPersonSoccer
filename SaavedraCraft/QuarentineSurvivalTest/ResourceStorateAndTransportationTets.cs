using System;
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
            IWarehouse<object> warehouse = new SimpleWareHouse<object>("Warehouse1", null, 0, 0);
            ICargoTransporter<object> player = new SinglePlayerModel<object>("Player", null, warehouse);
            ICargo<object> simpleCargo = new SimpleCargo();
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
       
    }
}
