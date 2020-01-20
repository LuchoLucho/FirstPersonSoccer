﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SaavedraCraft.Model.Interfaces
{
    public interface ICentralResourcesCommunicator<T>
    {
        void AddProducer(IResourceProducer<T> resourceProducer);
    }
}
