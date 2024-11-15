using Logic;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace UIConsole;
class Program
{
    static void Main(string[] args)
    {
        MaximizeWindow();
        Console.BackgroundColor = ConsoleColor.Black;
        Console.ForegroundColor = ConsoleColor.White;
        Console.Clear();
        Console.CursorVisible = false;
        MainMenu();
    }
    public static void MainMenu()
    {
        var settings = new Settings();
        var selectedOption = 0;
        List<String> options = new()
        {
            "Start new game",
            "Start game from notation",
            "Game settings"
        };
        while(true)
        {
            var menu = new Menu("Chess game", options, selectedOption);
            selectedOption = menu.HandleMenu();
            switch (selectedOption)
            {
                case 0:
                    Game(new GameState(Board.CreateNew(), Player.White, settings));
                    break;
                case 1:
                    break;
                case 2:
                    settings = Settings(settings);
                    break;
                default:
                    return;
            }
        }
    }
    public static Settings Settings(Settings settings)
    {
        List<String> options = new()
        {
            "Controls",
            "Time control",
        };
        while(true)
        {
            var menu = new Menu("Settings", options);
            switch (menu.HandleMenu())
            {
                case 0:
                    settings.control = SettingsControl(settings.control);
                    break;
                case 1:
                    settings.time = SettingsTime(settings.time);
                    break;
                default:
                    return settings;
            }
        }

    }
    public static CONTROL SettingsControl(CONTROL oldControl)
    {
        List<String> options = new()
        {
            "Notation",
            "Move input",
            "Board movement",
        };
        var menu = new Menu("Controls", options, (int) oldControl);
        switch (menu.HandleMenu())
        {
            case 0:
                return CONTROL.NOTATION;
            case 1:
                return CONTROL.TEXT;
            case 2:
                return CONTROL.BOARD;
            default:
                return oldControl;
        }
    }
    public static TIME SettingsTime(TIME oldTime)
    {
        List<String> options = new()
        {
            "1",
            "1 + 5",
            "5",
            "5 + 5",
            "10",
            "10 + 10",
            "30",
            "30 + 30",
        };
        var start = 0;
        switch (oldTime)
        {
            case TIME.ONE:
                start = 0;
                break;
            case TIME.ONE_PLUS:
                start = 1;
                break;
            case TIME.FIVE:
                start = 2;
                break;
            case TIME.FIVE_PLUS:
                start = 3;
                break;
            case TIME.TEN:
                start = 4;
                break;
            case TIME.TEN_PLUS:
                start = 5;
                break;
            case TIME.THIRTY:
                start = 6;
                break;
            case TIME.THIRTY_PLUS:
                start = 7;
                break;
        }
        var menu = new Menu("Time control", options, start);
        switch (menu.HandleMenu())
        {
            case 0:
                return TIME.ONE;
            case 1:
                return TIME.ONE_PLUS;
            case 2:
                return TIME.FIVE;
            case 3:
                return TIME.FIVE_PLUS;
            case 4:
                return TIME.TEN;
            case 5:
                return TIME.TEN_PLUS;
            case 6:
                return TIME.THIRTY_PLUS;
            case 7:
                return TIME.THIRTY_PLUS;
            default:
                return oldTime;
        }
    }

