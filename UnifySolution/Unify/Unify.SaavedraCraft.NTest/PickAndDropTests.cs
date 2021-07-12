using System;
using System.Collections.Generic;
using NUnit.Framework;
using QuarentineSurvival.Model;
using QuarentineSurvival.Model.Actions;
using QuarentineSurvival.Model.Interface;
using SaavedraCraft.Model.Interface;
using SaavedraCraft.Model.Interfaces;
using SaavedraCraft.Model.Interfaces.Transportation;
using SaavedraCraft.Model.Resources;
using SaavedraCraft.Model.Transportation;

namespace Unify.SaavedraCraft.NTest
{
    [TestFixture()]
    public class PickAndDropTests
    {

        [Test()]
        public void ExecutorPickupActioanbleResourcesTest()
        {
            ITransporterAndWarehouseManager<object> transporterAndWarehouseManager = new TransporterAndWarehouseManager<object>();
            IMovableMediumCollisionAware<object> piso = new ActionCollisionableMediumAware<object>("ActionStreet", null, 0, 0);
            QurentinePlayerModel<object> player = new QurentinePlayerModel<object>("player", null, piso, transporterAndWarehouseManager);
            ICargo<object> simpleCargo = new SimpleCargo<object>();
            IResource resource = new ActionableResource<object>(1, "Linterna", new EncenderLinternaAction<object>(), 0);
            IMovableMedium<object> destinyOfResources = null; // The resouce has no fixed destination
            simpleCargo.addResources(resource, destinyOfResources);
            player.LoadCargo(simpleCargo);
            Assert.AreEqual(1, player.ShowAvailableActions().Count);
        }

        [Test()]
        public void ExecutorDropActioanbleResourcesTest()
        {
            ITransporterAndWarehouseManager<object> transporterAndWarehouseManager = new TransporterAndWarehouseManager<object>();
            IMovableMediumCollisionAware<object> piso = new ActionCollisionableMediumAware<object>("ActionStreet", null, 0, 0);
            QurentinePlayerModel<object> player = new QurentinePlayerModel<object>("player", null, piso, transporterAndWarehouseManager);
            ICargo<object> simpleCargo = new SimpleCargo<object>();
            IResource resource = new ActionableResource<object>(1, "Linterna", new EncenderLinternaAction<object>(), 0);
            IMovableMedium<object> destinyOfResources = null; // The resouce has no fixed destination
            simpleCargo.addResources(resource, destinyOfResources);
            player.LoadCargo(simpleCargo);
            Assert.AreEqual(1, player.ShowAvailableActions().Count);
            player.UnloadCargo(simpleCargo);
            Assert.AreEqual(0, player.ShowAvailableActions().Count);
        }

        [Test()]
        public void StepOnPickeableItemsTest()
        {
            ITransporterAndWarehouseManager<object> transporterAndWarehouseManager = new TransporterAndWarehouseManager<object>();
            IMovableMediumCollisionAware<object> piso = new ActionCollisionableMediumAware<object>("ActionStreet", null, 0, 0);
            StepOnActionable<object> stepOnActionable = new StepOnActionable<object>("StepOnMe", null, piso, transporterAndWarehouseManager);
            stepOnActionable.SetDeltaI(0.3f);
            stepOnActionable.SetAutoAction(new AutoAction<object>(x => {
                IActionExecutor<object> executor = x;
                ICargo<object> simpleCargo = new SimpleCargo<object>();
                IResource resource = new SimpleResource(1, "Coin", 0);
                simpleCargo.addResources(resource, null);
                QurentinePlayerModel<object> playerToLoadNewCargoFromCollision = executor as QurentinePlayerModel<object>;
                playerToLoadNewCargoFromCollision.LoadCargo(simpleCargo);
            }));
            QurentinePlayerModel<object> player = new QurentinePlayerModel<object>("player", null, piso, transporterAndWarehouseManager);
            player.SetDirectionI(1.0f);
            player.SetDirectionJ(0.0f);
            player.SetVelocity(1.0f);
            Assert.AreEqual(0, player.showCargo().Count);
            player.TimeTick(0.5f);
            Assert.AreEqual(1, player.showCargo().Count);
        }

        [Test()]
        public void StepOnPickeableItemsWithCoinImplementationTest() //Same as test above
        {
            ITransporterAndWarehouseManager<object> transporterAndWarehouseManager = new TransporterAndWarehouseManager<object>();
            IMovableMediumCollisionAware<object> piso = new ActionCollisionableMediumAware<object>("ActionStreet", null, 0, 0);
            
            
            QurentinePlayerModel<object> player = new QurentinePlayerModel<object>("player", null, piso, transporterAndWarehouseManager);
            player.SetDirectionI(1.0f);
            player.SetDirectionJ(0.0f);
            player.SetVelocity(1.0f);

            StepOnActionablePickableDroppable<object> stepOnCoin
                = new PickableCoin<object>("Coin", null, piso, transporterAndWarehouseManager,
                ()=>
                {
                    IEnvironment<object> env = player.GetMedium() as IEnvironment<object>;
                    return env;
                });
            stepOnCoin.SetDeltaI(0.3f);
            piso.addActionable(stepOnCoin); // Yep this seems to be needed too!

            Assert.AreEqual(0, player.showCargo().Count);
            Assert.AreEqual(0, player.ShowAllActionables().Count);
            Assert.AreEqual(2, piso.GetMovablesOnMedium().Count);
            player.TimeTick(0.5f);
            Assert.AreEqual(1, player.showCargo().Count);
            Assert.AreEqual(1, player.ShowAllActionables().Count);
            Assert.AreEqual(1, piso.GetMovablesOnMedium().Count);
        }

