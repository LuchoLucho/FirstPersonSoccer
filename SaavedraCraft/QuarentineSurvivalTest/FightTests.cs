using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuarentineSurvival.Model;
using QuarentineSurvival.Model.Actions;
using QuarentineSurvival.Model.Interface;
using SaavedraCraft.Model.CollisionEngine;
using SaavedraCraft.Model.Interface;
using SaavedraCraft.Model.Interfaces;
using SaavedraCraft.Model.Interfaces.Transportation;
using SaavedraCraft.Model.Transportation;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuarentineSurvivalTest
{
    /// <summary>
    /// Summary description for FightTests
    /// </summary>
    [TestClass]
    public class FightTests
    {
        public FightTests()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void AliveFightDropDeadTest()
        {
            ITransporterAndWarehouseManager<object> transporterAndWarehouseManager = new TransporterAndWarehouseManager<object>();
            float initialHealth = 100.0f;
            IMovableMediumCollisionAware<object> medium = new ActionCollisionableMediumAware<object>("aMediumCollionAware", null, 0, 0);
            IAlive<object> playerAlive = new MockAlive<object>("aLive",null, medium, transporterAndWarehouseManager, initialHealth);
            playerAlive.SetNewIJ(-0.5f, 0.0f);
            Assert.IsTrue(playerAlive.IsAlive());
            IWeapon<object> weapon = new MockWeapon<object>("weapon1",null, medium, 100.0f);
            weapon.SetNewIJ(0.4f,0.0f);
            //weapon.Hurt(playerAlive);
            int[] directionEast = new[] { 1, 0 };
            playerAlive.SetDirectionI(directionEast[0]);
            playerAlive.SetDirectionJ(directionEast[1]);
            playerAlive.SetVelocity(10.0f);
            playerAlive.TimeTick(1.0f);
            Assert.IsFalse(playerAlive.IsAlive());
            Assert.AreEqual(0.0f,playerAlive.GetVelocity());
        }

        [TestMethod]
        public void WeaponHolderShowTest()
        {
            ITransporterAndWarehouseManager<object> transporterAndWarehouseManager = new TransporterAndWarehouseManager<object>();
            IMovableMediumCollisionAware<object> medium = new ActionCollisionableMediumAware<object>("aMediumCollionAware", null, 0, 0);
            QurentinePlayerModel<object> weaponHolder = new QurentinePlayerModel<object>("player", null, medium, transporterAndWarehouseManager);
            ICargo<object> simpleCargo = new SimpleCargo<object>();
            float attackPower = 5.0f;
            Func<IWeapon<object>> weaponBuilder = () =>
            {
                return new MockWeapon<object>("aWeapon",null, weaponHolder.GetMedium(), attackPower);
            };
            Action<IWeapon<object>> weaponDestroyer = (w) => {};
            IAction<object> exposeHideWeaponAction = new ExposeHideWeaponAction<object>(weaponBuilder, weaponDestroyer);
            IResource resource = new ActionableResource<object>(1, "WeaponHolder", exposeHideWeaponAction, 0);
            IMovableMedium<object> destinyOfResources = null; // The resouce has no fixed destination
            simpleCargo.addResources(resource, destinyOfResources);
            weaponHolder.LoadCargo(simpleCargo);
            List<IAction<object>> actionsInTheHolder = weaponHolder.ShowAvailableActions();
            Assert.AreEqual(1, actionsInTheHolder.Count);
            List<IActionable<object>> actionables = weaponHolder.ShowAllActionables();
            Assert.AreEqual(1, actionables.Count);
            Assert.AreEqual(1, medium.GetMovablesOnMedium().Count);
            actionsInTheHolder[0].execute(weaponHolder, weaponHolder, actionables[0]);
            Assert.AreEqual(2, medium.GetMovablesOnMedium().Count);
        }

        [TestMethod]
        public void WeaponHolderBuildAndDestroyTest()
        {
            ITransporterAndWarehouseManager<object> transporterAndWarehouseManager = new TransporterAndWarehouseManager<object>();
            IMovableMediumCollisionAware<object> medium = new ActionCollisionableMediumAware<object>("aMediumCollionAware", null, 0, 0);
            QurentinePlayerModel<object> weaponHolder = new QurentinePlayerModel<object>("player", null, medium, transporterAndWarehouseManager);
            ICargo<object> simpleCargo = new SimpleCargo<object>();
            float attackPower = 5.0f;
            Func<IWeapon<object>> weaponBuilder = () =>
            {
                return new MockWeapon<object>("aWeapon", null, weaponHolder.GetMedium(), attackPower);
            };
            bool wasTheDestroyerCalled = false;
            Action<IWeapon<object>> weaponDestroyer = (w) =>
            {
                wasTheDestroyerCalled = true;
                medium.MovablePart(w);
            };
            IAction<object> exposeHideWeaponAction = new ExposeHideWeaponAction<object>(weaponBuilder, weaponDestroyer);
            IResource resource = new ActionableResource<object>(1, "WeaponHolder", exposeHideWeaponAction, 0);
            IMovableMedium<object> destinyOfResources = null; // The resouce has no fixed destination
            simpleCargo.addResources(resource, destinyOfResources);
            weaponHolder.LoadCargo(simpleCargo);
            List<IAction<object>> actionsInTheHolder = weaponHolder.ShowAvailableActions();
            List<IActionable<object>> actionables = weaponHolder.ShowAllActionables();
            actionsInTheHolder[0].execute(weaponHolder, weaponHolder, actionables[0]);
            Assert.AreEqual(2, medium.GetMovablesOnMedium().Count);
            actionsInTheHolder[0].execute(weaponHolder, weaponHolder, actionables[0]);
            Assert.AreEqual(1, medium.GetMovablesOnMedium().Count);
            Assert.IsTrue(wasTheDestroyerCalled);
        }

        [TestMethod]
        public void WeaponCreatedToTheRightHurtOtherPlayerTest()
        {
            ITransporterAndWarehouseManager<object> transporterAndWarehouseManager = new TransporterAndWarehouseManager<object>();
            IMovableMediumCollisionAware<object> medium = new ActionCollisionableMediumAware<object>("aMediumCollionAware", null, 0, 0);
            QurentinePlayerModel<object> weaponHolder = new QurentinePlayerModel<object>("player", null, medium, transporterAndWarehouseManager);
            weaponHolder.SetNewIJ(-0.8f,-0.8f);
            ICargo<object> simpleCargo = new SimpleCargo<object>();
            float attackPower = 5.0f;
            Func<IWeapon<object>> weaponBuilder = () =>
            {
                IWeapon<object> weapon = new MockWeapon<object>("aWeapon", null, weaponHolder.GetMedium(), attackPower);                
                weapon.SetNewIJ(0.7f,0.7f);
                return weapon;
            };            
            IAction<object> exposeHideWeaponAction = new ExposeHideWeaponAction<object>(weaponBuilder, x=> { });
            IResource resource = new ActionableResource<object>(1, "WeaponHolder", exposeHideWeaponAction, 0);
            IMovableMedium<object> destinyOfResources = null; // The resouce has no fixed destination
            simpleCargo.addResources(resource, destinyOfResources);
            weaponHolder.LoadCargo(simpleCargo);
            List<IAction<object>> actionsInTheHolder = weaponHolder.ShowAvailableActions();
            List<IActionable<object>> actionables = weaponHolder.ShowAllActionables();
            IAlive<object> toBeHurt = new MockAlive<object>("toBeHurt", null, medium, transporterAndWarehouseManager,1.0f);
            toBeHurt.SetVelocity(0.01f);
            toBeHurt.SetNewIJ(0.8f, 0.8f);
            actionsInTheHolder[0].execute(weaponHolder, weaponHolder, actionables[0]);
            Assert.IsTrue(toBeHurt.IsAlive());
            toBeHurt.TimeTick(1.0f);
            Assert.IsFalse(toBeHurt.IsAlive());
        }

        public class ExposeHideWeaponAction<T> : IAction<T>
        {
            private Func<IWeapon<T>> weaponBuilder;
            private Action<IWeapon<T>> weaponDestroyer;
            private IWeapon<T> weapon;

            public ExposeHideWeaponAction(Func<IWeapon<T>> weaponBuilder, Action<IWeapon<T>> weaponDestroyer)
            {
                this.weaponBuilder = weaponBuilder;
                this.weaponDestroyer = weaponDestroyer;
            }            

            public bool canExecute(IActionExecutor<T> executor, IHolder<T> holder, IActionable<T> impactedActionable)
            {
                throw new NotImplementedException();
            }

            public void execute(IActionExecutor<T> executor, IHolder<T> holder, IActionable<T> impactedActionable, object param = null)
            {
                if (weapon == null)
                {
                    weapon = this.weaponBuilder();
                }
                else
                {
                    weaponDestroyer(weapon);
                    weapon = null;
                }
            }

            public IActionable<T> getSourceActionable()
            {
                throw new NotImplementedException();
            }
        }

        public class MockWeapon<T> : SimpleTransporterCollisionable<T>, IWeapon<T>
        {
            private float attack;

            public MockWeapon(string aName, T comp, IMovableMedium<T> medium,float attack) : base(aName, comp, medium, new TransporterAndWarehouseManager<T>())
            {
                this.attack = attack;
            }

            public void Hurt(IAlive<T> alive)
            {
                alive.Hurt(attack);
            }

            public override QuarentineCollision<T> GetCustomCollision(List<ICollisionable<T>> list, float collisionTime)
            {
                List<ICollisionable<T>> toBeHurt = list.FindAll(x => x != this);
                if (toBeHurt.Count == 0)
                {
                    return null;
                }
                IAlive<T> singleToBeHurt = toBeHurt[0] as IAlive<T>;
                return new DamageCollision<T>(this, singleToBeHurt,collisionTime);
            }
        }

        public interface IWeapon<T> : ICollisionable<T>
        {
            void Hurt(IAlive<T> alive);
        }

        public class MockAlive<T> : QurentinePlayerModel<T>, IAlive<T>
        {
            private float health;

            public MockAlive(string aName, T comp, IMovableMediumCollisionAware<T> medium, ITransporterAndWarehouseManager<T> transporterAndWarehouseManager, float initialHealth) : base(aName, comp, medium, transporterAndWarehouseManager)
            {
                health = initialHealth;
            }

            public float GetHealth()
            {
                return health;
            }

            public bool IsAlive()
            {
                return health > 0.0f;
            }

            public void SetHealth(float toSetHealth)
            {
                this.health = toSetHealth;
            }

            public override QuarentineCollision<T> GetCustomCollision(List<ICollisionable<T>> list, float timeOfCollision)
            {
                List<QuarentineCollision<T>> collisions = new List<QuarentineCollision<T>>();
                list.ForEach(x => collisions.Add(x.GetCustomCollision(list,timeOfCollision)));
                return base.GetCustomCollision(list, timeOfCollision);
            }

            public void Hurt(float damage)
            {
                health -= damage;
            }
        }

        public interface IAlive<T> : ICollisionable<T>
        {
            float GetHealth();
            bool IsAlive();
            void SetHealth(float toSetHealth);

            void Hurt(float damage);
        }

        public class DamageCollision<T> : HardCollision<T>
        {
            private IWeapon<T> weapon;
            private IAlive<T> alive;

            public DamageCollision(IWeapon<T> weapon, IAlive<T> alive, float timeOfCollision) : base(new List<ICollisionable<T>> { weapon, alive}, timeOfCollision)
            {
                this.weapon = weapon;
                this.alive = alive;
            }

            public override IAction<T> GetActionOnBodyFromCollision(IMovable<T> body)
            {
                return new DamageFullStopAction<T>(weapon, alive);
            }
        }

        public class DamageFullStopAction<T> : FullStopAction<T>
        {
            private IWeapon<T> weapon;
            private IAlive<T> alive;

            public DamageFullStopAction(IWeapon<T> weapon, IAlive<T> alive)
            {
                this.weapon = weapon;
                this.alive = alive;
            }

            public override void execute(IActionExecutor<T> executor, IHolder<T> holder, IActionable<T> impactedActionable, object param = null)
            {
                base.execute(executor, holder, impactedActionable, param);
                weapon.Hurt(alive);
            }
        }
    }
}
