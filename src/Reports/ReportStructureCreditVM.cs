namespace fcrd
{
    [Header("Структура доходов")]
    public class ReportStructureCreditVM : BaseReportVM<ReportStructureCreditM>
    {
        private const string BaseSqlText = "\r\n                            select \r\n                                p.title as title,\r\n                                - round(tx.total / 100.00, 2) as total\r\n                            from (\r\n                            select t.category_id, sum(case when {0}=1 then t.from_amount else t.from_amount_default_crr end) as total\r\n                            from transactions t \r\n                            where \r\n                                (payee_id > 0 or category_id > 0 or project_id > 0) and t.from_amount > 0\r\n                                {1} /*FILTERS*/\r\n                            group by \r\n                                t.category_id\r\n                                ) tx inner join category p on p._id = tx.category_id\r\n                    ";

        protected override string GetSql()
        {
            string str = string.Empty;
            if (this.CurentCurrency.ID.HasValue)
                str = string.Format(" and from_account_crc_id = {0}", (object)this.CurentCurrency.ID);
            string standartTrnFilter = this.GetStandartTrnFilter();
            if (standartTrnFilter != string.Empty)
                str = str + " and " + standartTrnFilter;
            return string.Format("\r\n                            select \r\n                                p.title as title,\r\n                                - round(tx.total / 100.00, 2) as total\r\n                            from (\r\n                            select t.category_id, sum(case when {0}=1 then t.from_amount else t.from_amount_default_crr end) as total\r\n                            from transactions t \r\n                            where \r\n                                (payee_id > 0 or category_id > 0 or project_id > 0) and t.from_amount > 0\r\n                                {1} /*FILTERS*/\r\n                            group by \r\n                                t.category_id\r\n                                ) tx inner join category p on p._id = tx.category_id\r\n                    ", (object)(this.CurentCurrency.ID.HasValue ? 1 : 0), (object)str);
        }
    }
}