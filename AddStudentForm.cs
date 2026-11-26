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
    public partial class AddStudentForm : Form
    {
        private byte[] photoBytes = null;
        public AddStudentForm()
        {
            InitializeComponent();
        }

        protected virtual void OnSaveClick(object sender, EventArgs e)
        {
            if(string.IsNullOrWhiteSpace(textBoxName.Text) || string.IsNullOrWhiteSpace(textBoxEmail.Text))
            {
                MessageBox.Show("Заполните все поля");
                textBoxName.Focus();
                return;
            }
            if(photoBytes == null)
            {
                MessageBox.Show("добавьте изображение пользователю");
                buttonChoosePhoto.Focus();
                return;
            }
            try
            {
                using (MySqlConnection connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["connection"].ConnectionString))
                using (MySqlCommand command = new MySqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "INSERT INTO students (nameStudent, emailStudent,PhotoBytesStudent) VALUES (@name, @email, @photoBytes)";
                    command.Parameters.AddWithValue("@name", textBoxName.Text);
                    command.Parameters.AddWithValue("@email", textBoxEmail.Text);
                    command.Parameters.AddWithValue("@photoBytes", photoBytes);
                    connection.Open();
                    if (command.ExecuteNonQuery() > 0)
                    {
                        MessageBox.Show($"Пользователь {textBoxName.Text} сохранен");
                        DialogResult = DialogResult.OK;
                    }   
                    else MessageBox.Show("Не удалось сохранить пользователя");
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void buttonSave_Click(object sender, EventArgs e)
        {
            OnSaveClick(sender, e);
        }

        protected virtual void ChoosePhoto(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Изображение | *.png;*.jpeg;*.jpg;*.bmp";
                ofd.Title = "Выберите фото";
                ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
                if(ofd.ShowDialog() == DialogResult.OK)
                {
                    photoBytes = File.ReadAllBytes(ofd.FileName);
                    pictureBoxPhoto.Image = Image.FromFile(ofd.FileName);
                }
            }
        }
        private void buttonChoosePhoto_Click(object sender, EventArgs e)
        {
            ChoosePhoto(sender, e);
        }
    }
}
