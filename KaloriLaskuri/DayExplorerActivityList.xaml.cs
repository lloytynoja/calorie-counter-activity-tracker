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
    /// Used to pick portions for DayExplorer class.
    /// </summary>
    public partial class DayExplorerActivityList : Window
    {
        public DayExplorerActivityList()
        {
            InitializeComponent();
            dh = new DbHandler();
            this.LoadActivities();
        }
        DbHandler dh;
        DataSet activities;  

        public int Duration
        {
            get { return (int)GetValue(DurationProperty); }
            set { SetValue(DurationProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Duration.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DurationProperty = 
            DependencyProperty.Register("Duration", typeof(int), typeof(DayExplorerActivityList), new UIPropertyMetadata(0));

        public int Hours
        {
            get { return (int)GetValue(HoursProperty); }
            set { SetValue(HoursProperty, value); }
        }

        public static readonly DependencyProperty HoursProperty =
            DependencyProperty.Register("Hours", typeof(int), typeof(DayExplorerActivityList), new UIPropertyMetadata(DateTime.Now.Hour));
        
        public int Minutes
        {
            get { return (int)GetValue(MinutesProperty); }
            set { SetValue(MinutesProperty, value); }
        }

        public static readonly DependencyProperty MinutesProperty =
            DependencyProperty.Register("Minutes", typeof(int), typeof(DayExplorerActivityList), new UIPropertyMetadata(DateTime.Now.Minute));
        
        /// <summary>
        /// Fills activities datagrid
        /// </summary>
        private void LoadActivities()
        {
            activities = dh.getActivities();
            dataGridDoneActivityCRUD.DataContext = activities.Tables[0].DefaultView;
        }

        private void buttonAddDoneActivity_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridDoneActivityCRUD.SelectedIndex != -1)
            {
                DateTime when;
                DataRowView selected = (DataRowView)dataGridDoneActivityCRUD.SelectedItem;
                var myObject = this.Owner as DayExplorer;
                
                textBoxAmount.GetBindingExpression(TextBox.TextProperty).UpdateSource();

                if (!textBoxAmount.GetBindingExpression(TextBox.TextProperty).HasError &&
                    !textBoxTunnit.GetBindingExpression(TextBox.TextProperty).HasError &&
                    !textBoxMinuutit.GetBindingExpression(TextBox.TextProperty).HasError)
                {
                    when = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, Convert.ToInt16(textBoxTunnit.Text), Convert.ToInt16(textBoxMinuutit.Text), 0);
                    myObject.addToDoneActivities(selected, Duration, when);
                }
            }
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
