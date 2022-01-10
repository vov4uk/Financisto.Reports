namespace fcrd
{
    [Header("По месяцам")]
    public class ReportByPeriodMonthCrcVM : BaseReportVM<ReportByPeriodMonthCrcM>
    {
        private const string BaseSqlText = "\r\n                select \r\n                    tx.date_year as date_year,\r\n                    tx.date_month as date_month,\r\n                    round(tx.credit_sum, 2) as credit_sum,\r\n                    round(tx.debit_sum, 2) as debit_sum,\r\n                    round(tx.credit_sum - tx.debit_sum, 2) as saldo\r\n                from (\r\n                select \r\n                    date_year,\r\n                    date_month,\r\n                    sum( case when from_amount > 0 then (case when {0} = 1 then from_amount else from_amount_default_crr end ) else 0 end) / 100.00 as credit_sum,\r\n                    sum( case when from_amount < 0 then - (case when {0} = 1 then from_amount else from_amount_default_crr end ) else 0 end) / 100.00  as debit_sum\r\n                from transactions \r\n                where to_account_id = 0 and (payee_id > 0 or category_id > 0 or project_id > 0)\r\n                        {1} /*FILTERS*/\r\n                group by \r\n                    date_year,\r\n                    date_month\r\n                order by\r\n                    date_year,\r\n                    date_month\r\n                ) tx \r\n        ";

        protected override string GetSql()
        {
            string str = string.Empty;
            if (this.CurentCurrency.ID.HasValue)
                str = string.Format("and from_account_crc_id = {0}", (object)this.CurentCurrency.ID);
            string standartTrnFilter = this.GetStandartTrnFilter();
            if (standartTrnFilter != string.Empty)
                str = str + " and " + standartTrnFilter;
            return string.Format("\r\n                select \r\n                    tx.date_year as date_year,\r\n                    tx.date_month as date_month,\r\n                    round(tx.credit_sum, 2) as credit_sum,\r\n                    round(tx.debit_sum, 2) as debit_sum,\r\n                    round(tx.credit_sum - tx.debit_sum, 2) as saldo\r\n                from (\r\n                select \r\n                    date_year,\r\n                    date_month,\r\n                    sum( case when from_amount > 0 then (case when {0} = 1 then from_amount else from_amount_default_crr end ) else 0 end) / 100.00 as credit_sum,\r\n                    sum( case when from_amount < 0 then - (case when {0} = 1 then from_amount else from_amount_default_crr end ) else 0 end) / 100.00  as debit_sum\r\n                from transactions \r\n                where to_account_id = 0 and (payee_id > 0 or category_id > 0 or project_id > 0)\r\n                        {1} /*FILTERS*/\r\n                group by \r\n                    date_year,\r\n                    date_month\r\n                order by\r\n                    date_year,\r\n                    date_month\r\n                ) tx \r\n        ", (object)(this.CurentCurrency.ID.HasValue ? 1 : 0), (object)str);
        }
    }
}