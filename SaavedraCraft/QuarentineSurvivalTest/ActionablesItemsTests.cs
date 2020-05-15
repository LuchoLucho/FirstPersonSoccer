﻿using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuarentineSurvival.Model;
using QuarentineSurvival.Model.Interface;
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
    }
}
