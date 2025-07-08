using System;
using System.Linq;
using System.Windows.Forms;
using MaterialSkin;
using MaterialSkin.Controls;
using ClothingStoreApp.Data;

namespace ClothingStoreApp
{
    public partial class LoginForm : MaterialForm
    {
        private readonly AppDbContext _context;

        private MaterialTextBox? txtUsername;
        private MaterialTextBox2? txtPassword;
        private MaterialButton? btnLogin;
        private MaterialButton? btnRegister;

        public LoginForm()
        {

            var skinManager = MaterialSkinManager.Instance;
            skinManager.AddFormToManage(this);
            skinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            skinManager.ColorScheme = new ColorScheme(
                Primary.Blue600, Primary.Blue700,
                Primary.Blue200, Accent.Blue200,
                TextShade.WHITE
            );

            _context = new AppDbContext();
            SetupUI();
        }

        private void SetupUI()
        {
            this.Text = "Login";
            this.Size = new System.Drawing.Size(400, 500); // العرض 800، الطول 600


            txtUsername = new MaterialTextBox
            {
                Hint = "Username",
                Location = new System.Drawing.Point(70, 110),
                Size = new System.Drawing.Size(250, 48)
            };

            txtPassword = new MaterialTextBox2
            {
                Hint = "Password",
                Location = new System.Drawing.Point(70, 180),
                Size = new System.Drawing.Size(250, 48),
                UseSystemPasswordChar = true
            };

            btnLogin = new MaterialButton
            {
                Text = "Login",
                Location = new System.Drawing.Point(70, 260),
                AutoSize = true
            };
            btnLogin.Click += BtnLogin_Click;

            btnRegister = new MaterialButton
            {
                Text = "Register",
                Location = new System.Drawing.Point(170, 260),
                AutoSize = true
            };
            btnRegister.Click += BtnRegister_Click;

            this.Controls.Add(txtUsername);
            this.Controls.Add(txtPassword);
            this.Controls.Add(btnLogin);
            this.Controls.Add(btnRegister);
        }

        private void BtnLogin_Click(object? sender, EventArgs e)
        {
            string username = txtUsername!.Text.Trim();
            string password = txtPassword!.Text.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter both username and password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var user = _context.Users.FirstOrDefault(u => u.Username == username && u.Password == password);
            if (user != null)
            {
                MessageBox.Show("Login successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // فتح شاشة الـ Dashboard
                DashboardForm dashboard = new DashboardForm(user.Username, user.Role);
                dashboard.Show();

                // إخفاء شاشة الدخول
                this.Hide();
            }
            else
            {
                MessageBox.Show("Invalid username or password.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void BtnRegister_Click(object sender, EventArgs e)
        {
            RegisterForm registerForm = new RegisterForm();
            registerForm.Show();
            this.Hide(); // أو this.Close();
        }
    }
}
