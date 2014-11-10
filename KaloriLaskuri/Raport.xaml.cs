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
    /// User can create report, which contains weight progress between two dates.
    /// </summary>
    public partial class Raport : Window
    {
        int PersonId;
        int rows = 0;
        DbHandler dh;
        DataSet ds;
        LineDiagram ld;

        public Raport(int PersonId)
        {
            InitializeComponent();
            this.PersonId = PersonId;
            dh = new DbHandler();
            ld = new LineDiagram();

            datePickerStart.SelectedDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            datePickerEnd.SelectedDate = DateTime.Now;
        }

        /// <summary>
        /// Get data from database and convert to stringformat, so the LineDiagram -component can use it.
        /// </summary>
        public void getConvertData()
        {
            rows = 0;

            ds = dh.getDays(PersonId, (DateTime)datePickerStart.SelectedDate, (DateTime)datePickerEnd.SelectedDate);
            
            foreach (DataRow drw in ds.Tables["paivat"].Rows)
            {
                rows++;
            }

            if (rows < 2)
            {
                MessageBox.Show("Valitulta aikaväliltä tulee löytyä vähintään kaksi merkintää.");
            }
            else
            {
                string[,] data = new string[rows, 2];
                int counter = 0;
                foreach (DataRow drw in ds.Tables["paivat"].Rows)
                {
                    DateTime t = (DateTime)drw.ItemArray[1];
                    String date = t.Year + "-" + t.Month + "-" + t.Day;
                    data[counter, 0] = date;
                    data[counter, 1] = Convert.ToString(Convert.ToDecimal(drw.ItemArray[2]));
                    counter++;
                }
                ld.yAxisName = "Paino";
                ld.xAxisName = "Aika";
                ld.setDataFormat(1);
                ld.setData(data);
                root.Children.Add(ld);
                Grid.SetRow(ld, 1);

                /* rest of the report */
                labelAvg.Content = "Paino, keskiarvo: " + dh.getAverageWeight(PersonId, (DateTime)datePickerStart.SelectedDate, (DateTime)datePickerEnd.SelectedDate).ToString();
                labelMin.Content = "Paino, alin: " + dh.getMinWeight(PersonId, (DateTime)datePickerStart.SelectedDate, (DateTime)datePickerEnd.SelectedDate).ToString();
                labelMax.Content = "Paino, ylin: " + dh.getMaxWeight(PersonId, (DateTime)datePickerStart.SelectedDate, (DateTime)datePickerEnd.SelectedDate).ToString();

            }
        }

        private void buttonGenerate_Click(object sender, RoutedEventArgs e)
        {
            if (datePickerStart.SelectedDate != null && datePickerEnd.SelectedDate != null)
            {
                getConvertData();
            }
            else
            {
                MessageBox.Show("Valitse ensin aloitus- ja lopetuspäivämäärät.");
            }
        }
    }
}
