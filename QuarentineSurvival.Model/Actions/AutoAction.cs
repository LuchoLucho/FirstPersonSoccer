using SaavedraCraft.Model.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuarentineSurvival.Model.Actions
{
    public class AutoAction<T> : IAction<T>
    {
        private bool wasExecuted = false;
        private Action<IActionExecutor<T>> actionToExecute;

        public AutoAction(Action<IActionExecutor<T>> actionToExecute)
        {
            this.actionToExecute = actionToExecute;
        }

        public bool WasExecuted()
        {
            return wasExecuted;
        }

        public bool canExecute(IActionExecutor<T> executor, IHolder<T> holder, IActionable<T> impactedActionable)
        {
            return true;
        }        

        public IActionable<T> getSourceActionable()
        {
            throw new NotImplementedException();
        }

        void IAction<T>.execute(IActionExecutor<T> executor, IHolder<T> holder, IActionable<T> impactedActionable, object param)
        {
            actionToExecute(executor);
            wasExecuted = true;
        }
    }
}
