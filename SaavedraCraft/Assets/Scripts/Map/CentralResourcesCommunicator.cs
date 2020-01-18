using Assets.Scripts.Interfaces;
using SaavedraCraft.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Map
{
    public class CentralResourcesCommunicator : ICentralResourcesCommunicator
    {
        public List<IResourceProducer> producersWithProductionAvailable = new List<IResourceProducer>();

        public void ResourcesAvailables(IResourceProducer resourceProducer)
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
