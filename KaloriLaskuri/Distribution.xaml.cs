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
    /// Shows how energy is distributed between protein/carbs/fat.
    /// </summary>
    public partial class Distribution : UserControl
    {
        public Distribution()
        {
            InitializeComponent();
        }

        public double P { get; set; }
        public double C { get; set; }
        public double F { get; set; }

        public double Protein
        {
            get { return (double)GetValue(ProteinProperty); }
            set { SetValue(ProteinProperty, value); }
        }

        public static readonly DependencyProperty ProteinProperty =
            DependencyProperty.Register("Protein", typeof(double), typeof(Distribution));

        public double Carbs
        {
            get { return (double)GetValue(CarbsProperty); }
            set { SetValue(CarbsProperty, value); }
        }

        public static readonly DependencyProperty CarbsProperty =
            DependencyProperty.Register("Carbs", typeof(double), typeof(Distribution));

        public double Fat
        {
            get { return (double)GetValue(FatProperty); }
            set { SetValue(FatProperty, value); }
        }

        public static readonly DependencyProperty FatProperty =
            DependencyProperty.Register("Fat", typeof(double), typeof(Distribution));

        /// <summary>
        /// Actual drawing is done here.
        /// </summary>
        public void Update()
        {
            P = Math.Round(P, 1);
            C = Math.Round(C, 1);
            F = Math.Round(F, 1);

            labelProtein.Content = "P" + P + "%";
            labelCarbs.Content = "H" + C + "%";
            labelFat.Content = "R" + F + "%";

            Canvas.SetLeft(labelProtein, 2);
            Canvas.SetBottom(labelProtein, 3);

            Canvas.SetLeft(labelCarbs, 2);
            Canvas.SetBottom(labelCarbs, 3);

            Canvas.SetLeft(labelFat, 2);
            Canvas.SetBottom(labelFat, 3);

            Protein = (double)460 / (double)100 * (double)P;
            Carbs = (double)460 / (double)100 * (double)C;
            Fat = (double)460 / (double)100 * (double)F;
        }
    }
}
