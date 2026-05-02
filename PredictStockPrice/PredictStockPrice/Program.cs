using System;
using System.Windows.Forms;

namespace PredictStockPrice
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // var data = Helper.DbHelper.GetData();
            Actions.ALGLIB_SSAStock.Execute();

            Application.Run(new Form1());
        }
    }
}
