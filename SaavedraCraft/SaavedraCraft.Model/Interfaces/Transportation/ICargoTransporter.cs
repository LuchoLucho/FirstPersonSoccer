using System;
using System.Collections.Generic;
using System.Text;

namespace SaavedraCraft.Model.Interfaces.Transportation
{
    public interface ICargoTransporter<T>
    {
        void LoadCargo(ICargo<T> singleCargo);
        ICargo<T> UnloadCargo(ICargo<T> cargoToRemove);
        List<ICargo<T>> showCargo();
        bool CanCargoBeLoaded(ICargo<T> currentCargo);
    }
}
