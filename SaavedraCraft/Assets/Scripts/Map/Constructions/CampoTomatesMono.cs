using SaavedraCraft.Model.Constructions;
using SaavedraCraft.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using SaavedraCraft.Model.Constructions.Implementations;

namespace Assets.Scripts.Map.Constructions
{
    public class CampoTomatesMono : CampoTomates<Component>
    {
        public CampoTomatesMono(string aName, Component aComponent, int newI, int newj, ICentralMarket<Component> newCentralMarket) : base(aName, aComponent, newI, newj, newCentralMarket)
        {
        }

        public override void SetComponentInstanciaReal(Component componentReal)
        {
            componentInstanciaReal = componentReal;
            ConstructionClickable construcClickable = componentReal.gameObject.GetComponent<ConstructionClickable>();
            if (construcClickable != null)
            {   
                construcClickable.SetConstruction(this);
            }
        }

        public override IConstruction<Component> CloneMe()
        {
            return new CampoTomatesMono(this.name, this.componentMolde, this.GetCoordI(), this.GetCoordJ(), centralMarket);
        }

    }
}
