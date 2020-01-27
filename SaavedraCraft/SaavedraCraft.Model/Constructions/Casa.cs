using SaavedraCraft.Model.Interfaces;
using SaavedraCraft.Model.Resources;
using SaavedraCraft.Model.Utils;
using System;
using System.Collections.Generic;

namespace SaavedraCraft.Model.Constructions
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
            int totalNumberOfResources = 0;
            string toRet = this.GetName() + "\r\n";// + "\r\nProd:\r\n";
           // producedResources.ForEach(x => toRet += x.GetResourceAmount() + " " + x.GetResourceName() + "\r\n");
            toRet += "Externo:\r\n";
            getAllExternalResources().ForEach(x => toRet += x.GetResourceAmount() + " " + x.GetResourceName() + "\r\n");
            toRet.TrimEnd(new char[] { '\r', '\n' });
            return toRet;
        }        
        /*
        public virtual void SetComponentInstanciaReal(T componentReal)
        {
            componentInstanciaReal = componentReal;
            ConstructionClickable construcClickable = componentReal.gameObject.GetComponent<ConstructionClickable>();
            if (construcClickable != null)
            {
                construcClickable.SetConstruction(this);
            }           
        }*/    

        /*Rectangle IResourceProducer<T>.GetBroadCastingProductionArea()
        {
            return new Rectangle(this.GetCoordI(), this.GetCoordJ(), this.GetWidh(), this.GetHeigh());
        }*/

        /*public virtual void Sell(List<IResource> list)
        {
            throw new NotImplementedException();
        }*/

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
