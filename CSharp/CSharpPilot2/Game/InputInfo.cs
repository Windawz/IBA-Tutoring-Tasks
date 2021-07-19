namespace Game {
    class InputInfo {
        public InputInfo(string text, double time, bool valid) =>
            (Text, Time, Valid) = (text, time, valid);

        public string Text { get; }
        // In seconds.
        public double Time { get; }
        public bool Valid { get; }
    }
}