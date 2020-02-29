﻿using SaavedraCraft.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SaavedraCraft.Model
{
    public abstract class BasicObject<T> : IObject<T>
    {
        private string name;
        private T componentMolde;
        private int i, j;
        private bool active;
        private T componentInstanciaReal;

        public BasicObject(string aName, T aComponent, int newI, int newj)
        {
            name = aName;
            componentMolde = aComponent;
            i = newI;
            j = newj;
        }

        public abstract IObject<T> CloneMe();

        public T GetComponentMolde()
        {
            return componentMolde;
        }

        public virtual int GetCoordI()
        {
            return i;
        }

        public virtual int GetCoordJ()
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

        public IObject<T> SetNewIJ(int newI, int newJ)
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
    }
}
