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
    /// Interaction logic for ActivitiesCreate.xaml
    /// </summary>
    public partial class ActivitiesCreate : Window
    {
        public int Kulutus
        {
            get { return (int)GetValue(KulutusProperty); }
            set { SetValue(KulutusProperty, value); }
        }

        public static readonly DependencyProperty KulutusProperty =
            DependencyProperty.Register("Kulutus", typeof(int), typeof(ActivitiesCreate), new UIPropertyMetadata(0));

        public ActivitiesCreate()
        {
            InitializeComponent();
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void buttonOK_Click(object sender, RoutedEventArgs e)
        {
            textBoxKulutus.GetBindingExpression(TextBox.TextProperty).UpdateSource();
            if (!textBoxKulutus.GetBindingExpression(TextBox.TextProperty).HasError)
            {
                DbHandler dbh = new DbHandler();
                dbh.addActivity(textBoxNimi.Text, Convert.ToInt32(textBoxKulutus.Text));
                this.Close();
            }
        }
    }
}
