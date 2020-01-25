using SaavedraCraft.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class Calle : IConstruction<Component>
    {
        private string name;
        private Component calleNS1;
        private int i;
        private int j;
        private bool active;
        private Component componentInstanciaReal;

        public Calle(string name, Component calleNS1, int newI, int newJ)
        {
            this.name = name;
            this.calleNS1 = calleNS1;
            this.i = newI;
            this.j = newJ;
        }

        public IConstruction<Component> CloneMe()
        {
            return new Calle(name,calleNS1, GetCoordI(), GetCoordJ());
        }

        public List<IResource> getAllExternalResources()
        {
            return new List<IResource>();
        }

        public List<IResource> getAllProducedResources()
        {
            return new List<IResource>();
        }

        public Component GetComponentMolde()
        {
            return calleNS1;
        }

        public string GetConstructionInfo()
        {
            return "Calle!";
        }

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

        public void SetActive(bool newValue)
        {
            active = newValue;
        }

        public void SetComponentInstanciaReal(Component componentReal)
        {
            componentInstanciaReal = componentReal;
        }

        public IConstruction<Component> SetNewIJ(int v1, int v2)
        {
            this.i = v1;
            this.j = v2;
            return this;
        }

        public void TimeTick(float timedelta)
        {
            //throw new NotImplementedException();
        }
    }
}
