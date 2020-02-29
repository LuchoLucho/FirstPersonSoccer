using System;
using System.Collections.Generic;
using System.Text;

namespace SaavedraCraft.Model.Interfaces
{
    public interface IMovable<T> : IObject<T>
    {
        int GetDirectionI();
        int GetDirectionJ();
        float GetVelocity();

        IMovableMedium<T> GetMedium();
        void SetMedium(IMovableMedium<T> newMedium);
        void SetDirectionI(int newDirectionI);
        void SetDirectionJ(int newDirectionI);
        void SetVelocity(float newVelocity);
    }

    public interface IMovableMedium<T> : IObject<T> //Una calla, el mar, el aire...
    {
        float MaxSpeed();
        IMovableMedium<T> GetMovableMediumAtNorth();
        IMovableMedium<T> GetMovableMediumAtSouth();
        IMovableMedium<T> GetMovableMediumAtWest();
        IMovableMedium<T> GetMovableMediumAtEast();
        void SetMovableMediumAtNorth(IMovableMedium<T> streetDestiny);
    }
}
