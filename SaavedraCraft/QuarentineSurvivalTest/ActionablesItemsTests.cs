using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuarentineSurvival.Model;
using QuarentineSurvival.Model.Actions;
using QuarentineSurvival.Model.Interface;
using SaavedraCraft.Model.CollisionEngine;
using SaavedraCraft.Model.Interface;
using SaavedraCraft.Model.Interfaces;
using SaavedraCraft.Model.Interfaces.Transportation;
using SaavedraCraft.Model.Resources;
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

        [TestMethod]
        public void StepOnOrShouldGenerateCollisionOnceTest()
        {
            ITransporterAndWarehouseManager<object> transporterAndWarehouseManager = new TransporterAndWarehouseManager<object>();
            IMovableMediumCollisionAware<object> piso = new ActionCollisionableMediumAware<object>("ActionStreet", null, 0, 0);
            StepOnActionable<object> stepOnActionable = new StepOnActionable<object>("StepOnMe", null, piso, transporterAndWarehouseManager);
            stepOnActionable.SetDeltaI(0.3f);
            int wasTheActionInsideTheActionExecuted = 0;
            stepOnActionable.SetAutoAction(new AutoAction<object>(x => 
            {
                wasTheActionInsideTheActionExecuted++;
                stepOnActionable.SwitchOn = true;
            }));
            QurentinePlayerModel<object> player = new QurentinePlayerModel<object>("player", null, piso, transporterAndWarehouseManager);
            player.SetDirectionI(1.0f);
            player.SetDirectionJ(0.0f);
            player.SetVelocity(1.0f);
            player.TimeTick(0.5f);
            Assert.AreEqual(1,wasTheActionInsideTheActionExecuted);
            player.TimeTick(0.1f);
            Assert.AreEqual(1, wasTheActionInsideTheActionExecuted);
        }

        [TestMethod]
        public void StepOnShouldNotInterrumpMyMovementTest()
        {
            ITransporterAndWarehouseManager<object> transporterAndWarehouseManager = new TransporterAndWarehouseManager<object>();
            IMovableMediumCollisionAware<object> piso = new ActionCollisionableMediumAware<object>("ActionStreet", null, 0, 0);
            StepOnActionable<object> stepOnActionable = new StepOnActionable<object>("StepOnMe", null, piso, transporterAndWarehouseManager);
            stepOnActionable.SetWidh(0.4f);
            stepOnActionable.SetHeigh(0.4f);
            stepOnActionable.SetNewIJ(0.5f,0.5f);            
            int wasTheActionInsideTheActionExecuted = 0;
            stepOnActionable.SetAutoAction(new AutoAction<object>(x =>
            {
                wasTheActionInsideTheActionExecuted++;
                stepOnActionable.SwitchOn = true;
            }));
            QurentinePlayerModel<object> player = new QurentinePlayerModel<object>("player", null, piso, transporterAndWarehouseManager);
            player.SetNewIJ(0.2491f, 0.4998327f);
            player.SetDirectionI(1.0f);
            player.SetDirectionJ(0.0f);
            player.SetVelocity(0.45f);
            player.TimeTick(1f/24f);
            float firstI = player.GetCoordI();
            player.TimeTick(0.1f);
            float secondI = player.GetCoordI();
            player.TimeTick(0.5f);
            float thridI = player.GetCoordI();
            Assert.IsTrue(thridI > secondI);
        }

        [TestMethod]
        public void ExecutorPickupActioanbleResourcesTest()
        {
            ITransporterAndWarehouseManager<object> transporterAndWarehouseManager = new TransporterAndWarehouseManager<object>();
            IMovableMediumCollisionAware<object> piso = new ActionCollisionableMediumAware<object>("ActionStreet", null, 0, 0);
            QurentinePlayerModel<object> player = new QurentinePlayerModel<object>("player", null, piso, transporterAndWarehouseManager);
            ICargo<object> simpleCargo = new SimpleCargo<object>();
            IResource resource = new ActionableResource<object>(1, "Linterna", new EncenderLinternaAction<object>() , 0);
            IMovableMedium<object> destinyOfResources = null; // The resouce has no fixed destination
            simpleCargo.addResources(resource, destinyOfResources);
            player.LoadCargo(simpleCargo);
            Assert.AreEqual(1, player.ShowAvailableActions().Count);
        }

        [TestMethod]
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

        [TestMethod]
        public void ExecutorTransformActionableResourceTest()
        {
            ITransporterAndWarehouseManager<object> transporterAndWarehouseManager = new TransporterAndWarehouseManager<object>();
            IMovableMediumCollisionAware<object> piso = new ActionCollisionableMediumAware<object>("ActionStreet", null, 0, 0);
            QurentinePlayerModel<object> player = new QurentinePlayerModel<object>("player", null, piso, transporterAndWarehouseManager);
            ICargo<object> simpleCargo = new SimpleCargo<object>();
            IResource resource = new TransformableActionableResource<object>(1, "ToTransform", 0);
            IMovableMedium<object> destinyOfResources = null; // The resouce has no fixed destination
            simpleCargo.addResources(resource, destinyOfResources);
            player.LoadCargo(simpleCargo);            
            IActionable<object> currentActionable = player.ShowAllActionables()[0];
            Assert.AreEqual("ToTransform", ((SimpleResource)currentActionable).GetResourceName());
            currentActionable.ShowAvailableActions()[0].execute(player, player, currentActionable);
            Assert.AreEqual(1, player.ShowAvailableActions().Count);
            currentActionable = player.ShowAllActionables()[0];
            Assert.AreEqual("NewTransformed!", ((SimpleResource)currentActionable).GetResourceName());
        }

        [TestMethod]
        public void ExecutorCombineResourceTest()
        {
            ITransporterAndWarehouseManager<object> transporterAndWarehouseManager = new TransporterAndWarehouseManager<object>();
            IMovableMediumCollisionAware<object> piso = new ActionCollisionableMediumAware<object>("ActionStreet", null, 0, 0);
            QurentinePlayerModel<object> player = new QurentinePlayerModel<object>("player", null, piso, transporterAndWarehouseManager);
            ICargo<object> simpleCargo = new SimpleCargo<object>();
            IResource resource = new CombinableIntoActionableResource<object>(1, "Sock", "Scissors", "SocketMask", new WearResourceAction<object>(), 0);
            IResource otherResourceToCombine = new CombinableIntoActionableResource<object>(1, "Scissors", "Sock", "SocketMask", new WearResourceAction<object>(), 0);
            IMovableMedium<object> destinyOfResources = null; // The resouce has no fixed destination
            simpleCargo.addResources(resource, destinyOfResources);
            player.LoadCargo(simpleCargo);
            simpleCargo = new SimpleCargo<object>();
            simpleCargo.addResources(otherResourceToCombine, destinyOfResources);
            player.LoadCargo(simpleCargo);
            int cargoCount = player.showCargo().Count;
            IActionable<object> currentActionable = player.ShowAllActionables()[0];
            Assert.AreEqual("Sock", ((SimpleResource)currentActionable).GetResourceName());
            Assert.AreEqual(2, cargoCount);
            currentActionable.ShowAvailableActions()[0].execute(player, player, currentActionable, otherResourceToCombine);
            cargoCount = player.showCargo().Count;
            Assert.AreEqual(1, player.ShowAvailableActions().Count);
            currentActionable = player.ShowAllActionables()[0];
            Assert.AreEqual("SocketMask", ((SimpleResource)currentActionable).GetResourceName());
            Assert.AreEqual(1, cargoCount);
        }    

    }
}
