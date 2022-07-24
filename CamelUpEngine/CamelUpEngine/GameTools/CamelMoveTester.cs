using CamelUpEngine.Core.Enums;
using CamelUpEngine.GameObjects;
using System.Collections.Generic;
using System.Linq;

namespace CamelUpEngine.GameTools
{
    public class CamelMoveTester
    {
        public static IReadOnlyCollection<ICamel> Camels { get; } = new Camel[]
        {
            new Camel(Colour.Red),
            new Camel(Colour.Green),
            new Camel(Colour.Black),
            new Camel(Colour.Blue),
            new Camel(Colour.Yellow),
            new Camel(Colour.White),
        };

        private Field field = new Field(0);

        public IReadOnlyCollection<ICamel> FieldCamels => field.Camels;

        public void ResetField() => field = new Field(0);

        public void PutCamelsOnTop(List<ICamel> camels) => field.PutCamels(Convert(camels), StackPutType.Top);

        public void PutCamelsOnBottom(List<ICamel> camels) => field.PutCamels(Convert(camels), StackPutType.Bottom);

        public List<ICamel> TakeOffCamel(Colour colour) => Convert(field.TakeOffCamel(colour));


        private List<ICamel> Convert(List<Camel> camels) => camels.Cast<ICamel>().ToList();
        private List<Camel> Convert(List<ICamel> camels) => camels.Cast<Camel>().ToList();
    }
}
