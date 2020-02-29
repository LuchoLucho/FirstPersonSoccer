using System;
using System.Collections.Generic;
using System.Text;

namespace SaavedraCraft.Model.Interfaces
{
    public interface IObject<T>
    {
        string GetName();
        int GetCoordI();
        int GetCoordJ();
        int GetWidh();
        int GetHeigh();
        T GetComponentMolde();
        void SetComponentInstanciaReal(T componentReal);
        IObject<T> CloneMe();
        IObject<T> SetNewIJ(int v1, int v2);
        void TimeTick(float timedelta);     
        bool isActive();
        void SetActive(bool newValue);
    }
}
