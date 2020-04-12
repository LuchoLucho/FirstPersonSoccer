using System;
using System.Collections.Generic;
using System.Text;

namespace SaavedraCraft.Model.Interfaces.Transportation
{
    public interface IWarehouse<T> : IMovableMedium<T>, IParkingSpot<T>
    {
        void addCargo(ICargo<T> newCargo);
        void removeCargo(ICargo<T> toRemove);

        void addCargoTransporter(ICargoTransporter<T> newCargoTransporter);
        void removeCargoTransporter(ICargoTransporter<T> newCargoTransporter);

        void LoadUnload();
        List<ICargo<T>> GetAllCargo();
    }
}
