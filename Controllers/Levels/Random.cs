using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Projekt1.Controllers.Levels
{
    public class Random
    {
        private TableLayoutPanel _grid;

        public Random(TableLayoutPanel grid)
        {
            _grid = grid;
        }

        public void MakeMove(int x, int y)
        {
            var emptyButtons = _grid.Controls.OfType<Button>()
                                    .Where(b => b.Text == "")
                                    .ToList();

            if (emptyButtons.Any())
            {
                System.Random rnd = new System.Random();
                var choice = emptyButtons[rnd.Next(emptyButtons.Count)];

                choice.Text = "O";
                choice.ForeColor = Color.Magenta;
            }
        }
    }
}