    public static void Game(GameState state)
    {
        Console.ResetColor();
        WriteBoard();
        UpdateScoreBoard(state);
        FillBoard(state);
        state.StartTimer();
        while(!state.IsGameOver())
        {
            HideCheck(state.Board);
            if (state.Board.IsInCheck(state.CurrentPlayer))
            {
                ShowCheck(state.Board);
            }
            var move = AskForMove(state);
            if (move != null)
            {
                MakeMove(state, move);
            }
            UpdateScoreBoard(state);
        }
        RemoveCursor();
        HideCheck(state.Board);
        if (state.Board.IsInCheck(state.CurrentPlayer))
        {
            ShowCheck(state.Board);
        }
        ShowWinner(state);
        while (true)
        {
            var key = Console.ReadKey(intercept: true).Key;
            if (key == ConsoleKey.Escape || key == ConsoleKey.Enter)
            {
                return;
            }
        }
    }
    private static Move? AskForMove(GameState state)
    {
        Console.BackgroundColor = ConsoleColor.Black;
        Console.ForegroundColor = ConsoleColor.White;
        Console.SetCursorPosition(0, 41);
        switch (state.Settings.control)
        {
            case CONTROL.TEXT:
                char temp = '\0';
                Position from;
                Position to;
                IEnumerable<Move> posibleMoves;
                while(!state.IsGameOver())
                {
                    int row = -1;
                    int col = -1;
                    Console.SetCursorPosition(0, 41);
                    Console.Write("Enter position of piece you want to move:                           ");
                    while (!state.IsGameOver())
                    {
                        UpdateScoreBoard(state);
                        RemovePosibleMoves();
                        Console.SetCursorPosition(42, 41);
                        temp = Console.ReadKey().KeyChar;
                        if (temp >= 'a' && temp <= 'h')
                        {
                            col = temp - 'a';
                            break;
                        }
                        if (temp == 27 || temp == 8)
                        {
                            col = 0;
                            break;
                        }
                        Console.SetCursorPosition(42, 41);
                        Console.Write(" ");
                    }
                    if (state.IsGameOver())
                    {
                        break;
                    }
                    if (temp == 27 || temp == 8)
                    {
                        continue;
                    }
                    while (!state.IsGameOver())
                    {
                        UpdateScoreBoard(state);
                        Console.SetCursorPosition(43, 41);
                        temp = Console.ReadKey().KeyChar;
                        if (temp >= '1' && temp <= '8')
                        {
                            row = temp - '1';
                            break;
                        }
                        if (temp == 27 || temp == 8)
                        {
                            row = 0;
                            break;
                        }
                        Console.SetCursorPosition(43, 41);
                        Console.Write(" ");
                        Console.CursorLeft -= 1;
                    }
                    if (state.IsGameOver())
                    {
                        break;
                    }
                    if (temp == 27 || temp == 8)
                    {
                        continue;
                    }
                    from = new Position(row, col);
                    if (!state.Board.IsEmpty(from) && 
                        state.Board[from] != null && 
                        state.Board[from].Color == state.CurrentPlayer)
                    {
                        posibleMoves = state.LegalMovesForPiece(from);
                        if (posibleMoves.Count() == 0)
                        {
                            ShowError("Piece has no posible moves");
                            continue;
                        }
                    }
                    else
                    {
                        ShowError("Wrong piece was selected");
                        continue;
                    }
                    ShowPosibleMoves(posibleMoves);
                    HideError();
                    while (!state.IsGameOver())
                    {
                        Console.SetCursorPosition(0, 41);
                        Console.Write("Enter position where you want to move:                            ");
                        Console.CursorLeft = 41;
                        while (!state.IsGameOver())
                        {
                            UpdateScoreBoard(state);
                            Console.SetCursorPosition(42, 41);
                            temp = Console.ReadKey().KeyChar;
                            if (temp >= 'a' && temp <= 'h')
                            {
                                col = temp - 'a';
                                break;
                            }
                            if (temp == 27 || temp == 8)
                            {
                                col = 0;
                                break;
                            }
                            Console.SetCursorPosition(42, 41);
                            Console.Write(" ");
                            Console.CursorLeft -= 1;
                        }
                        if (state.IsGameOver())
                        {
                            break;
                        }
                        if (temp == 27 || temp == 8)
                        {
                            break;
                        }
                        while (!state.IsGameOver())
                        {
                            UpdateScoreBoard(state);
                            Console.SetCursorPosition(43, 41);
                            temp = Console.ReadKey().KeyChar;
                            if (temp >= '1' && temp <= '8')
                            {
                                row = temp - '1';
                                break;
                            }
                            if (temp == 27 || temp == 8)
                            {
                                row = 0;
                                break;
                            }
                            Console.SetCursorPosition(43, 41);
                            Console.Write(" ");
                            Console.CursorLeft -= 1;
                        }
                        if (state.IsGameOver())
                        {
                            break;
                        }
                        if (temp == 27 || temp == 8)
                        {
                            continue;
                        }
                        to = new Position(row, col);
                        if (posibleMoves.Any(move => move.To == to))
                        {
                            RemovePosibleMoves();
                            HideError();
                            var move = posibleMoves.Where(move => move.To == to).First();
                            return move;
                        }
                        else
                        {
                            ShowError("Move is imposible (" + (char)(from.Column + 'a') + (char)(from.Row + '1') + "->" + (char)(to.Column + 'a') + (char)(to.Row + '1') + ")");
                        }
                    }
                }
                break;
            case CONTROL.NOTATION:
                Console.WriteLine("Enter your move: ");
                break;
            case CONTROL.BOARD:
                Console.CursorVisible = false;
                var cursorPosition = new Position(state.CurrentPlayer == Player.White ? 0 : 7, 3);
                ConsoleKey tempBoard;
                while (!state.IsGameOver())
                {
                    UpdateScoreBoard(state);
                    RemovePosibleMoves();
                    ShowCursor(cursorPosition);
                    tempBoard = Console.ReadKey(intercept: true).Key;
                    switch (tempBoard)
                    {
                        case ConsoleKey.D:
                        case ConsoleKey.RightArrow:
                            if (Board.IsInside(cursorPosition + Direction.East))
                            {
                                cursorPosition += Direction.East;
                            }
                            break;
                        case ConsoleKey.A:
                        case ConsoleKey.LeftArrow:
                            if (Board.IsInside(cursorPosition + Direction.West))
                            {
                                cursorPosition += Direction.West;
                            }
                            break;
                        case ConsoleKey.W:
                        case ConsoleKey.UpArrow:
                            if (Board.IsInside(cursorPosition + Direction.North))
                            {
                                cursorPosition += Direction.North;
                            }
                            break;
                        case ConsoleKey.S:
                        case ConsoleKey.DownArrow:
                            if (Board.IsInside(cursorPosition + Direction.South))
                            {
                                cursorPosition += Direction.South;
                            }
                            break;
                        case ConsoleKey.Enter:
                            HideError();
                            Position fromBoard = cursorPosition.Copy();
                            if (!state.Board.IsEmpty(fromBoard) &&
                                state.Board[fromBoard] != null &&
                                state.Board[fromBoard].Color == state.CurrentPlayer)
                            {
                                posibleMoves = state.LegalMovesForPiece(fromBoard);
                                if (posibleMoves.Count() == 0)
                                {
                                    ShowError("Piece has no posible moves");
                                    continue;
                                }
                            }
                            else
                            {
                                ShowError("Wrong piece was selected");
                                continue;
                            }
                            ShowPosibleMoves(posibleMoves);
                            var exit = false;
                            while (!state.IsGameOver())
                            {
                                UpdateScoreBoard(state);
                                ShowCursor(cursorPosition);
                                tempBoard = Console.ReadKey(intercept: true).Key;
                                switch (tempBoard)
                                {
                                    case ConsoleKey.D:
                                    case ConsoleKey.RightArrow:
                                        if(Board.IsInside(cursorPosition + Direction.East))
                                        {
                                            cursorPosition += Direction.East;
                                        }
                                        break;
                                    case ConsoleKey.A:
                                    case ConsoleKey.LeftArrow:
                                        if (Board.IsInside(cursorPosition + Direction.West))
                                        {
                                            cursorPosition += Direction.West;
                                        }
                                        break;
                                    case ConsoleKey.W:
                                    case ConsoleKey.UpArrow:
                                        if (Board.IsInside(cursorPosition + Direction.North))
                                        {
                                            cursorPosition += Direction.North;
                                        }
                                        break;
                                    case ConsoleKey.S:
                                    case ConsoleKey.DownArrow:
                                        if (Board.IsInside(cursorPosition + Direction.South))
                                        {
                                            cursorPosition += Direction.South;
                                        }
                                        break;
                                    case ConsoleKey.Enter:
                                        HideError();
                                        Position toBoard = cursorPosition.Copy();
                                        if (posibleMoves.Any(move => move.To == toBoard))
                                        {
                                            RemovePosibleMoves();
                                            HideError();
                                            var move = posibleMoves.Where(move => move.To == toBoard).First();
                                            return move;
                                        }
                                        else
                                        {
                                            ShowError("Move is imposible (" + (char)(fromBoard.Column + 'a') + (char)(fromBoard.Row + '1') + "->" + (char)(toBoard.Column + 'a') + (char)(toBoard.Row + '1') + ")");
                                        }
                                        break;
                                    case ConsoleKey.Escape:
                                    case ConsoleKey.Backspace:
                                        exit = true;
                                        break;
                                    default:
                                        break;
                                }
                                if (exit)
                                {
                                    break;
                                }
                            }
                            break;
                    default:
                        break;
                    }
                }
                break;
            default:
                break;
        }
        RemovePosibleMoves();
        HideError();
        RemoveCursor();
        return null;
    }
    private static void MakeMove(GameState state, Move move)
    {
        ClearTile(move.From);
        PutPiece(state.Board[move.From], move.To);
        state.MakeMove(move);
    }
    private static void ShowCheck(Board board)
    {
        foreach (var position in board.GetKings())
        {
            if (board.IsInCheck(board[position].Color))
            {
                var coords = GetSquereCoordinates(position);
                Console.SetCursorPosition(coords.Item2, coords.Item1);
                Console.BackgroundColor = ConsoleColor.Red;
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(board[position].Mark);
            }
        }
    }
    private static void HideCheck(Board board)
    {
        foreach (var position in board.GetKings())
        {
            var coords = GetSquereCoordinates(position);
            Console.SetCursorPosition(coords.Item2, coords.Item1);
            Console.BackgroundColor = board[position].Color == Player.Black ? ConsoleColor.Black : ConsoleColor.White;
            Console.ForegroundColor = board[position].Color == Player.Black ? ConsoleColor.White : ConsoleColor.Black;
            Console.Write(board[position].Mark);
        }
    }
    private static void ShowError(string message)
    {
        HideError();
        var oldBG = Console.BackgroundColor;
        var oldFG = Console.ForegroundColor;
        var oldCursorLeft = Console.CursorLeft;
        var oldCursorTop = Console.CursorTop;
        Console.ForegroundColor = ConsoleColor.White;
        Console.BackgroundColor = ConsoleColor.Red;
        Console.SetCursorPosition(0, 42);
        Console.Write(message);
        Console.SetCursorPosition(oldCursorLeft, oldCursorTop);
        Console.ForegroundColor = oldFG;
        Console.BackgroundColor = oldBG;
    }
    private static void HideError()
    {
        var oldBG = Console.BackgroundColor;
        var oldCursorLeft = Console.CursorLeft;
        var oldCursorTop = Console.CursorTop;
        Console.BackgroundColor = ConsoleColor.Black;
        Console.SetCursorPosition(0, 42);
        for (int i = 0; i < Console.WindowWidth; i++)
        {
            Console.Write(" ");
        }
        Console.SetCursorPosition(oldCursorLeft, oldCursorTop);
        Console.BackgroundColor = oldBG;
    }

