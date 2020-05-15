using Assets.Scripts.MapObjects;
using QuarentineSurvival.Model;
using SaavedraCraft.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.SimpleBehaviours
{
    public class VisibleInvisbleBehaviour : MonoBehaviour
    {
        private bool areValuesInitiated = false;
        private ComponentModelAware modelAware;
        private SimpleDoor<Component> door;


        public VisibleInvisbleBehaviour()
        {
        }

        public void Start()
        {
            
        }

        public void Update()
        {
            if (!areValuesInitiated)
            {
                modelAware = GetComponent<ComponentModelAware>();
                if (modelAware != null)
                {
                    IMovableMedium<Component> floorHoldingDoor = modelAware.GetQuarentineModel() as IMovableMedium<Component>;
                    floorHoldingDoor.GetMovablesOnMedium().ForEach(x => 
                    {
                        if (x is SimpleDoor<Component>)
                        {
                            door = (SimpleDoor<Component>) x;
                        }
                    });                    
                    if (door != null)
                    {                        
                        areValuesInitiated = true;
                    }
                }
            }
            else
            {
                GetComponent<Renderer>().enabled = !door.IsOpen();
            }
        }        
    }
}
