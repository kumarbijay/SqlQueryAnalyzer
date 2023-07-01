using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace SqlQueryAnalyzer
{
    public partial class MainForm : Form
    {
        private TextBox queryTextBox;
        private RichTextBox resultTextBox;
        private Button analyzeButton;
        private Button copyButton;
        private Button analyzeTempTbl;
        private Label label1;
        public MainForm()
        {
            InitializeComponent();

            // Create the form and its controls
            Size = new Size(1100, 750);
            Text = "SQL Query Analyzer - By Bijay Kumar";

            queryTextBox = new TextBox();
            queryTextBox.Multiline = true;
            queryTextBox.ScrollBars = ScrollBars.Both;
            queryTextBox.Font = new Font(FontFamily.GenericMonospace, 10f);
            queryTextBox.Location = new Point(10, 10);
            queryTextBox.Width = (int)(Width * 0.45);
            queryTextBox.Height = (int)(Height * 0.80);

            resultTextBox = new RichTextBox();
            resultTextBox.Multiline = true;
            resultTextBox.ScrollBars = RichTextBoxScrollBars.Both;
            resultTextBox.Font = new Font(FontFamily.GenericMonospace, 10f);
            resultTextBox.Location = new Point(queryTextBox.Right + (int)(Width * 0.05), 10);
            resultTextBox.Width = (int)(Width * 0.45);
            resultTextBox.Height = (int)(Height * 0.80);

            analyzeButton = new Button();
            analyzeButton.Text = "Analyze NOLOCKS";
            analyzeButton.Location = new Point(10, queryTextBox.Bottom + 10);
            analyzeButton.Size = new Size(200, 40);
            analyzeButton.Click += AnalyzeButton_Click;

            analyzeTempTbl = new Button();
            analyzeTempTbl.Text = "Analyze Temp Tables";
            analyzeTempTbl.Location = new Point(analyzeButton.Right + 10, queryTextBox.Bottom + 10 );
            analyzeTempTbl.Size = new Size(200, 40);
            analyzeTempTbl.Click += AnalyzeTempTblButton_Click;


            copyButton = new Button();
            copyButton.Text = "Copy Output";
            copyButton.Location = new Point(resultTextBox.Right - 150, resultTextBox.Bottom + 10);
            copyButton.Size = new Size(150, 40);
            copyButton.Click += CopyButton_Click;

            label1 = new Label();
            label1.Text = "Crafted By Bijay Kumar";
            label1.ForeColor = Color.RosyBrown;
            label1.Location = new Point(resultTextBox.Left-110, analyzeButton.Bottom + 2);
            label1.Width = 220;
            label1.Height = 30;

            Controls.Add(queryTextBox);
            Controls.Add(resultTextBox);
            Controls.Add(analyzeButton);
            Controls.Add(analyzeTempTbl);
            Controls.Add(copyButton);
            Controls.Add(label1);

            // Add event handler for the Resize event
            Resize += MainForm_Resize;
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            // Update the width and height of the text boxes based on the current form size
            queryTextBox.Width = (int)(Width * 0.45);
            queryTextBox.Height = (int)(Height * 0.80);

            resultTextBox.Location = new Point(queryTextBox.Right + (int)(Width * 0.05), 10);
            resultTextBox.Width = (int)(Width * 0.45);
            resultTextBox.Height = (int)(Height * 0.80);

            analyzeButton.Location = new Point(10, queryTextBox.Bottom + 10);
            copyButton.Location = new Point(resultTextBox.Right - 150, resultTextBox.Bottom + 10);
            label1.Location = new Point(resultTextBox.Left - 110, analyzeButton.Bottom + 2);
        }

        private void AnalyzeButton_Click(object sender, EventArgs e)
        {
            string query = queryTextBox.Text;
            string result = AnalyzeNoLocks(query);
            resultTextBox.Clear();
            resultTextBox.Text = result;
            HighlightNoLocks();
        }
        private void AnalyzeTempTblButton_Click(object sender, EventArgs e)
        {
            resultTextBox.Clear();
            string query = queryTextBox.Text;
            string result = AnalyzeTempTables(query);
            resultTextBox.Text = result;
        }
        private void HighlightNoLocks()
        {
            string pattern = @"\bWITH\s*\(NOLOCK\)";
            MatchCollection matches = Regex.Matches(resultTextBox.Text, pattern, RegexOptions.IgnoreCase);

            foreach (Match match in matches)
            {
                int startIndex = match.Index;
                int length = match.Length;

                resultTextBox.Select(startIndex, length);
                resultTextBox.SelectionBackColor = Color.Yellow;
                resultTextBox.DeselectAll();
            }
            pattern = @"\b(FROM|JOIN)\b\s+\S+\b";
            matches = Regex.Matches(resultTextBox.Text, pattern, RegexOptions.IgnoreCase);

            foreach (Match match in matches)
            {
                int startIndex = match.Index;
                int length = match.Length;

                resultTextBox.Select(startIndex, length);
                resultTextBox.SelectionBackColor = Color.LightCyan;
                resultTextBox.DeselectAll();
            }
        }

        private string AnalyzeNoLocks(string query)
        {
            // Analyze the query and add NOLOCK where necessary
            string pattern = @"\bFROM\b\s+(\S+)(?!.*\bWITH\s*\(NOLOCK\))";
            string replacement = "FROM $1 WITH (NOLOCK)";
            string result = Regex.Replace(query, pattern, replacement, RegexOptions.IgnoreCase);

            //Aldo add NOLOCK hints in JOIN statements
            pattern = @"\bJOIN\b\s+(\S+)(?!.*\bWITH\s*\(NOLOCK\))";
            replacement = "JOIN $1 WITH (NOLOCK)";
            result = Regex.Replace(result, pattern, replacement, RegexOptions.IgnoreCase);

            return result;
        }


        private string AnalyzeTempTables(string queries)
        {
            string result="";

            // Analyze created temp tables
            string createPattern = @"(?i)CREATE\s+TABLE\s+#\w+\b";
            MatchCollection createMatches = Regex.Matches(queries, createPattern);

            if (createMatches.Count > 0)
            {
                result += "\n\nCreated Temp Tables:\n";
                foreach (Match match in createMatches)
                {
                    result += match.Value + "\n";
                }
            }

            // Analyze dropped temp tables
            string dropPattern = @"(?i)DROP\s+TABLE\s+#\w+\b";
            MatchCollection dropMatches = Regex.Matches(queries, dropPattern);

            if (dropMatches.Count > 0)
            {
                result += "\n\nDropped Temp Tables:\n";
                foreach (Match match in dropMatches)
                {
                    result += match.Value + "\n";
                }
            }

            // Analyze stored procedures
            string spPattern = @"(?i)CREATE\s+PROCEDURE\s+\w+\b";
            MatchCollection spMatches = Regex.Matches(queries, spPattern);

            if (spMatches.Count > 0)
            {
                result += "\n\nStored Procedures:\n";
                foreach (Match match in spMatches)
                {
                    result += match.Value + "\n";
                }
            }

            return result;
        }

        private void CopyButton_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(resultTextBox.Text);
        }

    }
}