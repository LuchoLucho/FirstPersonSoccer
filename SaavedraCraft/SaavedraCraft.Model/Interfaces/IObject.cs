using System;
using System.Collections.Generic;
using System.Text;

namespace SaavedraCraft.Model.Interfaces
{
    public interface IObject<T>
    {
        string GetName();
        float GetCoordI();
        float GetCoordJ();
        float GetWidh();
        float GetHeigh();
        T GetComponentMolde();
        void SetComponentInstanciaReal(T componentReal);
        IObject<T> CloneMe();
        IObject<T> SetNewIJ(float v1, float v2);
        void TimeTick(float timedelta);     
        bool isActive();
        void SetActive(bool newValue);
        void Log(String message);
    }
}
