using SaavedraCraft.Model;
using SaavedraCraft.Model.Interfaces;
using SaavedraCraft.Model.Transportation;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuarentineSurvival.Model
{
    public class SinglePlayerModel<T> : SimpleMovable<T>
    {
        public SinglePlayerModel(string aName, T aComponent, IMovableMedium<T> originMedium) : base(aName, aComponent, originMedium)
        {
        }

        public override void TimeTick(float timedelta)
        {
            base.TimeTick(timedelta);            
        }
    }
}
