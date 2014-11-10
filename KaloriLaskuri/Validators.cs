using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Threading;
using System.Globalization;

namespace KaloriLaskuri
{
    public class IntegerRule : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            int i = 0;
            try
            {
                i = Convert.ToInt32(value);
            }
            catch (FormatException)
            {
                return new ValidationResult(false, "Syötteen tulee olla kokonaisluku.");
            }
            if (i < 0)
            {
                return new ValidationResult(false, "Syötteen tulee nolla tai suurempi.");
            }
            else
            {
                return ValidationResult.ValidResult;
            }
        }
    }
    public class PositiveIntegerRule : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            int i = 0;
            try
            {
                i = Convert.ToInt32(value);
            }
            catch (FormatException)
            {
                return new ValidationResult(false, "Syötteen tulee olla kokonaisluku.");
            }
            if (i > 0)
            {
                return ValidationResult.ValidResult;
            }
            else
            {
                return new ValidationResult(false, "Syötteen tulee olla suurempi kuin nolla.");
            }
        }
    }
    public class PositiveDoubleRule : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            double i = 0;
            try
            {
                i = Convert.ToDouble(value);
            }
            catch (FormatException)
            {
                return new ValidationResult(false, "Syötteen tulee olla kokonaisluku.");
            }
            if (i > 0)
            {
                return ValidationResult.ValidResult;
            }
            else
            {
                return new ValidationResult(false, "Syötteen tulee yksi tai suurempi.");
            }
        }
    }

    public class HourRule : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            int i = 0;
            try
            {
                i = Convert.ToInt32(value);
            }
            catch (FormatException)
            {
                return new ValidationResult(false, "Syötteen tulee olla kokonaisluku.");
            }
            if (i < 0 || i > 23)
            {
                return new ValidationResult(false, "Syötteen tulee olla väliltä 0-23");
            }
            else
            {
                return ValidationResult.ValidResult;
            }
        }
    }

    public class MinuteRule : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            int i = 0;
            try
            {
                i = Convert.ToInt32(value);
            }
            catch (FormatException)
            {
                return new ValidationResult(false, "Syötteen tulee olla kokonaisluku.");
            }
            if (i < 0 || i > 59)
            {
                return new ValidationResult(false, "Syötteen tulee olla väliltä 0-59");
            }
            else
            {
                return ValidationResult.ValidResult;
            }
        }
    }

    public class DecimalRule : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            String val = value as String;
            //Decimal i;
            
            try
            {
                Console.WriteLine(value.ToString());
                Decimal i = Convert.ToDecimal(val);

                if (i != null && i < 0)
                {
                    return new ValidationResult(false, "Syötteen tulee olla nolla tai suurempi.");
                }
                else
                {
                    return ValidationResult.ValidResult;
                }
            }
            catch (FormatException)
            {
                return new ValidationResult(false, "Syötteen tulee olla desimaaliluku.");
            }

        }
    }
    public class DateTimeRule : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            DateTime t;
            try
            {
                t = Convert.ToDateTime(value);
            }
            catch (FormatException)
            {
                return new ValidationResult(false, "Syötettä ei voitu muuttaa päivämääräksi");
            }
            if (t == null)
            {
                return new ValidationResult(false, "Syötettä ei voitu muuttaa päivämääräksi.");
            }
            else
            {
                return ValidationResult.ValidResult;
            }
        }
    }
}
