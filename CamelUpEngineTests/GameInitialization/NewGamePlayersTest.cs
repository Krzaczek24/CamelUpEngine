﻿using CamelUpEngine;
using CamelUpEngine.Exceptions;
using CamelUpEngine.GameObjects;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace TestCamelUpEngine.GameInitialization
{
    internal class NewGamePlayersTest
    {
        private static IReadOnlyCollection<string> players = new[] { "Bezimienny", "Diego", "Gorn", "Milten", "Lester" };
        private Game game;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            game = new Game(players);
        }

        [Test]
        public void TestAllPlayersAreSet() => CollectionAssert.AreEqual(players, game.Players.Select(player => player.Name));

        [Test]
        public void TestCurrentPlayerIsSet() => Assert.AreEqual(players.First(), game.Players.First().Name);

        [Test]
        public void TestAllPlayersHaveInitialCoins() => game.Players.ToList().ForEach(player => Assert.AreEqual(IPlayer.InitialCoinsCount, player.Coins));

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
        public void TestValidPlayerName() => Assert.DoesNotThrow(() => new Game(new[] { "Test_One", "T3stTW0", "XDxd" }));
    }
}
