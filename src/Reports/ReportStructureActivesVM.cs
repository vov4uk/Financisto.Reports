namespace fcrd
{
    [Header("Структура активов")]
    public class ReportStructureActivesVM : BaseReportVM<ReportStructureActivesM>
    {
        private const string BaseSqlText = "\r\n                             select \r\n                                title,\r\n                                round(case when {0}= 1 then total_amount else total_amount_indef end / 100.00, 2) as total\r\n                            from account\r\n                            where 1 = 1 \r\n                            {1} /*FILTERS*/\r\n                    ";

        protected override string GetSql()
        {
            string str = string.Empty;
            if (this.CurentCurrency.ID.HasValue)
                str = string.Format("and currency_id = {0}", (object)this.CurentCurrency.ID);
            return string.Format("\r\n                             select \r\n                                title,\r\n                                round(case when {0}= 1 then total_amount else total_amount_indef end / 100.00, 2) as total\r\n                            from account\r\n                            where 1 = 1 \r\n                            {1} /*FILTERS*/\r\n                    ", (object)(this.CurentCurrency.ID.HasValue ? 1 : 0), (object)str);
        }
    }
}