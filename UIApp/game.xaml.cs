using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Annotations;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Logic;
using static System.Windows.Forms.AxHost;

namespace UIApp
{
    /// <summary>
    /// Logika interakcji dla klasy game.xaml
    /// </summary>
    public partial class game : Window
    {
        private Images images;
        private readonly Image[,] pieceImages = new Image[8, 8];
        private readonly Rectangle[,] highlights = new Rectangle[8, 8];
        private readonly Dictionary<Position,Move> moveCache = new();
        private GameState gameState;
        private Position selectedPosition;

        public game(Board board, Settings settings, FrontSettings frontSettings)
        {
            /*timer*/
            System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
            timer.Interval = (100); // 10 seconds in milliseconds
            timer.Tick += new EventHandler(timer_Tick);
            timer.Start();

            InitializeComponent();
            images = new(frontSettings.GetPieceType());
            ImageBrush myBrush = (ImageBrush)BoardGrid.Background;
            myBrush.ImageSource = Images.GetBoardImage(frontSettings.GetBoardType());
            gameState = new(board, Player.White, settings);
            InitializeBoard();
            DrawBoard(gameState.Board);
            UpdateScore();
        }
        private void InitializeBoard()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    var image = new Image();
                    pieceImages[i, j] = image;
                    PieceGrid.Children.Add(image);

                    var highlight = new Rectangle();
                    highlights[i, j] = highlight;
                    HighlightGrid.Children.Add(highlight);
                }
            }
        }
        private void DrawBoard(Board board)
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    pieceImages[i, j].Source = images.GetImage(board[7 - i,  7 - j]);
                }
            }
        }
        private void timer_Tick(object sender, EventArgs e)
        {
            UpdateTime();
        }
        private void UpdateTime()
        {
            WhiteTimer.Content = getTime(gameState.TimeWhite);
            BlackTimer.Content = getTime(gameState.TimeBlack);
            if (gameState.CurrentPlayer == Player.White)
            {
                WhiteTimer.Foreground = Brushes.Black;
                WhiteTimer.Background = Brushes.White;
                BlackTimer.Foreground = Brushes.White;
                BlackTimer.Background = Brushes.Black;
            }
            else
            {
                BlackTimer.Foreground = Brushes.Black;
                BlackTimer.Background = Brushes.White;
                WhiteTimer.Foreground = Brushes.White;
                WhiteTimer.Background = Brushes.Black;
            }
        }
        private string getTime(int time)
        {
            return $"{(time / 60).ToString("00")}:{(time % 60).ToString("00")}";
        }
        private void UpdateScore()
        {
            WhiteScore.Content = $"{(gameState.Board.Score >= 0 ? "+" + gameState.Board.Score : "-" + gameState.Board.Score * -1) }";
            BlackScore.Content = $"{(gameState.Board.Score <= 0 ? "+" + gameState.Board.Score * -1 : "-" + gameState.Board.Score)}";
        }

        private void BoardGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Point point = e.GetPosition(BoardGrid);
            var pos = ToSquarePosition(point);
            if (selectedPosition == null)
            {
                OnFromPositionSelected(pos);
            }
            else
            {
                OnToPositionSelected(pos);
            }
        }
        private Position ToSquarePosition(Point point)
        {
            double squareSize = PieceGrid.ActualWidth / 8;
            int row = (int)(point.Y / squareSize);
            int col = (int)(point.X / squareSize);
            return new Position(7 - row, 7 - col);
        }
        private void OnFromPositionSelected(Position pos)
        {
            var moves = gameState.LegalMovesForPiece(pos);
            if (moves.Any())
            {
                selectedPosition = pos;
                CacheMoves(moves);
                ShowHighlights();
            }
        }
        private void OnToPositionSelected(Position pos)
        {
            selectedPosition = null;
            HideHighlights();

            if (moveCache.TryGetValue(pos, out Move move))
            {
                HandleMove(move);
            }
        }
        private void HandleMove(Move move)
        {
            AddNotation(move.GetNotation(gameState.Board), gameState.CurrentPlayer);
            gameState.MakeMove(move);
            DrawBoard(gameState.Board);
            UpdateTime();
            UpdateScore();
        }
        private void AddNotation(string notation, Player color)
        {
            var label = new Label();
            label.Content = notation;
            if (color == Player.White)
            {
                var labelBlack = new Label();
                MovesWhite.Items.Add(label);
                MovesBlack.Items.Add(labelBlack);
                MovesWhite.ScrollIntoView(label);
                MovesWhite.SelectedIndex = MovesWhite.Items.Count - 1;
                MovesBlack.ScrollIntoView(labelBlack);
                MovesBlack.SelectedIndex = -1;
            } else
            {
                MovesBlack.Items.RemoveAt(MovesBlack.Items.Count - 1);
                MovesBlack.Items.Add(label);
                MovesBlack.SelectedIndex = MovesWhite.Items.Count - 1;
                MovesWhite.SelectedIndex = -1;
                MovesBlack.ScrollIntoView(label);
            }
        }
        private void CacheMoves(IEnumerable<Move> moves)
        {
            moveCache.Clear();
            foreach (Move move in moves)
            {
                moveCache[move.To] = move;
            }
        }
        private void ShowHighlights()
        {
            Color color = Color.FromArgb(150, 125, 255, 125);

            foreach (var to in moveCache.Keys)
            {
                highlights[7 - to.Row, 7 - to.Column].Fill = new SolidColorBrush(color);
            }
        }
        private void HideHighlights()
        {
            foreach (var to in moveCache.Keys)
            {
                highlights[7 - to.Row, 7 - to.Column].Fill = Brushes.Transparent;
            }
        }
    }
}
