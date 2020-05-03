using QuarentineSurvival.Model.Interface;
using SaavedraCraft.Model.Interfaces;
using SaavedraCraft.Model.Transportation;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuarentineSurvival.Model
{
    public class ActionStreet<T> : SimpleStreet<T>, IEnvironment<T>
    {
        private List<IActionable<T>> allActionables;
        private IActionExecutor<T> currentActionExector = null;

        public ActionStreet(string aName, T aComponent, float newI, float newj) : base(aName, aComponent, newI, newj)
        {
            allActionables = new List<IActionable<T>>();
        }

        public void addActionable(IActionable<T> actionableToAdd)
        {
            allActionables.Add(actionableToAdd);
        }

        public void NotifyExecutorInEnvironmentRefreshActions()
        {
            throw new NotImplementedException();
        }

        public void NotifyRefreshActions(IHolder<T> receiber)
        {
            //throw new NotImplementedException(); TODO!
        }

        public void OnActionExecutorArrived(IActionExecutor<T> arrivingExecutor)
        {
            currentActionExector = arrivingExecutor;
            allActionables.ForEach(x => currentActionExector.addActionable(x));
        }

        public void OnActionExecutorLeave(IActionExecutor<T> leavingExecutor)
        {
            allActionables.ForEach(x => currentActionExector.removeActionable(x));
            currentActionExector = null;
        }

        public void removeActionable(IActionable<T> actionableToRemove)
        {
            throw new NotImplementedException();
        }

        public List<IActionable<T>> ShowAllActionables()
        {
            throw new NotImplementedException();
        }

        public override void MovableArrived(IMovable<T> newMovable)
        {
            base.MovableArrived(newMovable);
            IActionExecutor<T> movableArrivedAsExecutor = newMovable as IActionExecutor<T>;
            if (movableArrivedAsExecutor != null)
            {
                OnActionExecutorArrived(movableArrivedAsExecutor);
            }
        }

        public override void MovableLeft(IMovable<T> toRemoveMovable)
        {
            base.MovableLeft(toRemoveMovable);
            IActionExecutor<T> movableLeavingAsExecutor = toRemoveMovable as IActionExecutor<T>;
            if (movableLeavingAsExecutor != null)
            {
                OnActionExecutorLeave(movableLeavingAsExecutor);
            }
        }
    }
}
