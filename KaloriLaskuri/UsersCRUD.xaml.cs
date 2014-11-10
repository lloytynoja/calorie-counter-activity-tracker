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
using System.Windows.Shapes;
using System.Data;
using System.Collections.ObjectModel;

namespace KaloriLaskuri
{
    /// <summary>
    /// User listing, CRUD.
    /// </summary>
    public partial class UsersCRUD : Window
    {
        DbHandler dh;
        DataSet users;

        public UsersCRUD()
        {
            InitializeComponent();
            dh = new DbHandler();
            LoadUsers();
        }
        private void LoadUsers()
        {
            users = dh.getUsers();
            dataGridUsersCRUD.DataContext = users.Tables[0].DefaultView;
        }

        private void buttonNewUser_Click(object sender, RoutedEventArgs e)
        {
            saveChanges();
            UsersCreate uc = new UsersCreate();
            uc.ShowDialog();
            this.LoadUsers();
        }

        private void saveChanges()
        {
            if(users.HasChanges())
            {
                DataSet changes = users.GetChanges();
                foreach (DataRow drw in changes.Tables["kayttajat"].Rows)
                {
                    dh.modifyUser((int)drw.ItemArray[0], 
                                    drw.ItemArray[1].ToString(), 
                                    drw.ItemArray[2].ToString(), 
                                    drw.ItemArray[4].ToString(), 
                                    (DateTime)drw.ItemArray[3], 
                                    (Decimal)drw.ItemArray[5]);
                }
            }
        }

        private void buttonRemoveUser_Click(object sender, RoutedEventArgs e)
        {
            saveChanges();
            if (dataGridUsersCRUD.SelectedIndex != -1)
            {
                DataRowView selected = (DataRowView)dataGridUsersCRUD.SelectedItem;
                MessageBoxResult rs = MessageBox.Show("Halutako varmasti poistaa käyttäjän " + selected.Row.ItemArray[1] + "," + selected.Row.ItemArray[2] + "?", 
                                                        "Varmistus", MessageBoxButton.YesNo);
                if (rs == MessageBoxResult.Yes)
                {
                    dh.removeUserById((int)selected.Row.ItemArray[0]);
                }
                this.LoadUsers();
            }
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            saveChanges();
        }
    }

    public class Genders : List<String>
    {
        public Genders()
        {
            this.Add("M");
            this.Add("N");
        }
    }
}
