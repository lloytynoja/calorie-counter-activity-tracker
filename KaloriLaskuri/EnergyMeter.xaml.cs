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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KaloriLaskuri
{
    /// <summary>
    /// Shows how much energy is consumed vs. the need of it.
    /// </summary>
    public partial class EnergyMeter : UserControl
    {
        public double Cons { get; set; }

        public EnergyMeter()
        {
            InitializeComponent();
        }
        
        public double Consumed
        {
            get { return (double)GetValue(ConsumedProperty); }
            set { SetValue(ConsumedProperty, value); }
        }

        public static readonly DependencyProperty ConsumedProperty =
            DependencyProperty.Register("Consumed", typeof(double), typeof(EnergyMeter));
         
        public void Update()
        {

            Cons = Math.Round(Cons, 1);
            labelConsumed.Content = Cons + "%";
            labelConsumedZeroVal.Content = Cons + "%";
            Consumed = (double)460 / (double)100 * (double)Cons;

            /* Keep digits visible and inside meter*/

            if (Cons > 10)
            {
                labelConsumed.Visibility = Visibility.Visible;
                labelConsumedZeroVal.Visibility = Visibility.Hidden;
                Canvas.SetRight(labelConsumed, 2);
                Canvas.SetBottom(labelConsumed, 3);
            }
            else
            {
                labelConsumed.Visibility = Visibility.Hidden;
                labelConsumedZeroVal.Visibility = Visibility.Visible;
                Canvas.SetLeft(labelConsumed, 2);
                Canvas.SetBottom(labelConsumed, 3);
            }
            if (Consumed > 460) { Consumed = 460; }
        }
    }
}
