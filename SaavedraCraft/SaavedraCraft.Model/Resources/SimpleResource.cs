﻿using SaavedraCraft.Model.Interfaces;
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
        private float autoGeneratedIncrementPerTick;

        public SimpleResource(int initialAmount, string name, float autoGeneratedIncrementPerTick = 0.1f)
        {
            floatResourceCnt = initialAmount;
            this.name = name;
            this.autoGeneratedIncrementPerTick = autoGeneratedIncrementPerTick;
        }        

        public IResource Clone()
        {
            return new SimpleResource((int)this.floatResourceCnt, this.name, autoGeneratedIncrementPerTick);
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
            if (Math.Abs(autoGeneratedIncrementPerTick)<0.001)
            {
                //It should NOT auto increase!
                return;
            }
            floatResourceCnt += autoGeneratedIncrementPerTick * timedelta;
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

        public override int GetHashCode()
        {
            return GetResourceName().GetHashCode();
        }

        public override string ToString()
        {
            return "Resource: " + GetResourceName() + " (" + GetResourceAmount() + ")";
        }
    }
}
