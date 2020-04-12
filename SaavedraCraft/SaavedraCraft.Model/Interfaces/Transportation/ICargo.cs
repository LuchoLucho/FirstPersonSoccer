using System;
using System.Collections.Generic;
using System.Text;

namespace SaavedraCraft.Model.Interfaces.Transportation
{
    public interface ICargo<T>
    {
        void addResources(IResource newCargo, IMovableMedium<T> destination);
        List<IResource> removeResources();
    }
}
