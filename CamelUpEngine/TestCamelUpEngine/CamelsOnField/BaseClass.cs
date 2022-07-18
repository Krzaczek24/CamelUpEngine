using CamelUpEngine;
using NUnit.Framework;
using System.Collections.Generic;

namespace TestCamelUpEngine.CamelsOnField
{
    internal abstract class BaseClass
    {
        protected static IReadOnlyCollection<Camel> Camels { get; } = new Camel[]
        {
            new Camel(Colour.Red),
            new Camel(Colour.Green),
            new Camel(Colour.Black),
            new Camel(Colour.Blue),
            new Camel(Colour.Yellow),
            new Camel(Colour.White),
        };

        protected Field field;

        [SetUp]
        public virtual void Setup()
        {
            field = new Field(0);
        }
    }
}
