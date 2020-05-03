using QuarentineSurvival.Model.Interface;
using SaavedraCraft.Model.Interfaces;
using SaavedraCraft.Model.Transportation;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuarentineSurvival.Model
{
    public class SimpleDoor<T> : SimpleMovable<T>, IActionable<T>
    {
        private bool isOpen = false;
        private IEnvironment<T> environment;

        public SimpleDoor(string aName, T aComponent, IEnvironment<T> environmenMedium) : base(aName, aComponent, environmenMedium)
        {
            this.environment = environmenMedium;
        }

        public virtual void NotifyRefreshActions(IHolder<T> holder)
        {
            holder.NotifyRefreshActions(holder);
        }

        public List<IAction<T>> ShowAvailableActions()
        {
            if (isOpen)
            {
                return new List<IAction<T>> { new CerrarPuerta<T>(this) };
            }
            return new List<IAction<T>> { new AbrirPuerta<T>(this) };
        }

        public bool IsOpen()
        {
            return isOpen;
        }

        public void SetOpen(bool newOpenStatus)
        {
            isOpen = newOpenStatus;
            NotifyRefreshActions(environment);//Now the actions change> if the door is open, it should notify that the new action is close!
        }
    }

    public class AbrirPuerta<T> : IAction<T>
    {
        private SimpleDoor<T> puerta;

        public AbrirPuerta(SimpleDoor<T> puerta)
        {
            this.puerta = puerta;
        }

        public bool canExecute(IActionExecutor<T> executor, IHolder<T> holder, IActionable<T> impactedActionable)
        {
            return !puerta.IsOpen();
        }

        public void execute(IActionExecutor<T> executor, IHolder<T> holder, IActionable<T> impactedActionable)
        {
            puerta.SetOpen(true);
        }

        public override string ToString()
        {
            return "Abrir";
        }
    }

    public class CerrarPuerta<T> : IAction<T>
    {
        private SimpleDoor<T> puerta;

        public CerrarPuerta(SimpleDoor<T> puerta)
        {
            this.puerta = puerta;
        }

        public bool canExecute(IActionExecutor<T> executor, IHolder<T> holder, IActionable<T> impactedActionable)
        {
            return puerta.IsOpen();
        }

        public void execute(IActionExecutor<T> executor, IHolder<T> holder, IActionable<T> impactedActionable)
        {
            puerta.SetOpen(false);
        }

        public override string ToString()
        {
            return "Cerrar";
        }
    }
}
