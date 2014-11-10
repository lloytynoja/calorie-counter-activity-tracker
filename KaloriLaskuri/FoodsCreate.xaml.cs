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
    /// New food is added via this window.
    /// </summary>
    public partial class FoodsCreate : Window
    {
        public int Energy
        {
            get { return (int)GetValue(EnergyProperty); }
            set { SetValue(EnergyProperty, value); }
        }

        public static readonly DependencyProperty EnergyProperty =
            DependencyProperty.Register("Energy", typeof(int), typeof(FoodsCreate));
        
        public decimal Protein
        {
            get { return (decimal)GetValue(ProteinProperty); }
            set { SetValue(ProteinProperty, value); }
        }

        public static readonly DependencyProperty ProteinProperty =
            DependencyProperty.Register("Protein", typeof(decimal), typeof(FoodsCreate));
                
        public Decimal Carbs
        {
            get { return (Decimal)GetValue(CarbsProperty); }
            set { SetValue(CarbsProperty, value); }
        }

        public static readonly DependencyProperty CarbsProperty =
            DependencyProperty.Register("Carbs", typeof(Decimal), typeof(FoodsCreate));
                
        public Decimal Fat
        {
            get { return (Decimal)GetValue(FatProperty); }
            set { SetValue(FatProperty, value); }
        }

        public static readonly DependencyProperty FatProperty =
            DependencyProperty.Register("Fat", typeof(Decimal), typeof(FoodsCreate));
                
        public FoodsCreate()
        {
            InitializeComponent();
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void buttonOK_Click(object sender, RoutedEventArgs e)
        {
            textBoxEnergia.GetBindingExpression(TextBox.TextProperty).UpdateSource();
            textBoxHiilihydraatti.GetBindingExpression(TextBox.TextProperty).UpdateSource();
            textBoxProteiini.GetBindingExpression(TextBox.TextProperty).UpdateSource();
            textBoxRasva.GetBindingExpression(TextBox.TextProperty).UpdateSource();

            
            if (!textBoxEnergia.GetBindingExpression(TextBox.TextProperty).HasError &&
                !textBoxHiilihydraatti.GetBindingExpression(TextBox.TextProperty).HasError &&
                !textBoxProteiini.GetBindingExpression(TextBox.TextProperty).HasError &&
                !textBoxRasva.GetBindingExpression(TextBox.TextProperty).HasError)
            {
                DbHandler dbh = new DbHandler();
                dbh.addFood(textBoxNimi.Text, Convert.ToInt32(textBoxEnergia.Text),
                            Convert.ToDecimal(textBoxProteiini.Text), Convert.ToDecimal(textBoxHiilihydraatti.Text),
                            Convert.ToDecimal(textBoxRasva.Text),0);
                this.Close();
            }
        }
    }
}
