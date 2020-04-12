using SaavedraCraft.Model;
using SaavedraCraft.Model.Interfaces;
using SaavedraCraft.Model.Interfaces.Transportation;
using SaavedraCraft.Model.Transportation;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuarentineSurvival.Model
{
    public class SinglePlayerModel<T> : SimpleMovable<T>, ICargoTransporter<T>
    {
        private List<ICargo<T>> allCargo;

        public SinglePlayerModel(string aName, T aComponent, IMovableMedium<T> originMedium) : base(aName, aComponent, originMedium)
        {
            allCargo = new List<ICargo<T>>();
        }

        public bool CanCargoBeLoaded(ICargo<T> currentCargo)
        {
            return true; // INFINIT CARGO PLACE!... for now...
        }

        public void LoadCargo(ICargo<T> singleCargo)
        {
            allCargo.Add(singleCargo);
        }

        public List<ICargo<T>> showCargo()
        {
            return allCargo;
        }

        public override void TimeTick(float timedelta)
        {
            base.TimeTick(timedelta);            
        }

        public ICargo<T> UnloadCargo(ICargo<T> cargoToRemove)
        {
            allCargo.Remove(cargoToRemove);
            return cargoToRemove;
        }
    }
}
