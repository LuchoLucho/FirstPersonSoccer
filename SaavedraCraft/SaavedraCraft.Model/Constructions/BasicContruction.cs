using SaavedraCraft.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SaavedraCraft.Model.Constructions
{
    public abstract class BasicContruction<T> : IConstruction<T>
    {
        protected string name;
        protected T componentMolde;
        protected int i, j;
        private bool active;
        protected T componentInstanciaReal;

        public BasicContruction(string aName, T aComponent, int newI, int newj)
        {
            name = aName;
            componentMolde = aComponent;
            i = newI;
            j = newj;
        }

        public abstract IConstruction<T> CloneMe();        

        public T GetComponentMolde()
        {
            return componentMolde;
        }

        public abstract string GetConstructionInfo();

        public int GetCoordI()
        {
            return i;
        }

        public int GetCoordJ()
        {
            return j;
        }

        public int GetHeigh()
        {
            return 1;
        }

        public string GetName()
        {
            return name;
        }

        public int GetWidh()
        {
            return 1;
        }

        public bool isActive()
        {
            return active;
        }

        public virtual void SetActive(bool newValue)
        {
            active = newValue;            
        }

        public virtual void SetComponentInstanciaReal(T componentReal)
        {
            componentInstanciaReal = componentReal;
        }

        public IConstruction<T> SetNewIJ(int newI, int newJ)
        {
            i = newI;
            j = newJ;
            return this;
        }

        public abstract void TimeTick(float timedelta);

        public override bool Equals(object obj)
        {
            IConstruction<T> other = obj as IConstruction<T>;
            if (other == null)
            {
                return false;
            }
            return this.GetCoordI() == other.GetCoordI() && this.GetCoordJ() == other.GetCoordJ();
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

    }
}
