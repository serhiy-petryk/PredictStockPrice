using System;
using System.Data.Common;

namespace PredictStockPrice.Models
{
    public class Quote
    {
        public string Symbol { get; set; }
        public DateTime Date { get; set; }
        public float Open { get; set; }
        public float High { get; set; }
        public float Low { get; set; }
        public float Close { get; set; }
        public float Volume { get; set; }

        public Quote(DbDataReader rdr)
        {
            Symbol = (string)rdr["Symbol"];
            Date = (DateTime)rdr["Date"];
            Open = (float)rdr["Open"]; ;
            High = (float)rdr["High"]; ;
            Low = (float)rdr["Low"]; ;
            Close = (float)rdr["Close"]; ;
            Volume = (float)rdr["Volume"]; ;
        }
    }
}