    private static void ShowPosibleMoves(IEnumerable<Move> moves)
    {
        var oldBG = Console.BackgroundColor;
        Console.BackgroundColor = ConsoleColor.Green;
        var coords = GetSquereCoordinates(moves.First().From.Row, moves.First().From.Column);
        for (int i = -4; i <= 4; i += 8)
        {
            for (int j = -1; j <= 1; j += 2)
            {
                Console.SetCursorPosition(coords.Item2 + i, coords.Item1 + j);
                Console.Write(" ");
            }
        }
        Console.BackgroundColor = ConsoleColor.Yellow;
        foreach (var move in moves)
        {
            coords = GetSquereCoordinates(move.To.Row, move.To.Column);
            for(int i = -4; i <= 4; i += 8)
            {
                for (int j = -1; j <= 1; j += 2)
                {
                    Console.SetCursorPosition(coords.Item2 + i, coords.Item1 + j);
                    Console.Write(" ");
                }
            }
        }
        Console.BackgroundColor = oldBG;
    }
    private static void RemoveCursor()
    {
        var oldBG = Console.BackgroundColor;
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                var position = new Position(i, j);
                var coords = GetSquereCoordinates(i, j);
                for (int k = -4; k <= 4; k += 8)
                {
                    for (int l = -1; l <= 1; l += 2)
                    {
                        Console.BackgroundColor = position.SquareColor() == Player.Black ? ConsoleColor.Black : ConsoleColor.White;
                        Console.SetCursorPosition(coords.Item2 - 1, coords.Item1 - 1);
                        Console.Write("   ");
                        Console.SetCursorPosition(coords.Item2 - 4, coords.Item1);
                        Console.Write("   ");
                        Console.SetCursorPosition(coords.Item2 + 2, coords.Item1);
                        Console.Write("   ");
                        Console.SetCursorPosition(coords.Item2 - 1, coords.Item1 + 1);
                        Console.Write("   ");
                    }
                }
            }
        }
        Console.BackgroundColor = oldBG;
    }
    private static void ShowCursor(Position position)
    {
        RemoveCursor();
        var oldBG = Console.BackgroundColor;
        Console.BackgroundColor = ConsoleColor.Red;
        var coords = GetSquereCoordinates(position.Row, position.Column);
        Console.SetCursorPosition(coords.Item2 - 1, coords.Item1 - 1);
        Console.Write("   ");
        Console.SetCursorPosition(coords.Item2 - 4, coords.Item1);
        Console.Write("   ");
        Console.SetCursorPosition(coords.Item2 + 2, coords.Item1);
        Console.Write("   ");
        Console.SetCursorPosition(coords.Item2 - 1, coords.Item1 + 1);
        Console.Write("   ");
        Console.BackgroundColor = oldBG;
    }
    private static void RemovePosibleMoves()
    {
        var oldBG = Console.BackgroundColor;
        for (int i = 0; i < 8;  i++)
        {
            for (int j = 0; j < 8; j++)
            {
                var position = new Position(i, j);
                var coords = GetSquereCoordinates(i, j);
                for (int k = -4; k <= 4; k += 8)
                {
                    for (int l = -1; l <= 1; l += 2)
                    {
                        Console.BackgroundColor = position.SquareColor() == Player.Black ? ConsoleColor.Black : ConsoleColor.White;
                        Console.SetCursorPosition(coords.Item2 + k, coords.Item1 + l);
                        Console.Write(" ");
                    }
                }
            }
        }
        Console.BackgroundColor = oldBG;
    }
    private static void WriteBoard()
    {
        /**
         * grid   81x34
         * chars ╔ ╤ ╗ ═ ╚ ╧ ╝ ╟ ╢ ║ ═ │ ─ ┼ */
        Console.Clear();

        Console.SetCursorPosition(0, 2);
        Console.BackgroundColor = ConsoleColor.Gray; Console.ForegroundColor = ConsoleColor.Black;
        Console.WriteLine("╔═════════╤═════════╤═════════╤═════════╤═════════╤═════════╤═════════╤═════════╗");
        for (int i = 0; i < 8; i++)
        {
            for (int k = 0; k < 3; k++)
            {
                for (int j = 0; j < 8; j++)
                {
                    var position = new Position(7 - i, j);
                    if (j == 0)
                    {
                        Console.Write("║");
                    }
                    Console.BackgroundColor = position.SquareColor() == Player.Black ? ConsoleColor.Black : ConsoleColor.White;
                    Console.Write("         ");
                    Console.BackgroundColor = ConsoleColor.Gray; Console.ForegroundColor = ConsoleColor.Black;
                    if (j < 7)
                    {
                        Console.Write("│");
                    } 
                    else
                    {
                        Console.Write("║");
                        if (k == 1)
                        {
                            Console.BackgroundColor = ConsoleColor.Black; Console.ForegroundColor = ConsoleColor.White;
                            Console.Write(" " + (char)('8' - i));
                            Console.BackgroundColor = ConsoleColor.Gray; Console.ForegroundColor = ConsoleColor.Black;
                        }
                        Console.Write("\n");
                    }
                }
            }
            if (i < 7)
            {
                Console.WriteLine("╟─────────┼─────────┼─────────┼─────────┼─────────┼─────────┼─────────┼─────────╢");
            }
        }
        Console.WriteLine("╚═════════╧═════════╧═════════╧═════════╧═════════╧═════════╧═════════╧═════════╝");
        Console.BackgroundColor = ConsoleColor.Black;
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("     a         b         c         d         e         f         g         h\n\n\n\n");
    }
    private static void ShowWinner(GameState state)
    {
        string winnerString;
        string resultReason;
        Console.SetCursorPosition(0, 40);

        Console.BackgroundColor = ConsoleColor.Gray; Console.ForegroundColor = ConsoleColor.Black;
        Console.WriteLine("╔═══════════════════════════════════════════════════════════════════════════════╗");
        for(int i = 0; i < 5; i++)
        {
            Console.Write("║");
            Console.CursorLeft += 79;
            Console.Write("║\n");
        }
        Console.WriteLine("╚═══════════════════════════════════════════════════════════════════════════════╝");
        if (state.Result.Winner == Player.White)
        {
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            winnerString = "White Won!!";
        }
        else if (state.Result.Winner == Player.Black)
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            winnerString = "Black Won!!";
        }
        else
        {
            winnerString = "Draw!";
        }
        for (int i = 41; i < 41 + 5; i++)
        {
            Console.SetCursorPosition(1, i);
            for (int j = 0; j < 79; j++)
            {
                Console.Write(" ");
            }
        }
        Console.SetCursorPosition(1 + (79 - winnerString.Length) / 2, 42);
        Console.Write(winnerString);
        switch (state.Result.Reason)
        {
            case EndReason.InsufficientMaterial:
                resultReason = (state.Result.Winner == Player.White ? "Black" : "White") + " has insuficient material.";
                break;
            case EndReason.Time:
                resultReason = (state.Result.Winner == Player.White ? "Black" : "White") + " has no time left.";
                break;
            case EndReason.FiftyMoveRule:
                resultReason = "There was 50 moves without pawn move or take.";
                break;
            case EndReason.Stalemate:
                resultReason = "Stelmate.";
                break;
            case EndReason.Checkmate:
                resultReason = "Checkmate.";
                break;
            case EndReason.ThreefoldRepetition:
                resultReason = "Moves repeted 3 times.";
                break;
            default:
                resultReason = "";
                break;
        }
        Console.SetCursorPosition(1 + (79 - resultReason.Length) / 2, 44);
        Console.Write(resultReason);
    }
    private static void UpdateScoreBoard(GameState state)
    {
        var oldBG = Console.BackgroundColor;
        var oldFG = Console.ForegroundColor;
        var oldCursorLeft = Console.CursorLeft;
        var oldCursorTop = Console.CursorTop;

        if (state.CurrentPlayer == Player.Black)
        {
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
        }
        else
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
        }
        Console.SetCursorPosition(0, 0);
        Console.Write($"              BLACK                 point dif: {(state.Board.Score <= 0 ? "+" + state.Board.Score * -1 : "-" + state.Board.Score)}                    {(state.TimeBlack/60).ToString("00")}:{(state.TimeBlack % 60).ToString("00")}       ");
        if (state.CurrentPlayer == Player.White)
        {
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
        }
        else
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
        }
        Console.SetCursorPosition(0, 38);
        Console.Write($"              WHITE                 point dif: {(state.Board.Score >= 0 ? "+" + state.Board.Score : "-" + state.Board.Score * -1) }                    {(state.TimeWhite / 60).ToString("00")}:{(state.TimeWhite % 60).ToString("00")}       ");

        Console.SetCursorPosition(oldCursorLeft, oldCursorTop);
        Console.ForegroundColor = oldFG;
        Console.BackgroundColor = oldBG;
    }
    private static void FillBoard(GameState state)
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                var position = new Position(i, j);
                var piece = state.Board[position];
                if (piece != null)
                {
                    PutPiece(piece, position);
                }
            }
        }
    }
    private static void PutPiece(Piece piece, Position position)
    {
        var oldBG = Console.BackgroundColor;
        var oldFG = Console.ForegroundColor;
        var oldCursorLeft = Console.CursorLeft;
        var oldCursorTop = Console.CursorTop;
        var coords = GetSquereCoordinates(position.Row, position.Column);
        Console.SetCursorPosition(coords.Item2 - 1, coords.Item1);
        Console.BackgroundColor = piece.Color == Player.Black ? ConsoleColor.Black : ConsoleColor.White;
        Console.ForegroundColor = piece.Color == Player.Black ? ConsoleColor.White : ConsoleColor.Black;
        Console.Write(" " + piece.Mark + " ");
        Console.SetCursorPosition(oldCursorLeft, oldCursorTop);
        Console.ForegroundColor = oldFG;
        Console.BackgroundColor = oldBG;
    }
    private static void ClearTile(Position position)
    {
        var oldBG = Console.BackgroundColor;
        var oldCursorLeft = Console.CursorLeft;
        var oldCursorTop = Console.CursorTop;
        var coords = GetSquereCoordinates(position.Row, position.Column);
        Console.SetCursorPosition(coords.Item2 - 1, coords.Item1);
        Console.BackgroundColor = position.SquareColor() == Player.Black ? ConsoleColor.Black : ConsoleColor.White;
        Console.Write("   ");
        Console.SetCursorPosition(oldCursorLeft, oldCursorTop);
        Console.BackgroundColor = oldBG;
    }
    private static Tuple<int, int> GetSquereCoordinates(int row, int col)
    {
        return Tuple.Create(4 + (7 - row) * 4, 5 + col * 10);
    }
    private static Tuple<int, int> GetSquereCoordinates(Position position)
    {
        return Tuple.Create(4 + (7 - position.Row) * 4, 5 + position.Column * 10);
    }








    // Structure used by GetWindowRect
    struct Rect
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;
    }
    private static void MaximizeWindow()
    {
        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        [DllImport("user32.dll")]
        static extern bool GetWindowRect(IntPtr hWnd, out Rect lpRect);
        [DllImport("user32.dll")]
        static extern bool MoveWindow(IntPtr hWnd, int x, int y, int nWidth, int nHeight, bool bRepaint);
        const int SW_MAXIMIZE = 3;
        IntPtr consoleWindowHandle = GetForegroundWindow();
        ShowWindow(consoleWindowHandle, SW_MAXIMIZE);
        Rect screenRect;
        GetWindowRect(consoleWindowHandle, out screenRect);
        int width = screenRect.Right - screenRect.Left;
        int height = screenRect.Bottom - screenRect.Top;
        MoveWindow(consoleWindowHandle, screenRect.Left, screenRect.Top, width, height, true);
    }
}