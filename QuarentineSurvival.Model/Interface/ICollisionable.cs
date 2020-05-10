using QuarentineSurvival.Model.CollisionEngine;
using SaavedraCraft.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuarentineSurvival.Model.Interface
{
    public interface ICollisionable<T> : IMovable<T>
    {
        Edge<T>[] ShowEdges();
        Vertex2d<T>[] ShowCorners();
        float GetCollisionTime(ICollisionable<T> other);        
    }
}
