﻿using CamelUpEngine.Core;
using CamelUpEngine.Exceptions.AudienceTilesExceptions;
using CamelUpEngine.Exceptions.CamelsExceptions;
using CamelUpEngine.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace CamelUpEngine.GameObjects
{
    public interface IField
    {
        public int Index { get; }
        public IReadOnlyCollection<ICamel> Camels { get; }
        public IAudienceTile AudienceTile { get; }
    }

    internal sealed class Field : IField
    {
        public int Index { get; }

        private List<Camel> camels = new();
        public IReadOnlyCollection<ICamel> Camels => camels;

        public IAudienceTile AudienceTile { get; private set; }
        
        public bool PossibleToPutAudienceTile => Index != 1 && !camels.Any() && AudienceTile == null;

        private Field() { }
        public Field(int number)
        {
            Index = number;
        }

        public List<Camel> TakeOffCamel(Colour colour)
        {
            if (!Camels.Any(c => c.Colour == colour))
            {
                throw new NoCamelOnFieldFoundException(colour, Index);
            }

            var takenCamels = camels.TakeUntil(camel => camel.Colour == colour, true).ToList();
            takenCamels.ForEach(camel => camels.Remove(camel));
            return takenCamels;
        }

        public void PutCamelsOnTop(List<Camel> camels) => this.camels.InsertRange(0, camels);

        public void PutCamelsOnBottom(List<Camel> camels) => this.camels.AddRange(camels);

        public void RemoveAudienceTile() => AudienceTile = null;

        public void PutAudienceTile(AudienceTile audienceTile)
        {
            if (!PossibleToPutAudienceTile)
            {
                throw new PuttingAudienceTileNotAllowedException(Index);
            }

            AudienceTile = audienceTile;
        }
    }
}
