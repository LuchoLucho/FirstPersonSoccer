using System;
using System.Collections.Generic;
using System.Text;
using SaavedraCraft.Model.Interfaces.Transportation;
using SaavedraCraft.Model.Transportation;

namespace SaavedraCraft.Model.Interfaces
{
    public interface ITransporterAndWarehouseManager<T>
    {
        void SubscribeAsCargoTransporter(Transportation.ICargoTransporter<T> cargoTransporter);
        void SubscribeAsWarehouse(IWarehouse<T> warehouse);
        void NotifyMovableArrivedToWarehouse(SimpleWareHouse<T> simpleWareHouse, IMovable<T> newMovable);
    }
}
