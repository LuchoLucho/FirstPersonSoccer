using Assets.Scripts.MapObjects;
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
        private MapManangerBehaviour constructionManager;
        private SimpleDoorComp door;


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
                constructionManager = GetComponentInParent<MapManangerBehaviour>();
                if (constructionManager != null)
                {
                    door = constructionManager.GetSingleDoor();
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
