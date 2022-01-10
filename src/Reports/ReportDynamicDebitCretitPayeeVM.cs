using System.Windows;

namespace fcrd
{
    [Header("Динамика расходов-доходов")]
    public class ReportDynamicDebitCretitPayeeVM : BaseReportVM<ReportDynamicDebitCretitPayeeM>
    {
        private const string BaseSqlText = "\r\n                        select \r\n                            tx.date_year as date_year, tx.date_month as date_month, round(tx.total / 100.00, 2) as total\r\n                        from \r\n                        (select trn.date_year, trn.date_month,  trn.category_id, sum(case when {0} = 1 then from_amount else from_amount_default_crr end) as total\r\n                        from transactions trn \r\n                        where (payee_id > 0 or category_id > 0 or project_id > 0)\r\n                            {1} /*FILTERS*/\r\n                        group by trn.date_year, trn.date_month ) tx \r\n                        order by \r\n                            tx.date_year, tx.date_month\r\n\r\n        ";

        protected override string GetSql()
        {
            long? id;
            int num1;
            if (!this.Payee.ID.HasValue)
            {
                id = this.Category.ID;
                num1 = id.HasValue ? 1 : 0;
            }
            else
                num1 = 1;
            if (num1 == 0)
            {
                int num2 = (int)MessageBox.Show("Укажите получателя или категорию!");
                return string.Empty;
            }
            string str = string.Empty;
            id = this.CurentCurrency.ID;
            if (id.HasValue)
                str = string.Format(" and from_account_crc_id = {0}", (object)this.CurentCurrency.ID);
            string standartTrnFilter = this.GetStandartTrnFilter();
            if (standartTrnFilter != string.Empty)
                str = str + " and " + standartTrnFilter;
            id = this.CurentCurrency.ID;
            return string.Format("\r\n                        select \r\n                            tx.date_year as date_year, tx.date_month as date_month, round(tx.total / 100.00, 2) as total\r\n                        from \r\n                        (select trn.date_year, trn.date_month,  trn.category_id, sum(case when {0} = 1 then from_amount else from_amount_default_crr end) as total\r\n                        from transactions trn \r\n                        where (payee_id > 0 or category_id > 0 or project_id > 0)\r\n                            {1} /*FILTERS*/\r\n                        group by trn.date_year, trn.date_month ) tx \r\n                        order by \r\n                            tx.date_year, tx.date_month\r\n\r\n        ", (object)(id.HasValue ? 1 : 0), (object)str);
        }
    }
}