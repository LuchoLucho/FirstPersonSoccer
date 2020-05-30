using System;
using System.Collections.Generic;
using System.Text;

namespace SaavedraCraft.Model.Interfaces
{
    public interface IResource
    {
        void TimeTick(float timedelta);
        int GetResourceAmount();
        string GetResourceName();
        bool isActive();
        void setActive(bool newValue);
        IResource Clone();
        void Add(int toAdd);
        void Subtract(int toSubtract);        
    }

    public interface ICombinableResource : IResource
    {
        bool CanCombineWith(IResource other);
        IResource CombineWith(IResource other);
        void OnDestroyResource();
    }
}
