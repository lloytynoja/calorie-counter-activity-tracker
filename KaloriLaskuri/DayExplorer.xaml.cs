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
using System.Globalization;
using System.Threading;

namespace KaloriLaskuri
{
    /// <summary>
    /// This view is used for day-specific data entering for users.
    /// </summary>
    public partial class DayExplorer : Window
    {
        bool isReady = false;
        int PersonId;
        int Activities = 0;
        int TotalConsumption = 0;
        Double BaseConsumption = 0;
        int Energy = 0;
        double MacroEnergy = 0;
        double ProtGrams = 0;
        double CarbGrams = 0;
        double FatGrams = 0;

        DbHandler dh;
        DataSet dayInfo;
        DataSet foods;
        DataSet activities;

        public double Weight
        {
            get { return ( double)GetValue(WeightProperty); }
            set { SetValue(WeightProperty, value); }
        }

        public static readonly DependencyProperty WeightProperty =
            DependencyProperty.Register("Weight", typeof(double), typeof(DayExplorer));
                
        public DayExplorer(int PersonId)
        {
            InitializeComponent();
            dh = new DbHandler();
            this.PersonId = PersonId;
            this.Title = dh.getUserName(PersonId);
            calendarDayExp.SelectedDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            doUpdates();
        }

        /// <summary>
        /// Updates basic info of day, portions, activities and meters.
        /// </summary>
        public void doUpdates()
        {
            LoadDayInfo();
            LoadPortions();
            LoadActivities();
            UpdateMeters();
            isReady = true;
        }

        /// <summary>
        /// All meter updating done here:
        /// - Sleep
        /// - Weight
        /// - Energy and energy distribution from portions
        /// - Energy consumption from activities
        /// - Base energy consumption
        /// - Total energy consumption 
        /// </summary>

        public void UpdateMeters()
        {
            MacroEnergy = 0;
            ProtGrams = 0;
            CarbGrams = 0;
            FatGrams = 0;
            Energy = 0;
            TotalConsumption = 0;
            Activities = 0;
            BaseConsumption = 0;
            energyMtr.Cons = 0;

            /* Meters are shown ONLY if day's weight is set */

            if (dh.isDaySet(PersonId, (DateTime)calendarDayExp.SelectedDate))
            {
                /* Calculate energy distribution */

                foreach (DataRow drw in foods.Tables["annokset"].Rows)
                {
                    ProtGrams += Convert.ToDouble(Convert.ToDecimal(drw.ItemArray[6]));
                }

                foreach (DataRow drw in foods.Tables["annokset"].Rows)
                {
                    CarbGrams += Convert.ToDouble(Convert.ToDecimal(drw.ItemArray[7]));
                }

                foreach (DataRow drw in foods.Tables["annokset"].Rows)
                {
                    FatGrams += Convert.ToDouble(Convert.ToDecimal(drw.ItemArray[8]));
                }

                foreach (DataRow drw in foods.Tables["annokset"].Rows)
                {
                    Energy += (int)drw.ItemArray[5];
                }
                labelEnergiaa.Content = Energy.ToString();

                foreach (DataRow drw in activities.Tables["suoritukset"].Rows)
                {
                    Activities += (int)drw.ItemArray[5];
                }

                labelActivities.Content = Activities.ToString();
                labelEnergiaa.Content = Energy.ToString();

                /* Calculate calorie requirements */

                if (dh.getUserSex(PersonId).Equals("M") && textBoxWeight.Text.Length > 0)
                {
                    BaseConsumption = 88.362 + ((double)13.397 * (double)Convert.ToDouble(textBoxWeight.Text)) + ((double)4.799 * (double)dh.getUserHeight(PersonId)) - ((double)5.677 * (double)dh.getUserAge(PersonId));
                }
                if (dh.getUserSex(PersonId).Equals("N") && textBoxWeight.Text.Length > 0)
                {
                    BaseConsumption = 447.593 + ((double)9.247 * (double)Convert.ToDouble(textBoxWeight.Text)) + ((double)3.098 * (double)dh.getUserHeight(PersonId)) - ((double)4.330 * (double)dh.getUserAge(PersonId));
                }
                
                /* Take base activity in account */

                switch (comboBoxActivity.SelectedIndex)
                {
                    case (0):
                        BaseConsumption = BaseConsumption * 1.2;
                        break;
                    case (1):
                        BaseConsumption = BaseConsumption * 1.375;
                        break;
                    case (2):
                        BaseConsumption = BaseConsumption * 1.55;
                        break;
                    case (3):
                        BaseConsumption = BaseConsumption * 1.725;
                        break;
                    case (4):
                        BaseConsumption = BaseConsumption * 1.9;
                        break;
                    default:
                        break;
                }

                labelPerusKulutus.Content = Convert.ToInt16(BaseConsumption).ToString();
                TotalConsumption = Convert.ToInt16(BaseConsumption + Activities);
                labelKokonaisKulutus.Content = TotalConsumption.ToString();

                /* Macro distribution */
                MacroEnergy = (double)(ProtGrams * 4 + CarbGrams * 4 + FatGrams * 9);
                distributionMtr.Visibility = Visibility.Visible;
                distributionMtr.P = (double)(((ProtGrams * 4) / MacroEnergy) * 100);
                distributionMtr.C = (double)(((CarbGrams * 4) / MacroEnergy) * 100);
                distributionMtr.F = (double)(((FatGrams * 9) / MacroEnergy) * 100);
                distributionMtr.Update();
            }
            else
            {
                /* day is not set, zero everything */
                labelKokonaisKulutus.Content = "0";
                labelPerusKulutus.Content = "0";
                labelEnergiaa.Content = "0";
                labelActivities.Content = "0";

            }
            
            /* Only show distribution if portions are added! */

            if (Energy > 0)
            {
                distributionMtr.Visibility = Visibility.Visible;
            }
            else
            {
                distributionMtr.Visibility = Visibility.Hidden;
            }

            /* Energy amount, no zeros in division! */
            if (Energy != 0 && TotalConsumption != 0)
            {
                energyMtr.Cons = ((double)Energy / (double)TotalConsumption) * 100;
            }
            energyMtr.Update();
            energyMtr.UpdateLayout();

        }
        /*
        public void addToActivities(int d, int duration)
        {            
            LoadActivities();
        }
        */

