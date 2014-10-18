using IT7302_MonopolyProject_21102588_JoshuaCandish.Factories;
using NUnit.Framework;

namespace IT7302_MonopolyProject_21102588_JoshuaCandish.Tests
{
    [TestFixture]
    public class LuckTests
    {
        private Luck _card;
        private readonly LuckFactory _luckFactory = new LuckFactory();
        private Player _player;
        private Monopoly _gameOfMonoploy;

        [TestFixtureSetUp]
        public void SetUp()
        {
            _gameOfMonoploy = new Monopoly();
            _gameOfMonoploy.SetUpProperties();
            _player = new Player("Josh");
        }

        [Test]
        public void go_to_jail_card()
        {
            _card = _luckFactory.create("Chance: go to jail", false, 0);
            _card.LandOn(ref _player);
            var playersLocation = Board.Access().GetProperty(_player.GetLocation());
            var jail = Board.Access().GetProperty(10);

            Assert.AreEqual(playersLocation, jail);
            Assert.IsTrue(_player.IsInJail);
        }

        [Test]
        public void get_out_of_jail_free_card()
        {
            _card = _luckFactory.create("Community Chest: get out of jail free", false, 0);
            _card.LandOn(ref _player);

            Assert.AreEqual(_player.GetOutOfJailCardCount, 1);
        }
    }
}
