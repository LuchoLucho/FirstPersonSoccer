using SaavedraCraft.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuarentineSurvival.Model.Interface
{
    public interface IMovableMediumCollisionAware<T> : IEnvironment<T>
    {
        float GetCollisionTime(ICollisionable<T> other);
    }
}
