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
    /// Interaction logic for DayExplorerFoodList.xaml
    /// </summary>
    public partial class DayExplorerFoodList : Window
    {
        DbHandler dh;
        DataSet foods;
        
        public int FoodAmount
        {
            get { return (int)GetValue(FoodAmountProperty); }
            set { SetValue(FoodAmountProperty, value); }
        }

        public static readonly DependencyProperty FoodAmountProperty =
            DependencyProperty.Register("FoodAmount", typeof(int), typeof(DayExplorerFoodList), new UIPropertyMetadata());

        public int Hours
        {
            get { return (int)GetValue(HoursProperty); }
            set { SetValue(HoursProperty, value); }
        }

        public static readonly DependencyProperty HoursProperty =
            DependencyProperty.Register("Hours", typeof(int), typeof(DayExplorerFoodList), new UIPropertyMetadata(DateTime.Now.Hour));
        
        public int Minutes
        {
            get { return (int)GetValue(MinutesProperty); }
            set { SetValue(MinutesProperty, value); }
        }

        public static readonly DependencyProperty MinutesProperty =
            DependencyProperty.Register("Minutes", typeof(int), typeof(DayExplorerFoodList), new UIPropertyMetadata(DateTime.Now.Minute));
                
        public DayExplorerFoodList()
        {
            InitializeComponent();
            dh = new DbHandler();
            this.LoadFoods();
        }

        /// <summary>
        /// Fills foods datagrid.
        /// </summary>
        private void LoadFoods()
        {
            foods = dh.getFoods();
            dataGridFoodsCRUD.DataContext = foods.Tables[0].DefaultView;
        }

        private void buttonAddFood_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridFoodsCRUD.SelectedIndex != -1)
            {
                DateTime when;
                DataRowView selected = (DataRowView)dataGridFoodsCRUD.SelectedItem;
                var myObject = this.Owner as DayExplorer;

                textBoxAmount.GetBindingExpression(TextBox.TextProperty).UpdateSource();

                if (!textBoxAmount.GetBindingExpression(TextBox.TextProperty).HasError &&
                    !textBoxTunnit.GetBindingExpression(TextBox.TextProperty).HasError &&
                    !textBoxMinuutit.GetBindingExpression(TextBox.TextProperty).HasError)
                {
                    when = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, Convert.ToInt16(textBoxTunnit.Text), Convert.ToInt16(textBoxMinuutit.Text), 0);
                    myObject.addToPortions(selected, FoodAmount, when);
                }
            }
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
