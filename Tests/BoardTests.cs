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
    }
}
