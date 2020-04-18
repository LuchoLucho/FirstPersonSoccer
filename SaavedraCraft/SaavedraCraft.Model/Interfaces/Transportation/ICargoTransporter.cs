using System;
using System.Collections.Generic;
using System.Text;
using SaavedraCraft.Model.Transportation;

namespace SaavedraCraft.Model.Interfaces.Transportation
{
    public interface ICargoTransporter<T> : IMovable<T>
    {
        void LoadCargo(ICargo<T> singleCargo);
        ICargo<T> UnloadCargo(ICargo<T> cargoToRemove);
        List<ICargo<T>> showCargo();
        bool CanCargoBeLoaded(ICargo<T> currentCargo);
        void OnParkinSpaceAvailableFromWarehouse(Action<IWarehouse<T>> handlerOnParkinSpaceAvailableFromWarehouse);
        void NotifyParkingspaceAvailable(IWarehouse<T> simpleWareHouse);
        void OnTransportPartFromWarehouse(Action<IWarehouse<T>> handlerTransportPartFromWarehouse);
        void NotifyTransportPartFromWarehouse(IWarehouse<T> simpleWareHouse);
    }
}
