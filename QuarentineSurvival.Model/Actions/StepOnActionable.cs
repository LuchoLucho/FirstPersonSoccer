﻿using QuarentineSurvival.Model.Collision;
using SaavedraCraft.Model.CollisionEngine;
using SaavedraCraft.Model.Interface;
using SaavedraCraft.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuarentineSurvival.Model.Actions
{
    public class StepOnActionable<T> : SimpleTransporterCollisionable<T>, IActionable<T>
    {
        private AutoAction<T> action;

        private bool wasAlreadyTriggered;
        public bool WasAlreadyTriggered
        {
            get
            {
                Log("StepOnActionable.WasAlreadTriggered GET =" + wasAlreadyTriggered);
                return wasAlreadyTriggered;
            }
            set
            {                
                wasAlreadyTriggered = value;
                Log("StepOnActionable.WasAlreadTriggered SET =" + wasAlreadyTriggered);
            }
        }

        public StepOnActionable(string aName, T aComponent, IMovableMedium<T> originMedium, ITransporterAndWarehouseManager<T> transporterAndWarehouseManager) : base(aName, aComponent, originMedium, transporterAndWarehouseManager)
        {
        }

        public void SetAutoAction(AutoAction<T> newAutoAction)
        {
            action = newAutoAction;
        }

        public AutoAction<T> GetAutoAction()
        {
            return action;
        }

        public void NotifyRefreshActions(IHolder<T> receiber)
        {
            throw new NotImplementedException();
        }

        public List<IAction<T>> ShowAvailableActions()
        {
            return new List<IAction<T>> { action };
        }

        public override QuarentineCollision<T> GetCollision(ICollisionable<T> other)
        {
            if (WasAlreadyTriggered)
            {
                QuarentineCollision<T> nullCollision = new HardCollision<T>(new List<ICollisionable<T>> { this, other }, float.MaxValue);
                return nullCollision;
            }
            QuarentineCollision<T> oldCollision = base.GetCollision(other);
            return new SoftCollision<T>(new List<ICollisionable<T>> { this, other }, oldCollision.GetTimeOfCollision(), action);
        }
    }

}
