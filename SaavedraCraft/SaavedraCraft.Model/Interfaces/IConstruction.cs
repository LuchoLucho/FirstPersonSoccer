
using System.Collections.Generic;

namespace SaavedraCraft.Model.Interfaces
{
    public interface IConstruction<T> : IObject<T>
    {        
        string GetConstructionInfo();        
    }
}