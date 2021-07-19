namespace Game {
    class Player {
        public Player(int index, bool isDefeated) => (Index, IsDefeated) = (index, isDefeated);
        public Player(Player player) : this(player.Index, player.IsDefeated) { }

        public int Index { get; }
        public bool IsDefeated { get; }
    }
}