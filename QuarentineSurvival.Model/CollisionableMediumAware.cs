using QuarentineSurvival.Model.Interface;
using SaavedraCraft.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuarentineSurvival.Model
{
    public class ActionCollisionableMediumAware<T> : ActionStreet<T>, IMovableMediumCollisionAware<T>
    {
        public const float EPSILON = 0.001f;
        public ActionCollisionableMediumAware(string aName, T aComponent, float newI, float newj) : base(aName, aComponent, newI, newj)
        {
        }

        private List<ICollisionable<T>> ShowAllCollisionables()
        {
            List<ICollisionable<T>> ret = new List<ICollisionable<T>>();
            foreach (IMovable<T> currentMovable in this.GetMovablesOnMedium())
            {
                ICollisionable<T> collisionableCandidate = currentMovable as ICollisionable<T>;
                ret.Add(collisionableCandidate);
            }
            return ret;
        }

        public float GetCollisionTime(ICollisionable<T> other)
        {
            List<ICollisionable<T>> otherToCollideWith = ShowAllCollisionables();
            float nextCollision = float.MaxValue;
            foreach (ICollisionable<T> currentCollisionable in otherToCollideWith)
            {
                if (currentCollisionable != other)
                {
                    float currentPossibleCollision = currentCollisionable.GetCollisionTime(other);
                    if (nextCollision > currentPossibleCollision)
                    {
                        nextCollision = currentPossibleCollision;
                    }
                }
            }
            return nextCollision;
        }

        public override void OnMovableMoving(IMovable<T> simpleMovable, float timedelta)
        {
            ICollisionable<T> simpleMovableAsCollisionable = simpleMovable as ICollisionable<T>;
            if (simpleMovableAsCollisionable != null)
            {
                float nextCollisionTime = GetCollisionTime(simpleMovableAsCollisionable);
                Log("ActionCollisionableMediumAware.NextCollisionTime = " + nextCollisionTime);
                if ((nextCollisionTime> EPSILON) && (nextCollisionTime <= timedelta))
                {                    
                    //I need to reduce a little bit the collision time since otherwise it will be RIGHT next to the CAGE
                    nextCollisionTime -= EPSILON*2;
                    //----
                    base.OnMovableMoving(simpleMovable, nextCollisionTime);//ProcessNormally with a delta = to the time of collission
                    timedelta -= nextCollisionTime;
                    float movableDeltaI;
                    float movableDeltaJ;
                    movableDeltaI = simpleMovable.GetDeltaI();//simpleMovable.GetCoordI() - (this.GetCoordI() + MOVABLE_MEDIUM_EDGE_LIMIT / 2);
                    movableDeltaJ = simpleMovable.GetDeltaJ();//simpleMovable.GetCoordJ() - (this.GetCoordJ() + MOVABLE_MEDIUM_EDGE_LIMIT / 2);
                    simpleMovable.OnColissionAt(movableDeltaI, movableDeltaJ);
                    return;
                }
            }
            base.OnMovableMoving(simpleMovable, timedelta);
        }
    }
}
