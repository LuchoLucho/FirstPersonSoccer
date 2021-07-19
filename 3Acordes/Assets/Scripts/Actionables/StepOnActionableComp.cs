using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QuarentineSurvival.Model;
using QuarentineSurvival.Model.Actions;
using SaavedraCraft.Model.Interfaces;
using SaavedraCraft.Model.CollisionEngine;
using SaavedraCraft.Model.Interface;

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
        //Debug.Log(message);
    }
}
