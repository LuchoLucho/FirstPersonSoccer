﻿using System;
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
            IResourceConsumer<object> resourceConsumer = new CasaTest("Casa", null, 0, 0, centralMarket);//new MockResourceProducer();
            IResourceProducer<object> resourceProducer = new CampoTomatesTest("Campo", null, 1, 1, centralMarket);
            centralMarket.AddProducer(resourceProducer);
            centralMarket.AddConsumer(resourceConsumer);
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
            //Assert.AreEqual(0, resourceConsumer.getAllResources().Find(x=>x == tomate).GetResourceAmount()); solo hay personas!
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
            centralMarket.AddProducer(resourceProducer);
            centralMarket.AddConsumer(resourceConsumer1);
            centralMarket.AddConsumer(resourceConsumer2);
            List<Transaction<object>> transactions = centralMarket.GetTransactions();
            Assert.AreEqual(1, transactions.Count);
        }

        [TestMethod]
        public void ConsumerProducerHybridSimpleConsutrctionInfoTest()
        {
            BasicConstrucHybridConsumerProducer<object> casaCampo = new CasaDeCampoTest("casaCampoHybrid",null,1,3,null);
            string hybridInfo = casaCampo.GetConstructionInfo();
            Assert.IsTrue(hybridInfo.Contains("Hybrid"));
        }

        public class CasaDeCampoTest : BasicConstrucHybridConsumerProducer<object>
        {
            public CasaDeCampoTest(string aName, object aComponent, int newI, int newj, ICentralMarket<object> newCentralMarket) : base(aName, aComponent, newI, newj, newCentralMarket)
            {
            }

            public override List<IResource> AddInitialProducedResources()
            {
                return new List<IResource>() { new SimpleResource(0, "Tomates/s") }; ;
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

            public override BasicContrucConsumer<object> getNewInstanceMeAsConsumer(string aName, object aComponent, int newI, int newj, ICentralMarket<object> newCentralCommunicator)
            {
                return new CasaTest(aName,aComponent,newI,newj,newCentralCommunicator);
            }
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

            public override List<IResource> AddInitialProducedResources()
            {
                return new List<IResource>() { new SimpleResource(1, "Tomates/s") }; ;
            }

            /*public override void Sell(List<IResource> list)
            {                
                //tomatesVendidos += list[0].GetResourceAmount();
                this.getAllProducedResources().FindAll(x => list.Contains(x)).ForEach(y => y.Subtract(list.Find(z => z.Equals(y)).GetResourceAmount()));
                //base.Sell(list);
            }*/
        }
        
    }
}
