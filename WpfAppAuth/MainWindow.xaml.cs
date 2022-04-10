using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfAppAuth
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private string GetHashString(string s)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(s);

            MD5CryptoServiceProvider CSP = new MD5CryptoServiceProvider();
            byte[] byteHash = CSP.ComputeHash(bytes);
            string hash = "";
            foreach (byte b in byteHash)
            {
                hash += string.Format("{0:x2}", b);
            }
            return hash;
        }

        private void BnReg_Click(object sender, RoutedEventArgs e)
        {
            using (DbUsersEntities db = new DbUsersEntities())
            {
                foreach (Users users in db.Users)
                {
                    if (users.Log == TbLog.Text)
                    {
                        MessageBox.Show($"Пользователь с логином {TbLog.Text} существует!");
                        return;
                    }
                }
                db.Users.Add(new Users() { Log = TbLog.Text, Pass = GetHashString(TbPass.Text), Email = TbEmail.Text });
                db.SaveChanges();
            }
            MessageBox.Show("Регистрация прошла успешно!");
        }

        private void BnAuth_Click(object sender, RoutedEventArgs e)
        {
            using (DbUsersEntities db = new DbUsersEntities())
            {
                foreach (Users users in db.Users)
                {
                    if(users.Log == TbLog.Text && users.Pass == GetHashString(TbPass.Text))
                    {
                        MessageBox.Show("Авторизация прошла успешно!");
                        return;
                    }
                }
            }
            MessageBox.Show("Авторизация провалена!");
        }

        private void BnForgotPass_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
