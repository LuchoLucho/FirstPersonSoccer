using System;
using System.Collections.Generic;
using NUnit.Framework;
using QuarentineSurvival.Model;
using QuarentineSurvival.Model.Actions;
using QuarentineSurvival.Model.Interface;
using SaavedraCraft.Model.CollisionEngine;
using SaavedraCraft.Model.Interface;
using SaavedraCraft.Model.Interfaces;
using SaavedraCraft.Model.Interfaces.Transportation;
using SaavedraCraft.Model.Transportation;

namespace Unify.SaavedraCraft.NTest
{
    [TestFixture()]
    public class FightTests
    {
        /*[Test()]
        public void AliveFightDropDeadTest()
        {
            ITransporterAndWarehouseManager<object> transporterAndWarehouseManager = new TransporterAndWarehouseManager<object>();
            float initialHealth = 100.0f;
            IMovableMediumCollisionAware<object> medium = new ActionCollisionableMediumAware<object>("aMediumCollionAware", null, 0, 0);
            IAlive<object> playerAlive = new MockAlive<object>("aLive", null, medium, transporterAndWarehouseManager, initialHealth);
            playerAlive.SetNewIJ(-0.5f, 0.0f);
            Assert.IsTrue(playerAlive.IsAlive());
            MockWeapon<object> weapon = new MockWeapon<object>("weapon1", null, medium, 100.0f);
            weapon.SetNewIJ(0.4f, 0.0f);
            //weapon.Hurt(playerAlive);
            int[] directionEast = new[] { 1, 0 };
            playerAlive.SetDirectionI(directionEast[0]);
            playerAlive.SetDirectionJ(directionEast[1]);
            playerAlive.SetVelocity(10.0f);
            playerAlive.TimeTick(1.0f);
            Assert.IsFalse(playerAlive.IsAlive());
            Assert.AreEqual(0.0f, playerAlive.GetVelocity());
        }

        [Test()]
        public void WeaponHolderShowTest()
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
            IAction<object> exposeHideWeaponAction = new ExposeHideWeaponAction<object>(weaponBuilder);
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

        [Test()]
        public void HoldMyWeaponTest()
        {
            
        }

        [Test()]
        public void ShowWeaponAndCollideEnemyTest()
        {
            ITransporterAndWarehouseManager<object> transporterAndWarehouseManager = new TransporterAndWarehouseManager<object>();
            IMovableMediumCollisionAware<object> medium = new ActionCollisionableMediumAware<object>("aMediumCollionAware", null, 0, 0);
            QurentinePlayerModel<object> attacker = new QurentinePlayerModel<object>("playerAttacker", null, medium, transporterAndWarehouseManager);
            ICargo<object> simpleCargo = new SimpleCargo<object>();
            float attackPower = 5.0f;
            Func<IWeapon<object>> weaponBuilder = () =>
            {
                return new MockWeapon<object>("aWeapon", null, attacker.GetMedium(), attackPower);
            };
            IAction<object> exposeHideWeaponAction = new ExposeHideWeaponAction<object>(weaponBuilder);
            IResource resource = new ActionableResource<object>(1, "WeaponHolder", exposeHideWeaponAction, 0);
            IMovableMedium<object> destinyOfResources = null; // The resouce has no fixed destination
            simpleCargo.addResources(resource, destinyOfResources);
            attacker.LoadCargo(simpleCargo);
            attacker.SetNewIJ(.4f, 0.0f);
            attacker.SetDirectionI(-1.0f);
            float attackerVelocity = 1.5f;
            attacker.SetVelocity(attackerVelocity);
            //----
            float initialHealth = 100.0f;
            MockAlive<object> victim = new MockAlive<object>("playerVictim", null, medium, transporterAndWarehouseManager, initialHealth);
            victim.SetNewIJ(-.4f, 0.0f);
            victim.SetDirectionI(1.0f);
            victim.SetVelocity(0.0f); // NO TWO MOVABLES!
            //----
            List<IAction<object>> actionsInTheHolder = attacker.ShowAvailableActions();
            List<IActionable<object>> actionables = attacker.ShowAllActionables();
            Assert.AreEqual(initialHealth,victim.GetHealth());
            Assert.AreEqual(2, medium.GetMovablesOnMedium().Count);
            actionsInTheHolder[0].execute(attacker, attacker, actionables[0]);
            Assert.AreEqual(3, medium.GetMovablesOnMedium().Count);
            MockWeapon<object> mockWeapon = medium.GetMovablesOnMedium().Find(x => x is MockWeapon<object>) as MockWeapon<object>;
            float weaponVelocity = mockWeapon.GetVelocity();
            Assert.AreEqual(attackerVelocity, weaponVelocity); // SHOULD THE HOLDER BE SOMEONE THAT HAS ON ITS ITEMS?
            attacker.TimeTick(1.0f);
            Assert.AreNotEqual(initialHealth, victim.GetHealth());
        }

        public class ExposeHideWeaponAction<T> : IAction<T>
        {
            private Func<IWeapon<T>> weaponBuilder;

            public ExposeHideWeaponAction(Func<IWeapon<T>> weaponBuilder)
            {
                this.weaponBuilder = weaponBuilder;
            }

            public bool canExecute(IActionExecutor<T> executor, IHolder<T> holder, IActionable<T> impactedActionable)
            {
                throw new NotImplementedException();
            }

            public void execute(IActionExecutor<T> executor, IHolder<T> holder, IActionable<T> impactedActionable, object param = null)
            {
                this.weaponBuilder();
            }

            public IActionable<T> getSourceActionable()
            {
                throw new NotImplementedException();
            }
        }

        public class MockWeapon<T> : SimpleTransporterCollisionable<T>, IWeapon<T>
        {
            private float attack;

            public MockWeapon(string aName, T comp, IMovableMedium<T> medium, float attack) : base(aName, comp, medium, new TransporterAndWarehouseManager<T>())
            {
                this.attack = attack;
            }

            public void Hurt(IAlive<T> alive)
            {
                alive.Hurt(attack);
            }

            public override QuarentineCollision<T> GetCustomCollision(List<IMovable<T>> list, float collisionTime)
            {
                List<IMovable<T>> toBeHurt = list.FindAll(x => x != this);
                if (toBeHurt.Count == 0)
                {
                    return null;
                }
                IAlive<T> singleToBeHurt = toBeHurt[0] as IAlive<T>;
                return new DamageCollision<T>(this, singleToBeHurt, collisionTime);
            }
        }

        public interface IWeapon<T> //: ITangibleActionable<T> Commentted just to build since I put the interface temporally in another file.
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

            public override QuarentineCollision<T> GetCustomCollision(List<IMovable<T>> list, float timeOfCollision)
            {
                return base.GetCustomCollision(list, timeOfCollision);
            }

            public void Hurt(float damage)
            {
                health -= damage;
            }

            public override string ToString()
            {
                return base.ToString() + " Health: " + health;
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

            public DamageCollision(IWeapon<T> weapon, IAlive<T> alive, float timeOfCollision) : base(new List<IMovable<T>> { weapon, alive }, timeOfCollision)
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
        }*/
    }
    
}
