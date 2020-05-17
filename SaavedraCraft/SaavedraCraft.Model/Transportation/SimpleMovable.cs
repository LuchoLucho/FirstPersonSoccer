using SaavedraCraft.Model.Constructions;
using SaavedraCraft.Model.Interfaces;
using SaavedraCraft.Model.Interfaces.Transportation;
using System;
using System.Collections.Generic;
using System.Text;

namespace SaavedraCraft.Model.Transportation
{
    public class SimpleCargo<T> : ICargo<T>
    {
        private IResource resource;
        private IMovableMedium<T> destination;

        public void addResources(IResource newCargo, IMovableMedium<T> destination)
        {
            resource = newCargo;
            this.destination = destination;
        }

        public List<IResource> removeResources()
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return resource.ToString();
        }
    }

    public class SimpleTransporter<T> : SimpleMovable<T>, ICargoTransporter<T>
    {
        private List<ICargo<T>> allCargo;
        private Action<IWarehouse<T>> handlerOnParkinSpaceAvailableFromWarehouse;
        private Action<IWarehouse<T>> handlerOnTransporterLeftWarehouse;

        public SimpleTransporter(string aName, T aComponent, IMovableMedium<T> originMedium, ITransporterAndWarehouseManager<T> transporterAndWarehouseManager) : base(aName, aComponent, originMedium)
        {
            allCargo = new List<ICargo<T>>();
            transporterAndWarehouseManager.SubscribeAsCargoTransporter(this);            
        }

        public bool CanCargoBeLoaded(ICargo<T> currentCargo)
        {
            //I transport everything!
            //return singleCargo == null;
            return true;
        }

        public void LoadCargo(ICargo<T> singleCargo)
        {
            allCargo.Add(singleCargo);
        }

        public virtual void NotifyParkingspaceAvailable(IWarehouse<T> simpleWareHouse)
        {
            handlerOnParkinSpaceAvailableFromWarehouse?.Invoke(simpleWareHouse);
        }

        public void NotifyTransportPartFromWarehouse(IWarehouse<T> simpleWareHouse)
        {
            handlerOnTransporterLeftWarehouse?.Invoke(simpleWareHouse);
        }

        public void OnParkinSpaceAvailableFromWarehouse(Action<IWarehouse<T>> handlerOnParkinSpaceAvailableFromWarehouse)
        {
            this.handlerOnParkinSpaceAvailableFromWarehouse = handlerOnParkinSpaceAvailableFromWarehouse;
        }

        public void OnTransportPartFromWarehouse(Action<IWarehouse<T>> handlerTransportPartFromWarehouse)
        {
            handlerOnTransporterLeftWarehouse = handlerTransportPartFromWarehouse;
        }

        public List<ICargo<T>> showCargo()
        {
            return allCargo;
        }

        public ICargo<T> UnloadCargo(ICargo<T> cargoToRemove)
        {
            allCargo.Remove(cargoToRemove);
            return cargoToRemove;
        }       
    }

    public class SimpleWareHouse<T> : SimpleStreet<T>, IWarehouse<T>, IParkingSpotForMovable<T>
    {
        private List<ICargo<T>> allCargo;
        private List<ICargoTransporter<T>> allCargoTransporters;
        private ITransporterAndWarehouseManager<T> transporterAndWarehouseManager;

        public SimpleWareHouse(string aName, T aComponent, float newI, float newj,ITransporterAndWarehouseManager<T> transporterAndWarehouseManager) : base(aName, aComponent, newI, newj)
        {
            allCargo = new List<ICargo<T>>();
            allCargoTransporters = new List<ICargoTransporter<T>>();
            this.transporterAndWarehouseManager = transporterAndWarehouseManager;
            transporterAndWarehouseManager.SubscribeAsWarehouse(this);
        }

        public void addCargo(ICargo<T> newCargo)
        {
            allCargo.Add(newCargo);
        }

        public void addCargoTransporter(ICargoTransporter<T> newCargoTransporter)
        {
            allCargoTransporters.Add(newCargoTransporter);
        }

        public List<ICargo<T>> ShowAllCargo()
        {
            return allCargo;
        }

        public void LoadUnloadAllAvailableCargo()
        {
            if ((allCargo.Count == 0) || (allCargoTransporters.Count == 0))
            {
                return;
            }
            List<ICargo<T>> cargoThatWasLoadedIntoTransporters = new List<ICargo<T>>();
            foreach (ICargo<T> currentCargo in allCargo)
            {
                foreach (ICargoTransporter<T> currentTransporter in allCargoTransporters)
                {
                    if (currentTransporter.CanCargoBeLoaded(currentCargo))
                    {
                        currentTransporter.LoadCargo(currentCargo);
                        cargoThatWasLoadedIntoTransporters.Add(currentCargo);
                        break;
                    }
                }
            }
            cargoThatWasLoadedIntoTransporters.ForEach(x => allCargo.Remove(x));
        }

        public void LoadCargoFrom(ICargo<T> newCargoToLoad)
        {
            foreach (ICargoTransporter<T> currentTransporter in allCargoTransporters)
            {
                if (currentTransporter.showCargo() == newCargoToLoad)
                {
                    allCargo.Add(newCargoToLoad);
                }
            }
        }

        public void UnloadCargoToCurrentTransporters(ICargo<T> cargoToLoadedIntoTransporter)
        {
            foreach (ICargoTransporter<T> currentTransporter in allCargoTransporters)
            {
                if (currentTransporter.CanCargoBeLoaded(cargoToLoadedIntoTransporter))
                {
                    allCargo.Remove(cargoToLoadedIntoTransporter);
                    currentTransporter.LoadCargo(cargoToLoadedIntoTransporter);
                }
            }
        }        

        public void removeCargo(ICargo<T> toRemove)
        {
            throw new NotImplementedException();
        }

        public void removeCargoTransporter(ICargoTransporter<T> newCargoTransporter)
        {
            allCargoTransporters.Remove(newCargoTransporter);
        }        

        public void parkTransporter(ICargoTransporter<T> newMovableToPark)
        {
            throw new NotImplementedException();
        }

        public void unparkTransporter(ICargoTransporter<T> newMovableToPark)
        {
            throw new NotImplementedException();
        }

        public override void MovableArrived(IMovable<T> newMovable)
        {
            base.MovableArrived(newMovable);
            transporterAndWarehouseManager.NotifyMovableArrivedToWarehouse(this, newMovable);
        }

        public override void MovablePart(IMovable<T> toRemoveMovable)
        {
            base.MovablePart(toRemoveMovable);
            transporterAndWarehouseManager.NotifyMovablePartFromWarehouse(this, toRemoveMovable);
        }

        public List<ICargoTransporter<T>> ShowAllTransporter()
        {
            return allCargoTransporters;
        }
    }

    public class SimpleStreet<T> : BasicContruction<T>, IMovableMedium<T>
    {
        public const float MOVABLE_MEDIUM_EDGE_LIMIT = 1;

        private IMovableMedium<T> movableMediumAtNorth;
        private IMovableMedium<T> movableMediumAtSouth;
        private IMovableMedium<T> movableMediumAtWest;
        private IMovableMedium<T> movableMediumAtEast;
        private List<IMovable<T>> movablesInMedium;
        private Action<IMovableMedium<T>> onMovableArrivedAlsoCustomAction;
        private Action<IMovableMedium<T>> onMovableLeftAlsoCustomAction;

        public SimpleStreet(string aName, T aComponent, float newI, float newj) : base(aName, aComponent, newI, newj)
        {
            movablesInMedium = new List<IMovable<T>>();
        }

        public override IObject<T> CloneMe()
        {
            throw new NotImplementedException();
        }

        public override string GetConstructionInfo()
        {
            throw new NotImplementedException();
        }

        public IMovableMedium<T> GetMovableMediumAtEast()
        {
            return movableMediumAtEast;
        }

        public IMovableMedium<T> GetMovableMediumAtNorth()
        {
            return movableMediumAtNorth;
        }

        public IMovableMedium<T> GetMovableMediumAtSouth()
        {
            return movableMediumAtSouth;
        }

        public IMovableMedium<T> GetMovableMediumAtWest()
        {
            return movableMediumAtWest;
        }

        public List<IMovable<T>> GetMovablesOnMedium()
        {
            return movablesInMedium;
        }

        public float MaxSpeed()
        {
            throw new NotImplementedException();
        }

        public virtual void MovableArrived(IMovable<T> newMovable)
        {
            movablesInMedium.Add(newMovable);
            onMovableArrivedAlsoCustomAction?.Invoke(this);
        }

        public virtual void MovablePart(IMovable<T> toRemoveMovable)
        {
            movablesInMedium.Remove(toRemoveMovable);
            onMovableLeftAlsoCustomAction?.Invoke(this);
        }

        public void OnMovableArrivedAlsoDo(Action<IMovableMedium<T>> onMovableArrivedAlsoCustomAction)
        {
            this.onMovableArrivedAlsoCustomAction = onMovableArrivedAlsoCustomAction;
        }

        public void OnMovableLeftAlsoDo(Action<IMovableMedium<T>> onMovableLeftAlsoCustomAction)
        {
            this.onMovableLeftAlsoCustomAction = onMovableLeftAlsoCustomAction;
        }

        public virtual void OnMovableMoving(IMovable<T> simpleMovable, float timedelta)
        {
            float movableDeltaI;
            float movableDeltaJ;
            movableDeltaI = simpleMovable.GetDeltaI();//simpleMovable.GetCoordI() - (this.GetCoordI() + MOVABLE_MEDIUM_EDGE_LIMIT / 2) ;
            movableDeltaJ = simpleMovable.GetDeltaJ();//simpleMovable.GetCoordJ() - (this.GetCoordJ() + MOVABLE_MEDIUM_EDGE_LIMIT / 2) ;
            //----MoveToBe:            
            movableDeltaI += simpleMovable.GetDirectionI() * simpleMovable.GetVelocity() * timedelta;
            movableDeltaJ += simpleMovable.GetDirectionJ() * simpleMovable.GetVelocity() * timedelta;
            //----            
            if (movableDeltaJ > MOVABLE_MEDIUM_EDGE_LIMIT / 2)
            {
                if (GetMovableMediumAtNorth() == null)
                {
                    //Log("Collision at NORTH!");
                    //this.SetVelocity(0);
                    movableDeltaJ = MOVABLE_MEDIUM_EDGE_LIMIT / 2;
                    simpleMovable.OnColissionAt(movableDeltaI,movableDeltaJ);
                    return;
                }
                float distanceUntilBoudery = (this.GetCoordJ() + MOVABLE_MEDIUM_EDGE_LIMIT) - simpleMovable.GetCoordJ();
                float verticalVelocityNorth = simpleMovable.GetDirectionJ() * simpleMovable.GetVelocity();
                float remainingTime = timedelta- Math.Abs(distanceUntilBoudery/ verticalVelocityNorth);//timeDelta - timeTookToHitEndOfMedium
                simpleMovable.traslateNorth(movableDeltaJ - (MOVABLE_MEDIUM_EDGE_LIMIT / 2), remainingTime);
            }
            else if (movableDeltaJ < -MOVABLE_MEDIUM_EDGE_LIMIT / 2)
            {
                if (GetMovableMediumAtSouth() == null)
                {
                    //Log("Collision at SOUTH!");
                    //this.SetVelocity(0);
                    movableDeltaJ = -MOVABLE_MEDIUM_EDGE_LIMIT / 2;
                    simpleMovable.OnColissionAt(movableDeltaI, movableDeltaJ);
                    return;
                }
                float distanceUntilBoudery = simpleMovable.GetCoordJ() - this.GetCoordJ();
                float verticalVelocitySouth = simpleMovable.GetDirectionJ() * simpleMovable.GetVelocity();
                float remainingTime = timedelta - Math.Abs(distanceUntilBoudery / verticalVelocitySouth);//timeDelta - timeTookToHitEndOfMedium
                simpleMovable.traslateSouth(movableDeltaJ - (-MOVABLE_MEDIUM_EDGE_LIMIT / 2), remainingTime);
            } else
            {
                simpleMovable.tralateInsideMediumJ(movableDeltaJ);
            }
            //----
            if (movableDeltaI > MOVABLE_MEDIUM_EDGE_LIMIT / 2)
            {
                if (GetMovableMediumAtEast() == null)
                {
                    //Log("Collision at EAST!");
                    //this.SetVelocity(0);
                    movableDeltaI = MOVABLE_MEDIUM_EDGE_LIMIT / 2;
                    return;
                }
                float distanceUntilBoudery = (this.GetCoordI() + MOVABLE_MEDIUM_EDGE_LIMIT) - simpleMovable.GetCoordI();
                float verticalVelocityEast = simpleMovable.GetDirectionI() * simpleMovable.GetVelocity();
                float remainingTime = timedelta - Math.Abs(distanceUntilBoudery / verticalVelocityEast);//timeDelta - timeTookToHitEndOfMedium
                simpleMovable.traslateEast(movableDeltaI - (MOVABLE_MEDIUM_EDGE_LIMIT / 2), remainingTime);
            }
            else if (movableDeltaI < -MOVABLE_MEDIUM_EDGE_LIMIT / 2)
            {
                if (GetMovableMediumAtWest() == null)
                {
                    //Log("Collision at WEST!");
                    //this.SetVelocity(0);
                    movableDeltaI = -MOVABLE_MEDIUM_EDGE_LIMIT / 2;
                    simpleMovable.OnColissionAt(movableDeltaI, movableDeltaJ);
                    return;
                }
                float distanceUntilBoudery = simpleMovable.GetCoordI() - this.GetCoordI();
                float verticalVelocityWest = simpleMovable.GetDirectionI() * simpleMovable.GetVelocity();
                float remainingTime = timedelta - Math.Abs(distanceUntilBoudery / verticalVelocityWest);//timeDelta - timeTookToHitEndOfMedium
                simpleMovable.traslateWest(movableDeltaI - (-MOVABLE_MEDIUM_EDGE_LIMIT / 2), remainingTime);
            }
            else
            {
                simpleMovable.tralateInsideMediumI(movableDeltaI);
            }
        }        

        public void SetMovableMediumAtEast(IMovableMedium<T> streetDestinyAtEast)
        {
            movableMediumAtEast = streetDestinyAtEast;
        }

        public void SetMovableMediumAtNorth(IMovableMedium<T> movableMediumAtNorth)
        {
            this.movableMediumAtNorth = movableMediumAtNorth;
        }

        public void SetMovableMediumAtSouth(IMovableMedium<T> streetDestiny)
        {
            this.movableMediumAtSouth = streetDestiny;
        }

        public void SetMovableMediumAtWest(IMovableMedium<T> streetDestinyAtWest)
        {
            movableMediumAtWest = streetDestinyAtWest;
        }

        public override void TimeTick(float timedelta)
        {
            throw new NotImplementedException();
        }
    }

    public class SimpleMovable<T> : BasicObject<T>, IMovable<T>
    {        

        private IMovableMedium<T> currentMovableMedium;
        private float directionI;
        private float directionJ;
        private float velocity;
        private float deltaI, deltaJ; //the distance inside the medium

        public SimpleMovable(string aName, T aComponent, IMovableMedium<T> originMedium) : base(aName, aComponent, originMedium.GetCoordI(), originMedium.GetCoordJ())
        {
            deltaI = deltaJ = 0.0f;
            currentMovableMedium = originMedium;
            currentMovableMedium.MovableArrived(this);
        }

        public override float GetCoordI()
        {
            return currentMovableMedium.GetCoordI() + SimpleStreet<T>.MOVABLE_MEDIUM_EDGE_LIMIT / 2 + deltaI;
        }

        public override float GetCoordJ()
        {
            return currentMovableMedium.GetCoordJ() + SimpleStreet<T>.MOVABLE_MEDIUM_EDGE_LIMIT / 2 + deltaJ;
        }

        public override IObject<T> CloneMe()
        {
            throw new NotImplementedException();
        }

        public float GetDirectionI()
        {
            return directionI;
        }
        public float GetDirectionJ()
        {
            return directionJ;
        }

        public IMovableMedium<T> GetMedium()
        {
            return currentMovableMedium;
        }

        public float GetVelocity()
        {
            return velocity;
        }

        public void SetDirectionI(float newDirectionI)
        {
            directionI = newDirectionI;
        }

        public void SetDirectionJ(float newDirectionJ)
        {
            directionJ = newDirectionJ;
        }

        public void SetMedium(IMovableMedium<T> newMedium)
        {
            throw new NotImplementedException();
        }

        public void SetVelocity(float newVelocity)
        {
            this.velocity = newVelocity;
        }

        public override void TimeTick(float timedelta)
        {
            if (Math.Abs(this.GetVelocity())<0.000001)
            {
                return;
            }                     
            currentMovableMedium.OnMovableMoving(this,timedelta);            
        }

        public void traslateEast(float extraDeltaI, float remainingDeltaTime)
        {
            Log("traslateEast");
            currentMovableMedium.MovablePart(this);
            currentMovableMedium = currentMovableMedium.GetMovableMediumAtEast();
            currentMovableMedium.MovableArrived(this);
            deltaI = -SimpleStreet<T>.MOVABLE_MEDIUM_EDGE_LIMIT / 2;//Now you are at the EDGE of the new north medium.
            //deltaI += extraDeltaI; I will replace this with the other medium to calculate for me.
            if (remainingDeltaTime > 0.001)
            {
                currentMovableMedium.OnMovableMoving(this, remainingDeltaTime);
            }
        }

        public void traslateWest(float extraDeltaI, float remainingDeltaTime)
        {
            Log("traslateWest");
            currentMovableMedium.MovablePart(this);
            currentMovableMedium = currentMovableMedium.GetMovableMediumAtWest();
            currentMovableMedium.MovableArrived(this);
            deltaI = +SimpleStreet<T>.MOVABLE_MEDIUM_EDGE_LIMIT / 2;//Now you are at the EDGE of the new north medium.
            //deltaI += extraDeltaI;
            if (remainingDeltaTime > 0.001)
            {
                currentMovableMedium.OnMovableMoving(this, remainingDeltaTime);
            }
        }

        public void traslateSouth(float extraDeltaJ, float remainingDeltaTime)
        {
            Log("traslateSouth");
            currentMovableMedium.MovablePart(this);
            currentMovableMedium = currentMovableMedium.GetMovableMediumAtSouth();
            currentMovableMedium.MovableArrived(this);
            deltaJ = +SimpleStreet<T>.MOVABLE_MEDIUM_EDGE_LIMIT / 2;//Now you are at the EDGE of the new north medium.
            //deltaJ += extraDeltaJ;
            if (remainingDeltaTime > 0.001)
            {
                currentMovableMedium.OnMovableMoving(this, remainingDeltaTime);
            }
        }

        public void traslateNorth(float extraDeltaJ, float remainingDeltaTime)
        {
            Log("traslateNorth");
            currentMovableMedium.MovablePart(this);
            currentMovableMedium = currentMovableMedium.GetMovableMediumAtNorth();
            currentMovableMedium.MovableArrived(this);
            deltaJ = -SimpleStreet<T>.MOVABLE_MEDIUM_EDGE_LIMIT / 2;//Now you are at the EDGE of the new north medium.
            //deltaJ += extraDeltaJ;
            if (remainingDeltaTime > 0.001)
            {
                currentMovableMedium.OnMovableMoving(this, remainingDeltaTime);
            }
        }

        public virtual void Log(string message)
        {
            //NOTTHING
        }

        public virtual void OnColissionAt(float movableDeltaI, float movableDeltaJ)
        {   
            this.SetVelocity(0);
            this.deltaI = movableDeltaI;
            this.deltaJ = movableDeltaJ;
            Log("Collision! Previous Velocity = " + this.GetVelocity() + " Direction: (" + this.GetDirectionI() + ";" + this.GetDirectionJ() + ") Position: (" + this.GetCoordI() + ";" + this.GetCoordJ() + ")");
        }

        public void tralateInsideMediumI(float movableDeltaI)
        {
            //Log("PRE tralateInsideMediumI Velocity = " + this.GetVelocity() + " Direction: (" + this.GetDirectionI() + ";" + this.GetDirectionJ() + ") Position: (" + this.GetCoordI() + ";" + this.GetCoordJ() + ") movableDeltaI="+ movableDeltaI + "CurrentDeltaI=" + this.deltaI);
            this.deltaI = movableDeltaI;
           // Log("POS tralateInsideMediumI Velocity = " + this.GetVelocity() + " Direction: (" + this.GetDirectionI() + ";" + this.GetDirectionJ() + ") Position: (" + this.GetCoordI() + ";" + this.GetCoordJ() + ") movableDeltaI=" + movableDeltaI + "CurrentDeltaI=" + this.deltaI);
        }

        public void tralateInsideMediumJ(float movableDeltaJ)
        {
            //Log("PRE tralateInsideMediumJ Velocity = " + this.GetVelocity() + " Direction: (" + this.GetDirectionI() + ";" + this.GetDirectionJ() + ") Position: (" + this.GetCoordI() + ";" + this.GetCoordJ() + ") movableDeltaJ="+movableDeltaJ + "CurrentDeltaJ=" + this.deltaJ);
            this.deltaJ = movableDeltaJ;
            //Log("POS tralateInsideMediumJ Velocity = " + this.GetVelocity() + " Direction: (" + this.GetDirectionI() + ";" + this.GetDirectionJ() + ") Position: (" + this.GetCoordI() + ";" + this.GetCoordJ() + ") movableDeltaJ=" + movableDeltaJ + "CurrentDeltaJ=" + this.deltaJ);
        }

        public float GetDeltaI()
        {
            return deltaI;
        }

        public float GetDeltaJ()
        {
            return deltaJ;
        }

        public void SetDeltaI(float newDeltaI)
        {
            deltaI = newDeltaI;
        }

        public void SetDeltaJ(float newDeltaJ)
        {
            deltaJ = newDeltaJ;
        }

        public override IObject<T> SetNewIJ(float newI, float newJ)
        {
            //currentMovableMedium.GetCoordI() + SimpleStreet<T>.MOVABLE_MEDIUM_EDGE_LIMIT / 2 + deltaI;
            newI = (newI - currentMovableMedium.GetCoordI()) - SimpleStreet<T>.MOVABLE_MEDIUM_EDGE_LIMIT / 2;
            newJ = (newJ - currentMovableMedium.GetCoordJ()) - SimpleStreet<T>.MOVABLE_MEDIUM_EDGE_LIMIT / 2;
            SetDeltaI(newI);
            SetDeltaJ(newJ);
            return this;
        }
    }
}
