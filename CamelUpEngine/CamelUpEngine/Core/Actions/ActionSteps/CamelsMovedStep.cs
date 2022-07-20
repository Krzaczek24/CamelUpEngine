using CamelUpEngine.Core.Enums;
using CamelUpEngine.GameObjects;
using System.Collections.Generic;

namespace CamelUpEngine.Core.Actions.ActionSteps
{
    public interface ICamelMovedStep
    {
        public IReadOnlyCollection<ICamel> Camels { get; }
        public IField From { get; }
        public IField To { get; }
        public StackPutType PutType { get; }
    }

    internal class CamelsMovedStep : ActionStep, ICamelMovedStep
    {
        public IReadOnlyCollection<ICamel> Camels { get; }
        public IField From { get; }
        public IField To { get; }
        public StackPutType PutType { get; }

        public CamelsMovedStep(List<Camel> camels, IField from, IField to, StackPutType putType = StackPutType.Top)
        {
            Camels = camels;
            From = from;
            To = to;
            PutType = putType;
        }
    }
}
