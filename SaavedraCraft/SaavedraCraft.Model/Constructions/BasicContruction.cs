using SaavedraCraft.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SaavedraCraft.Model.Constructions
{
    public abstract class BasicContruction<T> : BasicObject<T>, IConstruction<T>
    {
        public BasicContruction(string aName, T aComponent, float newI, float newj) : base(aName, aComponent, newI, newj)
        {
        }

        public abstract string GetConstructionInfo();        

    }
}
