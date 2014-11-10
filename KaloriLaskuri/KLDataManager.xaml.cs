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

namespace KaloriLaskuri
{
    /// <summary>
    /// All database data can be removed and test data added.
    /// </summary>
    public partial class KLDataManager : Window
    {
        DbHandler d;

        public KLDataManager()
        {
            InitializeComponent();
            d = new DbHandler();
        }

        private void buttonLisaaDummyData_Click(object sender, RoutedEventArgs e)
        {
            d.connect();
            d.insertDummyData();
            d.disconnect();
        }

        private void buttonPoistaData_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult rs = MessageBox.Show("Poistetaanko kaikki data?", "Varmistus", MessageBoxButton.YesNo);
            if (rs == MessageBoxResult.Yes)
            {
                d.connect();
                d.deleteAllRecords();
                d.disconnect();
            }
        }

        private void buttonPoistaOsaData_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult rs = MessageBox.Show("Poistetaanko käyttäjät, ruoka-aineet ja aktiviteetit?", "Varmistus", MessageBoxButton.YesNo);
            if (rs == MessageBoxResult.Yes)
            {
                d.connect();
                d.deleteUsersFoodsActivities();
                d.disconnect();
            }
        }
    }
}
