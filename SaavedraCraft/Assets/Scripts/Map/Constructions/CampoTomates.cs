using Assets.Scripts.Map.Resources;
using SaavedraCraft.Model.Interfaces;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Map.Constructions
{
    public class CampoTomates : Casa1
    {
        public CampoTomates(string aName, Component aComponent, int newI, int newj, ICentralMarket<Component> newCentralCommunicator) : base(aName, aComponent, newI, newj, newCentralCommunicator)
        {
        }

        public override List<IResource> AddInitialResources()
        {            
            return new List<IResource>() { new SimpleResource(0, "Tomates/s") }; ;
        }

        public override IConstruction<Component> CloneMe()
        {
            return new CampoTomates(this.name, this.componentMolde, this.GetCoordI(), this.GetCoordJ(), centralCommunicator);
        }


    }
}
