namespace Logic
{
    public static class PlayerExtentions
    {
        public static Player getOponent(Player player)
        {
            return player switch
            {
                Player.White => Player.Black,
                Player.Black => Player.White,
                _ => Player.None,
            };
        }
    }
}
