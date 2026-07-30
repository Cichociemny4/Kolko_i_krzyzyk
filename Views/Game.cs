using Projekt1.Controllers;
using Projekt1.Controllers.Levels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Projekt1.Views
{
    public partial class Game : Form
    {
        //private bool resizing = false;
        public event Action<int, int> CellClicked;
        private End sedzia = new End();
        private int winK;
        private string[,] board;
        private int _level;
        public Game(int n, int k, int level)
        {
            InitializeComponent();
            Stylowanie();
            this.winK = Math.Min(n, k);
            _level = level;
            board = new string[n, k];

            MapGenerator(n, k);

            panel1.AutoScroll = true;
            grid.Dock = DockStyle.None;
            grid.AutoSize = true;
            grid.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            grid.Anchor = AnchorStyles.None;
            grid.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            this.AutoScroll = false;

            grid.PerformLayout();
            CenterGrid();

            LevelSelect(n, k);
        }
        
        private void LevelSelect(int n, int k)
        {
            CellClicked = null;

            switch (_level)
            {
                case 0:
                    label2.Text = "Łatwy";
                    label2.ForeColor = Color.Green;
                    var easyBot = new Projekt1.Controllers.Levels.EasyBot(board, grid, n, k, winK);
                    CellClicked += easyBot.MakeMove;
                    //CellClicked += Easy;
                    break;

                case 1:
                    label2.Text = "Średni";
                    label2.ForeColor = Color.Yellow;
                    var mediumBot = new MediumBot(board, grid, n, k, winK);
                    CellClicked += mediumBot.MakeMove;
                    break;

                case 2:
                    label2.Text = "Trudny";
                    label2.ForeColor = Color.Red;
                    var randomBot = new Controllers.Levels.Random(grid);
                    CellClicked += randomBot.MakeMove;
                    break;

                case 4:
                    label2.Text = "PvP";
                    label2.ForeColor = Color.Blue;
                    var pvp = new Controllers.Levels.PvP(board, grid);
                    CellClicked += pvp.ApplyMove;
                    //CellClicked += PvP;
                    break;

                default: throw new ArgumentException();
            }
        }

        private void Easy(int n, int k)
        {
            //script easy
        }

        private void Medium(int n, int k)
        {
            //script medium
        }

        private void Hard(int n, int k)
        {
            //script hard
        }

        private void PvP(int n, int k)
        {
            //script pvp
        }
        private void MapGenerator(int rows, int cols)
        {
            grid.Controls.Clear();
            grid.ColumnStyles.Clear();
            grid.RowStyles.Clear();

            grid.RowCount = rows;
            grid.ColumnCount = cols;

            for (int r = 0; r < rows; r++)
                grid.RowStyles.Add(new RowStyle(SizeType.Absolute, 60f));

            for (int c = 0; c < cols; c++)
                grid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 60f));

            if (_level == 4) // pvp
            {
                for (int r = 0; r < rows; r++)
                {
                    for (int c = 0; c < cols; c++)
                    {
                        Button b = new Button
                        {
                            Dock = DockStyle.Fill,
                            Margin = new Padding(1),
                            Font = new Font("Arial", 14, FontStyle.Bold),
                            Tag = new Point(r, c)
                        };

                        Projekt1.Style.MyStyles.ButtonStyle(b);

                        b.Click += (s, e) =>
                        {
                            Button btn = (Button)s;
                            if (btn.Text != "") return;

                            Point p = (Point)btn.Tag;

                            CellClicked?.Invoke(p.X, p.Y);

                            HandleEndGame(p.X, p.Y, cols, rows, btn.Text);
                        };

                        grid.Controls.Add(b, c, r);
                    }
                }
            }
            else // boty
            {
                for (int r = 0; r < rows; r++)
                {
                    for (int c = 0; c < cols; c++)
                    {
                        Button b = new Button
                        {
                            Dock = DockStyle.Fill,
                            Margin = new Padding(1),
                            Font = new Font("Arial", 14, FontStyle.Bold),
                            Tag = new Point(r, c)
                        };

                        Projekt1.Style.MyStyles.ButtonStyle(b);

                        b.Click += (s, e) =>
                        {
                            Button btn = (Button)s;
                            if (btn.Text != "") return;

                            Point p = (Point)btn.Tag;

                            btn.Text = "X";
                            if (HandleEndGame(p.X, p.Y, cols, rows, "X")) return;

                            CellClicked?.Invoke(p.X, p.Y);

                            var lastBotButton = grid.Controls.OfType<Button>().FirstOrDefault(x => x.Name == "LastBotMove");

                            foreach (Button botBtn in grid.Controls.OfType<Button>().Where(x => x.Text == "O"))
                            {
                                Point bp = (Point)botBtn.Tag;
                                if (HandleEndGame(bp.X, bp.Y, cols, rows, "O")) return;
                            }
                        };

                        grid.Controls.Add(b, c, r);
                    }
                }
            }
        }
        private void CenterGrid()
        {
            if (grid == null || panel1 == null || this.WindowState == FormWindowState.Minimized) return;

            panel1.SuspendLayout();

            // 1. CAŁKOWITY RESET mechanizmu scrolla
            // Wyłączenie AutoScroll resetuje współrzędne panelu do (0,0)
            panel1.AutoScroll = false;

            Size gridSize = grid.PreferredSize;
            int margin = 20;

            // 2. OBLICZANIE POZYCJI
            // Jeśli plansza jest mniejsza niż okno -> centruj
            // Jeśli plansza jest WIĘKSZA -> ustaw na sztywny margines (żeby nie było pustki po prawej)

            int targetX = (panel1.ClientSize.Width > gridSize.Width + margin * 2)
                ? (panel1.ClientSize.Width - gridSize.Width) / 2
                : margin;

            int targetY = (panel1.ClientSize.Height > gridSize.Height + margin * 2)
                ? (panel1.ClientSize.Height - gridSize.Height) / 2
                : margin;

            // 3. Aplikujemy pozycję
            grid.Location = new Point(targetX, targetY);

            // 4. Przywracamy scrolla - WinForms sam wykryje rozmiar grida i ustawi suwaki
            panel1.AutoScroll = true;

            // Opcjonalnie: wymuszamy, żeby scrollbar zawsze pozwalał zobaczyć margines na dole/z prawej
            panel1.AutoScrollMinSize = new Size(
                grid.Location.X + gridSize.Width + margin,
                grid.Location.Y + gridSize.Height + margin
            );

            panel1.ResumeLayout();
        }

        private void Stylowanie()
        {
            // CZYŚCIMY WSZYSTKO, żeby pozbyć się "duchów" ze screenów
            this.Controls.Clear();

            // 1. Główny szkielet (jak GridLayout w Javie)
            TableLayoutPanel mainLayout = new TableLayoutPanel();
            mainLayout.Dock = DockStyle.Fill;
            mainLayout.RowCount = 3;
            mainLayout.ColumnCount = 1;

            // Rozmiary wierszy: napisy zajmują tyle ile muszą (AutoSize), panel resztę (100%)
            mainLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));

            // 2. Konfigurujemy labele
            label1.AutoSize = true;
            label1.Dock = DockStyle.Fill;
            label1.TextAlign = ContentAlignment.MiddleCenter;
            label1.Padding = new Padding(0, 10, 0, 5);

            label2.AutoSize = true;
            label2.Dock = DockStyle.Fill;
            label2.TextAlign = ContentAlignment.MiddleCenter;
            label2.Padding = new Padding(0, 0, 0, 10);

            // 3. Konfigurujemy panel gry
            panel1.Dock = DockStyle.Fill;
            panel1.AutoScroll = true;
            panel1.Controls.Clear();
            panel1.Controls.Add(grid); // Grid ląduje w panelu

            // 4. Konfigurujemy grid (planszę)
            grid.AutoSize = true;
            grid.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            grid.Dock = DockStyle.None;
            grid.Anchor = AnchorStyles.None; // To jest klucz do braku konfliktów

            // 5. Wrzucamy wszystko do szkieletu
            mainLayout.Controls.Add(label1, 0, 0);
            mainLayout.Controls.Add(label2, 0, 1);
            mainLayout.Controls.Add(panel1, 0, 2);

            this.Controls.Add(mainLayout);

            // W Stylowanie() lub Konstruktorze:
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
            panel1.GetType().GetProperty("DoubleBuffered",
                System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                .SetValue(panel1, true, null);

            // Twoje style
            Projekt1.Style.MyStyles.FormStyle(this);
            Projekt1.Style.MyStyles.LabelStyle(label1);
            Projekt1.Style.MyStyles.LabelStyle(label2);

            // Eventy
            panel1.Resize += (s, e) => CenterGrid();
        }

        private bool HandleEndGame(int r, int c, int rows, int cols, string symbol)
        {
            board[r, c] = symbol;
            int status = sedzia.CheckGameStatus(board, r, c, rows, cols, winK, symbol);

            if (status == 1) // Wygrana
            {
                string msg = "";
                if (_level == 4)
                {
                    msg = (symbol == "X") ? "Gracz 1 wygrał!" : "Gracz 2 wygrał!";
                }
                else
                {
                    msg = (symbol == "X") ? "Wygrałeś, gratulacje!" : "Bot Cię pokonał. Słabo...";
                }
                MessageBox.Show(msg, "Koniec gry");
                Views.Menu menuWindow = new Views.Menu();

                this.Hide();
                menuWindow.ShowDialog();
                this.Close();
                return true;
            }
            else if (status == 2) // Remis
            {
                MessageBox.Show("Plansza pełna. Remis!", "Koniec gry");
                Views.Menu menuWindow = new Views.Menu();

                this.Hide();
                menuWindow.ShowDialog();
                this.Close();
                return true;
            }
            return false; // Gramy dalej
        }
    }
}
