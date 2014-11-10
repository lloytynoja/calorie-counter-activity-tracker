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
    /// Add new user from this window.
    /// </summary>
    public partial class UsersCreate : Window
    {
        public double Pituus
        {
            get { return (double)GetValue(PituusProperty); }
            set { SetValue(PituusProperty, value); }
        }

        public static readonly DependencyProperty PituusProperty =
            DependencyProperty.Register("Pituus", typeof(double), typeof(UsersCreate));

        public DateTime BirthDate
        {
            get { return (DateTime)GetValue(BirthDateProperty); }
            set { SetValue(BirthDateProperty, value); }
        }

        public static readonly DependencyProperty BirthDateProperty =
            DependencyProperty.Register("BirthDate", typeof(DateTime), typeof(UsersCreate));

        public UsersCreate()
        {
            BirthDate = DateTime.Now;
            InitializeComponent();
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void buttonOK_Click(object sender, RoutedEventArgs e)
        {
            textBoxPituus.GetBindingExpression(TextBox.TextProperty).UpdateSource();
            datePickerSA.GetBindingExpression(DatePicker.SelectedDateProperty).UpdateSource();
            
            if (!textBoxPituus.GetBindingExpression(TextBox.TextProperty).HasError &&
                !datePickerSA.GetBindingExpression(DatePicker.SelectedDateProperty).HasError)
            {
                DbHandler dbh = new DbHandler();
                dbh.addUser(textBoxSukunimi.Text,
                            textBoxEtunimi.Text,
                            comboboxSukupuoli.SelectedValue.ToString(),
                            datePickerSA.SelectedDate.Value.Date,
                            Convert.ToDecimal(textBoxPituus.Text));
                this.Close();

            }
        }
    }
}
