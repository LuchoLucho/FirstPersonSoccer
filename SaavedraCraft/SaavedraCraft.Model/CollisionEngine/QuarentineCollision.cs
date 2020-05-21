using SaavedraCraft.Model.Interface;
using SaavedraCraft.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SaavedraCraft.Model.CollisionEngine
{
    public abstract class QuarentineCollision<T>
    {
        private List<IMovable<T>> involveInCollision;
        private float timeOfCollision;

        public QuarentineCollision(List<IMovable<T>> bodies, float timeOfCollision)
        {
            involveInCollision = bodies;
            this.timeOfCollision = timeOfCollision;
        }

        public abstract IAction<T> GetActionOnBodyFromCollision(IMovable<T> body);

        public float GetTimeOfCollision()
        {
            return timeOfCollision;
        }

        public List<IMovable<T>> ShowInvolveMovablesInCollision()
        {
            return involveInCollision;
        }
    }

    public class HardCollision<T> : QuarentineCollision<T>
    {
        public HardCollision(List<IMovable<T>> bodies, float timeOfCollision) : base(bodies, timeOfCollision)
        {
        }

        public override IAction<T> GetActionOnBodyFromCollision(IMovable<T> body)
        {
            return new FullStopAction<T>();
        }

        public class FullStopAction<T2> : IAction<T2>
        {
            public bool canExecute(IActionExecutor<T2> executor, IHolder<T2> holder, IActionable<T2> impactedActionable)
            {
                return true;
            }

            public void execute(IActionExecutor<T2> executor, IHolder<T2> holder, IActionable<T2> impactedActionable)
            {
                ((ICollisionable<T2>)executor).SetVelocity(0);
            }

            public IActionable<T2> getSourceActionable()
            {
                throw new NotImplementedException();
            }
        }
    }
}
