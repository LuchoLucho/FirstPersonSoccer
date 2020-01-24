using SaavedraCraft.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SaavedraCraft.Model.Resources
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

        public IResource Clone()
        {
            return new SimpleResource((int)this.floatResourceCnt, this.name);
        }

        public int GetResourceAmount()
        {
            return (int)Math.Round(floatResourceCnt);
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

        public void Add(int toAdd)
        {
            floatResourceCnt += toAdd;
        }

        public void Subtract(int amountConsumed)
        {
            floatResourceCnt -= amountConsumed;
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

        public override bool Equals(object obj)
        {
            IResource other = obj as IResource;
            if (other == null)
            {
                return false;
            }
            return GetResourceName().Equals(other.GetResourceName());
        }
    }
}
