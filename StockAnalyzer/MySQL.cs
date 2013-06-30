using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace SotckAnalyzer
{
    public class MySQL
    {

        public  static MySqlConnection GetConnection()
        {

            string connStr = "server=localhost;user=root;database=Stock;port=3306;password=Password1;";

            MySqlConnection conn = new MySqlConnection(connStr);
            return conn;
        }

        public static  void InsertDailyRecord(MySqlConnection conn,  List<TradeRecord> set)
        {
            MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand();

            try
            {
                
                cmd.Connection = conn;

                //cmd.CommandText = "insert into test(id,date) values(?id,?date)";
                //cmd.CommandText = "insert into StockHistory(stockId,tradeDate,tradeTime,tradeNumber,tradeMoney,tradePrice,tradePriceChange,tradeType) values('600001','2013-06-13','11:00:00','123','12345','6.5','0.1','B')";

                cmd.CommandText =
                     "insert into StockHistory(stockId,tradeDate,tradeTime,tradeNumber,tradeMoney,tradePrice,tradePriceChange,tradeType) values(?stockId,?date,?time,?number,?money,?tradePrice,?tradePriceChange,?type)";
                cmd.Prepare();

                cmd.Parameters.AddWithValue("?stockId", "10000");
                cmd.Parameters.AddWithValue("?date", "1900-00-00");
                cmd.Parameters.AddWithValue("?time", "10-00-00");
                cmd.Parameters.AddWithValue("?number", 0);
                cmd.Parameters.AddWithValue("?money", 0);
                cmd.Parameters.AddWithValue("?tradePrice", 1.000);
                cmd.Parameters.AddWithValue("?tradePriceChange", 1.000);
                cmd.Parameters.AddWithValue("?type", 'Z');

                foreach (TradeRecord tr in set)
                {

                    cmd.Parameters["?stockId"].Value= tr.stockId;
                    cmd.Parameters["?date"].Value = tr.recordDate;
                    cmd.Parameters["?time"].Value = tr.recordTime;
                    cmd.Parameters["?number"].Value = tr.number;
                    cmd.Parameters["?money"].Value = tr.money;
                    cmd.Parameters["?tradePrice"].Value = tr.price;
                    cmd.Parameters["?tradePriceChange"].Value = tr.priceChange;
                    cmd.Parameters["?type"].Value = tr.type;
                    cmd.ExecuteNonQuery();
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            Console.WriteLine("nDone.");
        }

        //public bool InsertData(MySqlConnection conn,
    }
}

