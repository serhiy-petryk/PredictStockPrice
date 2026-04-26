using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using PredictStockPrice.Models;

namespace PredictStockPrice.Helper
{
    public static class DbHelper
    {
        private const string DbConnectionString = "Data Source=localhost;Initial Catalog=dbQ2024;Integrated Security=True;Connect Timeout=150;Encrypt=false";
        public static Dictionary<string, List<Quote>> GetData()
        {
            var data = new Dictionary<string, List<Quote>>();
            using (var conn = new SqlConnection(DbConnectionString))
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = "SELECT * from dbQ2024..DayPolygon where symbol='XOM' and year(date) between 2020 and 2025 order by date";
                cmd.CommandTimeout = 250;
                cmd.CommandType = CommandType.Text;
                using (var rdr = cmd.ExecuteReader())
                    while (rdr.Read())
                    {
                        var item = new Quote(rdr);
                        if (!data.ContainsKey(item.Symbol)) data.Add(item.Symbol, new List<Quote>());
                        data[item.Symbol].Add(item);
                    }
            }

            return data;
        }

    }
}