        /// <summary>
        /// Gets portion from DayExplorerFoodList. Adds portion to database, 
        /// updates datagrid and meters.
        /// </summary>
        /// <param name="d">DatarowView that consists needed food parameters</param>
        /// <param name="amount">How large portion is, in grams</param>
        /// <param name="when">Consumed exactly when? Optional.</param>

        public void addToPortions(DataRowView d, int amount, DateTime when)
        {

            double i = (double)amount / (double)100;
            Decimal energy = Convert.ToDecimal(i) * Convert.ToDecimal(d.Row.ItemArray[2]);
            int e = Convert.ToInt16(energy);

            dh.addPortion(PersonId, 
                          (DateTime)calendarDayExp.SelectedDate, 
                          amount,
                          when, 
                          Convert.ToString(d.Row.ItemArray[1]), 
                          e, 
                          Convert.ToDecimal(i) * Convert.ToDecimal(d.Row.ItemArray[3]), 
                          Convert.ToDecimal(i) * Convert.ToDecimal(d.Row.ItemArray[4]), 
                          Convert.ToDecimal(i) * Convert.ToDecimal(d.Row.ItemArray[5]));
            LoadPortions();
            UpdateMeters();
        }

        /// <summary>
        /// Gets activity from DayExplorerActivityList. Adds done activity to database, 
        /// updates datagrid and meters.
        /// </summary>
        /// <param name="d">DatarowView that consists needed activity parameters</param>
        /// <param name="amount">Length of activity, in minutes</param>
        /// <param name="when">Done exactly when? Optional.</param>
        public void addToDoneActivities(DataRowView d, int duration, DateTime when)
        {
            Decimal energyPerMinute = Convert.ToDecimal(d.Row.ItemArray[2]) / 60m;
            int e = Convert.ToInt16(energyPerMinute * duration);

            dh.addDoneActivity(PersonId,
                          (DateTime)calendarDayExp.SelectedDate,
                          duration,
                          when,
                          Convert.ToString(d.Row.ItemArray[1]),
                          e);
            LoadActivities();
            UpdateMeters();
        }


