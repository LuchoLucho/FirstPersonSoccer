using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuarentineSurvival.Model;
using SaavedraCraft.Model;
using SaavedraCraft.Model.Interfaces;
using SaavedraCraft.Model.Transportation;

namespace QuarentineSurvivalTest
{
    [TestClass]
    public class QuarenSurvSimpleMovesTests
    {
        [TestMethod]
        public void MovePlayerInsideAreaOfFreedomTest()
        {
            IMovableMedium<object> medium = new SimpleStreet<object>("Floor",null,0,0);
            SimpleMovable<object> player = new SinglePlayerModel<object>("Player", null,medium);
            player.SetVelocity(1.0f);
            player.SetDirectionI(1);
            player.SetDirectionJ(0);            
            Assert.AreEqual(medium.GetCoordI()+ SimpleMovable<object>.MOVABLE_MEDIUM_EDGE_LIMIT / 2, player.GetCoordI());
            player.TimeTick(0.5f);
            Assert.AreEqual(medium.GetCoordI() + SimpleMovable<object>.MOVABLE_MEDIUM_EDGE_LIMIT, player.GetCoordI());
        }        
    }
}
