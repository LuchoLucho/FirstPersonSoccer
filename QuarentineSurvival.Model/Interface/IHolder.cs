using SaavedraCraft.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuarentineSurvival.Model.Interface
{
    public interface IHolder<T>
    {
        List<IActionable<T>> ShowAllActionables();
        void addActionable(IActionable<T> actionableToAdd);
        void removeActionable(IActionable<T> actionableToRemove);
        void NotifyRefreshActions(IHolder<T> receiber);
    }

    public interface IEnvironment<T> : IHolder<T>, IMovableMedium<T>
    {
        void OnActionExecutorArrived(IActionExecutor<T> arrivingExecutor);
        void OnActionExecutorLeave(IActionExecutor<T> leavingExecutor);
        void NotifyExecutorInEnvironmentRefreshActions();
    }

    public interface IActionExecutor<T> : IHolder<T>
    {
        List<IAction<T>> ShowAvailableActions();
        void NotifyArribeToEnvironment(IEnvironment<T> newEnvironment);
        void NotifyLeaveEnvironment(IEnvironment<T> newEnvironment);
    }

    public interface IActionable<T>: IMovable<T>
    {
        List<IAction<T>> ShowAvailableActions();
        void NotifyRefreshActions(IHolder<T> receiber);//Cuando abro una puerta, la accion abrir desaparece y aparece la accion cerrar.
    }

    public interface IAction<T>
    {
        bool canExecute(IActionExecutor<T> executor, IHolder<T> holder, IActionable<T> impactedActionable);
        void execute(IActionExecutor<T> executor, IHolder<T> holder, IActionable<T> impactedActionable);
        IActionable<T> getSourceActionable();
    }
}
