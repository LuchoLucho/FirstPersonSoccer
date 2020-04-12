using SaavedraCraft.Model.Constructions;
using SaavedraCraft.Model.Interfaces;
using SaavedraCraft.Model.Interfaces.Transportation;
using System;
using System.Collections.Generic;
using System.Text;

namespace SaavedraCraft.Model.Transportation
{
    public class SimpleCargo : ICargo<object>
    {
        private IResource resource;
        private IMovableMedium<object> destination;

        public void addResources(IResource newCargo, IMovableMedium<object> destination)
        {
            resource = newCargo;
            this.destination = destination;
        }

        public List<IResource> removeResources()
        {
            throw new NotImplementedException();
        }
    }

    public class SimpleTransporter : SimpleMovable<object>, ICargoTransporter<object>
    {
        private ICargo<object> singleCargo;

        public SimpleTransporter(string aName, object aComponent, IMovableMedium<object> originMedium) : base(aName, aComponent, originMedium)
        {
        }

        public bool CanCargoBeLoaded(ICargo<object> currentCargo)
        {
            //I transport everything!
            return singleCargo == null;
        }

        public void LoadCargo(ICargo<object> singleCargo)
        {
            this.singleCargo = singleCargo;
        }

        public ICargo<object> showCargo()
        {
            return singleCargo;
        }

        public ICargo<object> UnloadCargo()
        {
            throw new NotImplementedException();
        }
    }

    public class SimpleWareHouse : SimpleStreet<object>, IWarehouse<object>, IParkingSpot<object>
    {
        private List<ICargo<object>> allCargo;
        private List<ICargoTransporter<object>> allCargoTransporters;

        public SimpleWareHouse(string aName, object aComponent, float newI, float newj) : base(aName, aComponent, newI, newj)
        {
            allCargo = new List<ICargo<object>>();
            allCargoTransporters = new List<ICargoTransporter<object>>();
        }

        public void addCargo(ICargo<object> newCargo)
        {
            allCargo.Add(newCargo);
        }

        public void addCargoTransporter(ICargoTransporter<object> newCargoTransporter)
        {
            allCargoTransporters.Add(newCargoTransporter);
        }

        public List<ICargo<object>> GetAllCargo()
        {
            return allCargo;
        }

        public void LoadUnload()
        {
            if ((allCargo.Count == 0) || (allCargoTransporters.Count == 0))
            {
                return;
            }
            List<ICargo<object>> cargoThatWasLoadedIntoTransporters = new List<ICargo<object>>();
            foreach (ICargo<object> currentCargo in allCargo)
            {
                foreach (ICargoTransporter<object> currentTransporter in allCargoTransporters)
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

        public void parkMovable(IMovable<object> newMovableToPark)
        {
            throw new NotImplementedException();
        }

        public void removeCargo(ICargo<object> toRemove)
        {
            throw new NotImplementedException();
        }

        public void removeCargoTransporter(ICargoTransporter<object> newCargoTransporter)
        {
            throw new NotImplementedException();
        }

        public void unparkMovable(IMovable<object> newMovableToPark)
        {
            throw new NotImplementedException();
        }
    }

    public class SimpleStreet<T> : BasicContruction<T>, IMovableMedium<T>
    {
        private IMovableMedium<T> movableMediumAtNorth;
        private IMovableMedium<T> movableMediumAtSouth;
        private IMovableMedium<T> movableMediumAtWest;
        private IMovableMedium<T> movableMediumAtEast;

        public SimpleStreet(string aName, T aComponent, float newI, float newj) : base(aName, aComponent, newI, newj)
        {
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

        public float MaxSpeed()
        {
            throw new NotImplementedException();
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
                    return;
                }
                traslateNorth(deltaJ-(MOVABLE_MEDIUM_EDGE_LIMIT / 2));
            } else if (deltaJ < -MOVABLE_MEDIUM_EDGE_LIMIT / 2)
            {
                if (currentMovableMedium.GetMovableMediumAtSouth() == null)
                {
                    Log("Collision at SOUTH!");
                    this.SetVelocity(0);
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
                    return;
                }
                traslateWest(deltaI - (-MOVABLE_MEDIUM_EDGE_LIMIT / 2));
            }
        }

        private void traslateEast(float extraDeltaI)
        {
            Log("traslateEast");
            currentMovableMedium = currentMovableMedium.GetMovableMediumAtEast();
            deltaI = -MOVABLE_MEDIUM_EDGE_LIMIT / 2;//Now you are at the EDGE of the new north medium.
            deltaI += extraDeltaI;
        }

        private void traslateWest(float extraDeltaI)
        {
            Log("traslateWest");
            currentMovableMedium = currentMovableMedium.GetMovableMediumAtWest();
            deltaI = +MOVABLE_MEDIUM_EDGE_LIMIT / 2;//Now you are at the EDGE of the new north medium.
            deltaI += extraDeltaI;
        }

        private void traslateSouth(float extraDeltaJ)
        {
            Log("traslateSouth");
            currentMovableMedium = currentMovableMedium.GetMovableMediumAtSouth();
            deltaJ = +MOVABLE_MEDIUM_EDGE_LIMIT / 2;//Now you are at the EDGE of the new north medium.
            deltaJ += extraDeltaJ;
        }

        private void traslateNorth(float extraDeltaJ)
        {
            Log("traslateNorth");
            currentMovableMedium = currentMovableMedium.GetMovableMediumAtNorth();
            deltaJ = -MOVABLE_MEDIUM_EDGE_LIMIT / 2;//Now you are at the EDGE of the new north medium.
            deltaJ += extraDeltaJ;
        }

        public virtual void Log(string message)
        {
            //NOTTHING
        }
    }
}
