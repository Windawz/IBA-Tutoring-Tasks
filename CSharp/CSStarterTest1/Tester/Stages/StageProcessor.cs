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
            var data = (IEnumerable<object>)input;
            foreach (IStage stage in _stages)
            {
                PrintStagePreProcessMessages(stage);
                data = stage.Process(data);
                PrintStagePostProcessMessages(stage);
                PrintData(data, _indenter);
            }
            return (IEnumerable<TOut>)data;
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
        private static void PrintData(IEnumerable<object> data, SimpleConsoleIndenter indenter)
        {
            indenter.Increase();
            if (!data.Any())
            {
                Console.WriteLine("None");
            }
            else
            {
                foreach (object d in data)
                {
                    Console.WriteLine(d);
                }
            }
            indenter.Decrease();
        }
    }
}
