using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Configuration;

namespace DemoExam2
{
    public partial class SignIn : Form
    {
        public SignIn()
        {
            InitializeComponent();
        }

        private void pictureBoxEyeClose_Click(object sender, EventArgs e)
        {
            textBoxPassword.UseSystemPasswordChar = false;
            pictureBoxEyeOpen.Visible = true;
            pictureBoxEyeClose.Visible = false;
        }

        private void pictureBoxEyeOpen_Click(object sender, EventArgs e)
        {
            textBoxPassword.UseSystemPasswordChar = true;
            pictureBoxEyeClose.Visible = true;
            pictureBoxEyeOpen.Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (MySqlConnection connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["connection"].ConnectionString))
            using (MySqlCommand command = new MySqlCommand())
            {
                command.Connection = connection;
                command.CommandText = "SELECT * FROM register WHERE login_user = @login and password_user = @password;";
                command.Parameters.AddWithValue("@login", textBoxName.Text);
                command.Parameters.AddWithValue("@password", textBoxPassword.Text);
                connection.Open();
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        CurrentUser.Name = reader["login_user"].ToString();
                        MainForm mainForm = new MainForm();
                        mainForm.Show();
                        this.Hide();
                        MessageBox.Show($"Добро пожаловать {CurrentUser.Name}!");
                    }
                    else MessageBox.Show("Неправильный логин или пароль");
                }
            }
        }
    }
}
