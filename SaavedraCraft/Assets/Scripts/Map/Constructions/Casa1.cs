using SaavedraCraft.Model.Constructions;
using SaavedraCraft.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using SaavedraCraft.Model.Constructions.Implementations;

namespace Assets.Scripts
{
    public class Casa1 : Casa<Component>
    {
        public Casa1(string aName, Component aComponent, int newI, int newj, ICentralMarket<Component> newCentralCommunicator) : base(aName, aComponent, newI, newj, newCentralCommunicator)
        {
        }

        public override void SetComponentInstanciaReal(Component componentReal)
        {
            base.SetComponentInstanciaReal(componentReal);
            ConstructionClickable construcClickable = componentReal.gameObject.GetComponent<ConstructionClickable>();
            if (construcClickable != null)
            {
                construcClickable.SetConstruction(this);
            }
        }

        public override IObject<Component> CloneMe()
        {
            return new Casa1(this.GetName(), this.GetComponentMolde(), this.GetCoordI(), this.GetCoordJ(), centralMarket);
        }
    }
}
