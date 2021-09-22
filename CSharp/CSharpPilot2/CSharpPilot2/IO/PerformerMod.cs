namespace CSharpPilot2.IO
{
    abstract class PerformerMod : IPerformer
    {
        protected PerformerMod(IPerformer performer) =>
            _performer = performer;

        readonly IPerformer _performer;

        public IInputSource InputSource
        {
            get => _performer.InputSource;
            set => _performer.InputSource = value;
        }
        public IOutputTarget OutputTarget
        {
            get => _performer.OutputTarget;
            set => _performer.OutputTarget = value;
        }

        public virtual Input Perform(Request request) =>
            _performer.Perform(request);
    }
}
