﻿using CamelUpEngine.GameObjects;

namespace CamelUpEngine.Core.Actions.Steps
{
    public interface IAudienceTilePlacementStep : IActionStep
    {
        public int FieldIndex { get; }
        public IAudienceTile AudienceTile { get; }
    }

    internal class AudienceTilePlacementStep : IAudienceTilePlacementStep
    {
        public int FieldIndex { get; }
        public IAudienceTile AudienceTile { get; }

        public AudienceTilePlacementStep(int fieldIndex, IAudienceTile audienceTile)
        {
            FieldIndex = fieldIndex;
            AudienceTile = audienceTile;
        }
    }
}
