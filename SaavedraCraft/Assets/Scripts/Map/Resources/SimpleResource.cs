using Assets.Scripts.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Map.Resources
{
    public class SimpleResource : IResource
    {
        private float floatResourceCnt = 0;
        private string name;
        private bool active;

        public SimpleResource(int initialAmount, string name)
        {
            floatResourceCnt = initialAmount;
            this.name = name;
        }

        public int GetResourceAmount()
        {
            return Mathf.RoundToInt(floatResourceCnt);
        }

        public string GetResourceName()
        {
            return name;
        }

        public bool isActive()
        {
            return active;
        }

        public void setActive(bool newValue)
        {
            active = newValue;
        }

        public void TimeTick(float timedelta)
        {
            if (!active)
            {
                return;
            }
            floatResourceCnt += 0.1f * timedelta;
            //Debug.Log("REsource amount : " + GetResourceAmount());
        }
    }
}
