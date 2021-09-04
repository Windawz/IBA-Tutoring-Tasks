namespace CSharpPilot2.IO {
    abstract class PerformerMod : IPerformer {
        protected PerformerMod(IPerformer performer) =>
            _performer = performer;

        private readonly IPerformer _performer;

        public virtual Input Perform(Request request) =>
            _performer.Perform(request);
    }
}
