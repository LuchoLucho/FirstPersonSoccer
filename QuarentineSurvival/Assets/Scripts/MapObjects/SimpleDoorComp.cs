using Assets.Scripts.SimpleBehaviours;
using QuarentineSurvival.Model;
using QuarentineSurvival.Model.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.MapObjects
{
    public class SimpleDoorComp : SimpleDoor<Component>
    {        
        
        public SimpleDoorComp(string aName, Component aComponent, IEnvironment<Component> environmenMedium) : base(aName, aComponent, environmenMedium)
        {
            
        }

        public override void NotifyRefreshActions(IHolder<Component> holder)
        {
            Debug.Log("SimpleDoorComp.NotifyRefreshActions");
            base.NotifyRefreshActions(holder);                  
        }        
    }
}
