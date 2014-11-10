using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Windows;

namespace KaloriLaskuri
{
    /// <summary>
    /// Handles all database actions.
    /// </summary>
    class DbHandler
    {
        SqlConnection conn;

        public void connect()
        {
            conn = new SqlConnection();
            String path = System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);

            conn.ConnectionString = "Data Source=.\\SQLEXPRESS;AttachDbFilename="+ path + "\\KaloriLaskuriKanta.mdf;Integrated Security=True;Connect Timeout=30;User Instance=True";

//            conn.ConnectionString = "Data Source=.\\SQLEXPRESS;AttachDbFilename=D:\\C#\\GKO-Kalorilaskuri\\KaloriLaskuri\\KaloriLaskuri\\KaloriLaskuriKanta.mdf;Integrated Security=True;Connect Timeout=30;User Instance=True";
            conn.Open();
        }
        public void disconnect()
        {
            conn.Close();
        }
        public void runSQL(SqlCommand cmd)
        {
            cmd.Connection = conn;
            cmd.ExecuteNonQuery();
        }

        /* DAYS */

        public double getAverageWeight(int personId, DateTime start, DateTime end)
        {
            this.connect();
            SqlCommand cmd = new SqlCommand("SELECT AVG(paino) FROM paivat WHERE kayttaja_id = @personId", conn);
            cmd.Parameters.AddWithValue("@personId", personId);
            return Convert.ToDouble(cmd.ExecuteScalar());
        }
        public double getMinWeight(int personId, DateTime start, DateTime end)
        {
            this.connect();
            SqlCommand cmd = new SqlCommand("SELECT MIN(paino) FROM paivat WHERE kayttaja_id = @personId", conn);
            cmd.Parameters.AddWithValue("@personId", personId);
            return Convert.ToDouble(cmd.ExecuteScalar());
        }
        public double getMaxWeight(int personId, DateTime start, DateTime end)
        {
            this.connect();
            SqlCommand cmd = new SqlCommand("SELECT MAX(paino) FROM paivat WHERE kayttaja_id = @personId", conn);
            cmd.Parameters.AddWithValue("@personId", personId);
            return Convert.ToDouble(cmd.ExecuteScalar());
        }

