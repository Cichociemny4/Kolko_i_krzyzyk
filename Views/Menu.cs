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
    public partial class Menu : Form
    {
        public readonly Dictionary<string, Dictionary<string, string>> _languages = new Dictionary<string, Dictionary<string, string>>()
        {
            { "pl", new Dictionary<string, string> {
                { "title", "Kółko i krzyżyk" },
                { "combox", "-- wybierz --" },
                { "win", "Wygrałeś!" },
                { "draw", "Remis!" }
            }},
            { "en", new Dictionary<string, string> {
                { "title", "Tic Tac Toe"},
                { "combox", "-- select --" },
                { "win", "You won!" },
                { "draw", "Draw!" }
            }}
        };
        private int down = 3;
        private int up = 20;

        public Menu()
        {
            InitializeComponent();
            this.BackColor = Color.FromArgb(28, 28, 28);
            this.ForeColor = Color.White;

            this.Load += new System.EventHandler(this.Menu_Load);
        }

        private void Menu_Load(object sender, EventArgs e)
        {
            cbN.Items.Clear();
            cbK.Items.Clear();

            cbN.Items.Add("wybierz");
            cbK.Items.Add("wybierz");

            for (int i = down; i <= up; i++)
            {
                cbN.Items.Add(i.ToString());
                cbK.Items.Add(i.ToString());
            }

            cbN.SelectedIndex = 0;
            cbK.SelectedIndex = 0;
        }

        private int DimChecker(ComboBox en)
        {
            bool isValid = int.TryParse(en.Text, out int o);

            if (!isValid)
            {
                //MessageBox.Show("Please enter valid integers for board dimensions.", "Input Error");
                MessageBox.Show("Wprowadź poprawne wymiary planszy.", "Input Error");
                return 0;
            }

            if (o < down || o > up)
            {
                //MessageBox.Show($"Dimensions must be between {down} and {up}!", "Out of Range");
                MessageBox.Show($"Wymiary muszą mieć wartość pomiędzy {down} i {up}", "Out of Range");
                return 0;
            }

            return o;
        }
        private bool DimChecke2r()
        {
            bool isNValid = int.TryParse(cbN.Text, out int n);
            bool isMValid = int.TryParse(cbK.Text, out int k);

            if (!isNValid || !isMValid)
            {
                //MessageBox.Show("Please enter valid integers for board dimensions.", "Input Error");
                MessageBox.Show("Wprowadź poprawne wymiary planszy.", "Input Error");
                return false;
            }

            if (n < down || n > up || k < down || k > up)
            {
                //MessageBox.Show($"Dimensions must be between {down} and {up}!", "Out of Range");
                MessageBox.Show($"Wymiary muszą mieć wartość pomiędzy {down} i {up}", "Out of Range");
                return false;
            }
            return true;
        }
        private void n_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void k_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void easy_Click(object sender, EventArgs e)
        {
            int n, k;
            if ((n = DimChecker(cbN)) == 0) { return; }
            if ((k = DimChecker(cbK)) == 0) { return; }

            Views.Game gameWindow = new Views.Game(n, k, 0);

            this.Hide();
            gameWindow.ShowDialog();
            this.Close();
        }

        private void medium_Click(object sender, EventArgs e)
        {
            int n, k;
            if ((n = DimChecker(cbN)) == 0) { return; }
            if ((k = DimChecker(cbK)) == 0) { return; }

            Views.Game gameWindow = new Views.Game(n, k, 1);

            this.Hide();
            gameWindow.ShowDialog();
            this.Close();
        }

        private void hard_Click(object sender, EventArgs e)
        {
            int n, k;
            if ((n=DimChecker(cbN)) == 0) { return; }
            if ((k = DimChecker(cbK)) == 0) { return; }

            Views.Game gameWindow = new Views.Game(n, k, 2);

            this.Hide();
            gameWindow.ShowDialog();
            this.Close();
        }

        private void PwP_Click(object sender, EventArgs e)
        {
            int n, k;
            if ((n = DimChecker(cbN)) == 0) { return; }
            if ((k = DimChecker(cbK)) == 0) { return; }

            Views.Game gameWindow = new Views.Game(n, k, 4);

            this.Hide();
            gameWindow.ShowDialog();
            this.Close();
        }
    }
}
