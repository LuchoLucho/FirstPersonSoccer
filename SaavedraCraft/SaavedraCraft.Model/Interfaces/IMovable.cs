using System;
using System.Collections.Generic;
using System.Text;
using SaavedraCraft.Model.Transportation;

namespace SaavedraCraft.Model.Interfaces
{
    public interface IMovable<T> : IObject<T>
    {
        float GetDirectionI();
        float GetDirectionJ();
        float GetVelocity();

        IMovableMedium<T> GetMedium();
        void SetMedium(IMovableMedium<T> newMedium);
        void SetDirectionI(float newDirectionI);
        void SetDirectionJ(float newDirectionI);
        void SetVelocity(float newVelocity);
        void OnColissionAt(float movableDeltaI, float movableDeltaJ);
        void traslateNorth(float newDeltaJ, float remainingDeltaTime);
        void traslateSouth(float newDeltaJ, float remainingDeltaTime);
        void traslateEast(float v, float remainingDeltaTime);
        void traslateWest(float v, float remainingDeltaTime);
        void tralateInsideMediumI(float movableDeltaI);
        void tralateInsideMediumJ(float movableDeltaJ);
        float GetDeltaI();
        float GetDeltaJ();
    }

    public interface IMovableMedium<T> : IObject<T> //Una calla, el mar, el aire...
    {
        float MaxSpeed();
        IMovableMedium<T> GetMovableMediumAtNorth();
        IMovableMedium<T> GetMovableMediumAtSouth();
        IMovableMedium<T> GetMovableMediumAtWest();
        IMovableMedium<T> GetMovableMediumAtEast();
        void SetMovableMediumAtNorth(IMovableMedium<T> streetDestiny);
        void SetMovableMediumAtSouth(IMovableMedium<T> streetDestiny);
        void SetMovableMediumAtWest(IMovableMedium<T> streetDestinyAtWest);
        void SetMovableMediumAtEast(IMovableMedium<T> streetDestinyAtEast);
        void MovableArrived(IMovable<T> newMovable);
        void MovablePart(IMovable<T> newMovable);
        List<IMovable<T>> GetMovablesOnMedium();
        void OnMovableArrivedAlsoDo(Action<IMovableMedium<T>> onMovableArrivedAlsoCustomAction);
        void OnMovableLeftAlsoDo(Action<IMovableMedium<T>> onMovableLeftAlsoCustomAction);
        void OnMovableMoving(IMovable<T> simpleMovable, float timedelta);
    }
}
