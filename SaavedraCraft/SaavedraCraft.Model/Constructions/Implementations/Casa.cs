using SaavedraCraft.Model.Interfaces;
using SaavedraCraft.Model.Resources;
using SaavedraCraft.Model.Utils;
using System;
using System.Collections.Generic;

namespace SaavedraCraft.Model.Constructions.Implementations
{
    public class Casa<T> : BasicContrucConsumer<T>
    {
        public Casa(string aName, T aComponent, int newI, int newj, ICentralMarket<T> newCentralCommunicator) : base(aName, aComponent, newI, newj, newCentralCommunicator)
        {
        }

        public override IConstruction<T> CloneMe()
        {
            return new Casa<T>(name, componentMolde, i, j, centralMarket);
        }

        public override string GetConstructionInfo()
        {
            string toRet = this.GetName() + "\r\n";
            toRet += "Externo:\r\n";
            getAllExternalResources().ForEach(x => toRet += x.GetResourceAmount() + " " + x.GetResourceName() + "\r\n");
            toRet.TrimEnd(new char[] { '\r', '\n' });
            return toRet;
        }

        public override List<IResource> GetNeeds(List<IResource> resources)
        {
            int neededAmount = 1;
            IResource singleNeed = new SimpleResource(neededAmount, "Tomates/s");
            IResource alreadyGotResource = getAllExternalResources().Find(x => x.Equals(singleNeed));
            if (alreadyGotResource?.GetResourceAmount()>= neededAmount)
            {
                return new List<IResource>();//My need was fulfil!!!
            }
            return new List<IResource> { singleNeed };
        }
    }
}
