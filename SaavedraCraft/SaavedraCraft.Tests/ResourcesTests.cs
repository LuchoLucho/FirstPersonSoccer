using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SaavedraCraft.Model.Constructions;
using SaavedraCraft.Model.Constructions.Implementations;
using SaavedraCraft.Model.Constructions.Interfaces;
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
            IResourceConsumer<object> resourceConsumer = new CasaTest("Casa", null, 0, 0, centralMarket);//new MockResourceProducer();
            IResourceProducer<object> resourceProducer = new CampoTomatesTest("Campo", null, 1, 1, centralMarket);
            resourceConsumer.SetActive(true);
            resourceProducer.SetActive(true);
            List<Transaction<object>> transactions = centralMarket.GetTransactions();
            Assert.AreEqual(1, transactions.Count);
            Assert.AreEqual(resourceProducer, transactions[0].getProducer());
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
            Transaction<object> transaction = new Transaction<object>(resourceConsumer, resourceProducer, resourcesInTransactions);
            Assert.AreEqual(1, resourceProducer.getAllProducedResources()[0].GetResourceAmount());            
            transaction.DebitarAcreditar();
            Assert.AreEqual(0, resourceProducer.getAllProducedResources()[0].GetResourceAmount());
            Assert.AreEqual(1, resourceConsumer.getAllExternalResources().Find(x => x == tomate).GetResourceAmount());
        }

        [TestMethod]
        public void ConsumersShouldFulfillItsNeedTest()
        {
            CentralMarket<object> centralMarket = new CentralMarket<object>();
            IResourceConsumer<object> resourceConsumer = new CasaTest("Casa", null, 0, 0, centralMarket);//new MockResourceProducer();
            IResourceProducer<object> resourceProducer = new CampoTomatesTest("Campo", null, 1, 1, centralMarket);
            IResource tomate = new SimpleResource(1, "Tomates/s");
            List<IResource> resourcesInTransactions = new List<IResource> { tomate };
            Transaction<object> transaction = new Transaction<object>(resourceConsumer, resourceProducer, resourcesInTransactions);
            Assert.AreEqual(1, resourceConsumer.GetNeeds(resourcesInTransactions)[0].GetResourceAmount());
            transaction.DebitarAcreditar();
            Assert.AreEqual(0, resourceConsumer.GetNeeds(resourcesInTransactions).Count);
        }

        [TestMethod]
        public void SingleTransactionIfThereIsOnlyResourceTest()
        {
            CentralMarket<object> centralMarket = new CentralMarket<object>();
            IResourceConsumer<object> resourceConsumer1 = new CasaTest("Casa1", null, 0, 0, centralMarket);//new MockResourceProducer();
            IResourceConsumer<object> resourceConsumer2 = new CasaTest("Casa2", null, 1, 0, centralMarket);//new MockResourceProducer();
            IResourceProducer<object> resourceProducer = new CampoTomatesTest("Campo", null, 1, 1, centralMarket);
            resourceConsumer1.SetActive(true);
            resourceConsumer2.SetActive(true);
            resourceProducer.SetActive(true);
            List<Transaction<object>> transactions = centralMarket.GetTransactions();
            Assert.AreEqual(1, transactions.Count);
        }

        [TestMethod]
        public void ConsumerProducerHybridSimpleConsutrctionInfoTest()
        {
            ICentralMarket<object> centralMarket = new CentralMarket<object>();
            BasicConstrucHybridConsumerProducer<object> casaCampo = new TallerCampoTest("casaCampoHybrid", null, 1, 3, centralMarket);
            string hybridInfo = casaCampo.GetConstructionInfo();
            Assert.IsTrue(hybridInfo.Contains("Hybrid"));
        }

        [TestMethod]
        public void ConsumerProducerHybridTransactionTest()
        {
            ICentralMarket<object> centralMarket = new CentralMarket<object>();
            CampoTomatesTest campoTomatesTest = new CampoTomatesTest("CampoTomates", null, 1, 1, centralMarket);
            IHybridConsumerProducer<object> hybrid = new CasaWorker("Casa", null, 0, 0, centralMarket);
            campoTomatesTest.SetActive(true);//This is needed to be put in the centralMarket
            hybrid.SetActive(true);
            List<Transaction<object>> transactions = centralMarket.GetTransactions();
            Assert.AreEqual(1, transactions.Count);
        }

        [TestMethod]
        public void ConsumerProducerHybridTransactionShouldProduceWorkerTest()
        {
            ICentralMarket<object> centralMarket = new CentralMarket<object>();
            CampoTomatesTest campoTomatesTest = new CampoTomatesTest("CampoTomates", null, 1, 1, centralMarket);
            IHybridConsumerProducer<object> hybrid = new CasaWorker("Casa", null, 0, 0, centralMarket);
            campoTomatesTest.SetActive(true);//This is needed to be put in the centralMarket
            hybrid.SetActive(true);
            List<Transaction<object>> transactions = centralMarket.GetTransactions();
            Assert.AreEqual(1, campoTomatesTest.getAllProducedResources()[0].GetResourceAmount());
            Assert.AreEqual(0, hybrid.getAllProducedResources()[0].GetResourceAmount());
            transactions[0].DebitarAcreditar();
            //After buy tomatoes, a worker should be generated
            Assert.AreEqual(0, campoTomatesTest.getAllProducedResources()[0].GetResourceAmount());
            Assert.AreEqual(1, hybrid.getAllProducedResources()[0].GetResourceAmount());            
        }

        public class TallerCampoTest : BasicConstrucHybridConsumerProducer<object>
        {
            public TallerCampoTest(string aName, object aComponent, int newI, int newj, ICentralMarket<object> newCentralMarket) : base(aName, aComponent, newI, newj, newCentralMarket)
            {
            }

            public override List<IResource> AddInitialProducedResources()
            {
                return new List<IResource>() { new SimpleResource(1, "Tomates/s") }; ; // Necesito al menos un tomate para los tests!
            }

            public override IConstruction<object> CloneMe()
            {
                throw new NotImplementedException();
            }

            public override string GetConstructionInfo()
            {
                string toRet = string.Empty;
                getAllProducedResources().ForEach(x => toRet += x.GetResourceAmount() + " " + x.GetResourceName() + "\r\n");
                toRet += meAsConsumer.GetConstructionInfo(); // <<<< Hybrid
                return toRet;
            }

            public override BasicContrucConsumer<object> getNewInstanceMeAsConsumer(string aName, object aComponent, int newI, int newj)//, ICentralMarket<object> newCentralCommunicator)
            {
                return new TallerTest(aName, aComponent, newI, newj, null);
            }

            public override void newResoucesArrivedToBeTransformed(IResourceConsumer<object> meAsConsumer)
            {
                throw new NotImplementedException();
            }            
        }

        public class TallerTest : Casa<object>
        {
            public TallerTest(string aName, object aComponent, int newI, int newj, ICentralMarket<object> newCentralCommunicator) : base(aName, aComponent, newI, newj, newCentralCommunicator)
            {
            }

            public override List<IResource> GetNeeds(List<IResource> resources)
            {
                return new List<IResource>();//My need was fulfil!!!
            }
        }

        public class CasaTest : Casa<object>
        {
            public CasaTest(string aName, object aComponent, int newI, int newj, ICentralMarket<object> newCentralCommunicator) : base(aName, aComponent, newI, newj, newCentralCommunicator)
            {
            }            
        }

        //Consumer tomates y genera trabajadores
        public class CasaWorker : CasaWorkerModel<object>
        {
            public CasaWorker(string aName, object aComponent, int newI, int newj, ICentralMarket<object> newCentralMarket) : base(aName, aComponent, newI, newj, newCentralMarket)
            {
            }
        }

        public class CampoTomatesTest : CampoTomates<object>
        {
            public CampoTomatesTest(string aName, object aComponent, int newI, int newj, ICentralMarket<object> newCentralCommunicator) : base(aName, aComponent, newI, newj, newCentralCommunicator)
            {
            }

            public override List<IResource> AddInitialProducedResources()
            {
                return new List<IResource>() { new SimpleResource(1, "Tomates/s") }; ;// Necesito al menos un tomate para los tests!
            }                        
        }
        
    }
}
