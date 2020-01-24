using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SaavedraCraft.Model.Constructions;
using SaavedraCraft.Model.Interfaces;
using SaavedraCraft.Model.Resources;
using SaavedraCraft.Model.Utils;

namespace SaavedraCraft.Tests
{
    [TestClass]
    public class ResourcesTests
    {
        [TestMethod]
        public void SingleTransactionWithProducerConsumerTest()
        {
            CentralMarket<object> centralMarket = new CentralMarket<object>();
            IResourceConsumer<object> resourceConsumer = new CasaTest("Casa",null,0,0, centralMarket);//new MockResourceProducer();
            IResourceProducer<object> resourceProducer = new CampoTomatesTest("Campo",null,1,1,centralMarket);
            centralMarket.AddProducer(resourceProducer);
            centralMarket.AddConsumer(resourceConsumer);
            List<Transaction<object>> transactions = centralMarket.GetTransactions();
            Assert.AreEqual(1, transactions.Count);
            Assert.AreEqual(resourceProducer,transactions[0].getProducer());
            Assert.AreEqual(resourceConsumer, transactions[0].getConsumer());
            Assert.AreEqual(1, transactions[0].getResources().Count);
        }

        [TestMethod]
        public void SingleTransactionApplyTest()
        {
            CentralMarket<object> centralMarket = new CentralMarket<object>();
            IResourceConsumer<object> resourceConsumer = new CasaTest("Casa", null, 0, 0, centralMarket);//new MockResourceProducer();
            IResourceProducer<object> resourceProducer = new CampoTomatesTest("Campo", null, 1, 1, centralMarket);
            IResource tomate = new SimpleResource(1, "Tomates/s");
            List<IResource> resourcesInTransactions = new List<IResource> { tomate };
            Transaction<object> transaction = new Transaction<object>(resourceConsumer,resourceProducer, resourcesInTransactions);
            Assert.AreEqual(1, resourceProducer.getAllResources()[0].GetResourceAmount());            
            //Assert.AreEqual(0, resourceConsumer.getAllResources().Find(x=>x == tomate).GetResourceAmount()); solo hay personas!
            transaction.DebitarAcreditar();
            Assert.AreEqual(0, resourceProducer.getAllResources()[0].GetResourceAmount());
            Assert.AreEqual(1, resourceConsumer.getAllResources().Find(x => x == tomate).GetResourceAmount());
        }

        public class CasaTest : Casa<object>
        {
            public CasaTest(string aName, object aComponent, int newI, int newj, ICentralMarket<object> newCentralCommunicator) : base(aName, aComponent, newI, newj, newCentralCommunicator)
            {
            }
        }

        public class CampoTomatesTest : CampoTomates<object>
        {
            public CampoTomatesTest(string aName, object aComponent, int newI, int newj, ICentralMarket<object> newCentralCommunicator) : base(aName, aComponent, newI, newj, newCentralCommunicator)
            {
            }

            public override List<IResource> AddInitialResources()
            {
                return new List<IResource>() { new SimpleResource(1, "Tomates/s") }; ;
            }

            public override void Sell(List<IResource> list)
            {
                //tomatesVendidos += list[0].GetResourceAmount();
                this.getAllResources().FindAll(x => list.Contains(x)).ForEach(y => y.Subtract(list.Find(z => z.Equals(y)).GetResourceAmount()));
                //base.Sell(list);
            }
        }

