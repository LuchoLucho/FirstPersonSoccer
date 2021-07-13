using QuarentineSurvival.Model.Interface;
using SaavedraCraft.Model.CollisionEngine;
using SaavedraCraft.Model.Interface;
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

        public static List<ICollisionable<T>> ShowAllCollisionables(IMovableMediumCollisionAware<T> medium)
        {
            List<ICollisionable<T>> ret = new List<ICollisionable<T>>();
            foreach (IMovable<T> currentMovable in medium.GetMovablesOnMedium())
            {
                ICollisionable<T> collisionableCandidate = currentMovable as ICollisionable<T>;
                ret.Add(collisionableCandidate);
            }
            return ret;
        }

        public static QuarentineCollision<T> GetCollisionTimeToBeReUsed(IMovableMediumCollisionAware<T> thisMedium, ICollisionable<T> other)
        {
            List<ICollisionable<T>> otherToCollideWith = ShowAllCollisionables(thisMedium);
            QuarentineCollision<T> nullCollision = new HardCollision<T>(new List<ICollisionable<T>> { null, other }, float.MaxValue);
            QuarentineCollision<T> nextCollision = nullCollision;
            foreach (ICollisionable<T> currentCollisionable in otherToCollideWith)
            {
                if (currentCollisionable != other)
                {
                    QuarentineCollision<T> currentPossibleCollision = currentCollisionable.GetCollision(other);
                    if (nextCollision.GetTimeOfCollision() > currentPossibleCollision.GetTimeOfCollision())
                    {
                        nextCollision = currentPossibleCollision;
                    }
                }
            }
            return nextCollision;
        }        

        public QuarentineCollision<T> GetCollisionTime(ICollisionable<T> other)
        {
            return GetCollisionTimeToBeReUsed(this,other);
        }

        public override void OnMovableMoving(IMovable<T> simpleMovable, float timedelta)
        {
            ICollisionable<T> simpleMovableAsCollisionable = simpleMovable as ICollisionable<T>;
            if (simpleMovableAsCollisionable != null)
            {
                QuarentineCollision<T> nextCollision = GetCollisionTime(simpleMovableAsCollisionable);
                //Log("ActionCollisionableMediumAware.NextCollisionTime = " + nextCollisionTime);
                if ((nextCollision.GetTimeOfCollision() > EPSILON) && (nextCollision.GetTimeOfCollision() <= timedelta))
                {
                    //I need to reduce a little bit the collision time since otherwise it will be RIGHT next to the CAGE
                    float collisionTime = nextCollision.GetTimeOfCollision()-(EPSILON * 2);
                    //----
                    base.OnMovableMoving(simpleMovable, collisionTime);//ProcessNormally with a delta = to the time of collission
                    timedelta -= collisionTime;
                    float movableDeltaI;
                    float movableDeltaJ;
                    movableDeltaI = simpleMovable.GetDeltaI();//simpleMovable.GetCoordI() - (this.GetCoordI() + MOVABLE_MEDIUM_EDGE_LIMIT / 2);
                    movableDeltaJ = simpleMovable.GetDeltaJ();//simpleMovable.GetCoordJ() - (this.GetCoordJ() + MOVABLE_MEDIUM_EDGE_LIMIT / 2);
                    simpleMovable.OnColissionAt(movableDeltaI, movableDeltaJ,nextCollision);
                    return;
                }
            }
            base.OnMovableMoving(simpleMovable, timedelta);
        }
    }
}