        public class PickableCoin<T> : StepOnActionablePickableDroppable<T>
        {
            private Func<IEnvironment<T>> getEnvironmentRightNow;

            public PickableCoin(string aName, T aComponent, IMovableMedium<T> originMedium,
                ITransporterAndWarehouseManager<T> transporterAndWarehouseManager,
                Func<IEnvironment<T>> getEnvironmentRightNow) : base(aName, aComponent, originMedium, transporterAndWarehouseManager)
            {
                this.getEnvironmentRightNow = getEnvironmentRightNow;
            }

            protected override DroppableActionableResource<T> actionableResourceOncePickedFactory()
            {
                return new DroppableActionableResource<T>(1, "AresourceCoin", getEnvironmentRightNow, this);
            }
        }

        public interface IPickableDroppable<T> : IActionable<T>, ICollisionable<T>
        {
            bool IsPickable();
            bool IsDroppable();
            AutoAction<T> OnSteppedOn();
            void OnDropOff();
        }

        public abstract class StepOnActionablePickableDroppable<T> : StepOnActionable<T>, IPickableDroppable<T>
        {

            public StepOnActionablePickableDroppable(
                string aName, T aComponent, IMovableMedium<T> originMedium,
                ITransporterAndWarehouseManager<T> transporterAndWarehouseManager
                ) : base(aName, aComponent, originMedium, transporterAndWarehouseManager)
            {
                this.SetAutoAction(this.OnSteppedOn());
            }

            protected abstract DroppableActionableResource<T> actionableResourceOncePickedFactory();

            public bool IsDroppable()
            {
                return true;
            }

            public bool IsPickable()
            {
                return true;
            }

            public void OnDropOff()
            {
                throw new NotImplementedException();
            }

            public AutoAction<T> OnSteppedOn()
            {
                return new AutoAction<T>(x =>
                {
                    IActionExecutor<T> executor = x;
                    ICargo<T> simpleCargo = new SimpleCargo<T>();
                    IResource resource = actionableResourceOncePickedFactory();//new SimpleResource(1, "Coin", 0);
                    simpleCargo.addResources(resource, null);
                    QurentinePlayerModel<T> playerToLoadNewCargoFromCollision = executor as QurentinePlayerModel<T>;
                    playerToLoadNewCargoFromCollision.LoadCargo(simpleCargo);
                    removeThisFromMedium();
                });
            }

            private void removeThisFromMedium()
            {
                IEnvironment<T> environment = this.GetMedium() as IEnvironment<T>;
                if (environment == null)
                {
                    throw new Exception("You are remove this Actionable Pickable from a non Environment.");
                }
                environment.removeActionable(this);
                this.GetMedium().MovablePart(this);
            }
        }

        public class DroppableActionableResource<T> : SimpleResource, IActionable<T>
        {
            private IAction<T> dropAction;
            private Func<IEnvironment<T>> getMediumEnvironment;
            private bool isBeingCarried;

            public DroppableActionableResource(int initialAmount, string name, Func<IEnvironment<T>> getEnvironment,
                StepOnActionablePickableDroppable<T> stepOnActionableInstanceBeforeBeingPickedUp) : base(initialAmount, name, 0.0f)
            {
                this.getMediumEnvironment = getEnvironment;
                isBeingCarried = true;
                dropAction = new DropAction<T>(this, stepOnActionableInstanceBeforeBeingPickedUp);
            }

            public void NotifyRefreshActions(IHolder<T> receiber)
            {
                receiber.NotifyRefreshActions(receiber);
            }

            public List<IAction<T>> ShowAvailableActions()
            {
                return new List<IAction<T>> { dropAction };
            }

            public bool IsBeingCarried()
            {
                return isBeingCarried;
            }

            public IEnvironment<T> getEnvironment()
            {
                return getMediumEnvironment();
            }
        }

        public class DropAction<T> : IAction<T>
        {
            private DroppableActionableResource<T> droppableActionable;
            private StepOnActionablePickableDroppable<T> stepOnActionableToDropOnMedium;

            public DropAction(DroppableActionableResource<T> droppableAsResource,
                StepOnActionablePickableDroppable<T> stepOnActionableToDropOnMedium)
            {
                this.droppableActionable = droppableAsResource;
                this.stepOnActionableToDropOnMedium = stepOnActionableToDropOnMedium;
            }

            public bool canExecute(IActionExecutor<T> executor, IHolder<T> holder, IActionable<T> impactedActionable)
            {
                return droppableActionable.IsBeingCarried();
            }

            public void execute(IActionExecutor<T> executor, IHolder<T> holder, IActionable<T> impactedActionable, object param = null)
            {
                IEnvironment<T> environment = droppableActionable.getEnvironment();
                environment.addActionable(stepOnActionableToDropOnMedium);
            }

            public IActionable<T> getSourceActionable()
            {
                return droppableActionable;
            }
        }
    }
}
