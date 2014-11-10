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
    /// Listing of added foods, CRUD.
    /// </summary>
    public partial class FoodsCRUD : Window
    {
        DbHandler dh;
        DataSet foods;

        public FoodsCRUD()
        {
            InitializeComponent();
            dh = new DbHandler();
            this.LoadFoods();
        }
        private void LoadFoods()
        {
            foods = dh.getFoods();
            dataGridFoodsCRUD.DataContext = foods.Tables[0].DefaultView;
        }

        private void buttonNewFood_Click(object sender, RoutedEventArgs e)
        {

            saveChanges();
            FoodsCreate fc = new FoodsCreate();
            fc.ShowDialog();
            this.LoadFoods();
        }

        private void saveChanges()
        {
            if (foods.HasChanges())
            {
                DataSet changes = foods.GetChanges();
                foreach (DataRow drw in changes.Tables["ruoka_aineet"].Rows)
                {
                    dh.modifyFood((int)drw.ItemArray[0], 
                                    drw.ItemArray[1].ToString(),
                                    (int)drw.ItemArray[2],
                                    (Decimal)drw.ItemArray[3],
                                    (Decimal)drw.ItemArray[4],
                                    (Decimal)drw.ItemArray[5],
                                    (int)drw.ItemArray[6]);
                }
            }
        }

        private void buttonRemoveFood_Click(object sender, RoutedEventArgs e)
        {
            saveChanges();
            if (dataGridFoodsCRUD.SelectedIndex != -1)
            {
                DataRowView selected = (DataRowView)dataGridFoodsCRUD.SelectedItem;
                MessageBoxResult rs = MessageBox.Show("Halutako varmasti poistaa ruoka-aineen " + selected.Row.ItemArray[1], 
                                                        "Varmistus", MessageBoxButton.YesNo);
                if (rs == MessageBoxResult.Yes)
                {
                    dh.removeFoodById((int)selected.Row.ItemArray[0]);
                }
                this.LoadFoods();
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
