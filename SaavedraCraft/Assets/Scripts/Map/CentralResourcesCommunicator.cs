using SaavedraCraft.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Map
{
    public class CentralResourcesCommunicator : ICentralResourcesCommunicator<Component>
    {
        public List<IResourceProducer<Component>> producersWithProductionAvailable = new List<IResourceProducer<Component>>();

        public void ResourcesAvailables(IResourceProducer<Component> resourceProducer)
        {
            //throw new NotImplementedException();
            List<IResource> listOfAllResources = resourceProducer.getAllResources().FindAll(x=>x.GetResourceAmount()>0);
            Debug.Log("New Resources Availables from " + resourceProducer + " Total: " + listOfAllResources[0].GetResourceAmount());
            if (!producersWithProductionAvailable.Contains(resourceProducer))
            {
                producersWithProductionAvailable.Add(resourceProducer);
            }            
        }
    }
}
