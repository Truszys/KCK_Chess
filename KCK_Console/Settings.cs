using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public enum CONTROL
    {
        NOTATION = 0,
        TEXT = 1,
        BOARD = 2,
    }
    public enum TIME
    {
        ONE = 60,
        ONE_PLUS = 60 + 5,
        FIVE = 5 * 60,
        FIVE_PLUS = 5 * 60 + 5,
        TEN = 10 * 60,
        TEN_PLUS = 10 * 60 + 10,
        THIRTY = 30 * 60,
        THIRTY_PLUS = 30 * 60 + 30,
    }
    public class Settings
    {
        public CONTROL control { get; set; } = CONTROL.BOARD;
        public TIME time { get; set; } = TIME.ONE;
    }
}
