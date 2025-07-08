using System;
using System.Linq;
using System.Windows.Forms;
using MaterialSkin;
using MaterialSkin.Controls;
using ClothingStoreApp.Data;
using ClothingStoreApp.Models;

namespace ClothingStoreApp
{
    public class RegisterForm : MaterialForm
    {
        private readonly AppDbContext _context;

        private MaterialTextBox? txtUsername;
        private MaterialTextBox2? txtPassword;
        private MaterialComboBox? cmbRole;
        private MaterialButton? btnRegister;
        private MaterialButton? btnBackToLogin; // زر الرجوع

        public RegisterForm()
        {
            // إعداد الثيم
            var skinManager = MaterialSkinManager.Instance;
            skinManager.AddFormToManage(this);
            skinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            skinManager.ColorScheme = new ColorScheme(
                Primary.Blue600, Primary.Blue700,
                Primary.Blue200, Accent.Blue200,
                TextShade.WHITE
            );

            this.Text = "Register";
            this.Size = new System.Drawing.Size(400, 550);
            this.StartPosition = FormStartPosition.CenterScreen;

            _context = new AppDbContext();

            SetupUI();
        }

        private void SetupUI()
        {
            txtUsername = new MaterialTextBox
            {
                Hint = "Username",
                Location = new System.Drawing.Point(60, 100),
                Size = new System.Drawing.Size(300, 48)
            };

            txtPassword = new MaterialTextBox2
            {
                Hint = "Password",
                Location = new System.Drawing.Point(60, 170),
                Size = new System.Drawing.Size(300, 48),
                UseSystemPasswordChar = true
            };

            cmbRole = new MaterialComboBox
            {
                Hint = "Role (Admin - Employee)",
                Location = new System.Drawing.Point(60, 230),
                Size = new System.Drawing.Size(300, 48),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbRole.Items.Add("Admin");
            cmbRole.Items.Add("Employee");

            btnRegister = new MaterialButton
            {
                Text = "REGISTER",
                Location = new System.Drawing.Point(60, 300),
                AutoSize = true
            };
            btnRegister.Click += BtnRegister_Click;

            btnBackToLogin = new MaterialButton
            {
                Text = "Login",
                Location = new System.Drawing.Point(220, 300),
                AutoSize = true
            };
            btnBackToLogin.Click += BtnBackToLogin_Click;

            this.Controls.Add(txtUsername);
            this.Controls.Add(txtPassword);
            this.Controls.Add(cmbRole);
            this.Controls.Add(btnRegister);
            this.Controls.Add(btnBackToLogin);
        }

        private void BtnRegister_Click(object? sender, EventArgs e)
        {
            string username = txtUsername?.Text.Trim() ?? "";
            string password = txtPassword?.Text.Trim() ?? "";
            string role = cmbRole?.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(role))
            {
                MessageBox.Show("Please fill in all fields.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var existingUser = _context.Users.FirstOrDefault(u => u.Username == username);
            if (existingUser != null)
            {
                MessageBox.Show("Username already exists.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var newUser = new User
            {
                Username = username,
                Password = password,
                Role = role
            };

            _context.Users.Add(newUser);
            _context.SaveChanges();

            MessageBox.Show("Registration successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

            LoginForm loginForm = new LoginForm();
            loginForm.Show();
            this.Hide();
        }

        private void BtnBackToLogin_Click(object? sender, EventArgs e)
        {
            LoginForm loginForm = new LoginForm();
            loginForm.Show();
            this.Close();
        }
    }
}

//using System;
//using System.Linq;
//using System.Windows.Forms;
//using MaterialSkin;
//using MaterialSkin.Controls;
//using ClothingStoreApp.Data;
//using ClothingStoreApp.Models;

//namespace ClothingStoreApp
//{
//    public class RegisterForm : MaterialForm
//    {
//        private readonly AppDbContext _context;

//        private MaterialTextBox? txtUsername;
//        private MaterialTextBox2? txtPassword;
//        private MaterialComboBox? cmbRole;
//        private MaterialButton? btnRegister;

//        public RegisterForm()
//        {
//            // إعداد الثيم
//            var skinManager = MaterialSkinManager.Instance;
//            skinManager.AddFormToManage(this);
//            skinManager.Theme = MaterialSkinManager.Themes.LIGHT;
//            skinManager.ColorScheme = new ColorScheme(
//                Primary.Blue600, Primary.Blue700,
//                Primary.Blue200, Accent.Blue200,
//                TextShade.WHITE
//            );

//            this.Text = "RegisterForm";
//            this.Size = new System.Drawing.Size(400, 500);
//            this.StartPosition = FormStartPosition.CenterScreen;


//            _context = new AppDbContext();

//            SetupUI();
//        }

//        private void SetupUI()
//        {
//            txtUsername = new MaterialTextBox
//            {
//                Hint = "Username",
//                Location = new System.Drawing.Point(70, 100),
//                Size = new System.Drawing.Size(300, 48)
//            };

//            txtPassword = new MaterialTextBox2
//            {
//                Hint = "Password",
//                Location = new System.Drawing.Point(70, 160),
//                Size = new System.Drawing.Size(300, 48),
//                UseSystemPasswordChar = true
//            };

//            cmbRole = new MaterialComboBox
//            {
//                Hint = "Role (Admin-User)",
//                Location = new System.Drawing.Point(70, 220),
//                Size = new System.Drawing.Size(300, 48),
//                DropDownStyle = ComboBoxStyle.DropDownList
//            };
//            cmbRole.Items.Add("Admin");
//            cmbRole.Items.Add("Employee");

//            btnRegister = new MaterialButton
//            {
//                Text = "REGISTER",
//                Location = new System.Drawing.Point(150, 290),
//                AutoSize = true
//            };
//            btnRegister.Click += BtnRegister_Click;

//            this.Controls.Add(txtUsername);
//            this.Controls.Add(txtPassword);
//            this.Controls.Add(cmbRole);
//            this.Controls.Add(btnRegister);

//        }

//        private void BtnRegister_Click(object sender, EventArgs e)
//        {
//            string username = txtUsername.Text.Trim();
//            string password = txtPassword.Text.Trim();
//            string role = cmbRole.SelectedItem?.ToString();

//            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(role))
//            {
//                MessageBox.Show("Please fill in all fields.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
//                return;
//            }

//            var existingUser = _context.Users.FirstOrDefault(u => u.Username == username);
//            if (existingUser != null)
//            {
//                MessageBox.Show("Username already exists.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
//                return;
//            }

//            var newUser = new User
//            {
//                Username = username,
//                Password = password,
//                Role = role
//            };

//            _context.Users.Add(newUser);
//            _context.SaveChanges();

//            MessageBox.Show("Registration successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

//            // ✅ الانتقال لصفحة تسجيل الدخول
//            LoginForm loginForm = new LoginForm();
//            loginForm.Show();
//            this.Hide(); // أو this.Close();
//        }

//    }
//}
