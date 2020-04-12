﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SaavedraCraft.Model.Interfaces;
using SaavedraCraft.Model.Interfaces.Transportation;
using SaavedraCraft.Model.Resources;
using SaavedraCraft.Model.Transportation;

namespace SaavedraCraft.Tests
{
    [TestClass]
    public class TransportationTests
    {
        [TestMethod]
        public void TransportationPickUpCargoTest()
        {
            IWarehouse<object> warehouse = new SimpleWareHouse("Warehouse1", null, 0, 0);
            ICargoTransporter<object> simpleTransporter = new SimpleTransporter("MovableTransport", null, warehouse);
            ICargo<object> simpleCargo = new SimpleCargo();
            IResource resource = new SimpleResource(1, "Tomates", 0);
            IMovableMedium<object> destinyOfResources = new SimpleStreet<object>("Destination", null, 0, 0);
            simpleCargo.addResources(resource, destinyOfResources);
            warehouse.addCargo(simpleCargo);
            warehouse.addCargoTransporter(simpleTransporter);
            Assert.IsTrue(simpleTransporter.showCargo() == null);
            Assert.AreEqual(1, warehouse.GetAllCargo().Count);
            warehouse.LoadUnload();
            Assert.IsTrue(simpleTransporter.showCargo() != null);
            Assert.AreEqual(0, warehouse.GetAllCargo().Count);
        }

        [TestMethod]
        public void LoadedTransportationWithCheckpoinsTest()
        {
            /*IMovableMedium<object> sourceOfResources = new SimpleStreet<object>("Source", null, 0, 0);
            ICargoTransporter<object> simpleTransporter = new SimpleTransporter("MovableTransport",null,sourceOfResources);
            ICargo<object> simpleCargo = new SimpleCargo();
            IResource resource = new SimpleResource(1, "Tomates", 0);
            IMovableMedium<object> destinyOfResources = new SimpleStreet<object>("Destination", null, 1, 0);
            Assert.Fail();*/
        }
    }
}