        public DataSet getDays(int personId, DateTime start, DateTime end)
        {
            DataSet ds = new DataSet();
            this.connect();
            SqlCommand cmd = new SqlCommand("SELECT * FROM paivat WHERE kayttaja_id = @personId AND paivamaara >= @start AND paivamaara <= @end", conn);
            cmd.Parameters.AddWithValue("@personId", personId);
            cmd.Parameters.AddWithValue("@start", start);
            cmd.Parameters.AddWithValue("@end", end);
            cmd.ExecuteNonQuery();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds, "paivat");
            this.disconnect();
            return ds;
        }

        public void addDay(int personId, DateTime d, Decimal weight,
                       Decimal sleep, Decimal waist, int activity)
        {
            this.connect();
            SqlCommand cmd = new SqlCommand("INSERT INTO paivat (kayttaja_id, paivamaara, paino, unimaara, vyotaro, aktiivisuus) VALUES (@personId, @date, @weight, @sleep, @waist, @activity)", conn);
            cmd.Parameters.AddWithValue("@personId", personId);
            cmd.Parameters.AddWithValue("@date", d);
            cmd.Parameters.AddWithValue("@weight", weight);
            cmd.Parameters.AddWithValue("@sleep", sleep);
            cmd.Parameters.AddWithValue("@waist", waist);
            cmd.Parameters.AddWithValue("@activity", activity);
            cmd.ExecuteNonQuery();
            this.disconnect();
        }
        
        public void modifyDay(int personId, DateTime d, Decimal weight,
                       Decimal sleep, Decimal waist, int activity)
        {
            this.connect();
            SqlCommand cmd = new SqlCommand("UPDATE paivat SET paino = @weight, unimaara = @sleep, aktiivisuus = @activity WHERE kayttaja_id = @personId AND paivamaara = @date", conn);
            cmd.Parameters.AddWithValue("@personId", personId);
            cmd.Parameters.AddWithValue("@date", d);
            cmd.Parameters.AddWithValue("@weight", weight);
            cmd.Parameters.AddWithValue("@sleep", sleep);
            cmd.Parameters.AddWithValue("@activity", activity);
            cmd.ExecuteNonQuery();
            this.disconnect();
        }

        public bool isDaySet(int personId, DateTime d)
        {
            int num;
            this.connect();
            SqlCommand cmd = new SqlCommand("SELECT count(*) FROM paivat WHERE kayttaja_id = @personId AND paivamaara = @date", conn);
            cmd.Parameters.AddWithValue("@personId", personId);
            cmd.Parameters.AddWithValue("@date", d);
            num = (int)cmd.ExecuteScalar();
            if (num > 0) { return true; } else { return false; } 
        }

        public DataSet getDayInfo(int personId, DateTime d)
        {
            DataSet ds = new DataSet();
            this.connect();
            SqlCommand cmd = new SqlCommand("SELECT * FROM paivat WHERE kayttaja_id = @personId AND paivamaara = @date", conn);
            cmd.Parameters.AddWithValue("@personId", personId);
            cmd.Parameters.AddWithValue("@date", d);
            cmd.ExecuteNonQuery();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds, "paivat");
            this.disconnect();
            return ds;
        }

        /* DONE ACTIVITIES */

        public void addDoneActivity(int personId, DateTime d, int duration,
                                    DateTime when, String activityName, int energy)
        {
            this.connect();
            SqlCommand cmd = new SqlCommand("INSERT INTO suoritukset (kayttaja_id, paivamaara, kesto, suoritus_aika, nimi, energia) VALUES (@personId, @date, @duration, @when, @activity_name, @energy)", conn);
            cmd.Parameters.AddWithValue("@personId", personId);
            cmd.Parameters.AddWithValue("@date", d);
            cmd.Parameters.AddWithValue("@duration", duration);
            cmd.Parameters.AddWithValue("@when", when);
            cmd.Parameters.AddWithValue("@activity_name", activityName);
            cmd.Parameters.AddWithValue("@energy", energy);
            cmd.ExecuteNonQuery();
            this.disconnect();
        }

        public DataSet getDoneActivities(int personId, DateTime d)
        {
            this.connect();
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand("SELECT * FROM suoritukset WHERE kayttaja_id = @id AND paivamaara = @date", conn);
            cmd.Parameters.AddWithValue("@id", personId);
            cmd.Parameters.AddWithValue("@date", d);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds, "suoritukset");
            this.disconnect();
            return ds;
        }

        public void removeDoneActivityById(int id)
        {
            this.connect();
            SqlCommand cmd = new SqlCommand("DELETE FROM suoritukset WHERE suoritus_id = @id", conn);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
            this.disconnect();
        }

        /* PORTIONS */

        public void addPortion(int personId, DateTime d, int amount,
                               DateTime when, String foodName, int energy,
                                Decimal protein, Decimal carbs, Decimal fat)
        {
            this.connect();
            SqlCommand cmd = new SqlCommand("INSERT INTO annokset (kayttaja_id, paivamaara, syoty_maara, syonti_aika, nimi, energia, proteiini, hiilihydraatti, rasva) VALUES (@personId, @date, @amount, @when, @foodName, @energy, @protein, @carbs, @fat)", conn);
            cmd.Parameters.AddWithValue("@personId", personId);
            cmd.Parameters.AddWithValue("@date", d);
            cmd.Parameters.AddWithValue("@amount", amount);
            cmd.Parameters.AddWithValue("@when", when);
            cmd.Parameters.AddWithValue("@foodName", foodName);
            cmd.Parameters.AddWithValue("@energy", energy);
            cmd.Parameters.AddWithValue("@protein", protein);
            cmd.Parameters.AddWithValue("@carbs", carbs);
            cmd.Parameters.AddWithValue("@fat", fat);
            cmd.ExecuteNonQuery();
            this.disconnect();
        }

        public DataSet getPortions(int personId, DateTime d)
        {
            this.connect();
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand("SELECT * FROM annokset WHERE kayttaja_id = @id AND paivamaara = @date", conn);
            cmd.Parameters.AddWithValue("@id", personId);
            cmd.Parameters.AddWithValue("@date", d);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds, "annokset");
            this.disconnect();
            return ds;
        }

        public void removePortionById(int id)
        {
            this.connect();
            SqlCommand cmd = new SqlCommand("DELETE FROM annokset WHERE annos_id = @id", conn);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
            this.disconnect();
        }
        /* USERS */

        public String getUserName(int personId)
        {
            this.connect();
            SqlCommand cmd = new SqlCommand("SELECT etunimi+SPACE(1)+sukunimi FROM kayttajat WHERE kayttaja_id = @personId", conn);
            cmd.Parameters.AddWithValue("@personId", personId);
            return (String)cmd.ExecuteScalar();
        }

        public Decimal getUserHeight(int personId)
        {
            this.connect();
            SqlCommand cmd = new SqlCommand("SELECT pituus FROM kayttajat WHERE kayttaja_id = @personId", conn);
            cmd.Parameters.AddWithValue("@personId", personId);
            return (Decimal)cmd.ExecuteScalar();
        }

        public Decimal getUserAge(int personId)
        {
            DateTime birthdate;
            this.connect();
            SqlCommand cmd = new SqlCommand("SELECT syntyma_aika FROM kayttajat WHERE kayttaja_id = @personId", conn);
            cmd.Parameters.AddWithValue("@personId", personId);
            birthdate = (DateTime)cmd.ExecuteScalar();
            return DateTime.Now.Year - birthdate.Year;
        }

        public String getUserSex(int personId)
        {
            this.connect();
            SqlCommand cmd = new SqlCommand("SELECT sukupuoli FROM kayttajat WHERE kayttaja_id = @personId", conn);
            cmd.Parameters.AddWithValue("@personId", personId);
            return (String)cmd.ExecuteScalar();
        }

        public void addUser(String lastName, String firstName, 
                            String gender, DateTime birthDate,
                            Decimal height)
        {
            this.connect();
            SqlCommand cmd = new SqlCommand("INSERT INTO kayttajat (sukunimi, etunimi, syntyma_aika, sukupuoli, pituus) VALUES (@lastname, @firstname, @birthdate, @gender, @height)", conn);
            cmd.Parameters.AddWithValue("@lastname", lastName);
            cmd.Parameters.AddWithValue("@firstname", firstName);
            cmd.Parameters.AddWithValue("@birthdate", birthDate);
            cmd.Parameters.AddWithValue("@gender", gender);
            cmd.Parameters.AddWithValue("@height", height);
            cmd.ExecuteNonQuery();
            this.disconnect();
        }

        public void modifyUser(int id, String lastName, String firstName,
                                String gender, DateTime birthDate,
                                Decimal height)
        {
            this.connect();
            SqlCommand cmd = new SqlCommand("UPDATE kayttajat SET sukunimi = @lastname, etunimi = @firstname, syntyma_aika = @birthdate, sukupuoli = @gender, pituus = @height WHERE kayttaja_id = @id", conn);
            cmd.Parameters.AddWithValue("@lastname", lastName);
            cmd.Parameters.AddWithValue("@firstname", firstName);
            cmd.Parameters.AddWithValue("@birthdate", birthDate);
            cmd.Parameters.AddWithValue("@gender", gender);
            cmd.Parameters.AddWithValue("@height", height);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
            this.disconnect();
        }

        public void removeUserById(int id)
        {
            this.connect();

            SqlCommand cmd = new SqlCommand("DELETE FROM annokset WHERE kayttaja_id = @id", conn);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();

            cmd = new SqlCommand("DELETE FROM suoritukset WHERE kayttaja_id = @id", conn);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();

            cmd = new SqlCommand("DELETE FROM paivat WHERE kayttaja_id = @id", conn);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
            
            cmd = new SqlCommand("DELETE FROM kayttajat WHERE kayttaja_id = @id", conn);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
            this.disconnect();
        }

        public DataSet getUsers()
        {
            this.connect();
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand("select * from kayttajat ORDER BY sukunimi, etunimi", conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds, "kayttajat");
            this.disconnect();
            return ds;
        }

        /* ACTIVITIES */

        public DataSet getActivities()
        {
            this.connect();
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand("select * from aktiviteetit ORDER BY nimi", conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds, "aktiviteetit");
            this.disconnect();
            return ds;
        }

        public void addActivity(String name, int energy_usage)
        {
            this.connect();
            SqlCommand cmd = new SqlCommand("INSERT INTO aktiviteetit (nimi, kulutus) VALUES (@name, @energy_usage)", conn);
            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@energy_usage", energy_usage);
            cmd.ExecuteNonQuery();
            this.disconnect();
        }

        public void removeActivityById(int id)
        {
            this.connect();
            SqlCommand cmd = new SqlCommand("DELETE FROM aktiviteetit WHERE aktiviteetti_id = @id", conn);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
            this.disconnect();
        }

        public void modifyActivity(int id, String name, int energy_usage)
        {
            this.connect();
            SqlCommand cmd = new SqlCommand("UPDATE aktiviteetit SET nimi = @name, kulutus = @energy_usage WHERE aktiviteetti_id = @id", conn);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@energy_usage", energy_usage);
            cmd.ExecuteNonQuery();
            this.disconnect();
        }

        /* FOODS */

        public DataSet getFoods()
        {
            this.connect();
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand("select * from ruoka_aineet ORDER BY nimi", conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds, "ruoka_aineet");
            this.disconnect();
            return ds;
        }

        public void addFood(String name, int energy,
                            Decimal protein, Decimal carbs, 
                            Decimal fat, int category)
        {
            this.connect();
            SqlCommand cmd = new SqlCommand("INSERT INTO ruoka_aineet (nimi, energia, proteiini, hiilihydraatti, rasva, kategoria) VALUES (@name, @energy, @protein, @carbs, @fat, @category)", conn);
            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@energy", energy);
            cmd.Parameters.AddWithValue("@protein", protein);
            cmd.Parameters.AddWithValue("@carbs", carbs);
            cmd.Parameters.AddWithValue("@fat", fat);
            cmd.Parameters.AddWithValue("@category", category);
            cmd.ExecuteNonQuery();
            this.disconnect();
        }

        public void removeFoodById(int id)
        {
            this.connect();
            SqlCommand cmd = new SqlCommand("DELETE FROM ruoka_aineet WHERE ruoka_aine_id = @id", conn);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
            this.disconnect();
        }

        public void modifyFood(int id, String name, int energy,
                                Decimal protein, Decimal carbs,
                                Decimal fat, int category)
        {
            this.connect();
            SqlCommand cmd = new SqlCommand("UPDATE ruoka_aineet SET nimi = @name, energia = @energy, proteiini = @protein, hiilihydraatti = @carbs, rasva = @fat, kategoria = @category WHERE ruoka_aine_id = @id", conn);
            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@energy", energy);
            cmd.Parameters.AddWithValue("@protein", protein);
            cmd.Parameters.AddWithValue("@carbs", carbs);
            cmd.Parameters.AddWithValue("@fat", fat);
            cmd.Parameters.AddWithValue("@category", category);
            cmd.Parameters.AddWithValue("@id", id);

            cmd.ExecuteNonQuery();
            this.disconnect();
        }

        public void deleteAllRecords()
        {
            List<SqlCommand> commands = new List<SqlCommand>();

            commands.Add(new SqlCommand("DELETE FROM suoritukset", conn));
            commands.Add(new SqlCommand("DELETE FROM annokset", conn));
            commands.Add(new SqlCommand("DELETE FROM aktiviteetit", conn));
            commands.Add(new SqlCommand("DELETE FROM ruoka_aineet", conn));
            commands.Add(new SqlCommand("DELETE FROM paivat", conn));
            commands.Add(new SqlCommand("DELETE FROM kayttajat", conn));

            foreach (SqlCommand cmd in commands)
            {
                cmd.ExecuteNonQuery();
            }

        }

        public void deleteUsersFoodsActivities()
        {
            List<SqlCommand> commands = new List<SqlCommand>();

            commands.Add(new SqlCommand("DELETE FROM aktiviteetit", conn));
            commands.Add(new SqlCommand("DELETE FROM kayttajat", conn));
            commands.Add(new SqlCommand("DELETE FROM ruoka_aineet", conn));

            foreach (SqlCommand cmd in commands)
            {
                cmd.ExecuteNonQuery();
            }

        }
        
        public void insertDummyData()
        {
            List<SqlCommand> commands = new List<SqlCommand>();

            //kayttajat
            
            DateTime ika1 = new DateTime(1980, 2, 10).Date;
            DateTime ika2 = new DateTime(1970, 7, 1).Date;
            DateTime ika3 = new DateTime(1985, 3, 25).Date;

            commands.Add(new SqlCommand("INSERT INTO kayttajat (sukunimi, etunimi, syntyma_aika, sukupuoli, pituus) VALUES ('Takalo', 'Kari', @ika, 'M', 187)", conn));
            commands.Last().Parameters.AddWithValue("@ika", ika1);
            commands.Add(new SqlCommand("INSERT INTO kayttajat (sukunimi, etunimi, syntyma_aika, sukupuoli, pituus) VALUES ('Meikäläinen', 'Matti', @ika, 'M', 165)", conn));
            commands.Last().Parameters.AddWithValue("@ika", ika2);
            commands.Add(new SqlCommand("INSERT INTO kayttajat (sukunimi, etunimi, syntyma_aika, sukupuoli, pituus) VALUES ('Meikäläinen', 'Maija', @ika, 'N', 150)", conn));
            commands.Last().Parameters.AddWithValue("@ika", ika3);

            //ruoka-aineet

            commands.Add(new SqlCommand("INSERT INTO ruoka_aineet (nimi, energia, proteiini, hiilihydraatti, rasva) VALUES ('Lasagne', 173, 8.9, 14.6, 8.5)", conn));
            commands.Add(new SqlCommand("INSERT INTO ruoka_aineet (nimi, energia, proteiini, hiilihydraatti, rasva) VALUES ('Vaasan ruispalat', 252, 9.2, 45.4, 1.3)", conn));
            commands.Add(new SqlCommand("INSERT INTO ruoka_aineet (nimi, energia, proteiini, hiilihydraatti, rasva) VALUES ('Maito 1%', 43, 3.4, 4.8, 1)", conn));
            commands.Add(new SqlCommand("INSERT INTO ruoka_aineet (nimi, energia, proteiini, hiilihydraatti, rasva) VALUES ('Meijerivoi', 725, 1.2, 0, 81.3)", conn));
            commands.Add(new SqlCommand("INSERT INTO ruoka_aineet (nimi, energia, proteiini, hiilihydraatti, rasva) VALUES ('Peruna, keitetty', 76, 1.9, 15.5, 0.2)", conn));
            commands.Add(new SqlCommand("INSERT INTO ruoka_aineet (nimi, energia, proteiini, hiilihydraatti, rasva) VALUES ('Broilerin rintafilee, paistettu', 165, 27.3, 0, 6.1)", conn));
            commands.Add(new SqlCommand("INSERT INTO ruoka_aineet (nimi, energia, proteiini, hiilihydraatti, rasva) VALUES ('Broilerin koipireisi, paistettu', 293, 22.9, 0, 22.6)", conn));
            commands.Add(new SqlCommand("INSERT INTO ruoka_aineet (nimi, energia, proteiini, hiilihydraatti, rasva) VALUES ('Omena', 33, 0.1, 6.6, 0)", conn));
            commands.Add(new SqlCommand("INSERT INTO ruoka_aineet (nimi, energia, proteiini, hiilihydraatti, rasva) VALUES ('Appelsiini', 47, 0.6, 8.9, 0.3)", conn));
            commands.Add(new SqlCommand("INSERT INTO ruoka_aineet (nimi, energia, proteiini, hiilihydraatti, rasva) VALUES ('Maksalaatikko', 111, 5.9, 15.1, 2.8)", conn));
            
            //aktiviteetit

            commands.Add(new SqlCommand("INSERT INTO aktiviteetit (nimi, kulutus) VALUES ('Kävely, 6km/h', 400)", conn));
            commands.Add(new SqlCommand("INSERT INTO aktiviteetit (nimi, kulutus) VALUES ('Juoksu, 8km/h', 820)", conn));
            commands.Add(new SqlCommand("INSERT INTO aktiviteetit (nimi, kulutus) VALUES ('Hiihto, rauhallinen', 500)", conn));
            commands.Add(new SqlCommand("INSERT INTO aktiviteetit (nimi, kulutus) VALUES ('Uinti, kohtalainen rasitus', 670)", conn));
            commands.Add(new SqlCommand("INSERT INTO aktiviteetit (nimi, kulutus) VALUES ('Pyöräily, rauhallinen', 340)", conn));
            commands.Add(new SqlCommand("INSERT INTO aktiviteetit (nimi, kulutus) VALUES ('Pyöräily, vauhdikas', 500)", conn));
            commands.Add(new SqlCommand("INSERT INTO aktiviteetit (nimi, kulutus) VALUES ('Golf', 420)", conn));
            commands.Add(new SqlCommand("INSERT INTO aktiviteetit (nimi, kulutus) VALUES ('Sulkapallo, vauhdikas', 340)", conn));
            
            foreach (SqlCommand cmd in commands)
            {
                cmd.ExecuteNonQuery();
            }
        }
    }
}
