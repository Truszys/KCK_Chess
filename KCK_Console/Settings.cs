using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public enum CONTROL
    {
        NOTATION,
        TEXT,
        BOARD
    }
    public enum TIME
    {
        ONE_PLUS = 60 + 5,
        ONE = 60,
        FIVE_PLUS = 5 * 60 + 5,
        FIVE = 5 * 60,
        TEN_PLUS = 10 * 60 + 10,
        TEN = 10 * 60,
        THIRTY_PLUS = 30 * 60 + 30,
        THIRTY = 30 * 60
    }
    public class Settings
    {
        public CONTROL control { get; set; } = CONTROL.BOARD;
        public TIME time { get; set; } = TIME.ONE;
    }
}
