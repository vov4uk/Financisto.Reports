namespace fcrd
{
    [Header("Динамика остатков")]
    public class ReportDynamicRestVM : BaseReportVM<ReportDynamicRestM>
    {
        private const string BaseSqlText = "\r\n                           select\r\n                                    cr.year as year, cr.month as month, cr.day as day,  \r\n                                    round( (select sum(from_amount_default_crr) \r\n                                     from transactions trn where date(trn.datetime) <= cr.date \r\n                                            and to_account_id = 0 ) / 100.00, 2 )as total\r\n                                from                                                                         \r\n                                    (select distinct date(datetime) as date, date_year as year, date_month as month, date_day as day\r\n                                    from transactions                                    \r\n                                    where 1 = 1  \r\n                                    {0}/* FILTER */\r\n                                    ) cr\r\n                    ";

        protected override string GetSql()
        {
            string standartTrnFilter = this.GetStandartTrnFilter();
            return string.Format("\r\n                           select\r\n                                    cr.year as year, cr.month as month, cr.day as day,  \r\n                                    round( (select sum(from_amount_default_crr) \r\n                                     from transactions trn where date(trn.datetime) <= cr.date \r\n                                            and to_account_id = 0 ) / 100.00, 2 )as total\r\n                                from                                                                         \r\n                                    (select distinct date(datetime) as date, date_year as year, date_month as month, date_day as day\r\n                                    from transactions                                    \r\n                                    where 1 = 1  \r\n                                    {0}/* FILTER */\r\n                                    ) cr\r\n                    ", !string.IsNullOrEmpty(standartTrnFilter) ? (object)(" and " + standartTrnFilter) : (object)string.Empty);
        }
    }
}