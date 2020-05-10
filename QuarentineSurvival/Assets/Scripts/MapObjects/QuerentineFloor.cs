using QuarentineSurvival.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.MapObjects
{
    public class QuerentineFloor : ActionCollisionableMediumAware<Component>
    {
        public QuerentineFloor(string aName, Component aComponent, float newI, float newj) : base(aName, aComponent, newI, newj)
        {
        }

        public override void Log(string message)
        {
            Debug.Log(message);
        }
    }
}
