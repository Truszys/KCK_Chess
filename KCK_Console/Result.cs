using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public class Result
    {
        public Player Winner { get; }
        public EndReason Reason { get; }
        public Result(Player winner, EndReason reason)
        {
            Winner = winner;
            Reason = reason;
        }
        public static Result Win(Player player)
        {
            return new(player, EndReason.Checkmate);
        }
        public static Result LoseTime(Player player)
        {
            return new(PlayerExtentions.getOponent(player), EndReason.Time);
        }
        public static Result Draw(EndReason reason)
        {
            return new(Player.None, reason);
        }
    }
}
