using System;
using NUnit.Framework;
using SaavedraCraft.Model.Interfaces;
using SaavedraCraft.Model.Interfaces.Transportation;
using SaavedraCraft.Model.Resources;
using SaavedraCraft.Model.Transportation;

namespace Unify.SaavedraCraft.NTest
{
    [TestFixture()]
    public class TransportationTests
    {
        [Test()]
        public void TransportationPickUpCargoTest()
        {
            ITransporterAndWarehouseManager<object> transporterAndWarehouseManager = new TransporterAndWarehouseManager<object>();
            IWarehouse<object> warehouse = new SimpleWareHouse<object>("Warehouse1", null, 0, 0, transporterAndWarehouseManager);
            ICargoTransporter<object> simpleTransporter = new SimpleTransporter<object>("MovableTransport", null, warehouse, transporterAndWarehouseManager);
            ICargo<object> simpleCargo = new SimpleCargo<object>();
            IResource resource = new SimpleResource(1, "Tomates", 0);
            IMovableMedium<object> destinyOfResources = new SimpleStreet<object>("Destination", null, 0, 0);
            simpleCargo.addResources(resource, destinyOfResources);
            warehouse.addCargo(simpleCargo);
            warehouse.addCargoTransporter(simpleTransporter);
            Assert.AreEqual(0, simpleTransporter.showCargo().Count);
            Assert.AreEqual(1, warehouse.ShowAllCargo().Count);
            warehouse.LoadUnloadAllAvailableCargo();
            Assert.IsTrue(simpleTransporter.showCargo() != null);
            Assert.AreEqual(0, warehouse.ShowAllCargo().Count);
        }

        [Test()]
        public void TransportGetsToWarehouseIsAddedAsAvailableTransporterTest()
        {
            ITransporterAndWarehouseManager<object> transporterAndWarehouseManager = new TransporterAndWarehouseManager<object>();
            IMovableMedium<object> streetOrigin = new SimpleStreet<object>("CalleOrigin", null, 0, -1);
            IWarehouse<object> warehouse = new SimpleWareHouse<object>("Warehouse1", null, 0, 0, transporterAndWarehouseManager);
            streetOrigin.SetMovableMediumAtNorth(warehouse);
            ICargoTransporter<object> simpleTransporter = new SimpleTransporter<object>("Transporter", null, streetOrigin, transporterAndWarehouseManager);
            //----
            simpleTransporter.OnParkinSpaceAvailableFromWarehouse(x => x.addCargoTransporter(simpleTransporter)); // PARK <<<<<<
            //----
            int[] northDirection = new[] { 0, 1 };
            simpleTransporter.SetDirectionI(northDirection[0]);
            simpleTransporter.SetDirectionJ(northDirection[1]);
            simpleTransporter.SetVelocity(1.0f);//It will complete the trip in one tick
            Assert.AreEqual(0, warehouse.ShowAllTransporter().Count);
            simpleTransporter.TimeTick(1.0f);
            Assert.AreEqual(1, warehouse.ShowAllTransporter().Count);
        }

        [Test()]
        public void TransportLeavesWarehouseAndIsRemovedAsAvailableTransporterTest()
        {
            ITransporterAndWarehouseManager<object> transporterAndWarehouseManager = new TransporterAndWarehouseManager<object>();
            IMovableMedium<object> streetDestiny = new SimpleStreet<object>("CalleOrigin", null, 0, -1);
            IWarehouse<object> warehouse = new SimpleWareHouse<object>("Warehouse1", null, 0, 0, transporterAndWarehouseManager);
            warehouse.SetMovableMediumAtSouth(streetDestiny);
            ICargoTransporter<object> simpleTransporter = new SimpleTransporter<object>("Transporter", null, warehouse, transporterAndWarehouseManager);
            warehouse.addCargoTransporter(simpleTransporter);// emulate PARK <<<<<<
            //----
            simpleTransporter.OnTransportPartFromWarehouse(x => x.removeCargoTransporter(simpleTransporter)); // UNPARK <<<<<<
            //----
            int[] southDirection = new[] { 0, -1 };
            simpleTransporter.SetDirectionI(southDirection[0]);
            simpleTransporter.SetDirectionJ(southDirection[1]);
            simpleTransporter.SetVelocity(1.0f);//It will complete the trip in one tick
            Assert.AreEqual(1, warehouse.ShowAllTransporter().Count);
            simpleTransporter.TimeTick(1.0f);
            Assert.AreEqual(0, warehouse.ShowAllTransporter().Count);
        }

    }
}
