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

namespace KaloriLaskuri
{
    /// <summary>
    /// Interaction logic for ActivitiesCRUD.xaml
    /// </summary>
    public partial class ActivitiesCRUD : Window
    {
        DbHandler dh;
        DataSet activities;

        public ActivitiesCRUD()
        {
            InitializeComponent();
            dh = new DbHandler();
            LoadActivities();
        }

        /// <summary>
        /// Fills the datagrid
        /// </summary>
        private void LoadActivities()
        {
            dh.connect();
            activities = dh.getActivities();
            dataGridActivitiesCRUD.DataContext = activities.Tables[0].DefaultView;
            dh.disconnect();
        }

        private void buttonNewActivity_Click(object sender, RoutedEventArgs e)
        {
            saveChanges();
            ActivitiesCreate ac = new ActivitiesCreate();
            ac.ShowDialog();
            this.LoadActivities();
        }

        /// <summary>
        /// If dataset has changes, they are committed to database
        /// </summary>
        private void saveChanges()
        {
            if (activities.HasChanges())
            {
                DataSet changes = activities.GetChanges();
                foreach (DataRow drw in changes.Tables["aktiviteetit"].Rows)
                {
                    dh.modifyActivity((int)drw.ItemArray[0], 
                                    drw.ItemArray[1].ToString(), 
                                    (int)drw.ItemArray[2]);
                }
            }
        }

        private void buttonRemoveActivity_Click(object sender, RoutedEventArgs e)
        {
            saveChanges();
            if (dataGridActivitiesCRUD.SelectedIndex != -1)
            {
                DataRowView selected = (DataRowView)dataGridActivitiesCRUD.SelectedItem;
                MessageBoxResult rs = MessageBox.Show("Halutako varmasti poistaa aktiviteetin " + selected.Row.ItemArray[1], 
                                                        "Varmistus", MessageBoxButton.YesNo);
                if (rs == MessageBoxResult.Yes)
                {
                    dh.removeActivityById((int)selected.Row.ItemArray[0]);
                }
                this.LoadActivities();
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
}

