using Assets.Scripts.Interfaces;
using Assets.Scripts.Map.Resources;
using SaavedraCraft.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    public class Casa1 : IConstruction, IResourceProducer
    {
        protected string name;
        protected Component componentMolde;
        private int i, j;
        private bool active;
        protected ICentralResourcesCommunicator centralCommunicator;
        
        private Component componentInstanciaReal;

        private List<IResource> resources = new List<IResource>();

        public Casa1(string aName, Component aComponent, int newI, int newj, ICentralResourcesCommunicator newCentralCommunicator)
        {
            name = aName;
            componentMolde = aComponent;
            i = newI;
            j = newj;
            resources.AddRange(AddInitialResources());
            active = false;
            this.SetCentralCommunicator(newCentralCommunicator);
        }

        public virtual List<IResource> AddInitialResources()
        {
            return new List<IResource>() { new SimpleResource(4, "Persona/s")};
        }

        public virtual IConstruction CloneMe()
        {
            return new Casa1(this.name,this.componentMolde,this.i,this.j, centralCommunicator);
        }

        public Component GetComponentMolde()
        {
            return componentMolde;
        }

        public string GetConstructionInfo()
        {
            int totalNumberOfResources = 0;
            string toRet = this.GetName() + "\r\n";
            resources.ForEach(x => toRet += x.GetResourceAmount() + " " + x.GetResourceName() + "\r\n");
            toRet.TrimEnd(new char[] { '\r', '\n' });
            return toRet;
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

        public virtual void SetActive(bool newValue)
        {
            active = newValue;
            this.resources.ForEach(x => x.setActive(newValue));
        }

        public void SetComponentInstanciaReal(Component componentReal)
        {
            componentInstanciaReal = componentReal;
            ConstructionClickable construcClickable = componentReal.gameObject.GetComponent<ConstructionClickable>();
            if (construcClickable != null)
            {
                construcClickable.SetConstruction(this);
            }
            else
            {
                //Debug.Log("the clickable is not here!");
            }
        }

        public IConstruction SetNewIJ(int v1, int v2)
        {
            this.i = v1;
            this.j = v2;
            return this;
        }

        public void TimeTick(float timedelta)
        {
            foreach (IResource currentResource in resources)
            {
                int resourceCntPreviousTick = currentResource.GetResourceAmount();
                currentResource.TimeTick(timedelta);
                if (currentResource.GetResourceAmount() > resourceCntPreviousTick)
                {
                    centralCommunicator.ResourcesAvailables(this);
                }
            }
        }

        public Rect GetBroadCastingProductionArea()
        {
            return new Rect(this.GetCoordI(), this.GetCoordJ(), this.GetWidh(), this.GetHeigh());
        }

        public void SetCentralCommunicator(ICentralResourcesCommunicator newCentralCommunicator)
        {
            centralCommunicator = newCentralCommunicator;
        }

        public List<IResource> getAllResources()
        {
            return resources;
        }
    }
}
