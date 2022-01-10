using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;

namespace fcrd
{
    public class DataLoader
    {
        private readonly string _backupDir;

        public DataLoader(string backupDir) => this._backupDir = backupDir;

        public void Start()
        {
            string str = FilePrepare.Prepare(this._backupDir);
            SQLiteTransaction sqLiteTransaction = DB.Connection.BeginTransaction();
            DB.TruncateTables();
            DataReader dataReader = new DataReader();
            dataReader.OnEntityRead += new DataReader.EntityReadDelegate(this.DataReaderOnEntityRead);
            dataReader.Start(str);
            File.Delete(str);
            this.PrepareData();
            sqLiteTransaction.Commit();
            DbManual.ResetManuals();
        }

        private void DataReaderOnEntityRead(string type, Dictionary<string, string> items)
        {
            TableInfo tableInfo = DB.TablesInfo.FirstOrDefault<TableInfo>((Func<TableInfo, bool>)(p => p.TableName == type));
            if (tableInfo == null)
                return;
            StringBuilder stringBuilder1 = new StringBuilder(string.Format("INSERT INTO [{0}] (", (object)type));
            StringBuilder stringBuilder2 = new StringBuilder("VALUES (");
            using (Dictionary<string, string>.Enumerator enumerator = items.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    KeyValuePair<string, string> item = enumerator.Current;
                    ColumnInfo columnInfo = tableInfo.ColumnsInfo.FirstOrDefault<ColumnInfo>((Func<ColumnInfo, bool>)(p => p.ColName == item.Key));
                    if (columnInfo != null)
                    {
                        StringBuilder stringBuilder3 = stringBuilder1;
                        KeyValuePair<string, string> keyValuePair = item;
                        string key1 = keyValuePair.Key;
                        keyValuePair = items.First<KeyValuePair<string, string>>();
                        string key2 = keyValuePair.Key;
                        string format1 = key1 == key2 ? "[{0}] " : ",[{0}] ";
                        keyValuePair = item;
                        string key3 = keyValuePair.Key;
                        stringBuilder3.AppendFormat(format1, (object)key3);
                        string str1;
                        if (columnInfo.Type.Substring(0, 3) == "DAT")
                        {
                            keyValuePair = item;
                            double result;
                            double.TryParse(keyValuePair.Value, out result);
                            str1 = DataLoader.UnixTimeStampToDateTime(result / 1000.0).ToString("yyyy-MM-dd HH:mm:ss");
                        }
                        else
                        {
                            keyValuePair = item;
                            str1 = keyValuePair.Value.Replace("'", "''");
                        }
                        StringBuilder stringBuilder4 = stringBuilder2;
                        keyValuePair = item;
                        string key4 = keyValuePair.Key;
                        keyValuePair = items.First<KeyValuePair<string, string>>();
                        string key5 = keyValuePair.Key;
                        string format2 = key4 == key5 ? "'{0}' " : ",'{0}' ";
                        string str2 = str1;
                        stringBuilder4.AppendFormat(format2, (object)str2);
                    }
                }
            }
            stringBuilder1.Append(")");
            stringBuilder2.Append(")");
            DB.ExecuteNonQuery(stringBuilder1.AppendLine(stringBuilder2.ToString()).ToString());
        }

        private void PrepareData()
        {
            DB.ExecuteNonQuery("\r\n                                    update currency_exchange_rate \r\n                                    set rate_date = date( rate_date);\r\n\r\n                                    update currency_exchange_rate \r\n                                    set rate_date_end =  (select min (rate_date) from currency_exchange_rate x \r\n                                                         where x.from_currency_id = currency_exchange_rate.from_currency_id \r\n                                                             and x.to_currency_id = currency_exchange_rate.to_currency_id\r\n                                                             and x.rate_date > currency_exchange_rate.rate_date ) \r\n                                    ;  \r\n\r\n                                    update currency_exchange_rate \r\n                                    set rate_date_end = '9999-12-31'\r\n                                    where rate_date_end is null;\r\n                                ");
            DB.ExecuteNonQuery("\r\n                                    /* ид валют счетов проводки */\r\n                                    update transactions\r\n                                    set from_account_crc_id = (select currency_id from account acc where acc._id = from_account_id );\r\n\r\n                                    update transactions\r\n                                    set to_account_crc_id = (select currency_id from account acc where acc._id = to_account_id );\r\n                                    \r\n                                    /* сума проводки в домашней валюте*/\r\n                                    update transactions\r\n                                    set from_amount_default_crr =\r\n                                            case (select _id from currency where is_default = 1)\r\n                                                when from_account_crc_id then from_amount\r\n                                                else round(from_amount * ( select rate from currency_exchange_rate \r\n                                                        where to_currency_id = (select _id from currency where is_default = 1) \r\n                                                            and from_currency_id =  from_account_crc_id\r\n                                                            and datetime between rate_date and rate_date_end ) , 0 )\r\n                                            end;\r\n        \r\n                                    update transactions\r\n                                    set to_amount_default_crr =\r\n                                            case (select _id from currency where is_default = 1)\r\n                                                when to_account_crc_id then to_amount\r\n                                                else round(to_amount * ( select rate from currency_exchange_rate \r\n                                                        where to_currency_id = (select _id from currency where is_default = 1) \r\n                                                            and from_currency_id =  to_account_crc_id\r\n                                                            and datetime between rate_date and rate_date_end ) , 0 )\r\n                                            end;   \r\n\r\n                                     /* дата проводки по частям */\r\n                                    update transactions\r\n                                    set \r\n                                        date_year = strftime('%Y', datetime),\r\n                                        date_month = strftime('%m', datetime),\r\n                                        date_day = strftime('%d', datetime),\r\n                                        date_week = strftime('%W', datetime), \r\n                                        date_weekday = strftime('%w', datetime);\r\n                                ");
            DB.ExecuteNonQuery("\r\n                                   update account\r\n                                    set total_amount_indef =\r\n                                            case (select _id from currency where is_default = 1)\r\n                                                when currency_id then total_amount\r\n                                                else round(total_amount * ( select rate from currency_exchange_rate \r\n                                                        where to_currency_id = (select _id from currency where is_default = 1) \r\n                                                            and from_currency_id =  currency_id\r\n                                                            and rate_date_end = '9999-12-31' ) , 0 )\r\n                                            end; \r\n                                ");
            DB.ExecuteNonQuery("\r\n                                   update account\r\n                                    set total_amount_indef =\r\n                                            case (select _id from currency where is_default = 1)\r\n                                                when currency_id then total_amount\r\n                                                else round(total_amount * ( select rate from currency_exchange_rate \r\n                                                        where to_currency_id = (select _id from currency where is_default = 1) \r\n                                                            and from_currency_id =  currency_id\r\n                                                            and rate_date_end = '9999-12-31' ) , 0 )\r\n                                            end; \r\n                                ");
        }

        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp) => new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(unixTimeStamp).ToLocalTime();
    }
}