using System;
using IT7302_MonopolyProject_21102588_JoshuaCandish;
using IT7302_MonopolyProject_21102588_JoshuaCandish.Factories;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class FactoriesTest
    {
        [Test]
        public void test_property()
        {
            //create instance of factory
            PropertyFactory f = new PropertyFactory();
            //create instance from factory
            Property p = f.Create("Property");
            //check that it is right type
            Type t = new Property().GetType();
            Assert.IsInstanceOfType(t, p);
        }

        [TestFixture]
        class LuckTests
        {
            LuckFactory _luckFactory = new LuckFactory();

            [Test]
            public void test_luck()
            {
                //testing empty constructor
                var emptyLuck = new Luck();

                //create instance using factory
                var luck = _luckFactory.create("Luck", true, 50);

                // Assert that instance created is of luck type
                Assert.IsInstanceOf<Luck>(luck);
                Assert.IsInstanceOf<Luck>(emptyLuck);
            }

            [Test]
            public void landing_on_benefit_adds_money()
            {
                var player = new Player("Josh", 500);

                var benefitLuck = _luckFactory.create("Community Card", false, 50);

                benefitLuck.LandOn(ref player);

                Assert.AreEqual(550, player.GetBalance());
            }

            [Test]
            public void landing_on_penalty_subtracts_money()
            {
                var player = new Player("Josh", 500);

                var benefitLuck = _luckFactory.create("Tax", true, 50);

                benefitLuck.LandOn(ref player);

                Assert.AreEqual(450, player.GetBalance());
            }
        }

        [Test]
        public void test_residential()
        {
            //create instance of factory
            ResidentialFactory f = new ResidentialFactory();
            //create instance from factory
            Residential p = f.create("Residential", 50, 50, 50);
            //check that it is right type
            Type t = new Residential().GetType();
            Assert.IsInstanceOfType(t, p);
        }

        [Test]
        public void test_transport()
        {
            //create instance of factory
            TransportFactory f = new TransportFactory();
            //create instance from factory
            Transport p = f.create("Transport");
            //check that it is right type
            Type t = new Transport().GetType();
            Assert.IsInstanceOfType(t, p);
        }

        [Test]
        public void test_utility()
        {
            //create instance of factory
            UtilityFactory f = new UtilityFactory();
            //create instance from factory
            Utility p = f.create("Utility");
            //check that it is right type
            Type t = new Utility().GetType();
            Assert.IsInstanceOfType(t, p);
        }
    }
}
