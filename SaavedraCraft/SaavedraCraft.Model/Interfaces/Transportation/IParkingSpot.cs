using System;
using System.Collections.Generic;
using System.Text;

namespace SaavedraCraft.Model.Interfaces.Transportation
{
    public interface IParkingSpotForMovable<T>
    {
        void parkTransporter(ICargoTransporter<T> newMovableToPark);
        void unparkTransporter(ICargoTransporter<T> newMovableToPark);
    }
}
