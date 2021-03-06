﻿using SaavedraCraft.Model.Interface;
using SaavedraCraft.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuarentineSurvival.Model.Actions
{
    //Sock + scissors = socketmask
    public class CombinableIntoActionableResource<T> : CombinableActionableResource<T>
    {
        private string otherExpectedToCombine = string.Empty;
        private string resultingActionable = string.Empty;
        private IAction<T> actionInResultingActionable = null;

        public CombinableIntoActionableResource(int initialAmount, string name, string otherToCombineName, string resultActionable, IAction<T> actionInResultingActionable, float autoGeneratedIncrementPerTick = 0.1F) : base(initialAmount, name, autoGeneratedIncrementPerTick)
        {
            otherExpectedToCombine = otherToCombineName;
            resultingActionable = resultActionable;
            this.actionInResultingActionable = actionInResultingActionable;
        }

        public override bool CanCombineWith(IResource other)
        {
            if (other.GetResourceName().Equals(otherExpectedToCombine, StringComparison.CurrentCultureIgnoreCase))
            {
                return true;
            }
            return false;
        }

        public override IResource CombineWith(IResource other)
        {
            if (!CanCombineWith(other))
            {
                return null;
            }
            OnDestroyResource();
            return new ActionableResource<T>(1, resultingActionable, actionInResultingActionable, 0);
        }

        public override void NotifyRefreshActions(IHolder<T> receiber)
        {
            throw new NotImplementedException();
        }

        public override void OnDestroyResource()
        {
            //throw new NotImplementedException();
        }

        public override List<IAction<T>> ShowAvailableActions()
        {
            return new List<IAction<T>> { new CombineAction<T>() };
        }
    }
}
