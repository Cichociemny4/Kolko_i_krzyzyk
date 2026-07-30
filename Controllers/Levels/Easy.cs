using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Windows.Forms;

namespace Projekt1.Controllers.Levels
{
    public class EasyBot
    {
        private string[,] _board;
        private TableLayoutPanel _grid;
        private int _kRow, _kOthers, _kDiag;

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
                        heatmap[i, j] = EvaluateMove(i, j);
                    else
                        heatmap[i, j] = -1; // -1 oznacza pole zajęte
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

        public EasyBot(
            string[,] board,
            TableLayoutPanel grid,
            int kr,
            int ko,
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
            //SaveHeatmapToImage("stan_planszy_easy.png");

            Point move = FindBestMove();

            if (move.X == -1)
                return;

            _board[move.X, move.Y] = "O";

            Button btn = _grid.Controls
                .OfType<Button>()
                .FirstOrDefault(b => (Point)b.Tag == move);

            if (btn != null)
            {
                btn.Text = "O";
                btn.ForeColor = Color.Magenta;
            }
        }

        private Point FindBestMove()
        {
            int rows = _board.GetLength(0);
            int cols = _board.GetLength(1);

            double bestScore = -1;
            Point bestMove = new Point(-1, -1);

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (string.IsNullOrEmpty(_board[i, j]))
                    {
                        double score = EvaluateMove(i, j);
                        if (score > bestScore)
                        {
                            bestScore = score;
                            bestMove = new Point(i, j);
                        }
                    }
                }
            }

            return bestMove;
        }
        private double EvaluateMove(int r, int c)
        {
            // Erdősa-Selfridge’a
            // Liczymy wagę pola: im bliżej "K" znaków w linii, tym wyższa waga
            // Przykładowo: waga = 2 ^ (ilość znaków w linii)

            double score = 0;

            // Sprawdzamy wszystkie 4 kierunki
            score += GetLineScore(r, c, 0, 1, _kRow);    // Poziom
            score += GetLineScore(r, c, 1, 0, _kOthers); // Pion
            score += GetLineScore(r, c, 1, 1, _kDiag);   // Skos \
            score += GetLineScore(r, c, 1, -1, _kDiag);  // Skos /

            return score;
        }

        private double GetLineScore(int r, int c, int dr, int dc, int kTarget)
        {
            int rows = _board.GetLength(0);
            int cols = _board.GetLength(1);

            int oCount = 0;
            int xCount = 0;

            // sprawdzamy w 2 kierunkach
            for (int dir = -1; dir <= 1; dir += 2)
            {
                for (int i = 1; i < kTarget; i++)
                {
                    int nr = r + dr * i * dir;
                    int nc = c + dc * i * dir;

                    if (nr < 0 || nr >= rows || nc < 0 || nc >= cols)
                        break;

                    string cell = _board[nr, nc];

                    if (cell == "O") oCount++;
                    else if (cell == "X") xCount++;
                    else break;
                }
            }

            //  linia martwa (oba symbole)
            if (oCount > 0 && xCount > 0)
                return 0;

            //  blokowanie gracza (priorytet)
            if (xCount > 0)
                return Math.Pow(7, xCount);

            //  własne budowanie linii
            if (oCount > 0)
                return Math.Pow(4, oCount);

            System.Random rnd = new System.Random();
            return 1 + rnd.NextDouble();
        }
    }
}
