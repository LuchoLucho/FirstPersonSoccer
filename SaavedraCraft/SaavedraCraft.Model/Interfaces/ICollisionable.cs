using SaavedraCraft.Model.CollisionEngine;
using SaavedraCraft.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SaavedraCraft.Model.Interface
{
    public interface ICollisionable<T> : IMovable<T>
    {
        Edge<T>[] ShowEdges();
        Vertex2d<T>[] ShowCorners();
        QuarentineCollision<T> GetCollision(ICollisionable<T> other);
        QuarentineCollision<T> GetCustomCollision(List<ICollisionable<T>> lists, float timeOfCollision);
    }
}
