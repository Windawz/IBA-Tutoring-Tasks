using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using CSStarterTest1.TestUtils;

namespace CSStarterTest1.Tester.Stages.ConcreteStages
{
    internal class InstantiateTestsStage : Stage<TestType, Test>
    {
        public override string GetMessage(StageMessage messageKind) => messageKind switch
        {
            StageMessage.Starting => "Instantiating found test types",
            StageMessage.Results => "Test instantiation results",
            _ => base.GetMessage(messageKind),
        };
        public override IStageOutput<Test>[] Process(TestType[] input)
        {
            var provider = new LoggerProvider();
            var generator = new LogFileNameProvider();

            return input
                .Select(type => (Name: type.Type.Name, Test: TryInstantiateTest(type, provider, generator)))
                .Select(pair => new Output(pair.Test, pair.Name))
                .ToArray();
        }

        private static TextWriter GetLoggerOrLogOnFail(string testName, LoggerProvider provider, LogFileNameProvider generator)
        {
            TextWriter logger;
            
            try
            {
                logger = provider.GetLogger(generator.GetName(testName));
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create log file for \"{testName}\"; exception: \"{ex}\"");
                logger = TextWriter.Null;
            }

            return logger;
        }
        private static Test? TryInstantiateTestTypeOrLogOnFail(TestType testType, TextWriter logger)
        {
            Test? test;

            try
            {
                test = testType.Instantiate(logger);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to instantiate test \"{testType.Type.Name}\"; disposing logger; exception: \"{ex}\"");
                test = null;
            }

            return test;
        }
        private static Test? TryInstantiateTest(TestType testType, LoggerProvider provider, LogFileNameProvider generator)
        {
            string testName = testType.Type.Name;
            TextWriter logger = GetLoggerOrLogOnFail(testName, provider, generator);
            Test? test = TryInstantiateTestTypeOrLogOnFail(testType, logger);

            if (test is null)
            {
                // Failed to instantiate test, no need for the logger
                logger.Dispose();
            }

            return test;
        }

        private class Output : IStageOutput<Test>
        {
            public Output(Test? test, string testName)
            {
                _testName = testName;
                Data = test;
            }

            private string _testName;

            public Test? Data { get; }

            public StageOutputDisplayInfo GetDisplayInfo() => new()
            {
                Text = _testName,
                Color = Data is null ? ConsoleColor.Red : ConsoleColor.Green,
            };
        }
    }
}
