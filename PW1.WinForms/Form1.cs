using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using PW1.Model;

namespace PW1.WinForms
{
    public partial class Form1 : Form
    {
        private Logic _logic = new Logic();

        // Элементы управления
        private DataGridView dataGridView;
        private TextBox txtName, txtClan, txtProfession, txtAge, txtBeard, txtSearchProf;
        private Button btnAdd, btnUpdate, btnDelete, btnClear, btnGroup, btnSearch;
        private Label lblSearchResult;

        public Form1()
        {
            // ВНИМАНИЕ: строку InitializeComponent(); мы удалили,
            // потому что интерфейс строится кодом в SetupUI().
            SetupUI();
            LoadData();
        }

        private void SetupUI()
        {
            this.Text = "Реестр Гномов (Практическая работа №1)";
            this.Size = new Size(950, 600);

            // --- Таблица (DataGridView) ---
            dataGridView = new DataGridView
            {
                Location = new Point(10, 10),
                Size = new Size(650, 400),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                ReadOnly = true
            };
            dataGridView.SelectionChanged += DataGridView_SelectionChanged;
            this.Controls.Add(dataGridView);

            // --- Панель ввода данных ---
            Panel inputPanel = new Panel { Location = new Point(670, 10), Size = new Size(250, 400) };

            inputPanel.Controls.Add(new Label { Text = "Имя гнома:", Location = new Point(10, 10) });
            txtName = new TextBox { Location = new Point(10, 30), Width = 200 };
            inputPanel.Controls.Add(txtName);

            inputPanel.Controls.Add(new Label { Text = "Клан:", Location = new Point(10, 60) });
            txtClan = new TextBox { Location = new Point(10, 80), Width = 200 };
            inputPanel.Controls.Add(txtClan);

            inputPanel.Controls.Add(new Label { Text = "Профессия:", Location = new Point(10, 110) });
            txtProfession = new TextBox { Location = new Point(10, 130), Width = 200 };
            inputPanel.Controls.Add(txtProfession);

            inputPanel.Controls.Add(new Label { Text = "Возраст:", Location = new Point(10, 160) });
            txtAge = new TextBox { Location = new Point(10, 180), Width = 200 };
            inputPanel.Controls.Add(txtAge);

            inputPanel.Controls.Add(new Label { Text = "Длина бороды (см):", Location = new Point(10, 210) });
            txtBeard = new TextBox { Location = new Point(10, 230), Width = 200 };
            inputPanel.Controls.Add(txtBeard);

            btnAdd = new Button { Text = "Добавить", Location = new Point(10, 270), Width = 95, BackColor = Color.LightGreen };
            btnAdd.Click += BtnAdd_Click;
            inputPanel.Controls.Add(btnAdd);

            btnUpdate = new Button { Text = "Изменить", Location = new Point(115, 270), Width = 95, BackColor = Color.LightBlue };
            btnUpdate.Click += BtnUpdate_Click;
            inputPanel.Controls.Add(btnUpdate);

            btnDelete = new Button { Text = "Удалить", Location = new Point(10, 310), Width = 95, BackColor = Color.LightCoral };
            btnDelete.Click += BtnDelete_Click;
            inputPanel.Controls.Add(btnDelete);

            btnClear = new Button { Text = "Очистить поля", Location = new Point(115, 310), Width = 95 };
            btnClear.Click += (s, e) => ClearInputs();
            inputPanel.Controls.Add(btnClear);

            this.Controls.Add(inputPanel);

            // --- Панель бизнес-логики ---
            Panel logicPanel = new Panel { Location = new Point(10, 420), Size = new Size(910, 130), BorderStyle = BorderStyle.FixedSingle };

            logicPanel.Controls.Add(new Label { Text = "Бизнес-функции:", Location = new Point(10, 10), Font = new Font("Arial", 10, FontStyle.Bold) });

            btnGroup = new Button { Text = "Собрать по кланам", Location = new Point(10, 40), Width = 200, Height = 30 };
            btnGroup.Click += BtnGroup_Click;
            logicPanel.Controls.Add(btnGroup);

            logicPanel.Controls.Add(new Label { Text = "Поиск по профессии:", Location = new Point(230, 45) });
            txtSearchProf = new TextBox { Location = new Point(370, 43), Width = 150 };
            logicPanel.Controls.Add(txtSearchProf);

            btnSearch = new Button { Text = "Найти", Location = new Point(530, 41), Width = 80, Height = 25 };
            btnSearch.Click += BtnSearch_Click;
            logicPanel.Controls.Add(btnSearch);

            lblSearchResult = new Label { Location = new Point(10, 80), AutoSize = true, ForeColor = Color.DarkBlue };
            logicPanel.Controls.Add(lblSearchResult);

            this.Controls.Add(logicPanel);
        }

