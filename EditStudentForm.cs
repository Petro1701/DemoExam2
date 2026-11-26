using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DemoExam2
{
    public partial class EditStudentForm : AddStudentForm
    {
        /// <summary>
        /// Хранит всю строку данных студента из MainForm
        /// </summary>
        private Student student;

        public EditStudentForm(Student student)
        {
            InitializeComponent();
            labelNameOnTopPanel.Text = "Редактирование студента";
            panel1.BackColor = Color.Navy;
            textBoxName.Text = student.Name;
            textBoxEmail.Text = student.Email;
            using (MemoryStream ms = new MemoryStream(student.PhotoBytes))
            {
                pictureBoxPhoto.Image = Image.FromStream(ms);
            }
            this.student = student;
        }

        /// <summary>
        /// Сохранение изменений в БД
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnSaveClick(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(textBoxName.Text) || string.IsNullOrWhiteSpace(textBoxEmail.Text))
                {
                    MessageBox.Show("Остались пустые поля, заполните их");
                    return;
                }
                student.Name = textBoxName.Text;
                student.Email = textBoxEmail.Text;
                using (MySqlConnection connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["connection"].ConnectionString))
                using (MySqlCommand command = new MySqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "UPDATE students SET nameStudent = @name, emailStudent = @email, PhotoBytesStudent = @photoBytes WHERE id = @id";
                    command.Parameters.AddWithValue("@name", student.Name);
                    command.Parameters.AddWithValue("@email", student.Email);
                    command.Parameters.AddWithValue("@photoBytes", student.PhotoBytes);
                    command.Parameters.AddWithValue("@id", student.Id);
                    connection.Open();
                    if (command.ExecuteNonQuery() > 0) DialogResult = DialogResult.OK;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        /// <summary>
        /// Изменение фото студента
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void ChoosePhoto(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Изображение | *.png; *.jpeg; *.jpg; *.bmp";
                ofd.Title = "Выберите фото";
                ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
                if(ofd.ShowDialog() == DialogResult.OK)
                {
                    student.PhotoBytes = File.ReadAllBytes(ofd.FileName);
                    pictureBoxPhoto.Image = Image.FromFile(ofd.FileName);
                }
            }
        }
    }
}
