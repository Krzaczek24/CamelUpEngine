using CamelUpEngine.Core.Actions.Events;
using CamelUpEngine.Core.Enums;
using CamelUpEngine.Exceptions;
using CamelUpEngine.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CamelUpEngine.GameTools
{
    public class AudienceTilesManager
    {
        private readonly List<Field> fields;

        private Func<Guid> GenerateGuid { get; }
        internal Guid DrawGuid { get; private set; }

        internal AudienceTilesManager(IEnumerable<Field> fields, Func<Guid> guidGenerationFunction = null)
        {
            this.fields = fields.ToList();
            DrawGuid = (GenerateGuid = guidGenerationFunction ?? Guid.NewGuid)();
        }

        public static IAudienceTilePlacementEvent TestPlaceAudienceTile(IPlayer player, IEnumerable<IField> fields, IAvailableField targetField, AudienceTileSide tileSide, 
            out IAudienceTileRemovementEvent removementEvent, Func<Guid> guidGenerationFunction = null)
        {
            List<Field> copiedFields = fields.GetDeepCopy();
            AudienceTilesManager manager = new(copiedFields, guidGenerationFunction);
            return manager.PlaceAudienceTile(player, targetField, tileSide, out removementEvent);
        }

        internal IReadOnlyCollection<IAvailableField> GetAudienceTileAvailableFields()
        {
            List<IAvailableField> availableFields = new();

            for (int fieldIndex = 1; fieldIndex < fields.Count(); fieldIndex++)
            {
                IField currentField = fields.ElementAt(fieldIndex);
                if (currentField.AudienceTile != null)
                {
                    fieldIndex++;
                    continue;
                }

                IField previousField = fields.ElementAt(fieldIndex - 1);
                IField nextField = fields.ElementAt((fieldIndex + 1) % fields.Count());
                if (previousField.AudienceTile == null
                && !currentField.Camels.Any()
                && nextField.AudienceTile == null)
                {
                    availableFields.Add(new AvailableField(currentField.Index, DrawGuid));
                }
            }

            return availableFields;
        }

        public IAudienceTilePlacementEvent PlaceAudienceTile(IPlayer player, IAvailableField availableField, AudienceTileSide audienceTileSide, out IAudienceTileRemovementEvent removePreviousTileEvent)
        {
            if (DrawGuid != availableField.DrawGuid)
            {
                throw new FieldExpiredAvailabilityException();
            }

            IField previousField = fields.SingleOrDefault(field => field.Index == availableField.Index - 1);
            IField nextField = fields.SingleOrDefault(field => field.Index == availableField.Index + 1);
            if (previousField?.AudienceTile != null || nextField?.AudienceTile != null)
            {
                throw new NearbyFieldOccupiedByAudienceTileException(availableField.Index);
            }

            removePreviousTileEvent = null;
            Field previousAudienceTileField = fields.SingleOrDefault(field => field.AudienceTile?.Owner == player);
            if (previousAudienceTileField != null)
            {
                removePreviousTileEvent = new AudienceTileRemovementEvent(previousAudienceTileField.Index, previousAudienceTileField.AudienceTile);
                previousAudienceTileField.RemoveAudienceTile();
            }
            Field targetField = fields.Single(field => field.Index == availableField.Index);
            targetField.PutAudienceTile(new AudienceTile(player, audienceTileSide));
            IAudienceTilePlacementEvent placementEvent = new AudienceTilePlacementEvent(targetField.Index, targetField.AudienceTile);

            DrawGuid = GenerateGuid();
            return placementEvent;
        }
    }
}
