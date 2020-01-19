using System;
using System.Collections.Generic;

using System.Text;

namespace SaavedraCraft.Model.Interfaces
{
    public interface IConstructionManagerObserver<T>
    {
        void NewBuildCreated(IConstruction<T> constructionToBeRender);
    }
}
