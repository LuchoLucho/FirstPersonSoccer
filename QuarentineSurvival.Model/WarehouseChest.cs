using QuarentineSurvival.Model.Interface;
using SaavedraCraft.Model.Interfaces;
using SaavedraCraft.Model.Transportation;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuarentineSurvival.Model
{
    public class WarehouseChest<T> : SimpleWareHouse<T>, IMovableMediumCollisionAware<T>
    {
        private IMovableMediumCollisionAware<T> movableMediumCollisionAware;

        public WarehouseChest(string aName, T aComponent, float newI, float newj, ITransporterAndWarehouseManager<T> transporterAndWarehouseManager) : base(aName, aComponent, newI, newj, transporterAndWarehouseManager)
        {
            movableMediumCollisionAware = new ActionCollisionableMediumAware<T>(aName, aComponent, newI, newj);
        }

        public void addActionable(IActionable<T> actionableToAdd)
        {
            movableMediumCollisionAware.addActionable(actionableToAdd);
        }

        public float GetCollisionTime(ICollisionable<T> other)
        {
            return movableMediumCollisionAware.GetCollisionTime(other);
        }

        public void NotifyExecutorInEnvironmentRefreshActions()
        {
            movableMediumCollisionAware.NotifyExecutorInEnvironmentRefreshActions();
        }

        public void NotifyRefreshActions(IHolder<T> receiber)
        {
            movableMediumCollisionAware.NotifyRefreshActions(receiber);
        }

        public void OnActionExecutorArrived(IActionExecutor<T> arrivingExecutor)
        {
            movableMediumCollisionAware.OnActionExecutorArrived(arrivingExecutor);
        }

        public void OnActionExecutorLeave(IActionExecutor<T> leavingExecutor)
        {
            movableMediumCollisionAware.OnActionExecutorLeave(leavingExecutor);
        }

        public void removeActionable(IActionable<T> actionableToRemove)
        {
            movableMediumCollisionAware.removeActionable(actionableToRemove);
        }

        public List<IActionable<T>> ShowAllActionables()
        {
            return movableMediumCollisionAware.ShowAllActionables();
        }
    }
}
