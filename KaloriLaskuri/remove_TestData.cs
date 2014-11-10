using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace KaloriLaskuri
{
    class TestData
    {
        DataSet ds;
        
        public TestData()
        {
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = "Data Source=.\\SQLEXPRESS;AttachDbFilename=D:\\C#\\GKO-Kalorilaskuri\\KaloriLaskuri\\KaloriLaskuri\\KaloriLaskuriKanta.mdf;Integrated Security=True;Connect Timeout=30;User Instance=True";
            conn.Open();

            System.Console.WriteLine("Database open");

            // Insert

            DateTime ika = new DateTime(1983, 8, 19);
            SqlCommand insert = new SqlCommand("INSERT INTO kayttajat (sukunimi, etunimi, syntyma_aika, sukupuoli, pituus) VALUES ('Löytynoja', 'Lasse', @ika, 'M', 187)", conn);
            insert.Parameters.AddWithValue("@ika", ika);
            insert.ExecuteNonQuery();

            // Read to dataset

            String taulu = "kayttajat";

            ds = new DataSet();

            SqlCommand cmd = new SqlCommand("select * from kayttajat", conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds, taulu);
            //System.Console.WriteLine("Rivejä: " + x);

            conn.Close();
            System.Console.WriteLine("Database closed");

            // Iterate

            foreach (DataRow dr in ds.Tables["kayttajat"].Rows)
            {
                Console.WriteLine(dr["kayttaja_id"].ToString());
                Console.WriteLine(dr["sukunimi"].ToString());
            }

        }
    }
}
