using QuarentineSurvival.Model.Interface;
using SaavedraCraft.Model.Interfaces;
using SaavedraCraft.Model.Interfaces.Transportation;
using SaavedraCraft.Model.Transportation;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuarentineSurvival.Model
{
    public class QurentinePlayerModel<T> : SimpleTransporter<T>, IActionExecutor<T>
    {
        private List<IActionable<T>> allActionables = new List<IActionable<T>>();

        public QurentinePlayerModel(string aName, T aComponent, IMovableMedium<T> originMedium, ITransporterAndWarehouseManager<T> transporterAndWarehouseManager) : base(aName, aComponent, originMedium, transporterAndWarehouseManager)
        {
            
        }

        public void addActionable(IActionable<T> actionableToAdd)
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
    }
}
