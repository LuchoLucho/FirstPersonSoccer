using QuarentineSurvival.Model;
using SaavedraCraft.Model;
using SaavedraCraft.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Players
{
    public class SinglePlayerComp : SinglePlayerModel<Component>
    {
        public SinglePlayerComp(string aName, Component aComponent, IMovableMedium<Component> originMedium) : base(aName, aComponent, originMedium)
        {
        }

        public override void TimeTick(float timedelta)
        {
            base.TimeTick(timedelta);
        }

        public override void Log(string message)
        {
            Debug.Log(message);
        }
    }
}
