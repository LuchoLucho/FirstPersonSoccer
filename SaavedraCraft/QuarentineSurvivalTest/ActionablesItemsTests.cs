using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace QuarentineSurvivalTest
{
    [TestClass]
    public class ActionablesItemsTests
    {
        [TestMethod]
        public void ExecutorArriveToEnvironmentWithNewActionableAndReceivesNewActionsTest()
        {
            IEnvironment piso = new Piso();
            IActionable puerta = new Puerta(piso);            
            piso.addActionable(puerta);
            IActionExecutor player = new Player();
            Assert.AreEqual(0, player.ShowAvailableActions().Count);
            piso.OnActionExecutorArrived(player);
            Assert.AreEqual(1, player.ShowAvailableActions().Count);
        }

        [TestMethod]
        public void ExecutorOpensDoorAndCloseDoorActionIsTheOnlyAvailableTest()
        {
            IEnvironment piso = new Piso();
            IActionable puerta = new Puerta(piso);
            piso.addActionable(puerta);
            IActionExecutor player = new Player();            
            piso.OnActionExecutorArrived(player);
            IAction uniqueAction = player.ShowAvailableActions()[0];
            Assert.AreEqual("Abrir", uniqueAction.ToString());
            Assert.IsTrue(uniqueAction.canExecute(player,piso,puerta));
            uniqueAction.execute(player, piso, puerta);
            uniqueAction = player.ShowAvailableActions()[0];
            Assert.AreEqual("Cerrar", uniqueAction.ToString());
        }

        public class Player : IActionExecutor
        {
            private List<IActionable> allActionables;

            public Player()
            {
                allActionables = new List<IActionable>();
            }

            public void addActionable(IActionable actionableToAdd)
            {
                allActionables.Add(actionableToAdd);
            }

            public void NotifyArribeToEnvironment(IEnvironment newEnvironment)
            {
                throw new NotImplementedException();
            }

            public void NotifyLeaveEnvironment(IEnvironment newEnvironment)
            {
                throw new NotImplementedException();
            }

            public void NotifyRefreshActions(IHolder receiber)
            {
                throw new NotImplementedException();
            }

            public void removeActionable(IActionable actionableToRemove)
            {
                throw new NotImplementedException();
            }

            public List<IActionable> ShowAllActionables()
            {
                return allActionables;
            }

            public List<IAction> ShowAvailableActions()
            {
                List<IAction> allActions = new List<IAction>();
                ShowAllActionables().ForEach(x => allActions.AddRange(x.ShowAvailableActions()));
                return allActions;
            }
        }

        public class Piso : IEnvironment
        {
            private List<IActionable> allActionables;
            private IActionExecutor currentActionExector = null;

            public Piso()
            {
                allActionables = new List<IActionable>();
            }


            public void addActionable(IActionable actionableToAdd)
            {
                allActionables.Add(actionableToAdd);
            }

            public void NotifyExecutorInEnvironmentRefreshActions()
            {
                throw new NotImplementedException();
            }

            public void NotifyRefreshActions(IHolder receiber)
            {
                //throw new NotImplementedException(); TODO!
            }

            public void OnActionExecutorArrived(IActionExecutor arrivingExecutor)
            {
                currentActionExector = arrivingExecutor;
                allActionables.ForEach(x => currentActionExector.addActionable(x));
            }

            public void OnActionExecutorLeave(IActionExecutor leavingExecutor)
            {
                allActionables.ForEach(x => currentActionExector.removeActionable(x));
                currentActionExector = null;
            }

            public void removeActionable(IActionable actionableToRemove)
            {
                throw new NotImplementedException();
            }

            public List<IActionable> ShowAllActionables()
            {
                throw new NotImplementedException();
            }
        }

        public class Puerta : IActionable
        {
            private bool isOpen = false;
            private IEnvironment environment;

            public Puerta(IEnvironment environment)
            {
                this.environment = environment;
            }

            public void NotifyRefreshActions(IHolder holder)
            {
                holder.NotifyRefreshActions(holder);
            }

            public List<IAction> ShowAvailableActions()
            {
                if (isOpen)
                {
                    return new List<IAction> { new CerrarPuerta(this) };
                }
                return new List<IAction> { new AbrirPuerta(this) };
            }

            public bool IsOpen()
            {
                return isOpen;
            }

            public void SetOpen(bool newOpenStatus)
            {
                isOpen = newOpenStatus;
                NotifyRefreshActions(environment);//Now the actions change> if the door is open, it should notify that the new action is close!
            }
        }

        public class AbrirPuerta : IAction
        {
            private Puerta puerta;

            public AbrirPuerta(Puerta puerta)
            {
                this.puerta = puerta;
            }

            public bool canExecute(IActionExecutor executor, IHolder holder, IActionable impactedActionable)
            {
                return !puerta.IsOpen();
            }

            public void execute(IActionExecutor executor, IHolder holder, IActionable impactedActionable)
            {
                puerta.SetOpen(true);
            }

            public override string ToString()
            {
                return "Abrir";
            }
        }

        public class CerrarPuerta : IAction
        {
            private Puerta puerta;

            public CerrarPuerta(Puerta puerta)
            {
                this.puerta = puerta;
            }

            public bool canExecute(IActionExecutor executor, IHolder holder, IActionable impactedActionable)
            {
                throw new NotImplementedException();
            }

            public void execute(IActionExecutor executor, IHolder holder, IActionable impactedActionable)
            {
                throw new NotImplementedException();
            }

            public override string ToString()
            {
                return "Cerrar";
            }
        }

        public interface IHolder
        {
            List<IActionable> ShowAllActionables();
            void addActionable(IActionable actionableToAdd);
            void removeActionable(IActionable actionableToRemove);
            void NotifyRefreshActions(IHolder receiber);
        }

        public interface IEnvironment : IHolder
        {
            void OnActionExecutorArrived(IActionExecutor arrivingExecutor);
            void OnActionExecutorLeave(IActionExecutor leavingExecutor);
            void NotifyExecutorInEnvironmentRefreshActions();
        }

        public interface IActionExecutor : IHolder
        {
            List<IAction> ShowAvailableActions();
            void NotifyArribeToEnvironment(IEnvironment newEnvironment);
            void NotifyLeaveEnvironment(IEnvironment newEnvironment);
        }

        public interface IActionable
        {
            List<IAction> ShowAvailableActions();
            void NotifyRefreshActions(IHolder receiber);//Cuando abro una puerta, la accion abrir desaparece y aparece la accion cerrar.
        }

        public interface IAction
        {
            bool canExecute(IActionExecutor executor, IHolder holder, IActionable impactedActionable);
            void execute(IActionExecutor executor, IHolder holder, IActionable impactedActionable);
        }
    }
}
