using Assets.Scripts.Interfaces;
using Assets.Scripts.Map.Resources;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Map.Constructions
{
    public class CampoTomates : Casa1
    {
        public CampoTomates(string aName, Component aComponent, int newI, int newj, ICentralResourcesCommunicator newCentralCommunicator) : base(aName, aComponent, newI, newj, newCentralCommunicator)
        {
        }

        public override List<IResource> AddInitialResources()
        {            
            return new List<IResource>() { new SimpleResource(0, "Tomates/s") }; ;
        }

        public override IConstruction CloneMe()
        {
            return new CampoTomates(this.name, this.componentMolde, this.GetCoordI(), this.GetCoordJ(), centralCommunicator);
        }


    }
}
