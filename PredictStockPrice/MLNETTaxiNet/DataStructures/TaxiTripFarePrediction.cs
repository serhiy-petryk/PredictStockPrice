using Microsoft.ML.Data;

namespace MLNETTaxiNet.DataStructures
{
    public class TaxiTripFarePrediction
    {
        [ColumnName("Score")]
        public float FareAmount;
    }
}