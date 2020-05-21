using QuarentineSurvival.Model.Actions;
using SaavedraCraft.Model.CollisionEngine;
using SaavedraCraft.Model.Interface;
using SaavedraCraft.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.MapObjects
{
    public class StepOnActionableComp : StepOnActionable<Component>
    {
        public StepOnActionableComp(string aName, Component aComponent, IMovableMedium<Component> originMedium, ITransporterAndWarehouseManager<Component> transporterAndWarehouseManager) : base(aName, aComponent, originMedium, transporterAndWarehouseManager)
        {
        }

        public override QuarentineCollision<Component> GetCollision(ICollisionable<Component> other)
        {
            return base.GetCollision(other);
        }

        public override void Log(string message)
        {
            Debug.Log(message);
        }
    }
}
