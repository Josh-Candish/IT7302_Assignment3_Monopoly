using NUnit.Framework;

namespace IT7302_MonopolyProject_21102588_JoshuaCandish.Tests
{
   
    /// <summary>
    /// Test for the Banker class
    /// </summary>
   
    [TestFixture]
    public class BankerTest
    {
        [Test]
        public void test_singleton()
        { 
            //Access banker twice and tests that it is the same object
            Assert.AreSame(Banker.Access(), Banker.Access());
        }
    }
}
