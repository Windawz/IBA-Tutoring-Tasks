using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using CSStarterTest1.TestUtils;

namespace CSStarterTest1.Tester.Stages.ConcreteStages
{
    internal class InstantiateTestsStage : Stage<Type, Test>
    {
        public override string GetMessage(StageMessage messageKind) => messageKind switch
        {
            StageMessage.Starting => "Instantiating found test types",
            StageMessage.Results => "Test instantiation results",
            _ => base.GetMessage(messageKind),
        };
        public override IStageOutput<Test>[] Process(Type[] input)
        {
            var provider = new LoggerProvider();
            var generator = new TestLogNameGenerator();

            return input
                .Select(type => (Name: type.Name, Test: TryInstantiateTest(type, provider, generator)))
                .Select(pair => new Output(pair.Test, pair.Name))
                .ToArray();
        }

        private static TextWriter GetLoggerOrLogOnFail(string testName, LoggerProvider provider, TestLogNameGenerator generator)
        {
            TextWriter logger;
            
            try
            {
                logger = provider.GetLogger(generator.GetLogName(testName));
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create log file for \"{testName}\"; exception: \"{ex}\"");
                logger = TextWriter.Null;
            }

            return logger;
        }
        private static Test? TryInstantiateTestTypeOrLogOnFail(Type testType, TextWriter logger)
        {
            Test? test;

            try
            {
                test = TestInstantiator.Instantiate(testType, logger);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to instantiate test \"{testType.Name}\"; disposing logger; exception: \"{ex}\"");
                test = null;
            }

            return test;
        }
        private static Test? TryInstantiateTest(Type testType, LoggerProvider provider, TestLogNameGenerator generator)
        {
            string testName = testType.Name;
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
