using SaavedraCraft.Model.Interfaces;
using SaavedraCraft.Model.Transportation;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuarentineSurvival.Model
{
    public class WarehouseChest<T> : SimpleWareHouse<T>
    {
        public WarehouseChest(string aName, T aComponent, float newI, float newj, ITransporterAndWarehouseManager<T> transporterAndWarehouseManager) : base(aName, aComponent, newI, newj, transporterAndWarehouseManager)
        {
        }
    }
}
