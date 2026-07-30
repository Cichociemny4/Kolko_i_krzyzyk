using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Projekt1.Controllers.Levels
{
    internal class PvP
    {
        private string currentPlayer = "X";

        private string[,] _board;
        private TableLayoutPanel _grid;

        public PvP(string[,] board, TableLayoutPanel grid)
        {
            _board = board;
            _grid = grid;
        }
        internal void ApplyMove(int x, int y)
        {
            if (!string.IsNullOrEmpty(_board[x, y]))
                return;

            var btn = _grid.Controls
                .OfType<Button>()
                .FirstOrDefault(b => b.Tag is Point p && p.X == x && p.Y == y);

            if (btn != null)
            {
                btn.Text = currentPlayer;
                btn.ForeColor = currentPlayer == "X" ? Color.Blue : Color.Magenta;
            }

            SwitchPlayer();
        }
        private void SwitchPlayer()
        {
            currentPlayer = (currentPlayer == "X") ? "O" : "X";
        }
    }
}
