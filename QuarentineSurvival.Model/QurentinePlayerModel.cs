using QuarentineSurvival.Model.Interface;
using SaavedraCraft.Model.CollisionEngine;
using SaavedraCraft.Model.Interface;
using SaavedraCraft.Model.Interfaces;
using SaavedraCraft.Model.Interfaces.Transportation;
using SaavedraCraft.Model.Transportation;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuarentineSurvival.Model
{
    public class QurentinePlayerModel<T> : SimpleTransporterCollisionable<T>, IActionExecutor<T>
    {
        private List<IActionable<T>> allActionables = new List<IActionable<T>>();

        public QurentinePlayerModel(string aName, T aComponent, IMovableMediumCollisionAware<T> originMedium, ITransporterAndWarehouseManager<T> transporterAndWarehouseManager) : base(aName, aComponent, originMedium, transporterAndWarehouseManager)
        {
            
        }

        public virtual void addActionable(IActionable<T> actionableToAdd)
        {
            allActionables.Add(actionableToAdd);
        }             

        public void NotifyArribeToEnvironment(IEnvironment<T> newEnvironment)
        {
            throw new NotImplementedException();
        }

        public void NotifyLeaveEnvironment(IEnvironment<T> newEnvironment)
        {
            throw new NotImplementedException();
        }

        public void NotifyRefreshActions(IHolder<T> receiber)
        {
            throw new NotImplementedException();
        }

        public void removeActionable(IActionable<T> actionableToRemove)
        {
            allActionables.Remove(actionableToRemove);
        }

        public List<IActionable<T>> ShowAllActionables()
        {
            return allActionables;
        }

        public List<IAction<T>> ShowAvailableActions()
        {
            List<IAction<T>> allActions = new List<IAction<T>>();
            ShowAllActionables().ForEach(x => allActions.AddRange(x.ShowAvailableActions()));
            return allActions;
        }
        
        public override void OnColissionAt(float movableDeltaI, float movableDeltaJ, QuarentineCollision<T> quarentineCollision)
        {
            //base.OnColissionAt(movableDeltaI, movableDeltaJ, quarentineCollision);
            this.SetDeltaI(movableDeltaI);
            this.SetDeltaJ(movableDeltaJ);
            quarentineCollision.GetActionOnBodyFromCollision(this).execute(this, null, null);
        }

        public override void LoadCargo(ICargo<T> singleCargo)
        {
            base.LoadCargo(singleCargo);
            IResource singleResource = singleCargo.ShowResource();
            IActionable<T> actionableResource = singleResource as IActionable<T>;
            if (actionableResource != null)
            {
                addActionable(actionableResource);
            }
        }

        public override ICargo<T> UnloadCargo(ICargo<T> cargoToRemove)
        {
            IResource singleResource = cargoToRemove.ShowResource();
            IActionable<T> actionableResource = singleResource as IActionable<T>;
            if (actionableResource != null)
            {
                removeActionable(actionableResource);
            }
            return base.UnloadCargo(cargoToRemove);
        }
    }
}
