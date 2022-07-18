using CamelUpEngine;
using CamelUpEngine.Exceptions.PlayersExceptions;
using NUnit.Framework;
using System.Linq;

namespace TestCamelUpEngine.NewGame
{
    internal class NewGamePlayersTest : BaseClass
    {
        [Test]
        public void TestAllPlayersAreSet() => CollectionAssert.AreEqual(players, game.Players.Select(player => player.Name));

        [Test]
        public void TestCurrentPlayerIsSet() => Assert.AreEqual(players.First(), game.Players.First().Name);

        [Test]
        public void TestAllPlayersHaveInitialCoins() => game.Players.ToList().ForEach(player => Assert.AreEqual(IPlayer.INITIAL_COINS_COUNT, player.Coins));

        [Test]
        public void TestAllPlayersAreUnique() => CollectionAssert.AllItemsAreUnique(game.Players.Select(player => player.Name));

        [Test]
        public void TestPlayerNameExceptions() => Assert.Multiple(() =>
        {
            Assert.Throws<TooFewPlayersException>(() => new Game(null));
            Assert.Throws<TooFewPlayersException>(() => new Game(new string[] { }));
            Assert.Throws<TooFewPlayersException>(() => new Game(new[] { "One", "Two" }));
            Assert.Throws<TooManyPlayersException>(() => new Game(new[] { "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine" }));
            Assert.Throws<NotUniquePlayersNameException>(() => new Game(new[] { "NotUnique", "Unique", "nOTuNIQUE" }));
        });

        [Test]
        public void TestInvalidPlayerName([Values("_", "A", "A ", "ABC%")] string playerName) => Assert.Throws<InvalidPlayerNameException>(() => new Game(new[] { "One", "Two", playerName }));

        [Test]
        public void TestValidPlayerName() => Assert.DoesNotThrow(() => new Game(new[] { "Test_One", "T3st TW0", "__", "XD" }));
    }
}
