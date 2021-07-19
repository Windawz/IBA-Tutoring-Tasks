namespace Game {
    class InputInfo {
        public InputInfo(string text, double seconds, bool isValid) =>
            (Text, Seconds, IsValid) = (text, seconds, isValid);

        public string Text { get; }
        public double Seconds { get; }
        public bool IsValid { get; }
    }
}