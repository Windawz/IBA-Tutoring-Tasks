using System;
using System.Collections.Generic;
using System.IO;

using CSStarterTest1.TestUtils;

namespace CSStarterTest1.Tester.Stages
{
    internal class InstantiateTestsStage : Stage<Type, Test>
    {
        public override string GetMessage(StageMessage messageKind) => messageKind switch
        {
            StageMessage.Starting => "Instantiating found test types",
            StageMessage.Results => "Instantiated tests",
            _ => throw new ArgumentOutOfRangeException(nameof(messageKind)),
        };
        public override IEnumerable<Test> Process(IEnumerable<Type> input)
        {
            var tests = new List<Test>();
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
                    Console.Error.WriteLine($"Failed to instantiate test \"{testName}\"; disposing logger; exception: \"{ex}\"");
                    logger.Dispose();
                    continue;
                }
                tests.Add(test);
            }

            return tests.ToArray();
        }
    }
}
