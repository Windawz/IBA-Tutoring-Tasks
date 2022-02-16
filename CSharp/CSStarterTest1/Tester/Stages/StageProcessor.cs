using System;
using System.Collections.Generic;
using System.Linq;

namespace CSStarterTest1.Tester.Stages
{
    internal class StageProcessor<TIn, TOut>
    {
        public StageProcessor(SimpleConsoleIndenter indenter, IStage[] stages)
        {
            if (!ValidateStages(stages))
            {
                throw new ArgumentException(
                    "The input type of the first stage or the output " +
                    "type of the last stage don't match that of " +
                    $"{nameof(StageProcessor<TIn, TOut>)}",
                    nameof(stages)
                );
            }

            _indenter = indenter;
            _stages = stages;
        }

        SimpleConsoleIndenter _indenter;
        IStage[] _stages;

        public IEnumerable<TOut> Process(IEnumerable<TIn> input)
        {
            object[] stageInput = ((IEnumerable<object>)input).ToArray();

            foreach (IStage stage in _stages)
            {
                PrintStagePreProcessMessages(stage);
                var stageOutputs = stage.Process(stageInput);
                PrintStagePostProcessMessages(stage);
                PrintStageOutputs(stageOutputs);

                stageInput = stageOutputs
                    .Where(stageData => stageData.Data is not null)
                    .Select(stageData => stageData.Data!)
                    .ToArray();
            }

            return Array.ConvertAll(stageInput, o => (TOut)o);
        }

        private void PrintStageOutputs(IEnumerable<IStageOutput> stageOutputs)
        {
            _indenter.Increase();
            foreach (var displayInfo in stageOutputs.Select(o => o.GetDisplayInfo()))
            {
                var oldColor = Console.ForegroundColor;
                Console.ForegroundColor = displayInfo.Color;
                Console.WriteLine(displayInfo.Text);
                Console.ForegroundColor = oldColor;
            }
            _indenter.Decrease();
        }
        private static bool ValidateStages(IStage[] stages) =>
            stages.First().In.Equals(typeof(TIn))
            && stages.Last().Out.Equals(typeof(TOut))
            ;
        private static void PrintStagePreProcessMessages(IStage stage)
        {
            Console.WriteLine($"{stage.GetMessage(StageMessage.Starting)}...");
        }
        
        private static void PrintStagePostProcessMessages(IStage stage)
        {
            Console.WriteLine($"{stage.GetMessage(StageMessage.Results)}:");
        }
    }
}
