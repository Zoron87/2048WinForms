namespace _2048WinFormsApp
{
    public partial class MainForm : Form
    {
        private int mapSize;
        private Label[,] labelsMap;
        private static Random random = new Random();
        private int score = 0;
        int number;
        User user;
        IUserResultsStorage userResults;

        public MainForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            StartSettingsForm userNameForm = new StartSettingsForm();
            userNameForm.ShowDialog();

            user = new User(userNameForm.userName);
            mapSize = userNameForm.mapSize;

            userResults = new UserResultsStorageJson();
            bestScoreLabel.Text = GetBestScore().ToString();

            InitMap();
            GenerateNumber();
            ShowScore();
        }

        private int GetBestScore()
        {
            var allStatistics = userResults.GetAll();
            return allStatistics.Select(x => x.Score).Max(); ;
        }

        private void ShowScore()
        {
            scoreLabel.Text = score.ToString();
        }

        private void InitMap()
        {
            labelsMap = new Label[mapSize, mapSize];

            for (int i = 0; i < mapSize; i++)
            {
                for (int j = 0; j < mapSize; j++)
                {
                    var newLabel = CreateLabel(i, j, i * mapSize + j);
                    Controls.Add(newLabel);
                    labelsMap[i, j] = newLabel;
                }
                labelsMap[i, mapSize - 1].Margin = new Padding(0, 0, 0, 10);
            }
        }

        private void GenerateNumber()
        {
            while (true)
            {
                if (isFullMap())
                {
                    user.Score = score;
                    userResults.SaveAll(user);

                    if (MessageBox.Show("���� ��������! ��������� ���� ��������?", "Game Over!", MessageBoxButtons.OKCancel) == DialogResult.OK)
                        Application.Restart();
                    break;
                }
                var randomNumberLabel = random.Next(mapSize * mapSize);
                var indexRow = randomNumberLabel / mapSize;
                var indexColumn = randomNumberLabel % mapSize;

                if (labelsMap[indexRow, indexColumn].Text == string.Empty)
                {
                    labelsMap[indexRow, indexColumn].Text = (random.Next(100) < 75) ? "2" : "4";
                    ChangeLabelBackColor(labelsMap[indexRow, indexColumn]);
                    break;
                }
            }
        }

        private bool isFullMap()
        {
            for (int i = 0; i < mapSize; i++)
                for (int j = 0; j < mapSize; j++)
                {
                    if (labelsMap[i, j].Text == String.Empty) return false;
                }
            return true;
        }

        private Label CreateLabel(int indexRow, int indexColumn, int number)
        {
            var label = new Label();

            label.BackColor = Color.Gray;
            label.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point);
            label.ForeColor = Color.Black;
            label.Size = new Size(70, 70);
            label.TextAlign = ContentAlignment.MiddleCenter;
            int x = 10 + indexColumn * 76;
            int y = 70 + indexRow * 76;
            label.Location = new Point(x, y);
            label.TextChanged += Label_TextChanged;

            return label;
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Right)
            {
                ArrowKeysRight();
                GenerateNumber();
            }

            if (e.KeyCode == Keys.Left)
            {
                ArrowKeysLeft();
                GenerateNumber();
            }

            if (e.KeyCode == Keys.Up)
            {
                ArrowKeysUp();
                GenerateNumber();
            }

            if (e.KeyCode == Keys.Down)
            {
                ArrowKeysDown();
                GenerateNumber();
            }
            ShowScore();
        }

        private void ArrowKeysDown()
        {
            for (int j = 0; j < mapSize; j++)
            {
                for (int i = mapSize - 1; i >= 0; i--)
                {
                    if (labelsMap[i, j].Text != String.Empty)
                    {
                        for (int k = i - 1; k >= 0; k--)
                        {
                            if (labelsMap[k, j].Text != String.Empty)
                            {
                                if (labelsMap[i, j].Text == labelsMap[k, j].Text)
                                {
                                    number = int.Parse(labelsMap[i, j].Text);
                                    score += number * 2;

                                    labelsMap[i, j].Text = (number * 2).ToString();

                                    labelsMap[k, j].Text = String.Empty;
                                    labelsMap[k, j].BackColor = Color.Gray;
                                }
                                break;
                            }
                        }
                    }
                }
            }

            for (int j = 0; j < mapSize; j++)
            {
                for (int i = mapSize - 1; i >= 0; i--)
                {
                    if (labelsMap[i, j].Text == String.Empty)
                    {
                        for (int k = i - 1; k >= 0; k--)
                        {
                            if (labelsMap[k, j].Text != String.Empty)
                            {
                                labelsMap[i, j].Text = labelsMap[k, j].Text;

                                labelsMap[k, j].Text = String.Empty;
                                labelsMap[k, j].BackColor = Color.Gray;
                                break;
                            }
                        }
                    }
                }
            }
        }

        private void ArrowKeysUp()
        {
            for (int j = 0; j < mapSize; j++)
            {
                for (int i = 0; i < mapSize; i++)
                {
                    if (labelsMap[i, j].Text != String.Empty)
                    {
                        for (int k = i + 1; k < mapSize; k++)
                        {
                            if (labelsMap[k, j].Text != String.Empty)
                            {
                                if (labelsMap[i, j].Text == labelsMap[k, j].Text)
                                {
                                    number = int.Parse(labelsMap[i, j].Text);
                                    score += number * 2;

                                    labelsMap[i, j].Text = (number * 2).ToString();

                                    labelsMap[k, j].Text = String.Empty;
                                    labelsMap[k, j].BackColor = Color.Gray;
                                }
                                break;
                            }
                        }
                    }
                }
            }

            for (int j = 0; j < mapSize; j++)
            {
                for (int i = 0; i < mapSize; i++)
                {
                    if (labelsMap[i, j].Text == String.Empty)
                    {
                        for (int k = i + 1; k < mapSize; k++)
                        {
                            if (labelsMap[k, j].Text != String.Empty)
                            {
                                labelsMap[i, j].Text = labelsMap[k, j].Text;

                                labelsMap[k, j].Text = String.Empty;
                                labelsMap[k, j].BackColor = Color.Gray;
                                break;
                            }
                        }
                    }
                }
            }
        }

        private void ArrowKeysLeft()
        {
            for (int i = 0; i < mapSize; i++)
            {
                for (int j = 0; j < mapSize; j++)
                {
                    if (labelsMap[i, j].Text != String.Empty)
                    {
                        for (int k = j + 1; k < mapSize; k++)
                        {
                            if (labelsMap[i, k].Text != String.Empty)
                            {
                                if (labelsMap[i, j].Text == labelsMap[i, k].Text)
                                {
                                    number = int.Parse(labelsMap[i, j].Text);
                                    score += number * 2;

                                    labelsMap[i, j].Text = (number * 2).ToString();

                                    labelsMap[i, k].Text = String.Empty;
                                    labelsMap[i, k].BackColor = Color.Gray;
                                }
                                break;
                            }
                        }
                    }
                }
            }

            for (int i = 0; i < mapSize; i++)
            {
                for (int j = 0; j < mapSize; j++)
                {
                    if (labelsMap[i, j].Text == String.Empty)
                    {
                        for (int k = j + 1; k < mapSize; k++)
                        {
                            if (labelsMap[i, k].Text != String.Empty)
                            {
                                labelsMap[i, j].Text = labelsMap[i, k].Text;

                                labelsMap[i, k].Text = String.Empty;
                                labelsMap[i, k].BackColor = Color.Gray;
                                break;
                            }
                        }
                    }
                }
            }
        }

        private void ArrowKeysRight()
        {
            for (int i = 0; i < mapSize; i++)
            {
                for (int j = mapSize - 1; j >= 0; j--)
                {
                    if (labelsMap[i, j].Text != String.Empty)
                    {
                        for (int k = j - 1; k >= 0; k--)
                        {
                            if (labelsMap[i, k].Text != String.Empty)
                            {
                                if (labelsMap[i, j].Text == labelsMap[i, k].Text)
                                {
                                    number = int.Parse(labelsMap[i, j].Text);
                                    score += number * 2;

                                    labelsMap[i, j].Text = (number * 2).ToString();

                                    labelsMap[i, k].Text = String.Empty;
                                    labelsMap[i, k].BackColor = Color.Gray;
                                }
                                break;
                            }
                        }
                    }
                }
            }

            for (int i = 0; i < mapSize; i++)
            {
                for (int j = mapSize - 1; j >= 0; j--)
                {
                    if (labelsMap[i, j].Text == String.Empty)
                    {
                        for (int k = j - 1; k >= 0; k--)
                        {
                            if (labelsMap[i, k].Text != String.Empty)
                            {
                                labelsMap[i, j].Text = labelsMap[i, k].Text;

                                labelsMap[i, k].Text = String.Empty;
                                labelsMap[i, k].BackColor = Color.Gray;
                                break;
                            }
                        }
                    }
                }
            }
        }

        private Color ChangeLabelBackColor(Label currentLabel)
        {
            switch (currentLabel.Text)
            {
                case "2":
                    currentLabel.BackColor = ColorTranslator.FromHtml("#eee4da");
                    break;

                case "4":
                    currentLabel.BackColor = ColorTranslator.FromHtml("#ede0c8");
                    break;

                case "8":
                    currentLabel.BackColor = ColorTranslator.FromHtml("#f2b179");
                    break;

                case "16":
                    currentLabel.BackColor = ColorTranslator.FromHtml("#f59563");
                    break;

                case "32":
                    currentLabel.BackColor = ColorTranslator.FromHtml("#f67c5f");
                    break;

                case "64":
                    currentLabel.BackColor = ColorTranslator.FromHtml("#f65e3b");
                    break;

                case "128":
                    currentLabel.BackColor = ColorTranslator.FromHtml("#edcf72");
                    break;

                case "256":
                    currentLabel.BackColor = ColorTranslator.FromHtml("#edcc61");
                    break;

                case "512":
                    currentLabel.BackColor = ColorTranslator.FromHtml("#edc850");
                    break;

                case "1024":
                    currentLabel.BackColor = ColorTranslator.FromHtml("#edc53f");
                    break;

                case "2048":
                    currentLabel.BackColor = Color.Purple;
                    break;
            }
            return Color.Gray;
        }

        private void Label_TextChanged(object sender, EventArgs e)
        {
            ChangeLabelBackColor((Label)sender);
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            GameRulesForm gameRules = new GameRulesForm();
            gameRules.ShowDialog();
        }

        private void �����������������ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }

        private void �����ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void ��������������ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GameStatisticForm gameStatisticForm = new GameStatisticForm();
            gameStatisticForm.Show();
        }
    }
}