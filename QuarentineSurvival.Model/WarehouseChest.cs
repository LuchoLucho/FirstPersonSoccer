﻿using QuarentineSurvival.Model.Interface;
using SaavedraCraft.Model.Interface;
using SaavedraCraft.Model.Interfaces;
using SaavedraCraft.Model.Transportation;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuarentineSurvival.Model
{
    public class WarehouseChest<T> : SimpleWareHouse<T>, IMovableMediumCollisionAware<T>
    {
        public WarehouseChest(string aName, T aComponent, float newI, float newj, ITransporterAndWarehouseManager<T> transporterAndWarehouseManager) : base(aName, aComponent, newI, newj, transporterAndWarehouseManager)
        {
        }

        public void addActionable(IActionable<T> actionableToAdd)
        {
            throw new NotImplementedException();
        }

        public SaavedraCraft.Model.CollisionEngine.QuarentineCollision<T> GetCollisionTime(ICollisionable<T> other)
        {
            return ActionCollisionableMediumAware<T>.GetCollisionTimeToBeReUsed(this, other);
        }

        //I copied it from CollisanbleMediumAware since there is no other way to invoke the BASE class!
        public override void OnMovableMoving(IMovable<T> simpleMovable, float timedelta)
        {
            ICollisionable<T> simpleMovableAsCollisionable = simpleMovable as ICollisionable<T>;
            if (simpleMovableAsCollisionable != null)
            {
                SaavedraCraft.Model.CollisionEngine.QuarentineCollision<T> nextCollision = GetCollisionTime(simpleMovableAsCollisionable);
                //Log("ActionCollisionableMediumAware.NextCollisionTime = " + nextCollisionTime);
                if ((nextCollision.GetTimeOfCollision() > ActionCollisionableMediumAware<T>.EPSILON) && (nextCollision.GetTimeOfCollision() <= timedelta))
                {
                    //I need to reduce a little bit the collision time since otherwise it will be RIGHT next to the CAGE
                    float collisionTime = nextCollision.GetTimeOfCollision() - (ActionCollisionableMediumAware<T>.EPSILON * 2);
                    //----
                    base.OnMovableMoving(simpleMovable, collisionTime);//ProcessNormally with a delta = to the time of collission
                    timedelta -= collisionTime;
                    float movableDeltaI;
                    float movableDeltaJ;
                    movableDeltaI = simpleMovable.GetDeltaI();//simpleMovable.GetCoordI() - (this.GetCoordI() + MOVABLE_MEDIUM_EDGE_LIMIT / 2);
                    movableDeltaJ = simpleMovable.GetDeltaJ();//simpleMovable.GetCoordJ() - (this.GetCoordJ() + MOVABLE_MEDIUM_EDGE_LIMIT / 2);
                    simpleMovable.OnColissionAt(movableDeltaI, movableDeltaJ, nextCollision);
                    return;
                }
            }
            base.OnMovableMoving(simpleMovable, timedelta);
        }

        public void NotifyExecutorInEnvironmentRefreshActions()
        {
            throw new NotImplementedException();
        }

        public void NotifyRefreshActions(IHolder<T> receiber)
        {
            throw new NotImplementedException();
        }

        public void OnActionExecutorArrived(IActionExecutor<T> arrivingExecutor)
        {
            throw new NotImplementedException();
        }

        public void OnActionExecutorLeave(IActionExecutor<T> leavingExecutor)
        {
            throw new NotImplementedException();
        }

        public void removeActionable(IActionable<T> actionableToRemove)
        {
            throw new NotImplementedException();
        }

        public List<IActionable<T>> ShowAllActionables()
        {
            throw new NotImplementedException();
        }
    }
}
