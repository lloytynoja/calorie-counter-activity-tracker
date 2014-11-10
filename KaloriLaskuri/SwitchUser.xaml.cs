using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data;

namespace KaloriLaskuri
{
    /// <summary>
    /// User selection window. Contains menus from where user can create/update/delete users, foods and activities.
    /// </summary>
    public partial class SwitchUser : Window
    {
        DbHandler dh;
        DataSet users;

        public SwitchUser()
        {
            InitializeComponent();
            dh = new DbHandler();
            LoadUsers();
            dataGridUsers.IsReadOnly = true;
        }

        private void LoadUsers()
        {
            users = dh.getUsers();
            dataGridUsers.DataContext = users.Tables[0].DefaultView;
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridUsers.SelectedIndex != -1)
            {
                DataRowView selected = (DataRowView)dataGridUsers.SelectedItem;
                DayExplorer dx = new DayExplorer((int)selected.Row.ItemArray[0]);
                dx.ShowDialog();
            }
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
           KLDataManager kld = new KLDataManager();
           kld.Show();
        }

        private void MenuItem_Exit(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void MenuItem_EditUsers(object sender, RoutedEventArgs e)
        {
            UsersCRUD uc = new UsersCRUD();
            uc.ShowDialog();
            this.LoadUsers();
        }

        private void MenuItem_EditFoods(object sender, RoutedEventArgs e)
        {
            FoodsCRUD fc = new FoodsCRUD();
            fc.Show();
        }

        private void MenuItem_Activities(object sender, RoutedEventArgs e)
        {
            ActivitiesCRUD ac = new ActivitiesCRUD();
            ac.Show();
        }

        private void MenuItem_About(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Kalorilaskuri v.1.0\n\nLasse Löytynoja\n9.12.2012");
        }
    }
}
