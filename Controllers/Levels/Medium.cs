using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Windows.Forms;

namespace Projekt1.Controllers.Levels
{
    public class MediumBot
    {
        private readonly string[,] _board;
        private readonly TableLayoutPanel _grid;
        private readonly int _kRow, _kOthers, _kDiag;

        private readonly int _maxDepth = 4;

        private readonly System.Random _rng = new System.Random();

        private const string BOT = "O";
        private const string PLAYER = "X";
        private const double WIN_SCORE = 10000000;

        public double[,] GenerateHeatmapData()
        {
            int rows = _board.GetLength(0);
            int cols = _board.GetLength(1);
            double[,] heatmap = new double[rows, cols];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (string.IsNullOrEmpty(_board[i, j]))
                    {
                        // 1. Symulujemy ruch Bota na tym polu
                        _board[i, j] = BOT;

                        // 2. Używamy Twojego głównego algorytmu oceniającego!
                        // Dzięki temu heatmapa będzie dokładnie tym, co widzi Minimax
                        heatmap[i, j] = Evaluate(_board);

                        // 3. Cofamy ruch
                        _board[i, j] = null;
                    }
                    else
                    {
                        // Pole zajęte, nie rysujemy heatmapy
                        heatmap[i, j] = -1;
                    }
                }
            }
            return heatmap;
        }

        public void SaveHeatmapToImage(string filePath = "heatmap.png")
        {
            int cellSize = 100;
            int rows = _board.GetLength(0);
            int cols = _board.GetLength(1);

            double[,] heatmap = GenerateHeatmapData();

            double maxWeight = double.MinValue;
            double minWeight = double.MaxValue;

            foreach (double w in heatmap)
            {
                if (w != -1) // Ignorujemy zajęte pola
                {
                    if (w > maxWeight) maxWeight = w;
                    if (w < minWeight) minWeight = w;
                }
            }

            if (maxWeight == minWeight) maxWeight = minWeight + 0.1;

            using (Bitmap bmp = new Bitmap(cols * cellSize, rows * cellSize))
            using (Graphics g = Graphics.FromImage(bmp))
            {
                Font symbolFont = new Font("Arial", 36, FontStyle.Bold);
                Font weightFont = new Font("Arial", 16, FontStyle.Bold);
                StringFormat centerFormat = new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                };

                for (int r = 0; r < rows; r++)
                {
                    for (int c = 0; c < cols; c++)
                    {
                        Rectangle rect = new Rectangle(c * cellSize, r * cellSize, cellSize, cellSize);
                        string cellValue = _board[r, c];

                        if (!string.IsNullOrEmpty(cellValue))
                        {
                            g.FillRectangle(Brushes.LightGray, rect);

                            Brush textBrush = cellValue == "X" ? Brushes.Black : Brushes.Magenta;
                            g.DrawString(cellValue, symbolFont, textBrush, rect, centerFormat);
                        }
                        else
                        {
                            double rawWeight = heatmap[r, c];

                            double ratio = (rawWeight - minWeight) / (maxWeight - minWeight);
                            if (ratio < 0) ratio = 0;
                            if (ratio > 1) ratio = 1;

                            int red = 255;
                            int green = (int)(255 * (1 - ratio));
                            int blue = (int)(255 * (1 - ratio));

                            using (Brush bgBrush = new SolidBrush(Color.FromArgb(red, green, blue)))
                            {
                                g.FillRectangle(bgBrush, rect);
                            }

                            g.DrawString(Math.Round(rawWeight, 1).ToString(), weightFont, Brushes.Black, rect, centerFormat);
                        }

                        g.DrawRectangle(Pens.Black, rect);
                    }
                }

                bmp.Save(filePath, ImageFormat.Png);
            }
        }

        public MediumBot(
            string[,] board,
            TableLayoutPanel grid,
            int ko,
            int kr,
            int kd)
        {
            _board = board;
            _grid = grid;
            _kRow = kr;
            _kOthers = ko;
            _kDiag = kd;
        }

        public void MakeMove(int x, int y)
        {
            //SaveHeatmapToImage("stan_planszy_medium.png");

            if (TryForcedMove(BOT, out Point winMove))
            {
                PlaceMove(winMove);
                return;
            }

            if (TryForcedMove(PLAYER, out Point blockMove))
            {
                PlaceMove(blockMove);
                return;
            }

            Point move = FindBestMove(x, y);
            if (move.X == -1)
                return;

            PlaceMove(move);
        }

        private void PlaceMove(Point move)
        {
            if (move.X < 0 || move.Y < 0)
                return;

            if (!string.IsNullOrEmpty(_board[move.X, move.Y]))
                return;

            _board[move.X, move.Y] = BOT;

            Button btn = _grid.Controls
                .OfType<Button>()
                .FirstOrDefault(b => b.Tag is Point p && p.X == move.X && p.Y == move.Y);

            if (btn != null)
            {
                btn.Text = BOT;
                btn.ForeColor = Color.Magenta;
            }
        }

        private bool TryForcedMove(string symbol, out Point move)
        {
            int rows = _board.GetLength(0);
            int cols = _board.GetLength(1);

            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    if (!string.IsNullOrEmpty(_board[r, c]))
                        continue;

                    _board[r, c] = symbol;
                    bool win = HasWinner(_board, symbol);
                    _board[r, c] = null;

                    if (win)
                    {
                        move = new Point(r, c);
                        return true;
                    }
                }
            }

            move = new Point(-1, -1);
            return false;
        }

        private Point FindBestMove(int focusX, int focusY)
        {
            var candidates = GetCandidateMoves(focusX, focusY, radius: 2, limit: 10);

            if (candidates.Count == 0)
                return new Point(-1, -1);

            double bestScore = double.NegativeInfinity;
            List<Point> bestMoves = new List<Point>();

            foreach (var move in candidates)
            {
                _board[move.X, move.Y] = BOT;
                double score = Minimax(_board, _maxDepth - 1, false,
                    double.NegativeInfinity, double.PositiveInfinity);
                _board[move.X, move.Y] = null;

                if (score > bestScore + 0.001)
                {
                    bestScore = score;
                    bestMoves.Clear();
                    bestMoves.Add(move);
                }
                else if (Math.Abs(score - bestScore) <= 0.001)
                {
                    bestMoves.Add(move);
                }
            }

            if (bestMoves.Count == 0)
                return new Point(-1, -1);

            return bestMoves[_rng.Next(bestMoves.Count)];
        }

        // Wersja dla realnego ruchu: okolica ostatniego kliknięcia gracza
        private List<Point> GetCandidateMoves(int focusX, int focusY, int radius = 2, int limit = 8)
        {
            int rows = _board.GetLength(0);
            int cols = _board.GetLength(1);

            HashSet<Point> candidates = new HashSet<Point>();

            for (int r = focusX - radius; r <= focusX + radius; r++)
            {
                for (int c = focusY - radius; c <= focusY + radius; c++)
                {
                    if (r < 0 || r >= rows || c < 0 || c >= cols)
                        continue;

                    if (string.IsNullOrEmpty(_board[r, c]))
                        candidates.Add(new Point(r, c));
                }
            }

            if (candidates.Count == 0)
                candidates.Add(new Point(rows / 2, cols / 2));

            return candidates
                .OrderByDescending(p => QuickHeuristic(_board, p))
                .Take(limit)
                .ToList();
        }

        // Wersja dla minimaxa: kandydaci wokół już istniejących pionków
        private List<Point> GetCandidateMoves(string[,] board, int limit = 8)
        {
            int rows = board.GetLength(0);
            int cols = board.GetLength(1);

            HashSet<Point> candidates = new HashSet<Point>();
            bool hasAnyPiece = false;

            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    if (string.IsNullOrEmpty(board[r, c]))
                        continue;

                    hasAnyPiece = true;

                    for (int dr = -1; dr <= 1; dr++)
                    {
                        for (int dc = -1; dc <= 1; dc++)
                        {
                            if (dr == 0 && dc == 0)
                                continue;

                            int nr = r + dr;
                            int nc = c + dc;

                            if (nr >= 0 && nr < rows && nc >= 0 && nc < cols && string.IsNullOrEmpty(board[nr, nc]))
                                candidates.Add(new Point(nr, nc));
                        }
                    }
                }
            }

            if (!hasAnyPiece)
                return new List<Point> { new Point(rows / 2, cols / 2) };

            return candidates
                .OrderByDescending(p => QuickHeuristic(board, p))
                .Take(limit)
                .ToList();
        }

        private double QuickHeuristic(string[,] board, Point move)
        {
            double score = 0;

            for (int dr = -1; dr <= 1; dr++)
            {
                for (int dc = -1; dc <= 1; dc++)
                {
                    if (dr == 0 && dc == 0)
                        continue;

                    int r = move.X + dr;
                    int c = move.Y + dc;

                    if (r < 0 || c < 0 || r >= board.GetLength(0) || c >= board.GetLength(1))
                        continue;

                    if (board[r, c] == BOT) score += 2;
                    if (board[r, c] == PLAYER) score += 3;
                }
            }

            return score;
        }

        private double Minimax(string[,] board, int depth, bool isMax, double alpha, double beta)
        {
            if (depth == 0 || IsTerminal(board))
                return Evaluate(board);

            var moves = GetCandidateMoves(board, limit: 8);

            if (moves.Count == 0)
                return Evaluate(board);

            moves = moves
                .OrderByDescending(m => QuickMoveScore(board, m))
                .ToList();

            if (isMax)
            {
                double best = double.NegativeInfinity;

                foreach (var move in moves)
                {
                    board[move.X, move.Y] = BOT;

                    if (HasWinner(board, BOT))
                    {
                        board[move.X, move.Y] = null;
                        return WIN_SCORE;
                    }

                    best = Math.Max(best, Minimax(board, depth - 1, false, alpha, beta));
                    board[move.X, move.Y] = null;

                    alpha = Math.Max(alpha, best);
                    if (beta <= alpha)
                        break;
                }

                return best;
            }
            else
            {
                double best = double.PositiveInfinity;

                foreach (var move in moves)
                {
                    board[move.X, move.Y] = PLAYER;

                    if (HasWinner(board, PLAYER))
                    {
                        board[move.X, move.Y] = null;
                        return -WIN_SCORE;
                    }

                    best = Math.Min(best, Minimax(board, depth - 1, true, alpha, beta));
                    board[move.X, move.Y] = null;

                    beta = Math.Min(beta, best);
                    if (beta <= alpha)
                        break;
                }

                return best;
            }
        }

        private double QuickMoveScore(string[,] board, Point move)
        {
            double score = 0;

            for (int dr = -1; dr <= 1; dr++)
            {
                for (int dc = -1; dc <= 1; dc++)
                {
                    if (dr == 0 && dc == 0)
                        continue;

                    int r = move.X + dr;
                    int c = move.Y + dc;

                    if (r < 0 || c < 0 || r >= board.GetLength(0) || c >= board.GetLength(1))
                        continue;

                    if (board[r, c] == BOT) score += 3;
                    if (board[r, c] == PLAYER) score += 4;
                }
            }

            return score;
        }

        private double Evaluate(string[,] board)
        {
            if (HasWinner(board, BOT))
                return WIN_SCORE;

            if (HasWinner(board, PLAYER))
                return -WIN_SCORE;

            if (IsFull(board))
                return 0;

            double score = 0;

            score += ScoreDirection(board, 0, 1, _kRow);
            score += ScoreDirection(board, 1, 0, _kOthers);
            score += ScoreDirection(board, 1, 1, _kDiag);
            score += ScoreDirection(board, 1, -1, _kDiag);

            score += CenterBias(board);

            return score;
        }

        private double ScoreDirection(string[,] board, int dr, int dc, int targetK)
        {
            int rows = board.GetLength(0);
            int cols = board.GetLength(1);
            double score = 0;

            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    int endR = r + dr * (targetK - 1);
                    int endC = c + dc * (targetK - 1);

                    if (endR < 0 || endR >= rows || endC < 0 || endC >= cols)
                        continue;

                    int botCount = 0;
                    int playerCount = 0;
                    int emptyCount = 0;

                    for (int i = 0; i < targetK; i++)
                    {
                        int nr = r + dr * i;
                        int nc = c + dc * i;

                        string cell = board[nr, nc];

                        if (cell == BOT) botCount++;
                        else if (cell == PLAYER) playerCount++;
                        else emptyCount++;
                    }

                    if (botCount > 0 && playerCount > 0)
                        continue;

                    if (botCount > 0)
                        score += LineWeight(botCount, emptyCount);

                    if (playerCount > 0)
                        score -= LineWeight(playerCount, emptyCount) * 1.15;
                }
            }

            return score;
        }

        private double LineWeight(int count, int emptyCount)
        {
            double baseScore = Math.Pow(8, count);
            double opennessBonus = 1.0 + (emptyCount * 0.15);
            return baseScore * opennessBonus;
        }

        private double CenterBias(string[,] board)
        {
            int rows = board.GetLength(0);
            int cols = board.GetLength(1);

            double score = 0.0;
            double centerR = (rows - 1) / 2.0;
            double centerC = (cols - 1) / 2.0;

            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    if (string.IsNullOrEmpty(board[r, c]))
                        continue;

                    double dist = Math.Abs(r - centerR) + Math.Abs(c - centerC);
                    double weight = 1.0 / (1.0 + dist);

                    if (board[r, c] == BOT) score += 12 * weight;
                    if (board[r, c] == PLAYER) score -= 12 * weight;
                }
            }

            return score;
        }

        private bool IsTerminal(string[,] board)
        {
            return HasWinner(board, BOT) || HasWinner(board, PLAYER) || IsFull(board);
        }

        private bool IsFull(string[,] board)
        {
            foreach (var cell in board)
            {
                if (string.IsNullOrEmpty(cell))
                    return false;
            }

            return true;
        }

        private bool HasWinner(string[,] board, string symbol)
        {
            return HasLine(board, symbol, 0, 1, _kRow) ||
                   HasLine(board, symbol, 1, 0, _kOthers) ||
                   HasLine(board, symbol, 1, 1, _kDiag) ||
                   HasLine(board, symbol, 1, -1, _kDiag);
        }

        private bool HasLine(string[,] board, string symbol, int dr, int dc, int targetK)
        {
            int rows = board.GetLength(0);
            int cols = board.GetLength(1);

            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    int endR = r + dr * (targetK - 1);
                    int endC = c + dc * (targetK - 1);

                    if (endR < 0 || endR >= rows || endC < 0 || endC >= cols)
                        continue;

                    bool ok = true;

                    for (int i = 0; i < targetK; i++)
                    {
                        int nr = r + dr * i;
                        int nc = c + dc * i;

                        if (board[nr, nc] != symbol)
                        {
                            ok = false;
                            break;
                        }
                    }

                    if (ok)
                        return true;
                }
            }

            return false;
        }
    }
}
