using System;
using System.Collections.Generic;
using System.Linq;

namespace CSStarterTest1.Tester.Stages
{
    internal class StageProcessor<TIn, TOut>
    {
        public StageProcessor(IndentControl indenter, IStage[] stages)
        {
            if (stages.Length == 0)
            {
                throw new ArgumentException("Stage array is empty", nameof(stages));
            }
            if (!DoInputOutputTypesMatchProcessor(stages[0], stages[stages.Length - 1]))
            {
                throw new ArgumentException(
                    "The input type of the first stage or the output " +
                    "type of the last stage don't match that of " +
                    $"{nameof(StageProcessor<TIn, TOut>)}",
                    nameof(stages));
            }
            if (!DoInputOutputTypesMatchBetween(stages))
            {
                throw new ArgumentException(
                    "Not all stages have matching " +
                    "input or output types between each other",
                    nameof(stages));
            }

            _indenter = indenter;
            _stages = stages;
        }

        IndentControl _indenter;
        IStage[] _stages;

        public IEnumerable<TOut> Process(IEnumerable<TIn> input)
        {
            object[] stageInput = ((IEnumerable<object>)input).ToArray();

            foreach (IStage stage in _stages)
            {
                DisplayPreProcessMessages(stage);
                var stageOutputs = stage.Process(stageInput);
                DisplayPostProcessMessages(stage);
                DisplayOutputs(stageOutputs);

                stageInput = stageOutputs
                    .Where(stageData => stageData.Data is not null)
                    .Select(stageData => stageData.Data!)
                    .ToArray();
            }

            return Array.ConvertAll(stageInput, o => (TOut)o);
        }

        private void DisplayOutputs(IEnumerable<IStageOutput> stageOutputs)
        {
            _indenter.IncreaseLevel();
            foreach (var displayInfo in stageOutputs.Select(o => o.GetDisplayInfo()))
            {
                var oldColor = Console.ForegroundColor;
                Console.ForegroundColor = displayInfo.Color;
                Console.WriteLine(displayInfo.Text);
                Console.ForegroundColor = oldColor;
            }
            _indenter.DecreaseLevel();
        }
        // TODO: Move validation of stages into a separate class, like with TestType
        private static bool DoInputOutputTypesMatchProcessor(IStage first, IStage last) =>
            first.In.Equals(typeof(TIn))
            && last.Out.Equals(typeof(TOut))
            ;
        private static bool DoInputOutputTypesMatchBetween(IStage[] stages)
        {
            Type? prev = null;
            for (int i = 0; i < stages.Length; i++)
            {
                if (prev is null)
                {
                    continue;
                }

                var stage = stages[i];
                if (!stage.In.Equals(prev))
                {
                    return false;
                }

                prev = stage.Out;
            }
            return true;
        }
        private static void DisplayPreProcessMessages(IStage stage)
        {
            Console.WriteLine($"{stage.GetMessage(StageMessage.Starting)}...");
        }
        private static void DisplayPostProcessMessages(IStage stage)
        {
            Console.WriteLine($"{stage.GetMessage(StageMessage.Results)}:");
        }
    }
}
