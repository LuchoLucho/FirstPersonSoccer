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

        public override void MovableLeft(IMovable<T> toRemoveMovable)
        {
            base.MovableLeft(toRemoveMovable);
            transporterAndWarehouseManager.NotifyMovablePartFromWarehouse(this, toRemoveMovable);
        }

        public List<ICargoTransporter<T>> ShowAllTransporter()
        {
            return allCargoTransporters;
        }
    }

    public class SimpleStreet<T> : BasicContruction<T>, IMovableMedium<T>
    {
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

        public virtual void MovableLeft(IMovable<T> toRemoveMovable)
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
        public const float MOVABLE_MEDIUM_EDGE_LIMIT = 1;

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
            return currentMovableMedium.GetCoordI() + MOVABLE_MEDIUM_EDGE_LIMIT/2 + deltaI;
        }

        public override float GetCoordJ()
        {
            return currentMovableMedium.GetCoordJ() + MOVABLE_MEDIUM_EDGE_LIMIT / 2 + deltaJ;
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
            deltaI += this.GetDirectionI() * timedelta * this.GetVelocity();
            deltaJ += this.GetDirectionJ() * timedelta * this.GetVelocity();            
            if (deltaJ > MOVABLE_MEDIUM_EDGE_LIMIT/2)
            {
                if (currentMovableMedium.GetMovableMediumAtNorth() == null)
                {
                    Log("Collision at NORTH!");
                    this.SetVelocity(0);
                    deltaJ = MOVABLE_MEDIUM_EDGE_LIMIT / 2;
                    return;
                }
                traslateNorth(deltaJ-(MOVABLE_MEDIUM_EDGE_LIMIT / 2));
            } else if (deltaJ < -MOVABLE_MEDIUM_EDGE_LIMIT / 2)
            {
                if (currentMovableMedium.GetMovableMediumAtSouth() == null)
                {
                    Log("Collision at SOUTH!");
                    this.SetVelocity(0);
                    deltaJ = -MOVABLE_MEDIUM_EDGE_LIMIT / 2;
                    return;
                }
                traslateSouth(deltaJ - (-MOVABLE_MEDIUM_EDGE_LIMIT / 2));
            }
            //----
            if (deltaI > MOVABLE_MEDIUM_EDGE_LIMIT / 2)
            {
                if (currentMovableMedium.GetMovableMediumAtEast() == null)
                {
                    Log("Collision at EAST!");
                    this.SetVelocity(0);
                    deltaI = MOVABLE_MEDIUM_EDGE_LIMIT / 2;
                    return;
                }
                traslateEast(deltaI - (MOVABLE_MEDIUM_EDGE_LIMIT / 2));
            }
            else if(deltaI < -MOVABLE_MEDIUM_EDGE_LIMIT / 2)
            {
                if (currentMovableMedium.GetMovableMediumAtWest() == null)
                {
                    Log("Collision at WEST!");
                    this.SetVelocity(0);
                    deltaI = -MOVABLE_MEDIUM_EDGE_LIMIT / 2;
                    return;
                }
                traslateWest(deltaI - (-MOVABLE_MEDIUM_EDGE_LIMIT / 2));
            }
        }

        private void traslateEast(float extraDeltaI)
        {
            Log("traslateEast");
            currentMovableMedium.MovableLeft(this);
            currentMovableMedium = currentMovableMedium.GetMovableMediumAtEast();
            currentMovableMedium.MovableArrived(this);
            deltaI = -MOVABLE_MEDIUM_EDGE_LIMIT / 2;//Now you are at the EDGE of the new north medium.
            deltaI += extraDeltaI;
        }

        private void traslateWest(float extraDeltaI)
        {
            Log("traslateWest");
            currentMovableMedium.MovableLeft(this);
            currentMovableMedium = currentMovableMedium.GetMovableMediumAtWest();
            currentMovableMedium.MovableArrived(this);
            deltaI = +MOVABLE_MEDIUM_EDGE_LIMIT / 2;//Now you are at the EDGE of the new north medium.
            deltaI += extraDeltaI;
        }

        private void traslateSouth(float extraDeltaJ)
        {
            Log("traslateSouth");
            currentMovableMedium.MovableLeft(this);
            currentMovableMedium = currentMovableMedium.GetMovableMediumAtSouth();
            currentMovableMedium.MovableArrived(this);
            deltaJ = +MOVABLE_MEDIUM_EDGE_LIMIT / 2;//Now you are at the EDGE of the new north medium.
            deltaJ += extraDeltaJ;
        }

        private void traslateNorth(float extraDeltaJ)
        {
            Log("traslateNorth");
            currentMovableMedium.MovableLeft(this);
            currentMovableMedium = currentMovableMedium.GetMovableMediumAtNorth();
            currentMovableMedium.MovableArrived(this);
            deltaJ = -MOVABLE_MEDIUM_EDGE_LIMIT / 2;//Now you are at the EDGE of the new north medium.
            deltaJ += extraDeltaJ;
        }

        public virtual void Log(string message)
        {
            //NOTTHING
        }
    }
}
