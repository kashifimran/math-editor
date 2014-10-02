using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Editor.MVStorage;
using System.IO;

namespace Editor
{
    public delegate void FileSaved(string fileName);
    public partial class FileUploadForm : Form
    {
        //EquationRoot equationRoot = null;
        EditorControl editor = null;
        public event FileSaved FileSavedOnline = (x) => { };

        public FileUploadForm(/*EquationRoot er,*/ EditorControl editorControl, string title)
        {
            InitializeComponent();
            titleBox.Text = title;
            //equationRoot = er;
            editor = editorControl;
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            string name = titleBox.Text.Trim();
            string extension = ".med";
            if (!name.EndsWith(extension))
            {
                name += extension;
            }
            if (name.Length == extension.Length)
            {
                //name = Guid.NewGuid().ToString("N") + extension;
                MessageBox.Show("Please type a name for the file", "Message");
                return;
            }
            using (MemoryStream ms = new MemoryStream())
            {
                try
                {

                    editor.SaveFile(ms, name);
                    ms.Position = 0;
                    string meXml = Convert.ToBase64String(ms.ToArray());
                    if (MVService.AddFile(name, meXml))
                    {
                        MessageBox.Show("File uploaded successfully", "Congratulations!");
                        this.Cursor = Cursors.Arrow;
                        FileSavedOnline(name);
                        this.Close();
                        return;
                    }
                }
                catch { }
            }
            editor.Dirty = true;
            MessageBox.Show("An error occurred. File could not be uploaded", "Error!");
            this.Cursor = Cursors.Arrow;
        }
    }
}
