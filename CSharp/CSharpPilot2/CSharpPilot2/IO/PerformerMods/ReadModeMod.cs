namespace CSharpPilot2.IO.PerformerMods
{
    sealed class ReadModeMod : PerformerMod
    {
        public ReadModeMod(IPerformer performer, InputSourceReadMode readMode) : base(performer) =>
            _readMode = readMode;

        private readonly InputSourceReadMode _readMode;

        public sealed override Input Perform(Request request)
        {
            var old = InputSource.ReadMode;
            InputSource.ReadMode = _readMode;
            var input = base.Perform(request);
            InputSource.ReadMode = old;
            return input;
        }
    }
}
