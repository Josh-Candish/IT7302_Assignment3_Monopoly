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

        [Test]
        public void correct_banker_should_be_loaded_on_load_game()
        {
            var gameOfMonopoly = new Monopoly();

            // Make him pay a bit of money so we know if it's the same
            // banker being loaded
            Banker.Access().Pay(1000);

            var bankerBeforeSavedBalance = Banker.Access().GetBalance();

            // Need to players on the board for saving and loading correctly
            Board.Access().AddPlayer(new Player());
            Board.Access().AddPlayer(new Player());

            gameOfMonopoly.SaveGame();

            // Set the banker's balance to something random so we know
            // it's then changed when we load the game that has the
            // other banker's balance from the previous save
            Banker.Access().SetBalance(500);

            gameOfMonopoly.LoadGame();

            var bankerAfterLoadedBalance = Banker.Access().GetBalance();

            // The balance should be what it was when we saved it even though we 
            // changed it, this is because the banker we're loading is the one
            // before we changed the balance
            Assert.AreEqual(bankerBeforeSavedBalance, bankerAfterLoadedBalance);
        }
    }
}
