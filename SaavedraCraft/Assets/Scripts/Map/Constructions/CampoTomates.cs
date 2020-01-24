using SaavedraCraft.Model.Interfaces;
using SaavedraCraft.Model.Resources;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Map.Constructions
{
    public class CampoTomates : Casa1
    {
        private int tomatesVendidos = 0;

        public CampoTomates(string aName, Component aComponent, int newI, int newj, ICentralMarket<Component> newCentralCommunicator) : base(aName, aComponent, newI, newj, newCentralCommunicator)
        {
        }

        public override List<IResource> AddInitialResources()
        {            
            return new List<IResource>() { new SimpleResource(0, "Tomates/s") }; ;
        }

        public override IConstruction<Component> CloneMe()
        {
            return new CampoTomates(this.name, this.componentMolde, this.GetCoordI(), this.GetCoordJ(), centralMarket);
        }

        public override void Sell(List<IResource> list)
        {
            tomatesVendidos += list[0].GetResourceAmount();
            this.getAllResources().FindAll(x => list.Contains(x)).ForEach(y => y.Subtract(list.Find(z => z.Equals(y)).GetResourceAmount()));
        }

    }
}
