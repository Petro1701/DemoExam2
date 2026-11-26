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
            dataGridView1.RowTemplate.Height = 100; //Высота строк
            DataGridViewTextBoxColumn idColumn = new DataGridViewTextBoxColumn()
            {
                Name = "ID",
                DataPropertyName = "id",
                Visible = false
            };
            DataGridViewImageColumn photoColumn = new DataGridViewImageColumn()
            {
                Name = "Photo",
                DataPropertyName = "PhotoBytesStudent",
                HeaderText = "Фото",
                Width = 100,
                ImageLayout = DataGridViewImageCellLayout.Stretch
            };
            DataGridViewTextBoxColumn nameColumn = new DataGridViewTextBoxColumn()
            {
                Name = "Name",
                DataPropertyName = "nameStudent",
                HeaderText = "Имя студента",
                Width = 100
            };
            DataGridViewTextBoxColumn birthDateColumn = new DataGridViewTextBoxColumn()
            {
                Name = "BirthDate",
                DataPropertyName = "birthDate",
                HeaderText = "Дата рождения",
                Width = 100,
                DefaultCellStyle = new DataGridViewCellStyle() { Format = "dd.MM.yyyy" }
            };
            DataGridViewTextBoxColumn emailColumn = new DataGridViewTextBoxColumn()
            {
                Name = "Email",
                DataPropertyName = "emailStudent",
                HeaderText = "Имя студента",
                Width = 100
            };
            DataGridViewButtonColumn editColumn = new DataGridViewButtonColumn()
            {
                Name = "Edit",
                HeaderText = string.Empty,
                Text = "✏",
                UseColumnTextForButtonValue = true,
                Width = 30
            };
            DataGridViewButtonColumn deleteColumn = new DataGridViewButtonColumn()
            {
                Name = "Delete",
                HeaderText = string.Empty,
                Text = "🗑",
                UseColumnTextForButtonValue = true,
                Width = 30
            };
            dataGridView1.Columns.AddRange(idColumn, photoColumn, nameColumn,birthDateColumn, emailColumn, editColumn, deleteColumn);
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
                command.CommandText = "SELECT id, nameStudent,birthDate, emailStudent,PhotoBytesStudent FROM students;";
                connection.Open();
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    dataTable.Load(reader);
                    dataGridView1.DataSource = dataTable;
                }
            }
        }

        /// <summary>
        /// Удаление студента из БД test.students и из dataTable
        /// </summary>
        /// <param name="id">Идентификатор студента</param>
        /// <param name="rowIndex">Индекс строки</param>
        private void DeleteStudent(int id, int rowIndex)
        {
            using (MySqlConnection connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["connection"].ConnectionString))
            using (MySqlCommand command = new MySqlCommand())
            {
                command.Connection = connection;
                command.CommandText = "DELETE FROM students WHERE id = @id;";
                command.Parameters.AddWithValue("@id", id);
                connection.Open();
                if (command.ExecuteNonQuery() > 0) dataTable.Rows.RemoveAt(rowIndex);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            if (e.ColumnIndex == dataGridView1.Columns["Delete"].Index) DeleteStudent((int) dataGridView1.Rows[e.RowIndex].Cells["ID"].Value, e.RowIndex);

            if(e.ColumnIndex == dataGridView1.Columns["Edit"].Index)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                Student student = new Student()
                {
                    Id = Convert.ToInt32(row.Cells["ID"].Value),
                    Name = row.Cells["Name"].Value.ToString(),
                    Email = row.Cells["Email"].Value.ToString(),
                    PhotoBytes = (byte[]) row.Cells["Photo"].Value
                };
                using (EditStudentForm editStudentForm = new EditStudentForm(student))
                {
                    if (editStudentForm.ShowDialog() == DialogResult.OK) LoadData();
                }
            }
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void buttonAddStudent_Click(object sender, EventArgs e)
        {
            using (AddStudentForm addStudentForm = new AddStudentForm())
            {
                if (addStudentForm.ShowDialog() == DialogResult.OK) LoadData();
            }
        }
    }
}
