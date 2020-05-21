using QuarentineSurvival.Model;
using QuarentineSurvival.Model.Interface;
using SaavedraCraft.Model;
using SaavedraCraft.Model.CollisionEngine;
using SaavedraCraft.Model.Interfaces;
using SaavedraCraft.Model.Interfaces.Transportation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Players
{
    public class SinglePlayerComp : QurentinePlayerModel<Component>
    {
        public SinglePlayerComp(string aName, Component aComponent, IMovableMediumCollisionAware<Component> originMedium, ITransporterAndWarehouseManager<Component> transporterAndWarehouseManager) : base(aName, aComponent, originMedium, transporterAndWarehouseManager)
        {
        }

        public override void TimeTick(float timedelta)
        {
            base.TimeTick(timedelta);
        }

        public override void Log(string message)
        {
            Debug.Log(message);
        }

        public override void NotifyParkingspaceAvailable(IWarehouse<Component> simpleWareHouse)
        {
            base.NotifyParkingspaceAvailable(simpleWareHouse);
        }

        public override void OnColissionAt(float movableDeltaI, float movableDeltaJ, QuarentineCollision<Component> quarentineCollision)
        {
            Log("------------------------");
            Log("OnColissionAt type" + quarentineCollision.GetType() + "time = " + quarentineCollision.GetTimeOfCollision());
            quarentineCollision.ShowInvolveMovablesInCollision().ForEach(x =>
            {
                Log("OnColissionAt movable involved = " + x);
            });            
            Log("OnColissionAt Pre Vel = "+ this.GetVelocity() + this.ToString());
            base.OnColissionAt(movableDeltaI, movableDeltaJ, quarentineCollision);
            Log("OnColissionAt Pos Vel = " + this.GetVelocity() + this.ToString());
            Log("------------------------");
        }
    }
}
