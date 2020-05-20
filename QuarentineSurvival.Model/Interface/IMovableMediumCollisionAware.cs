using SaavedraCraft.Model.Interface;
using SaavedraCraft.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuarentineSurvival.Model.Interface
{
    public interface IMovableMediumCollisionAware<T> : IEnvironment<T>
    {
        SaavedraCraft.Model.CollisionEngine.QuarentineCollision<T> GetCollisionTime(ICollisionable<T> other);
    }
}
