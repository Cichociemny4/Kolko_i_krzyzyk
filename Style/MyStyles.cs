using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Projekt1.Style
{
    public static class MyStyles
    {
        public static void ButtonStyle(Button b)
        {
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderSize = 0;
            b.BackColor = Color.FromArgb(45, 45, 48);
            b.ForeColor = Color.Cyan;
            b.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            b.TextAlign = ContentAlignment.MiddleCenter;

            b.FlatAppearance.MouseOverBackColor = Color.FromArgb(63, 63, 70);
            b.Margin = new Padding(4);
        }

        public static void FormStyle(Form f)
        {
            f.BackColor = Color.FromArgb(28, 28, 28);
            f.ForeColor = Color.White;
            f.Padding = new Padding(20, 50, 20, 20);
        }

        public static void LabelStyle(Label l, Color? customColor = null)
        {
            l.ForeColor = customColor ?? Color.White;
            //l.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            l.BackColor = Color.Transparent;
        }
    }
}