        private void LoadData()
        {
            dataGridView.DataSource = null;
            dataGridView.DataSource = _logic.GetAllGnomes();
            if (dataGridView.Columns.Count > 0 && dataGridView.Columns["Id"] != null)
            {
                dataGridView.Columns["Id"].Visible = false; // Скрываем ID
            }
        }

        private void ClearInputs()
        {
            txtName.Clear(); txtClan.Clear(); txtProfession.Clear(); txtAge.Clear(); txtBeard.Clear();
        }

        private void DataGridView_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count > 0)
            {
                var gnome = (Gnome)dataGridView.SelectedRows[0].DataBoundItem;
                txtName.Text = gnome.Name;
                txtClan.Text = gnome.Clan;
                txtProfession.Text = gnome.Profession;
                txtAge.Text = gnome.Age.ToString();
                txtBeard.Text = gnome.BeardLength.ToString();
            }
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs()) return;
            _logic.AddGnome(txtName.Text, txtClan.Text, txtProfession.Text, int.Parse(txtAge.Text), int.Parse(txtBeard.Text));
            LoadData();
            ClearInputs();
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count == 0) { MessageBox.Show("Выберите гнома для редактирования!"); return; }
            if (!ValidateInputs()) return;

            var selectedGnome = (Gnome)dataGridView.SelectedRows[0].DataBoundItem;
            _logic.UpdateGnome(selectedGnome.Id, txtName.Text, txtClan.Text, txtProfession.Text, int.Parse(txtAge.Text), int.Parse(txtBeard.Text));
            LoadData();
            ClearInputs();
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count == 0) { MessageBox.Show("Выберите гнома для удаления!"); return; }

            var selectedGnome = (Gnome)dataGridView.SelectedRows[0].DataBoundItem;
            if (MessageBox.Show($"Удалить гнома \"{selectedGnome.Name}\"?", "Подтверждение", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                _logic.DeleteGnome(selectedGnome.Id);
                LoadData();
                ClearInputs();
            }
        }

        private void BtnGroup_Click(object sender, EventArgs e)
        {
            var grouped = _logic.GroupGnomesByClan();
            string result = "Группировка гномов по кланам:\n\n";
            foreach (var group in grouped)
            {
                result += $"Клан {group.Key} ({group.Value.Count} гномов):\n";
                foreach (var g in group.Value)
                {
                    result += $"   - {g.Name} ({g.Profession}, {g.Age} лет)\n";
                }
                result += "\n";
            }
            MessageBox.Show(result, "Результат группировки", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            string prof = txtSearchProf.Text.Trim();
            if (string.IsNullOrEmpty(prof)) { MessageBox.Show("Введите профессию для поиска."); return; }

            var results = _logic.GetGnomesByProfession(prof);
            if (results.Count == 0)
            {
                lblSearchResult.Text = "Гномов с такой профессией не найдено.";
            }
            else
            {
                lblSearchResult.Text = $"Найдено {results.Count} гномов:\n" + string.Join("\n", results.Select(g => $"{g.Age} лет - {g.Name} (Борода: {g.BeardLength} см)"));
            }
        }

        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(txtName.Text) || string.IsNullOrWhiteSpace(txtClan.Text) ||
                string.IsNullOrWhiteSpace(txtProfession.Text) || string.IsNullOrWhiteSpace(txtAge.Text) ||
                string.IsNullOrWhiteSpace(txtBeard.Text))
            {
                MessageBox.Show("Заполните все поля!");
                return false;
            }
            if (!int.TryParse(txtAge.Text, out _) || !int.TryParse(txtBeard.Text, out _))
            {
                MessageBox.Show("Возраст и длина бороды должны быть целыми числами!");
                return false;
            }
            return true;
        }
    }
}