﻿using CamelUpEngine.Core.Enums;

namespace CamelUpEngine.GameObjects
{
    public interface IAudienceTile
    {
        public IPlayer Owner { get; }
        public AudienceTileSide Side { get; }
    }

    internal class AudienceTile : IAudienceTile
    {
        public IPlayer Owner { get; }
        public AudienceTileSide Side { get; }
        public int MoveValue => (int)Side;

        public AudienceTile(IPlayer owner, AudienceTileSide side)
        {
            Owner = owner;
            Side = side;
        }

        public override string ToString() => $"{Owner.Name}'s {Side.ToString().ToLower()} audience tile";
    }
}
