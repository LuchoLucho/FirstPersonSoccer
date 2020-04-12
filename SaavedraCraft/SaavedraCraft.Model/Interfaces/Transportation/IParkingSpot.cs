using System;
using System.Collections.Generic;
using System.Text;

namespace SaavedraCraft.Model.Interfaces.Transportation
{
    public interface IParkingSpot<T>
    {
        void parkMovable(IMovable<T> newMovableToPark);
        void unparkMovable(IMovable<T> newMovableToPark);
    }
}
