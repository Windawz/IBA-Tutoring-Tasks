namespace CSharpPilot2.IO.PerformerMods
{
    sealed class RetryingMod : PerformerMod
    {
        public RetryingMod(IPerformer performer) : base(performer) { }

        public override Input Perform(Request request)
        {
            Input input;
            Request loopedRequest = request;
            if (loopedRequest.Condition is not null)
            {
                while (true)
                {
                    input = base.Perform(loopedRequest);
                    if (loopedRequest.Condition(input))
                    {
                        break;
                    }
                    else
                    {
                        loopedRequest = loopedRequest with
                        {
                            Before = RemoveUnconditionalOutput(loopedRequest.Before),
                            Anyway = RemoveUnconditionalOutput(loopedRequest.Anyway),
                        };
                    }
                }
            }
            else
            {
                input = base.Perform(request);
            }

            return input;
        }

        // to avoid output overlapping if request isn't satisfied
        static TSayDoBase? RemoveUnconditionalOutput<TSayDoBase>(TSayDoBase? sayDo) where TSayDoBase : SayDoBase =>
            sayDo is null ? sayDo : sayDo with
            {
                Output = null,
            };
    }
}
