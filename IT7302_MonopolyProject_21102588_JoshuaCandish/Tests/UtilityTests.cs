using System;
using NUnit.Framework;

namespace IT7302_MonopolyProject_21102588_JoshuaCandish.Tests
{
    [TestFixture]
    public class UtilityTest
    {
        [TestFixtureSetUp]
        public void SetUp()
        {
            var monopoly = new Monopoly();
            monopoly.SetUpProperties();
        }

        [Test]
        public void paying_rent()
        {
            var u = new Utility();

            var p = new Player("John", 1500);

            //move p so that utility rent can be calculated
            p.Move();

            u.PayRent(ref p);

            //get p last move
            int i = p.GetLastMove();

            //check that p has played correct rent of 6 times last move
            decimal balance = 1500 - (6 * i);
            Assert.AreEqual(balance, p.GetBalance());
        }

        [Test]
        public void landing_on_a_property()
        {
            Utility util = new Utility();

            //Create two players
            Player p1 = new Player("Bill");
            Player p2 = new Player("Fred", 1500);

            string msg;

            //test landon normally with no rent payable
            msg = util.LandOn(ref p1);
            Console.WriteLine(msg);

            //set owner to p1
            util.SetOwner(ref p1);

            //move p2 so that utility rent can be calculated
            p2.Move();

            //p2 lands on util and should pay rent
            msg = util.LandOn(ref p2);
            Console.WriteLine(msg);

            //check that correct rent  has been paid
            decimal balance = 1500 - (6 * p2.GetLastMove());
            Assert.AreEqual(balance, p2.GetBalance());
        }
    }
}