        /*public class MockResourceConsumer : IResourceConsumer<object>
        {
            private List<IResource> resources = new List<IResource>();

            public void Buy(List<IResource> list)
            {
                foreach(IResource toAdd in list)
                {
                    if (resources.Contains(toAdd))
                    {
                        resources.Find(x => x.Equals(toAdd)).Add(toAdd.GetResourceAmount());
                    }
                    else
                    {
                        resources.Add(toAdd);
                    }
                }
            }

            public IConstruction<object> CloneMe()
            {
                throw new NotImplementedException();
            }

            public List<IResource> getAllResources()
            {
                return resources;
            }

            public object GetComponentMolde()
            {
                throw new NotImplementedException();
            }

            public string GetConstructionInfo()
            {
                throw new NotImplementedException();
            }

            public int GetCoordI()
            {
                return 1;
            }

            public int GetCoordJ()
            {
                return 1;
            }

            public int GetHeigh()
            {
                throw new NotImplementedException();
            }

            public string GetName()
            {
                throw new NotImplementedException();
            }

            public List<IResource> GetNeeds(List<IResource> resourcesToCheck)
            {
                return new List<IResource>() { new MockResource()};
            }

            public List<IResource> GetResourceIntersectionWithProducer<T>(IResourceProducer<T> producer)
            {
                List<IResource> intersectionOfNeedsAndProvisionsFromProducer = producer.getAllResources().FindAll(x => this.GetNeeds(new List<IResource> { x }).Contains(x));
                List<IResource> resourcesIntersectionWithActualNeededAmount = cloneResourcesAndInactive(this.GetNeeds(intersectionOfNeedsAndProvisionsFromProducer));
                return resourcesIntersectionWithActualNeededAmount;
            }

            private List<IResource> cloneResourcesAndInactive(List<IResource> list)
            {
                List<IResource> toRet = new List<IResource>();
                foreach (IResource resource in list)
                {
                    IResource newResource = resource.Clone();
                    toRet.Add(newResource);
                }
                return toRet;
            }

            public int GetWidh()
            {
                throw new NotImplementedException();
            }

            public bool isActive()
            {
                throw new NotImplementedException();
            }

            public void SetActive(bool newValue)
            {
                throw new NotImplementedException();
            }

            public void SetComponentInstanciaReal(object componentReal)
            {
                throw new NotImplementedException();
            }

            public IConstruction<object> SetNewIJ(int v1, int v2)
            {
                throw new NotImplementedException();
            }

            public void TimeTick(float timedelta)
            {
                throw new NotImplementedException();
            }
        }

        public class MockResource : IResource
        {
            private int cnt = 1;

            public int GetResourceAmount()
            {
                return cnt;
            }

            public string GetResourceName()
            {
                return "MockResource1";
            }

            public bool isActive()
            {
                return true;
            }

            public void setActive(bool newValue)
            {
                //throw new NotImplementedException();
            }

            public void TimeTick(float timedelta)
            {
                //throw new NotImplementedException();
            }

            public override int GetHashCode() // all resource with the name are equals! this is needed for consumer producer intersection of resources!
            {
                return GetResourceName().GetHashCode();
            }

            public override bool Equals(object obj)  // all resource with the name are equals! this is needed for consumer producer intersection of resources!
            {
                IResource other = obj as IResource;
                if (other == null)
                {
                    return false;
                }
                return this.GetResourceName().Equals(other.GetResourceName());
            }

            public IResource Clone()
            {
                return new MockResource();
            }

            public void Add(int toAdd)
            {
                cnt += toAdd;
            }

            public void Subtract(int amountConsumed)
            {
                cnt -= amountConsumed;
            }

            
        }

        public class MockResourceProducer : IResourceProducer<object>
        {
            private IResource singleResource = new MockResource();

            public IConstruction<object> CloneMe()
            {
                throw new NotImplementedException();
            }

            public List<IResource> getAllResources()
            {
                return new List<IResource> { singleResource };
            }

            public Rectangle GetBroadCastingProductionArea()
            {
                throw new NotImplementedException();
            }

            public object GetComponentMolde()
            {
                throw new NotImplementedException();
            }

            public string GetConstructionInfo()
            {
                throw new NotImplementedException();
            }

            public int GetCoordI()
            {
                return 0;
            }

            public int GetCoordJ()
            {
                return 0;
            }

            public int GetHeigh()
            {
                throw new NotImplementedException();
            }

            public string GetName()
            {
                throw new NotImplementedException();
            }

            public int GetWidh()
            {
                throw new NotImplementedException();
            }

            public bool isActive()
            {
                throw new NotImplementedException();
            }

            public void Sell(List<IResource> list)
            {
                this.getAllResources().FindAll(x => list.Contains(x)).ForEach(y => y.Subtract(list.Find(z => z.Equals(y)).GetResourceAmount()));
            }

            public void SetActive(bool newValue)
            {
                throw new NotImplementedException();
            }

            public void SetCentralCommunicator(ICentralMarket<object> newCentralCommunicator)
            {
                throw new NotImplementedException();
            }

            public void SetComponentInstanciaReal(object componentReal)
            {
                throw new NotImplementedException();
            }

            public IConstruction<object> SetNewIJ(int v1, int v2)
            {
                throw new NotImplementedException();
            }

            public void TimeTick(float timedelta)
            {
                throw new NotImplementedException();
            }
        }*/
    }
}
