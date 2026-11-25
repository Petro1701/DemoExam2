using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DemoExam2
{
    public partial class MainForm : Form
    {
        private DataTable dataTable = new DataTable();
        public MainForm()
        {
            InitializeComponent();
            labelNameUser.Text = CurrentUser.Name;
            SetupDataGridView();
            LoadData();
        }


        /// <summary>
        /// Настройка колонок на dataGridView
        /// </summary>
        private void SetupDataGridView()
        {
            dataGridView1.AutoGenerateColumns = false;
            DataGridViewTextBoxColumn idColumn = new DataGridViewTextBoxColumn()
            {
                Name = "ID",
                DataPropertyName = "id",
                Visible = false
            };
            DataGridViewTextBoxColumn nameColumn = new DataGridViewTextBoxColumn()
            {
                Name = "Name",
                DataPropertyName = "nameStudent",
                HeaderText = "Имя студента",
                Width = 100
            };
            DataGridViewTextBoxColumn emailColumn = new DataGridViewTextBoxColumn()
            {
                Name = "Email",
                DataPropertyName = "emailStudent",
                HeaderText = "Имя студента",
                Width = 100
            };
            DataGridViewImageColumn photoColumn = new DataGridViewImageColumn()
            {
                Name = "Photo",
                DataPropertyName = "PhotoBytesStudent",
                HeaderText = "Фото",
                Width = 100,
                ImageLayout = DataGridViewImageCellLayout.Stretch
            };
            DataGridViewButtonColumn editColumn = new DataGridViewButtonColumn()
            {
                Name = "Edit",
                HeaderText = string.Empty,
                Text = "✏",
                Width = 30
            };
            DataGridViewButtonColumn deleteColumn = new DataGridViewButtonColumn()
            {
                Name = "Delete",
                HeaderText = string.Empty,
                Text = "🗑",
                Width = 30
            };
            dataGridView1.Columns.AddRange(idColumn,nameColumn, emailColumn, photoColumn, editColumn, deleteColumn);
        }

        /// <summary>
        /// Загрузка данных из БД test.students на dataTable => dataGridView1
        /// </summary>
        private void LoadData()
        {
            using (MySqlConnection connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["connection"].ConnectionString))
            using (MySqlCommand command = new MySqlCommand())
            {
                command.Connection = connection;
                command.CommandText = "SELECT id, nameStudent, emailStudent,PhotoBytesStudent FROM students;";
                connection.Open();
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    dataTable.Load(reader);
                    dataGridView1.DataSource = dataTable;
                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
