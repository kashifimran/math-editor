using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Editor
{
    public partial class MatrixInputForm : Form
    {
        public event Action<int, int> ProcessRequest = (x, y) => { };

        public MatrixInputForm(int rows, int columns)
        {
            InitializeComponent();
            rowsUpDown.Value = rows;
            columnsUpDown.Value = columns;
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            ProcessRequest((int)rowsUpDown.Value, (int)columnsUpDown.Value);
            this.Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
