using SaavedraCraft.Model.CollisionEngine;
using SaavedraCraft.Model.Interface;
using SaavedraCraft.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuarentineSurvival.Model.Collision
{
    public class SoftCollision<T> : QuarentineCollision<T>
    {
        private IAction<T> action;

        public SoftCollision(List<ICollisionable<T>> bodies, float timeOfCollision, IAction<T> actionToExecute) : base(bodies, timeOfCollision)
        {
            action = actionToExecute;
        }

        public override IAction<T> GetActionOnBodyFromCollision(IMovable<T> body)
        {
            return action;
        }
    }

}
