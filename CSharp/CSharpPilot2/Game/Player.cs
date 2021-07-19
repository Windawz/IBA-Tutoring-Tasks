namespace Game {
    class Player {
        public Player(int index, bool defeated) => (Index, Defeated) = (index, defeated);
        public Player(Player player) : this(player.Index, player.Defeated) { }

        public int Index { get; }
        public bool Defeated { get; }
    }
}