﻿using System;

namespace CamelUpEngine.GameObjects
{
    public interface IAvailableField
    {
        public int Index { get; }
        internal Guid DrawGuid { get; }
    }

    internal sealed class AvailableField : IAvailableField
    {
        public int Index { get; }
        public Guid DrawGuid { get; }

        public AvailableField(int index, Guid drawGuid)
        {
            Index = index;
            DrawGuid = drawGuid;
        }

        public override string ToString() => $"{Index}. field";
    }
}
