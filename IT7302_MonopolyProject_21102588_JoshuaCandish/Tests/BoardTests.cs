using System.Collections;
using IT7302_MonopolyProject_21102588_JoshuaCandish.Factories;
using NUnit.Framework;

namespace IT7302_MonopolyProject_21102588_JoshuaCandish.Tests
{
    [TestFixture]
    public class BoardTest
    {
        [Test]
        public void test_singleton()
        {
            //Access twice and tests that it is the same object
            Assert.AreSame(Board.Access(), Board.Access());
        }

        [Test]
        public void adding_player()
        {
            Board.Access().ResetBoard();
            Player p = new Player("Go");
            Board.Access().AddPlayer(p);

            //check that the player is equal to a new player with name "Go"
            Assert.AreEqual(Board.Access().GetPlayer("Go"), p);

            // test that the player count is correct
            Assert.AreEqual(1, Board.Access().GetPlayerCount());
        }

        [Test]
        public void adding_property()
        {
            Property p = new Property();
            Board.Access().AddProperty(p);

            Assert.AreEqual(p, Board.Access().GetProperty(0));

            ArrayList props = Board.Access().GetProperties();

            CollectionAssert.Contains(props, p);
        }

        [Test]
        public void getting_players_from_board_should_return_correct_players()
        {
            var newBoard = new Board();

            var josh = new Player("Josh");
            var hubert = new Player("Hubert");
            var jack = new Player("Jack");

            newBoard.AddPlayer(josh);
            newBoard.AddPlayer(hubert);
            newBoard.AddPlayer(jack);

            var players = newBoard.GetPlayers();

            CollectionAssert.Contains(players, josh);
            CollectionAssert.Contains(players, jack);
            CollectionAssert.Contains(players, hubert);
        }

        [Test]
        public void getting_player_when_player_doesnt_exist_should_return_null()
        {
            Assert.IsNull(Board.Access().GetPlayer("playerthatdoesntexist"));
        }

        [Test]
        public void getting_player_by_index_should_return_correct_player()
        {
            var newBoard = new Board();

            var josh = new Player("Josh");
            var hubert = new Player("Hubert");
            
            newBoard.AddPlayer(josh);
            newBoard.AddPlayer(hubert);

            var players = newBoard.GetPlayers();

            Assert.AreSame(newBoard.GetPlayer(0), players[0]);
        }

        [Test]
        public void no_chance_cards_exist()
        {
            Board.Access().ResetBoard();

            var chanceCard = Board.Access().GetChanceCard();

            Assert.IsNull(chanceCard);
        }

        [Test]
        public void getting_a_chance_card()
        {
            var luckFactory = new LuckFactory();
            var chanceCard = luckFactory.create("It's your birthday, collect $40.", false, 40);
            Board.Access().AddChanceCard(chanceCard);

            var cardFromBoard = Board.Access().GetChanceCard();

            // First we should get the card
            Assert.IsNotNull(cardFromBoard);

            // It should be the one we added
            Assert.AreSame(cardFromBoard, chanceCard);

            // Now we have it, should be removed
            Assert.IsNull(Board.Access().GetChanceCard());

            // So they'll be no cards left
            Assert.AreEqual(0, Board.Access().GetChanceCards().Count);
        }

        [Test]
        public void getting_a_community_chest_card()
        {
            var luckFactory = new LuckFactory();
            var communityChestCard = luckFactory.create("It's your birthday, collect $40.", false, 40);
            Board.Access().AddCommunityChestCard(communityChestCard);

            var cardFromBoard = Board.Access().GetCommunityChestCard();

            // First we should get the card
            Assert.IsNotNull(cardFromBoard);

            // It should be the one we added
            Assert.AreSame(cardFromBoard, communityChestCard);

            // Now we have it, should be removed
            Assert.IsNull(Board.Access().GetCommunityChestCard());

            // So they'll be no cards left
            Assert.AreEqual(0, Board.Access().GetCommunityChestCards().Count);
        }

        [Test]
        public void getting_property_by_name()
        {
            var property = new Property("Test");
            Board.Access().AddProperty(property);

            // Should not be null if it get us a property from this name
            Assert.IsNotNull(Board.Access().GetProperty("Test"));
        }
    }
}
