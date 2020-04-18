using SaavedraCraft.Model.Interfaces;
using SaavedraCraft.Model.Interfaces.Transportation;
using System;
using System.Collections.Generic;
using System.Text;

namespace SaavedraCraft.Model.Transportation
{
    public class TransporterAndWarehouseManager<T> : ITransporterAndWarehouseManager<T>
    {
        private List<ICargoTransporter<T>> cargoTransporters;
        private List<IWarehouse<T>> warehouses;

        public TransporterAndWarehouseManager()
        {
            cargoTransporters = new List<ICargoTransporter<T>>();
            warehouses = new List<IWarehouse<T>>();
        }

        public void NotifyMovableArrivedToWarehouse(SimpleWareHouse<T> simpleWareHouse, IMovable<T> newMovable)
        {
            ICargoTransporter<T> cargoTransporterFromMovable = newMovable as ICargoTransporter<T>;
            if (cargoTransporterFromMovable == null)
            {
                //What ever arrived is not a transporter!!!
                return;
            }
            cargoTransporterFromMovable.NotifyParkingspaceAvailable(simpleWareHouse);
        }

        public void NotifyMovablePartFromWarehouse(SimpleWareHouse<T> simpleWareHouse, IMovable<T> newMovable)
        {
            ICargoTransporter<T> cargoTransporterFromMovable = newMovable as ICargoTransporter<T>;
            if (cargoTransporterFromMovable == null)
            {
                //What ever arrived is not a transporter!!!
                return;
            }
            cargoTransporterFromMovable.NotifyTransportPartFromWarehouse(simpleWareHouse);
        }

        public void SubscribeAsCargoTransporter(ICargoTransporter<T> cargoTransporter)
        {
            cargoTransporters.Add(cargoTransporter);
        }

        public void SubscribeAsWarehouse(IWarehouse<T> warehouse)
        {
            warehouses.Add(warehouse);
        }
    }
}
