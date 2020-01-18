using UnityEngine;

public interface IConstruction
{
    string GetName();
    int GetCoordI();
    int GetCoordJ();
    int GetWidh();
    int GetHeigh();
    Component GetComponentMolde();
    void SetComponentInstanciaReal(Component componentReal);
    IConstruction CloneMe();
    IConstruction SetNewIJ(int v1, int v2);
    void TimeTick(float timedelta);
    string GetConstructionInfo();
    bool isActive();
    void SetActive(bool newValue);
}