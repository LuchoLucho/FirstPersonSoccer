
namespace SaavedraCraft.Model.Interfaces
{
    public interface IConstruction<T>
    {
        string GetName();
        int GetCoordI();
        int GetCoordJ();
        int GetWidh();
        int GetHeigh();
        T GetComponentMolde();
        void SetComponentInstanciaReal(T componentReal);
        IConstruction<T> CloneMe();
        IConstruction<T> SetNewIJ(int v1, int v2);
        void TimeTick(float timedelta);
        string GetConstructionInfo();
        bool isActive();
        void SetActive(bool newValue);
    }
}