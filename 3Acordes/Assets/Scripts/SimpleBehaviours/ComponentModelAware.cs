using SaavedraCraft.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.SimpleBehaviours
{
    public class ComponentModelAware : MonoBehaviour
    {
        private IObject<Component> quarentineModel;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetQuarentineModel(IObject<Component> newModel)
        {
            quarentineModel = newModel;
        }

        public IObject<Component> GetQuarentineModel()
        {
            return quarentineModel;
        }
    }
}
