using System;
using System.Collections.Generic;
using System.Text;

namespace SaavedraCraft.Model.Interfaces
{
    public interface ICentralMarket<T>
    {
        void AddProducer(IResourceProducer<T> resourceProducer);
    }
}
