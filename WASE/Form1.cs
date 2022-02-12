using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Threading;
using System.Runtime;
using System.Net.NetworkInformation;

namespace WASE
{
    public partial class Form1 : Form
    {

        bool debugMode = false;
        string path = "";
        bool saved = false;

        public Form1(bool debugMode_enable)
        {
            InitializeComponent();
            debugMode = debugMode_enable;
        }


        // Used to check for internet connection.
        public static bool GetConnectionInfo()
        {

            try
            {

                using (var client = new WebClient())
                using (var stream = client.OpenRead("https://www.google.com/")) //Google is always online.
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }

        }

        // Display text from file in editor upon startup.
        private void GetStartText()
        {

            bool connection = GetConnectionInfo();

            if (connection == true)
            {
                // If there is an internet connection, the program will get the text form the file on github.
                // This is used to change the text without creating an entire update for the app.

                WebClient wbc = new WebClient();

                string startmsg = "";
                startmsg = wbc.DownloadString("https://raw.githubusercontent.com/lucamueller873/WASE/main/startmsg.txt");

                fastColoredTextBox1.Text = startmsg;
                debugConnectLbl.Text = "Internet : TRUE";
            }
            else
            {
                // If there is no connection, error text.

                fastColoredTextBox1.Text = "!-! No Internet-Connection! !-!\nCould not download text!";
                debugConnectLbl.Text = "Internet : FALSE";

            }
        }



        private void Form1_Load(object sender, EventArgs e)
        {
            GetStartText();

            if (debugMode == true)
            {
                debugInfosDropdown.Visible = true;
            }
            else
            {
                debugInfosDropdown.Visible = false;
            }

        }

        private void batchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fastColoredTextBox1.Language = FastColoredTextBoxNS.Language.Custom;
            fastColoredTextBox1.DescriptionFile = "C:\\Users\\" + Environment.UserName + "\\AppData\\Roaming\\WASE\\descFile_BAT.xml"; // AppData folder will be added with install

            visualBasicScriptToolStripMenuItem.Checked = false;
            iNIToolStripMenuItem.Checked = false;
            toolStripStatusLabel1.Text = "Type: Batch, PowerShell";
        }

        private void visualBasicScriptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fastColoredTextBox1.Language = FastColoredTextBoxNS.Language.VB;

            batchToolStripMenuItem.Checked = false;
            iNIToolStripMenuItem.Checked = false;
            toolStripStatusLabel1.Text = "Type: Visual Basic (Script)";
        }

        private void iNIToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fastColoredTextBox1.Language = FastColoredTextBoxNS.Language.Custom;
            fastColoredTextBox1.DescriptionFile = "C:\\Users\\" + Environment.UserName + "\\AppData\\Roaming\\WASE\\descFile_INI.xml"; // AppData folder will be added with install

            batchToolStripMenuItem.Checked = false;
            visualBasicScriptToolStripMenuItem.Checked = false;
            toolStripStatusLabel1.Text = "Type: INI-File";
        }







        private void CUT(object sender, EventArgs e)
        {
            fastColoredTextBox1.Cut();
        }

        private void COPY(object sender, EventArgs e)
        {
            fastColoredTextBox1.Copy();
        }

        private void PASTE(object sender, EventArgs e)
        {
            fastColoredTextBox1.Paste();
        }

        private void SELECTALL(object sender, EventArgs e)
        {
            fastColoredTextBox1.SelectAll();
        }

        private void NEW(object sender, EventArgs e)
        {
            if(MessageBox.Show("Are you sure? Unsaved chnages will be lost!", "WASE", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {

                fastColoredTextBox1.Clear();

            }
        }

        private void OPEN(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Batch|*.bat|PowerShell|*.ps1|Visual Basic (Script)|*.vbs|INI-File|*.INI|All Files|*.*";
            ofd.Title = "Open File";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                path = ofd.FileName;

                StreamReader sr1 = new StreamReader(path);
                fastColoredTextBox1.Text = sr1.ReadToEnd();
                sr1.Close();

                this.Text = "WASE (" + path + ")";
                saved = true;
            }
        }

        private void SAVE(object sender, EventArgs e)
        {
            if (path == String.Empty)
            {

                DialogResult saveResult = saveFileDialog1.ShowDialog();

                if (saveResult == DialogResult.OK)
                {

                    path = saveFileDialog1.FileName;


                    try
                    {

                        StreamWriter SaveStremWriter = new StreamWriter(path);
                        SaveStremWriter.Write(fastColoredTextBox1.Text);
                        SaveStremWriter.Close();

                        this.Text = "WASE (" + path + ")";

                        saved = true;


                    }
                    catch (IOException ioe)
                    {

                        MessageBox.Show("Error Saving File : " + ioe.Message, "WASE", MessageBoxButtons.OK, MessageBoxIcon.Error);


                    }

                }

            }
            else
            {

                try
                {

                    StreamWriter SaveStreamWriter = new StreamWriter(path);
                    SaveStreamWriter.Write(fastColoredTextBox1.Text);
                    SaveStreamWriter.Close();


                    this.Text = "WASE (" + path + ")";

                    saved = true;



                }
                catch (IOException ioe)
                {

                    MessageBox.Show("Error Saving File : " + ioe.Message, "WASE", MessageBoxButtons.OK, MessageBoxIcon.Error);


                }

            }
        }

        private void SAVEAS(object sender, EventArgs e)
        {
            DialogResult saveResult = saveFileDialog1.ShowDialog();

            if (saveResult == DialogResult.OK)
            {

                path = saveFileDialog1.FileName;


                try
                {

                    StreamWriter SaveStremWriter = new StreamWriter(path);
                    SaveStremWriter.Write(fastColoredTextBox1.Text);
                    SaveStremWriter.Close();

                    this.Text = "WASE (*" + path + ")";

                    saved = true;

                }
                catch (IOException ioe)
                {
                    MessageBox.Show("Error Saving File : " + ioe.Message, "WASE", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }

            }
        }

        private void EXIT(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void RUN(object sender, EventArgs e)
        {
            Process.Start(path);
        }

        private void fastColoredTextBox1_TextChanged(object sender, FastColoredTextBoxNS.TextChangedEventArgs e)
        {
            if (saved == true)
            {
                saved = false;
                this.Text = "WASE (*" + path + ")";
            }

        }

        private void ABOUT(object sender, EventArgs e)
        {
            MessageBox.Show("Creator: Luca Müller\nCopyright: (C) Luca Müller 2022\nVersion: 1.0\n\nIcons: Yusuke Kamiyamane", "WASE About", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void terminalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("cmd.exe");
        }
    }
}
