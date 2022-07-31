#if DEBUG
using CamelUpEngine.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CamelUpEngine.Helpers.TestHelpers
{
    public static class FieldHelper
    {
        public static IReadOnlyCollection<IAvailableField> GetAllFields(Game game) => game.Fields.Select(field => new AvailableField(field.Index, Guid.Empty)).ToList();
    }
}
#endif