using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PredictStockPrice.Actions
{
    public static class ALGLIB_SSAStock
    {
        public static void Execute()
        {
            // 1. Prepare sample stock price data (Time series T)
            double[] stockPrices = new double[] { 150.2, 151.5, 152.1, 149.8, 153.4, 155.0, 154.2 };
            int windowLength = 3; // Window length L (e.g., L=14 is common for daily data)

            // 2. Initialize the SSA model
            alglib.ssa.ssamodel model = new alglib.ssa.ssamodel();
            alglib.ssa.ssacreate(model, alglib.serial);


            // 3. Set the time series data
            // alglib.ssa.ssasetdata1d(model, stockPrices, stockPrices.Length);
            alglib.ssa.ssaaddsequence(model, stockPrices, stockPrices.Length, alglib.serial);
            
            // 4. Set window length for embedding
            alglib.ssa.ssasetwindow(model, windowLength, alglib.serial);

            // 5. Perform decomposition
            /*alglib.ssa.ssaanalyze(model);

            // 6. Reconstruct the trend (using the first few singular values)
            // ALGLIB allows extracting specific components or denoised versions
            double[] trend;
            ssa.ssagetreconstructed(model, 0, out trend); // Extract the 1st component (trend)

            Console.WriteLine("Original vs. SSA Reconstructed Trend:");
            for (int i = 0; i < stockPrices.Length; i++)
            {
                Console.WriteLine($"{stockPrices[i]} -> {trend[i]:F2}");
            }*/
        }
    }
}