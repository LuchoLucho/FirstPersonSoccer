using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuarentineSurvival.Model;
using QuarentineSurvival.Model.Interface;
using SaavedraCraft.Model.CollisionEngine;
using SaavedraCraft.Model.Interface;
using SaavedraCraft.Model.Interfaces;
using SaavedraCraft.Model.Transportation;

namespace QuarentineSurvivalTest
{
    [TestClass]
    public class ActionablesItemsTests
    {
        [TestMethod]
        public void ExecutorArriveToEnvironmentWithNewActionableAndReceivesNewActionsTest()
        {
            ITransporterAndWarehouseManager<object> transporterAndWarehouseManager = new TransporterAndWarehouseManager<object>();
            IMovableMediumCollisionAware<object> piso = new ActionCollisionableMediumAware<object>("ActionStreet", null, 0, 0);
            IActionable<object> puerta = new SimpleDoor<object>("Puerta", null, piso, transporterAndWarehouseManager);
            piso.addActionable(puerta);
            IActionExecutor<object> player = new QurentinePlayerModel<object>("player", null, piso, transporterAndWarehouseManager);
            Assert.AreEqual(1, player.ShowAvailableActions().Count);
        }

        [TestMethod]
        public void ExecutorLeavesEnvironmentActionsShouldBeCleanedTest()
        {
            ITransporterAndWarehouseManager<object> transporterAndWarehouseManager = new TransporterAndWarehouseManager<object>();
            IMovableMediumCollisionAware<object> piso = new ActionCollisionableMediumAware<object>("ActionStreet", null, 0, 0);
            IMovableMediumCollisionAware<object> pisoWithNoActionable = new ActionCollisionableMediumAware<object>("NoActionStreet", null, 0, 1);
            piso.SetMovableMediumAtNorth(pisoWithNoActionable);
            IActionable<object> puerta = new SimpleDoor<object>("Puerta", null, piso, transporterAndWarehouseManager);
            piso.addActionable(puerta);
            QurentinePlayerModel<object> player = new QurentinePlayerModel<object>("player", null, piso, transporterAndWarehouseManager);
            player.SetVelocity(1);
            player.SetDirectionI(0);
            player.SetDirectionJ(1);
            Assert.AreEqual(1, player.ShowAvailableActions().Count);
            player.TimeTick(1000);
            Assert.AreEqual(0, player.ShowAvailableActions().Count);
        }

        [TestMethod]
        public void ExecutorOpensDoorAndCloseDoorActionIsTheOnlyAvailableTest()
        {
            ITransporterAndWarehouseManager<object> transporterAndWarehouseManager = new TransporterAndWarehouseManager<object>();
            IMovableMediumCollisionAware<object> piso = new ActionCollisionableMediumAware<object>("ActionStreet", null, 0, 0);
            IActionable<object> puerta = new SimpleDoor<object>("Puerta", null, piso, transporterAndWarehouseManager);
            piso.addActionable(puerta);
            IActionExecutor<object> player = new QurentinePlayerModel<object>("player", null, piso, transporterAndWarehouseManager);
            IAction<object> uniqueAction = player.ShowAvailableActions()[0];
            Assert.AreEqual("Abrir", uniqueAction.ToString());
            Assert.IsTrue(uniqueAction.canExecute(player, piso, puerta));
            uniqueAction.execute(player, piso, puerta);
            uniqueAction = player.ShowAvailableActions()[0];
            Assert.AreEqual("Cerrar", uniqueAction.ToString());
        }

        
        [TestMethod]
        public void StepOnOrSoftCollisionEventGenerationTest()
        {
            ITransporterAndWarehouseManager<object> transporterAndWarehouseManager = new TransporterAndWarehouseManager<object>();
            IMovableMediumCollisionAware<object> piso = new ActionCollisionableMediumAware<object>("ActionStreet", null, 0, 0);
            StepOnActionable<object> stepOnActionable = new StepOnActionable<object>("StepOnMe", null, piso, transporterAndWarehouseManager);
            stepOnActionable.SetDeltaI(0.3f);
            bool wasTheActionInsideTheActionExecuted = false;
            stepOnActionable.SetAutoAction(new AutoAction<object>(x => { wasTheActionInsideTheActionExecuted = true; }));
            QurentinePlayerModel<object> player = new QurentinePlayerModel<object>("player", null, piso, transporterAndWarehouseManager);            
            player.SetDirectionI(1.0f);
            player.SetDirectionJ(0.0f);
            player.SetVelocity(1.0f);
            Assert.IsFalse(stepOnActionable.GetAutoAction().WasExecuted());
            player.TimeTick(0.5f);
            Assert.IsTrue(player.GetVelocity() > 0);
            Assert.IsTrue(stepOnActionable.GetAutoAction().WasExecuted());
            Assert.IsTrue(wasTheActionInsideTheActionExecuted);
        }

        public class StepOnActionable<T> : SimpleTransporterCollisionable<T>, IActionable<T>
        {
            private AutoAction<T> action;

            public StepOnActionable(string aName, T aComponent, IMovableMedium<T> originMedium, ITransporterAndWarehouseManager<T> transporterAndWarehouseManager) : base(aName, aComponent, originMedium, transporterAndWarehouseManager)
            {                
            }

            public void SetAutoAction(AutoAction<T> newAutoAction)
            {
                action = newAutoAction; 
            }

            public AutoAction<T> GetAutoAction()
            {
                return action;
            }

            public void NotifyRefreshActions(IHolder<T> receiber)
            {
                throw new NotImplementedException();
            }

            public List<IAction<T>> ShowAvailableActions()
            {
                return new List<IAction<T>> { action };
            }

            public override QuarentineCollision<T> GetCollision(ICollisionable<T> other)
            {
                QuarentineCollision<T> oldCollision = base.GetCollision(other);
                return new SoftCollision<T>(new List<IMovable<T>> { this, other }, oldCollision.GetTimeOfCollision(), action);
            }
        }

        public class SoftCollision<T> : QuarentineCollision<T>
        {
            private IAction<T> action;

            public SoftCollision(List<IMovable<T>> bodies, float timeOfCollision, IAction<T> actionToExecute) : base(bodies, timeOfCollision)
            {
                action = actionToExecute;
            }

            public override IAction<T> GetActionOnBodyFromCollision(IMovable<T> body)
            {
                return action;
            }
        }

        public class AutoAction<T> : IAction<T>
        {
            private bool wasExecuted = false;
            private Action<IActionExecutor<T>> actionToExecute;

            public AutoAction(Action<IActionExecutor<T>> actionToExecute)
            {
                this.actionToExecute = actionToExecute;
            }

            public bool WasExecuted()
            {
                return wasExecuted;
            }

            public bool canExecute(IActionExecutor<T> executor, IHolder<T> holder, IActionable<T> impactedActionable)
            {
                return true;
            }

            public void execute(IActionExecutor<T> executor, IHolder<T> holder, IActionable<T> impactedActionable)
            {
                actionToExecute(executor);
                wasExecuted = true;
            }

            public IActionable<T> getSourceActionable()
            {
                throw new NotImplementedException();
            }
        }
        

    }
}
