using SaavedraCraft.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SaavedraCraft.Model
{
    public abstract class BasicObject<T> : IObject<T>
    {
        private string name;
        private T componentMolde;
        private float i, j;
        private bool active;
        private T componentInstanciaReal;
        private float width;
        private float heigh;

        public BasicObject(string aName, T aComponent, float newI, float newj)
        {
            name = aName;
            componentMolde = aComponent;
            i = newI;
            j = newj;
            width = heigh = 0.1f;
        }

        public abstract IObject<T> CloneMe();

        public T GetComponentMolde()
        {
            return componentMolde;
        }

        public virtual float GetCoordI()
        {
            return i;
        }

        public virtual float GetCoordJ()
        {
            return j;
        }

        public void SetWidh(float newWidth)
        {
            this.width = newWidth;
        }

        public void SetHeigh(float newHeigh)
        {
            this.heigh = newHeigh;
        }

        public float GetHeigh()
        {
            return heigh;
        }

        public string GetName()
        {
            return name;
        }

        public float GetWidh()
        {
            return width;
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

        public virtual IObject<T> SetNewIJ(float newI, float newJ)
        {
            i = newI;
            j = newJ;
            return this;
        }

        public abstract void TimeTick(float timedelta);

        public override bool Equals(object obj)
        {
            IObject<T> other = obj as IObject<T>;
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

        public override string ToString()
        {
            return this.name + "(" + this.i + "," + this.j + ")";
        }

        public virtual void Log(string message)
        {
            //Todo
        }
    }
}