        /// <summary>
        /// Basic info loaded here: weight, sleep, base activity parameters
        /// </summary>
        public void LoadDayInfo()
        {
            if (dh.isDaySet(PersonId, (DateTime)calendarDayExp.SelectedDate))
            {
                dayInfo = dh.getDayInfo(PersonId, (DateTime)calendarDayExp.SelectedDate);
                Weight = (double)Convert.ToDecimal(dayInfo.Tables["paivat"].Rows[0].ItemArray[2]);
                textBoxSleep.Text = dayInfo.Tables["paivat"].Rows[0].ItemArray[3].ToString();
                comboBoxActivity.SelectedIndex = Convert.ToInt16(dayInfo.Tables["paivat"].Rows[0].ItemArray[5]);
            }
            else
            {
                Weight = 0;
                //textBoxWeight.Text = "0";
                textBoxSleep.Text = "0";
                comboBoxActivity.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// Fills activity datagrid
        /// </summary>
        public void LoadActivities()
        {
            activities = dh.getDoneActivities(PersonId, (DateTime)calendarDayExp.SelectedDate);
            datagridDoneActivities.DataContext = activities.Tables[0].DefaultView;
        }

        /// <summary>
        /// Fills portion datagrid
        /// </summary>
        public void LoadPortions()
        {        
            foods = dh.getPortions(PersonId, (DateTime)calendarDayExp.SelectedDate);
            datagridPortions.DataContext = foods.Tables[0].DefaultView;
        }

        /// <summary>
        /// Checks if day's basic info can be committed to database.
        /// </summary>
        /// <returns></returns>
        public bool validateDay()
        {
            textBoxWeight.GetBindingExpression(TextBox.TextProperty).UpdateSource();

            if (!textBoxWeight.GetBindingExpression(TextBox.TextProperty).HasError)
            {
                return true;
            }
            else
            {
                MessageBox.Show("Syötä ensin päivän paino.");
                return false;
            }
        }

        /// <summary>
        /// Saves day info to database.
        /// </summary>
        public void saveDayData()
        {
            if(validateDay())
            {
                if (dh.isDaySet(PersonId, (DateTime)calendarDayExp.SelectedDate))
                {
                    dh.modifyDay(PersonId, (DateTime)calendarDayExp.SelectedDate, Convert.ToDecimal(textBoxWeight.Text), Convert.ToDecimal(textBoxSleep.Text), 0, comboBoxActivity.SelectedIndex);
                }
                else
                {
                    dh.addDay(PersonId, (DateTime)calendarDayExp.SelectedDate, Convert.ToDecimal(textBoxWeight.Text), Convert.ToDecimal(textBoxSleep.Text), 0, comboBoxActivity.SelectedIndex);
                }
                UpdateMeters();
            }
        }

        /*
         * Controls
         */

        private void buttonAddPortion_Click(object sender, RoutedEventArgs e)
        {
            if (validateDay())
            {
                saveDayData();
                DayExplorerFoodList defl = new DayExplorerFoodList();
                defl.Owner = this;
                defl.ShowDialog();
            }
        }

        private void buttonAddActivity_Click(object sender, RoutedEventArgs e)
        {
            if (validateDay())
            {
                saveDayData();
                DayExplorerActivityList deal = new DayExplorerActivityList();
                deal.Owner = this;
                deal.ShowDialog();
            }
        }

        private void buttonDeletePortion_Click(object sender, RoutedEventArgs e)
        {
            if (datagridPortions.SelectedIndex != -1)
            {
                DataRowView selected = (DataRowView)datagridPortions.SelectedItem;
                dh.removePortionById((int)selected.Row.ItemArray[9]);
                this.LoadPortions();
            }
            UpdateMeters();
        }

        private void buttonDeleteActivity_Click(object sender, RoutedEventArgs e)
        {
            if (datagridDoneActivities.SelectedIndex != -1)
            {
                DataRowView selected = (DataRowView)datagridDoneActivities.SelectedItem;
                dh.removeDoneActivityById((int)selected.Row.ItemArray[6]);
                this.LoadActivities();
            }
            UpdateMeters();
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            saveDayData();
        }

        private void calendarDayExp_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            doUpdates();
        }

        private void comboBoxActivity_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IsLoaded)
            {
                UpdateMeters();
            }
        }

        private void buttonRaport_Click(object sender, RoutedEventArgs e)
        {
            Raport r = new Raport(PersonId);
            r.ShowDialog();
        }
    }

    /// <summary>
    /// Options for DayExplorer's activity combobox
    /// </summary>
    public class BaseActivityClasses : List<String>
    {
        public BaseActivityClasses()
        {
            this.Add("1 - Erittäin kevyt");
            this.Add("2 - Kevyt");
            this.Add("3 - Kohtalainen");
            this.Add("4 - Raskas");
            this.Add("5 - Erittäin raskas");
        }
    }

    /// <summary>
    /// Converts full datetime to time only, for the daily portion list.
    /// </summary>
    public class TimeOnlyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime v = (DateTime)value;
            String hours = v.Hour.ToString();
            String minutes = v.Minute.ToString();

            if (hours.Length < 2)
            {
                hours = "0" + hours;
            }
            if (minutes.Length < 2)
            {
                minutes = "0" + minutes;
            }
            
            String time = hours + ":" + minutes;
            return time;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) 
        {
            return null;
        }
    }
}
