namespace CSharpPilot2.IO {
    sealed class RetryingPerformerMod : PerformerMod {
        public RetryingPerformerMod(IPerformer performer) : base(performer) { }

        public override Input Perform(Request request) {
            Input input;
            Request loopedRequest = request;
            if (loopedRequest.Condition is not null) {
                while (true) {
                    input = base.Perform(loopedRequest);
                    if (loopedRequest.Condition(input)) {
                        break;
                    } else {
                        loopedRequest = loopedRequest with {
                            Before = RemoveUnconditionalOutput(loopedRequest.Before),
                            Anyway = RemoveUnconditionalOutput(loopedRequest.Anyway),
                        };
                    }
                }
            } else {
                input = base.Perform(request);
            }
            
            return input;
        }

        // to avoid output overlapping if request isn't satisfied
        private static TSayDoBase? RemoveUnconditionalOutput<TSayDoBase>(TSayDoBase? sayDo) where TSayDoBase : SayDoBase =>
            sayDo is null ? sayDo : sayDo with {
                Output = null,
            };
    }
}
