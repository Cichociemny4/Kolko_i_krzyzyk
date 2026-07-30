using System;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;

namespace Projekt1.Controllers
{
    internal class End
    {
        // Sprawdza stan gry: 0 - gramy dalej, 1 - wygrana, 2 - remis
        public int CheckGameStatus(string[,] grid, int row, int col, int k_row, int k_others, int k, string symbol)
        {
            if (CheckWin(grid, row, col, k_row, k_others, k, symbol)) return 1; // Wygrana
            if (IsDraw(grid)) return 2;                      // Remis
            return 0;                                        // Gramy dalej
        }

        private bool CheckWin(string[,] grid, int row, int col, int k_row, int k_others, int minhold, string symbol)
        {
            var checkRules = new[]
            {
                new { dr = 0, dc = 1, targetK = k_row },    // Poziomo
                new { dr = 1, dc = 0, targetK = k_others }, // Pionowo
                new { dr = 1, dc = 1, targetK = minhold }, // Skos \
                new { dr = 1, dc = -1, targetK = minhold } // Skos /
            };

            foreach (var rule in checkRules)
            {
                int count = 1;

                count += CountInDirection(grid, row, col, rule.dr, rule.dc, symbol);
                count += CountInDirection(grid, row, col, -rule.dr, -rule.dc, symbol);

                if (count >= rule.targetK) return true;
            }

            return false;
        }

        private int CountInDirection(string[,] grid, int r, int c, int dr, int dc, string sym)
        {
            int count = 0;
            int rows = grid.GetLength(0);
            int cols = grid.GetLength(1);
            int maxSearch = Math.Max(rows, cols);

            for (int i = 1; i < maxSearch; i++)
            {
                int nr = r + dr * i;
                int nc = c + dc * i;

                if (nr < 0 || nr >= rows || nc < 0 || nc >= cols) break;

                if (grid[nr, nc] == sym)
                {
                    count++;
                }
                else
                {
                    break;
                }
            }
            return count;
        }

        private bool IsDraw(string[,] grid)
        {
            foreach (string cell in grid)
            {
                if (string.IsNullOrEmpty(cell))
                    return false; // nadal są ruchy
            }

            return true; // brak ruchów = remis
        }
    }
}
