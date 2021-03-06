﻿using SaavedraCraft.Model.Constructions;
using SaavedraCraft.Model.Constructions.Interfaces;
using SaavedraCraft.Model.Resources;
using System;
using System.Collections.Generic;
using System.Text;

namespace SaavedraCraft.Model.Interfaces
{
    public interface ICentralMarket<T>
    {
        void AddProducer(IResourceProducer<T> resourceProducer);
        void AddConsumer(IResourceConsumer<T> resourceConsumer);
        List<Transaction<T>> GetTransactions();
        void AddHybrid(IHybridConsumerProducer<T> resourceConsumer);
        void SetLogAction(Action<string> newAction);
        void RemoveProducer(IResourceProducer<T> basicConstrucProducer);
        void RemoveConsumer(IResourceConsumer<T> basicContrucConsumer);
        void RemoveHybrid(IHybridConsumerProducer<T> basicConstrucHybridConsumerProducer);
    }
}
