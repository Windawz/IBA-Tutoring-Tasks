namespace Game {
    class InputInfo {
        public InputInfo(string text, double seconds, bool valid) =>
            (Text, Seconds, Valid) = (text, seconds, valid);

        public string Text { get; }
        public double Seconds { get; }
        public bool Valid { get; }
    }
}