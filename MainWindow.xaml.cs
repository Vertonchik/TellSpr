using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data;
using Prism.Services.Dialogs;

namespace BazaDannih
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SqlConnection conn = new SqlConnection(@"Data source = LAPTOP-R0JE71H4\SQLEXPRESS; Initial Catalog = Kursach; Integrated Security = true; User ID = LAPTOP-R0JE71H4\Admin");
        SqlDataAdapter da;
        DataTable dt;
        bool bl;
        int Id;
        int IDplz;
        string IDgr;
        string name;
        string number;
        string dopnum;
        string email;
        string search;
        public System.Windows.Visibility Visibility { get; set; }
        public void aaa()
        {
            if (Name.Text != "")
            {
                if (Number.Text != "")
                {
                    conn.Open();
                    string na = Name.Text;
                    string nu = Number.Text;
                    dt = new DataTable();
                    da = new SqlDataAdapter();
                    SqlCommand con = new SqlCommand(@"SELECT * FROM dbo.Cheliki WHERE Name = '" + na + "'and Number = '" + nu + "' and user_id = '" + Id + "'", conn);
                    da.SelectCommand = con;
                    da.Fill(dt);
                    conn.Close();
                    if (dt.Rows.Count == 0) 
                    { 
                        bl = true;
                        try
                        {
                            conn.Open();
                            using (SqlCommand sql = conn.CreateCommand())
                            {
                                if (Group.SelectedItem == Nothing)
                                {
                                    IDgr = "NULL";
                                }
                                if (Group.SelectedItem == kollbox)
                                {
                                    IDgr = "1";
                                }
                                if (Group.SelectedItem == druzbox)
                                {
                                    IDgr = "2";
                                }
                                if (Group.SelectedItem == sembox)
                                {
                                    IDgr = "3";
                                }
                                SqlCommand com = new SqlCommand(@"INSERT INTO Cheliki(Name, Number, User_id, DopNumber, Email, gr_id)" + "VALUES('" + Name.Text + "','" + Number.Text + "','" + Id + "','" + DopNumber.Text + "','" + Email.Text + "'," + IDgr + ")", conn);
                                com.ExecuteNonQuery();
                                MessageBox.Show("Успешно сохранено");
                                da = new SqlDataAdapter(@"SELECT dbo.Cheliki.Name, dbo.Cheliki.Number, dbo.Cheliki.DopNumber, dbo.Cheliki.Email FROM dbo.Cheliki WHERE user_id = '" + Id + "'", conn);
                                dt = new DataTable();
                                da.Fill(dt);
                                BD.ItemsSource = dt.AsDataView();
                                IDgr = null;
                            }
                            conn.Close();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                            conn.Close();
                        }
                    } else
                        MessageBox.Show("Пользователь с таким именем или телефоном уже существует");
                }
                else
                    MessageBox.Show("Номер телефона отсутствует");
            }
            else
                MessageBox.Show("Имя не введено");
        }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            conn.Open();
            da = new SqlDataAdapter(@"SELECT dbo.Cheliki.Name, dbo.Cheliki.Number, dbo.Cheliki.DopNumber, dbo.Cheliki.Email FROM dbo.Cheliki WHERE user_id = '" + Id + "'", conn);
            dt = new DataTable();
            da.Fill(dt);
            BD.ItemsSource = dt.AsDataView();
            conn.Close();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            aaa();
            if (bl == true)
            {
                Group.Visibility = Visibility.Hidden;
                Save.Visibility = Visibility.Hidden;
                l1.Visibility = Visibility.Hidden;
                l2.Visibility = Visibility.Hidden;
                Name.Visibility = Visibility.Hidden;
                Number.Visibility = Visibility.Hidden;
                DopNumber.Visibility = Visibility.Hidden;
                Email.Visibility = Visibility.Hidden;
                l3.Visibility = Visibility.Hidden;
                l4.Visibility = Visibility.Hidden;
                Back.Visibility = Visibility.Hidden;
                Groups.Visibility = Visibility.Visible;
                Dobav.Visibility = Visibility.Visible;
                Redact.Visibility = Visibility.Visible;
                BD.Visibility = Visibility.Visible;
                Update.Visibility = Visibility.Visible;
                Delete.Visibility = Visibility.Visible;
                SearchBox.Visibility = Visibility.Visible;
                Search.Visibility = Visibility.Visible;
                Exit.Visibility = Visibility.Visible;
                bl = false;
            }
        }
        private void DataGridCell_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var dataGridCellTarget = (DataGridCell)sender;
            MessageBox.Show("Запись выбрана для работы");
            name = dt.DefaultView[BD.SelectedIndex]["Name"].ToString();
            number = dt.DefaultView[BD.SelectedIndex]["Number"].ToString();
            dopnum = dt.DefaultView[BD.SelectedIndex]["DopNumber"].ToString();
            email = dt.DefaultView[BD.SelectedIndex]["Email"].ToString();
            MessageBox.Show(name);
            MessageBox.Show(number);
            conn.Open();
            SqlCommand con = new SqlCommand(@"SELECT plz_id FROM dbo.Cheliki WHERE (Name = '" + name + "') AND (Number = '" + number + "') AND (DopNumber = '" + dopnum + "') AND (Email = '" + email + "')", conn);
            IDplz = Convert.ToInt32(con.ExecuteScalar());
            MessageBox.Show(IDplz.ToString());
            Name.Text = name;
            Number.Text = number;
            DopNumber.Text = dopnum;
            Email.Text = email;
            conn.Close();
        }
        
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            l1.Visibility = Visibility.Visible;
            l2.Visibility = Visibility.Visible;
            Name.Visibility = Visibility.Visible;
            Number.Visibility = Visibility.Visible;
            DopNumber.Visibility = Visibility.Visible;
            Email.Visibility = Visibility.Visible;
            l3.Visibility = Visibility.Visible;
            l4.Visibility = Visibility.Visible;
            Back.Visibility = Visibility.Visible;
            DELL.Visibility = Visibility.Visible;
            Dobav.Visibility = Visibility.Hidden;
            Redact.Visibility = Visibility.Hidden;
            Update.Visibility = Visibility.Hidden;
            Delete.Visibility = Visibility.Hidden;
            SearchBox.Visibility = Visibility.Hidden;
            Search.Visibility = Visibility.Hidden;
            Exit.Visibility = Visibility.Hidden;
            REDO.Visibility = Visibility.Hidden;
            Groups.Visibility = Visibility.Hidden;
            Name.Clear();
            Number.Clear();
            DopNumber.Clear();
            Email.Clear();
            MessageBox.Show("Выберете поле для удаления");
        }

        private void Vhod_Click(object sender, RoutedEventArgs e)
        {
            conn.Open();
            string log = UserName.Text;
            string pass = UserPassword.Password;
            dt = new DataTable();
            da = new SqlDataAdapter();
            SqlCommand com = new SqlCommand(@"SELECT * FROM dbo.Users WHERE UsName = '"+ log + "'and UsPass = '" + pass + "'", conn);
            da.SelectCommand = com;
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                MessageBox.Show("Вы были успешно авторизованы");
                SqlCommand con = new SqlCommand(@"SELECT user_id FROM dbo.Users WHERE (UsName = '" + UserName.Text + "') AND (UsPass = " + UserPassword.Password + ")", conn);
                Id = Convert.ToInt32(con.ExecuteScalar());
                //MessageBox.Show(Id.ToString());
                BD.Visibility = Visibility.Visible;
                Update.Visibility = Visibility.Visible;
                Dobav.Visibility = Visibility.Visible;
                Delete.Visibility = Visibility.Visible;
                SearchBox.Visibility = Visibility.Visible;
                Search.Visibility = Visibility.Visible;
                Exit.Visibility = Visibility.Visible;
                Redact.Visibility = Visibility.Visible;
                Groups.Visibility = Visibility.Visible;
                Voiti.Visibility = Visibility.Hidden;
                Regg.Visibility = Visibility.Hidden;
                UserName.Visibility = Visibility.Hidden;
                UserPassword.Visibility = Visibility.Hidden;
                da = new SqlDataAdapter(@"SELECT dbo.Cheliki.Name, dbo.Cheliki.Number, dbo.Cheliki.DopNumber, dbo.Cheliki.Email FROM dbo.Cheliki WHERE user_id = '" + Id + "'", conn);
                dt = new DataTable();
                da.Fill(dt);
                BD.ItemsSource = dt.AsDataView();
            }
            else
                MessageBox.Show("Авторизация не была пройдена");
            conn.Close();
        }
        private void Reg_Click(object sender, RoutedEventArgs e)
        {
            if (UserName.Text != "")
                if (UserPassword.Password != "")
                {
                    conn.Open();
                    string logg = UserName.Text;
                    dt = new DataTable();
                    da = new SqlDataAdapter();
                    SqlCommand cos = new SqlCommand(@"SELECT * FROM dbo.Users WHERE UsName = '" + logg + "'", conn);
                    da.SelectCommand = cos;
                    da.Fill(dt);
                    if (dt.Rows.Count == 0)
                    {
                        SqlCommand com = new SqlCommand(@"INSERT INTO Users(UsName, UsPass)" + "VALUES('" + UserName.Text + "','" + UserPassword.Password + "')", conn);
                        if (com.ExecuteNonQuery() == 1)
                        {
                            MessageBox.Show("Вы были успешно зарегестрированы");
                            string log = UserName.Text;
                            string pass = UserPassword.Password;
                            dt = new DataTable();
                            da = new SqlDataAdapter();
                            SqlCommand cor = new SqlCommand(@"SELECT * FROM dbo.Users WHERE UsName = '" + log + "'and UsPass = '" + pass + "'", conn);
                            da.SelectCommand = cor;
                            da.Fill(dt);
                            if (dt.Rows.Count > 0)
                            {
                                MessageBox.Show("Вы были автоматически авторизованы");
                                SqlCommand con = new SqlCommand(@"SELECT user_id FROM dbo.Users WHERE (UsName = '" + UserName.Text + "') AND (UsPass = " + UserPassword.Password + ")", conn);
                                Id = Convert.ToInt32(con.ExecuteScalar());
                                MessageBox.Show(Id.ToString());
                                BD.Visibility = Visibility.Visible;
                                Update.Visibility = Visibility.Visible;
                                Dobav.Visibility = Visibility.Visible;
                                Delete.Visibility = Visibility.Visible;
                                SearchBox.Visibility = Visibility.Visible;
                                Search.Visibility = Visibility.Visible;
                                Exit.Visibility = Visibility.Visible;
                                Redact.Visibility = Visibility.Visible;
                                Groups.Visibility = Visibility.Visible;
                                Voiti.Visibility = Visibility.Hidden;
                                Regg.Visibility = Visibility.Hidden;
                                UserName.Visibility = Visibility.Hidden;
                                UserPassword.Visibility = Visibility.Hidden;
                                da = new SqlDataAdapter(@"SELECT dbo.Cheliki.Name, dbo.Cheliki.Number, dbo.Cheliki.DopNumber, dbo.Cheliki.Email FROM dbo.Cheliki WHERE user_id = '" + Id + "'", conn);
                                dt = new DataTable();
                                da.Fill(dt);
                                BD.ItemsSource = dt.AsDataView();
                            }
                        }
                        else
                            MessageBox.Show("Регистрация не была пройдена");
                        conn.Close();
                    }
                    else
                        MessageBox.Show("Пользователь с таким именем уже существует");
                    conn.Close();
                }
                else
                    MessageBox.Show("Регистрация не была пройдена");
            conn.Close();
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            conn.Open();
            search = SearchBox.Text;
            da = new SqlDataAdapter(@"SELECT Name, Number, DopNumber, Email FROM dbo.Cheliki WHERE  (SUBSTRING(Name, 1," + search.Length + ") = '" + SearchBox.Text + "') and (user_id = '" + Id + "')", conn);
            dt = new DataTable();
            da.Fill(dt);
            BD.ItemsSource = dt.AsDataView();
            conn.Close();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            l1.Visibility = Visibility.Hidden;
            l2.Visibility = Visibility.Hidden;
            BD.Visibility = Visibility.Hidden;
            Name.Visibility = Visibility.Hidden;
            Number.Visibility = Visibility.Hidden;
            Update.Visibility = Visibility.Hidden;
            Save.Visibility = Visibility.Hidden;
            Dobav.Visibility = Visibility.Hidden;
            Delete.Visibility = Visibility.Hidden;
            DELL.Visibility = Visibility.Hidden;
            SearchBox.Visibility = Visibility.Hidden;
            Search.Visibility = Visibility.Hidden;
            Exit.Visibility = Visibility.Hidden;
            DopNumber.Visibility = Visibility.Hidden;
            Email.Visibility = Visibility.Hidden;
            l3.Visibility = Visibility.Hidden;
            l4.Visibility = Visibility.Hidden;
            Redact.Visibility = Visibility.Hidden;
            Groups.Visibility = Visibility.Hidden;
            Voiti.Visibility = Visibility.Visible;
            Regg.Visibility = Visibility.Visible;
            UserName.Visibility = Visibility.Visible;
            UserPassword.Visibility = Visibility.Visible;
            UserName.Clear();
            UserPassword.Clear();
            Name.Clear();
            Number.Clear();
            DopNumber.Clear();
            Email.Clear();
        }

        private void Dobav_Click(object sender, RoutedEventArgs e)
        {
            Back.Visibility = Visibility.Visible;
            Save.Visibility = Visibility.Visible;
            l1.Visibility = Visibility.Visible;
            l2.Visibility = Visibility.Visible;
            Name.Visibility = Visibility.Visible;
            Number.Visibility = Visibility.Visible;
            DopNumber.Visibility = Visibility.Visible;
            Email.Visibility = Visibility.Visible;
            l3.Visibility = Visibility.Visible;
            l4.Visibility = Visibility.Visible;
            Group.Visibility = Visibility.Visible;
            Groups.Visibility = Visibility.Hidden;
            Dobav.Visibility = Visibility.Hidden;
            Redact.Visibility = Visibility.Hidden;
            BD.Visibility = Visibility.Hidden;
            Update.Visibility = Visibility.Hidden;
            Delete.Visibility = Visibility.Hidden;
            SearchBox.Visibility = Visibility.Hidden;
            Search.Visibility = Visibility.Hidden;
            Exit.Visibility = Visibility.Hidden;
            Groups.Visibility = Visibility.Hidden;
            Name.Clear();
            Number.Clear();
            DopNumber.Clear();
            Email.Clear();
            Group.SelectedItem = Nothing;
        }

        private void Red_Click(object sender, RoutedEventArgs e)
        {
            REDO.Visibility = Visibility.Visible;
            l1.Visibility = Visibility.Visible;
            l2.Visibility = Visibility.Visible;
            Name.Visibility = Visibility.Visible;
            Number.Visibility = Visibility.Visible;
            DopNumber.Visibility = Visibility.Visible;
            Email.Visibility = Visibility.Visible;
            l3.Visibility = Visibility.Visible;
            l4.Visibility = Visibility.Visible;
            Back.Visibility = Visibility.Visible;
            Dobav.Visibility = Visibility.Hidden;
            Redact.Visibility = Visibility.Hidden;
            Update.Visibility = Visibility.Hidden;
            Delete.Visibility = Visibility.Hidden;
            SearchBox.Visibility = Visibility.Hidden;
            Search.Visibility = Visibility.Hidden;
            Exit.Visibility = Visibility.Hidden;
            Groups.Visibility = Visibility.Hidden;
            Name.Clear();
            Number.Clear();
            DopNumber.Clear();
            Email.Clear();
            MessageBox.Show("Выберете поле для редактирования");
        }

        private void REDO_Click(object sender, RoutedEventArgs e)
        {
            conn.Open();
            string na = Name.Text;
            string nu = Number.Text;
            dt = new DataTable();
            da = new SqlDataAdapter();
            SqlCommand con = new SqlCommand(@"SELECT * FROM dbo.Cheliki WHERE Name = '" + na + "'and Number = '" + nu + "'", conn);
            da.SelectCommand = con;
            da.Fill(dt);
            conn.Close();
            if (dt.Rows.Count > 0)
            {
                MessageBox.Show("Пользователь с таким именем и телефоном уже существует");
            }
            else
            {
                conn.Open();
                SqlCommand com = new SqlCommand(@"UPDATE dbo.Cheliki SET Name = '" + Name.Text + "', Number = '" + Number.Text + "', DopNumber = '" + DopNumber.Text + "', Email = '" + Email.Text + "' WHERE plz_id = " + IDplz, conn);
                com.ExecuteNonQuery();
                MessageBox.Show("Успешно обновлено");
                da = new SqlDataAdapter(@"SELECT dbo.Cheliki.Name, dbo.Cheliki.Number, dbo.Cheliki.DopNumber, dbo.Cheliki.Email FROM dbo.Cheliki WHERE user_id = '" + Id + "'", conn);
                dt = new DataTable();
                da.Fill(dt);
                BD.ItemsSource = dt.AsDataView();
                conn.Close();
                REDO.Visibility = Visibility.Hidden;
                l1.Visibility = Visibility.Hidden;
                l2.Visibility = Visibility.Hidden;
                Name.Visibility = Visibility.Hidden;
                Number.Visibility = Visibility.Hidden;
                DopNumber.Visibility = Visibility.Hidden;
                Email.Visibility = Visibility.Hidden;
                l3.Visibility = Visibility.Hidden;
                l4.Visibility = Visibility.Hidden;
                Back.Visibility = Visibility.Hidden;
                Dobav.Visibility = Visibility.Visible;
                Redact.Visibility = Visibility.Visible;
                Update.Visibility = Visibility.Visible;
                Delete.Visibility = Visibility.Visible;
                SearchBox.Visibility = Visibility.Visible;
                Search.Visibility = Visibility.Visible;
                Exit.Visibility = Visibility.Visible;
                Groups.Visibility = Visibility.Visible;
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            Save.Visibility = Visibility.Hidden;
            Group.Visibility = Visibility.Hidden;
            REDO.Visibility = Visibility.Hidden;
            l1.Visibility = Visibility.Hidden;
            l2.Visibility = Visibility.Hidden;
            Name.Visibility = Visibility.Hidden;
            Number.Visibility = Visibility.Hidden;
            DopNumber.Visibility = Visibility.Hidden;
            Email.Visibility = Visibility.Hidden;
            l3.Visibility = Visibility.Hidden;
            l4.Visibility = Visibility.Hidden;
            Back.Visibility = Visibility.Hidden;
            DELL.Visibility = Visibility.Hidden;
            Kl.Visibility = Visibility.Hidden;
            Dr.Visibility = Visibility.Hidden;
            Sm.Visibility = Visibility.Hidden;
            Koll.Visibility = Visibility.Hidden;
            Dru.Visibility = Visibility.Hidden;
            Sem.Visibility = Visibility.Hidden;
            Dobav.Visibility = Visibility.Visible;
            Redact.Visibility = Visibility.Visible;
            Update.Visibility = Visibility.Visible;
            Delete.Visibility = Visibility.Visible;
            SearchBox.Visibility = Visibility.Visible;
            Search.Visibility = Visibility.Visible;
            Exit.Visibility = Visibility.Visible;
            Groups.Visibility = Visibility.Visible;
            BD.Visibility = Visibility.Visible;
            Name.Clear();
            Number.Clear();
            DopNumber.Clear();
            Email.Clear();
        }

        private void DELL_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string messageBoxText = ("Вы уверены в удалении абонента " + name + " (" + number + ")");
                string caption = "Word Processor";
                MessageBoxButton button = MessageBoxButton.YesNo;
                MessageBoxImage icon = MessageBoxImage.Question;
                MessageBoxResult result = MessageBox.Show(messageBoxText, caption, button, icon);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        conn.Open();
                        SqlCommand con = new SqlCommand(@"DELETE FROM dbo.Cheliki WHERE plz_id = " + IDplz, conn);
                        con.ExecuteNonQuery();
                        da = new SqlDataAdapter(@"SELECT dbo.Cheliki.Name, dbo.Cheliki.Number, dbo.Cheliki.DopNumber, dbo.Cheliki.Email FROM dbo.Cheliki WHERE user_id = '" + Id + "'", conn);
                        dt = new DataTable();
                        da.Fill(dt);
                        BD.ItemsSource = dt.AsDataView();
                        conn.Close();
                        Name.Clear();
                        Number.Clear();
                        DopNumber.Clear();
                        Email.Clear();
                        break;
                    case MessageBoxResult.No:
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                conn.Close();
            }
        }

        private void Groups_Click(object sender, RoutedEventArgs e)
        {
            BD.Visibility = Visibility.Hidden;
            Back.Visibility = Visibility.Visible;
            Dobav.Visibility = Visibility.Hidden;
            Redact.Visibility = Visibility.Hidden;
            Update.Visibility = Visibility.Hidden;
            Delete.Visibility = Visibility.Hidden;
            SearchBox.Visibility = Visibility.Hidden;
            Search.Visibility = Visibility.Hidden;
            Exit.Visibility = Visibility.Hidden;
            Groups.Visibility = Visibility.Hidden;
            Kl.Visibility = Visibility.Visible;
            Dr.Visibility = Visibility.Visible;
            Sm.Visibility = Visibility.Visible;
            Koll.Visibility = Visibility.Visible;
            Dru.Visibility = Visibility.Visible;
            Sem.Visibility = Visibility.Visible;
            conn.Open();
            da = new SqlDataAdapter(@"SELECT dbo.Cheliki.Name, dbo.Cheliki.Number, dbo.Cheliki.DopNumber, dbo.Cheliki.Email FROM dbo.Cheliki WHERE user_id = '" + Id + "' AND gr_id = '" + 1 + "'", conn);
            dt = new DataTable();
            da.Fill(dt);
            Koll.ItemsSource = dt.AsDataView();
            conn.Close();
            conn.Open();
            da = new SqlDataAdapter(@"SELECT dbo.Cheliki.Name, dbo.Cheliki.Number, dbo.Cheliki.DopNumber, dbo.Cheliki.Email FROM dbo.Cheliki WHERE user_id = '" + Id + "' AND gr_id = '" + 2 + "'", conn);
            dt = new DataTable();
            da.Fill(dt);
            Dru.ItemsSource = dt.AsDataView();
            conn.Close();
            conn.Open();
            da = new SqlDataAdapter(@"SELECT dbo.Cheliki.Name, dbo.Cheliki.Number, dbo.Cheliki.DopNumber, dbo.Cheliki.Email FROM dbo.Cheliki WHERE user_id = '" + Id + "' AND gr_id = '" + 3 + "'", conn);
            dt = new DataTable();
            da.Fill(dt);
            Sem.ItemsSource = dt.AsDataView();
            conn.Close();
        }
    }
}
