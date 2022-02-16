using System;
using System.Collections.Generic;
using System.IO;

using CSStarterTest1.TestUtils;

namespace CSStarterTest1.Tester.Stages
{
    internal class InstantiateTestsStage : Stage<Type, NamedTest>
    {
        public override string GetMessage(StageMessage messageKind) => messageKind switch
        {
            StageMessage.Starting => "Instantiating found test types",
            StageMessage.Results => "Test instantiation results",
            _ => throw new ArgumentOutOfRangeException(nameof(messageKind)),
        };
        public override IStageOutput<NamedTest>[] Process(Type[] input)
        {
            var tests = new List<Output>();
            var loggerProvider = new LoggerProvider();
            var nameGen = new TestLogNameGenerator();

            foreach (Type testType in input)
            {
                string testName = testType.Name;

                TextWriter logger;
                try
                {
                    logger = loggerProvider.GetLogger(nameGen.GetLogName(testName));
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create log file for \"{testName}\"; exception: \"{ex}\"");
                    logger = TextWriter.Null;
                }

                Test test;
                try
                {
                    test = TestInstantiator.Instantiate(testType, logger);
                }
                catch (Exception ex)
                {
                    // Failed to instantiate test, no need for the logger
                    logger.Dispose();
                    tests.Add(new Output(new TestInstantiationInfo(null, testName)));

                    Console.Error.WriteLine($"Failed to instantiate test \"{testName}\"; disposing logger; exception: \"{ex}\"");
                    
                    continue;
                }
                tests.Add(new Output(new TestInstantiationInfo(test, testName)));
            }

            return tests.ToArray();
        }

        private class Output : IStageOutput<NamedTest>
        {
            public Output(TestInstantiationInfo info)
            {
                _testName = info.Name;
                Data = info.Test is null ? null : new NamedTest(info.Test, info.Name);
            }

            private string _testName;

            public NamedTest? Data { get; }

            public StageOutputDisplayInfo GetDisplayInfo() => new()
            {
                Text = _testName,
                Color = Data is null ? ConsoleColor.Red : ConsoleColor.Green,
            };
        }
    }
}
