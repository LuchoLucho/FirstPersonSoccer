using QuarentineSurvival.Model.Interface;
using SaavedraCraft.Model.CollisionEngine;
using SaavedraCraft.Model.Interface;
using SaavedraCraft.Model.Interfaces;
using SaavedraCraft.Model.Transportation;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuarentineSurvival.Model
{
    public class SimpleDoor<T> : SimpleTransporterCollisionable<T>, IActionable<T>
    {
        private bool isOpen = false;
        private IMovableMediumCollisionAware<T> environment;

        public SimpleDoor(string aName, T aComponent, IMovableMediumCollisionAware<T> originMedium, ITransporterAndWarehouseManager<T> transporterAndWarehouseManager) : base(aName, aComponent, originMedium, transporterAndWarehouseManager)
        {
            this.environment = originMedium;
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

        public override QuarentineCollision<T> GetCollision(ICollisionable<T> other)
        {
            if (isOpen)
            {
                return new HardCollision<T>(new List<ICollisionable<T>>() { this, other}, float.MaxValue);//Door open then no collision!!!
            }
            return base.GetCollision(other);
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

        public void execute(IActionExecutor<T> executor, IHolder<T> holder, IActionable<T> impactedActionable, object param = null)
        {
            puerta.SetOpen(true);
        }

        public override string ToString()
        {
            return "Abrir";
        }

        public IActionable<T> getSourceActionable()
        {
            return puerta;
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

        public void execute(IActionExecutor<T> executor, IHolder<T> holder, IActionable<T> impactedActionable, object param = null)
        {
            puerta.SetOpen(false);
        }

        public IActionable<T> getSourceActionable()
        {
            return puerta;
        }

        public override string ToString()
        {
            return "Cerrar";
        }
    }
}
