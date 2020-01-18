using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Interfaces
{
    public interface IResource
    {
        void TimeTick(float timedelta);
        int GetResourceAmount();
        string GetResourceName();
        bool isActive();
        void setActive(bool newValue);
    }
}
