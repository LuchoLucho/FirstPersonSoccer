using System;
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
            IEnvironment<object> piso = new ActionStreet<object>("ActionStreet",null,0,0);
            IActionable<object> puerta = new SimpleDoor<object>("Puerta", null, piso);
            piso.addActionable(puerta);
            IActionExecutor<object> player = new QurentinePlayerModel<object>("player",null, piso, transporterAndWarehouseManager);
            Assert.AreEqual(0, player.ShowAvailableActions().Count);
            piso.OnActionExecutorArrived(player);
            Assert.AreEqual(1, player.ShowAvailableActions().Count);
        }

        [TestMethod]
        public void ExecutorOpensDoorAndCloseDoorActionIsTheOnlyAvailableTest()
        {
            ITransporterAndWarehouseManager<object> transporterAndWarehouseManager = new TransporterAndWarehouseManager<object>();
            IEnvironment<object> piso = new ActionStreet<object>("ActionStreet", null, 0, 0);
            IActionable<object> puerta = new SimpleDoor<object>("Puerta",null,piso);
            piso.addActionable(puerta);
            IActionExecutor<object> player = new QurentinePlayerModel<object>("player", null, piso, transporterAndWarehouseManager);
            piso.OnActionExecutorArrived(player);
            IAction<object> uniqueAction = player.ShowAvailableActions()[0];
            Assert.AreEqual("Abrir", uniqueAction.ToString());
            Assert.IsTrue(uniqueAction.canExecute(player,piso,puerta));
            uniqueAction.execute(player, piso, puerta);
            uniqueAction = player.ShowAvailableActions()[0];
            Assert.AreEqual("Cerrar", uniqueAction.ToString());
        }
    }
}
