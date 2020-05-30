using SaavedraCraft.Model.Interface;
using SaavedraCraft.Model.Interfaces;
using SaavedraCraft.Model.Interfaces.Transportation;
using SaavedraCraft.Model.Transportation;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuarentineSurvival.Model.Actions
{
    public class CombineAction<T> : IAction<T>
    {
        public bool canExecute(IActionExecutor<T> executor, IHolder<T> holder, IActionable<T> impactedActionable)
        {
            throw new NotImplementedException();
        }

        public void execute(IActionExecutor<T> executor, IHolder<T> holder, IActionable<T> impactedActionable, object param)
        {
            CombinableActionableResource<T> otherResourceToCombineWith = (CombinableActionableResource<T>)param;
            ICombinableResource combinable = (ICombinableResource)impactedActionable;
            ActionableResource<T> newResource = (ActionableResource<T>)combinable.CombineWith(otherResourceToCombineWith);
            holder.removeActionable(impactedActionable);
            holder.removeActionable(otherResourceToCombineWith);
            if (holder is ICargoTransporter<T>) // If the holder is a cargoTransporter the actionable will be added as part of the LoadCargo!
            {
                ICargoTransporter<T> holderAsCargoLoader = (ICargoTransporter<T>)holder;
                ICargo<T> newCargo = new SimpleCargo<T>();
                newCargo.addResources(newResource,null);
                holderAsCargoLoader.LoadCargo(newCargo);
                //----
                List<ICargo<T>> cargoToRemoveSinceItWasTransformed = new List<ICargo<T>>();
                holderAsCargoLoader.showCargo().ForEach(x =>
                {
                    if ((x.ShowResource() == combinable) || (x.ShowResource() == otherResourceToCombineWith))
                    {
                        cargoToRemoveSinceItWasTransformed.Add(x);
                    }
                });
                cargoToRemoveSinceItWasTransformed.ForEach(x => holderAsCargoLoader.UnloadCargo(x));
            }
            else
            {
                holder.addActionable(newResource);
            }
        }

        public IActionable<T> getSourceActionable()
        {
            throw new NotImplementedException();
        }
    }
}
