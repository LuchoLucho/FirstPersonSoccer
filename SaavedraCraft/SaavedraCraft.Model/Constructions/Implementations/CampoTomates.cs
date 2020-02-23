using SaavedraCraft.Model.Interfaces;
using SaavedraCraft.Model.Resources;
using System;
using System.Collections.Generic;
using System.Text;

namespace SaavedraCraft.Model.Constructions.Implementations
{
    public class CampoTomates<T> : BasicConstrucProducer<T>
    {
        public CampoTomates(string aName, T aComponent, int newI, int newj, ICentralMarket<T> newCentralMarket) : base(aName, aComponent, newI, newj, newCentralMarket)
        {
        }

        public override List<IResource> AddInitialProducedResources()
        {
            return new List<IResource>() { new SimpleResource(0, "Tomates/s") }; ;
        }

        public override IConstruction<T> CloneMe()
        {
            return new CampoTomates<T>(this.name, this.componentMolde, this.GetCoordI(), this.GetCoordJ(), centralMarket);
        }

        public override string GetConstructionInfo()
        {            
            string toRet = this.GetName() + "\r\nProd:\r\n";
            getAllProducedResources().ForEach(x => toRet += x.GetResourceAmount() + " " + x.GetResourceName() + "\r\n");           
            toRet.TrimEnd(new char[] { '\r', '\n' });
            return toRet;
        }

        
    }
}
