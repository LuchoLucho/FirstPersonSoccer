using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
            IResourceProducer<object> resourceProducer = new MockResourceProducer();
            IResourceConsumer<object> resourceConsumer = new MockResourceConsumer();
            CentralResourcesCommunicator<object> resourcesCommunicator = new CentralResourcesCommunicator<object>();
            resourcesCommunicator.AddProducer(resourceProducer);
            resourcesCommunicator.AddConsumer(resourceConsumer);
            List<Transaction<object>> transactions = resourcesCommunicator.GetTransactions();
            Assert.AreEqual(1, transactions.Count);
            Assert.AreEqual(resourceProducer,transactions[0].getProducer());
            Assert.AreEqual(resourceConsumer, transactions[0].getConsumer());
            Assert.AreEqual(1, transactions[0].getResources().Count);
        }

        public class MockResourceConsumer : IResourceConsumer<object>
        {
            public IConstruction<object> CloneMe()
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

            public List<IResource> GetNeeds()
            {
                return new List<IResource>() { new MockResource()};
            }

            public List<IResource> GetResourceIntersectionWithProducer<T>(IResourceProducer<T> producer)
            {
                List<IResource> intersectionOfNeedsAndProvisionsFromProducer = producer.getAllResources().FindAll(x => this.GetNeeds().Contains(x));                          
                return intersectionOfNeedsAndProvisionsFromProducer;
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
            public int GetResourceAmount()
            {
                return 1;
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

            public void SetActive(bool newValue)
            {
                throw new NotImplementedException();
            }

            public void SetCentralCommunicator(ICentralResourcesCommunicator<object> newCentralCommunicator)
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
    }
}
