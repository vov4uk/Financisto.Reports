﻿using System.ComponentModel;

namespace fcrd
{
    public class ReportByPeriodMonthCrcM : BaseReportM
    {
        [DisplayName("Год")]
        [Field("date_year")]
        public long Year { get; protected set; }

        [Field("date_month")]
        [DisplayName("Месяц")]
        public long Month { get; protected set; }

        public string PeriodDesr => string.Format("{0} {1}", (object)this.Month, (object)this.Year);

        [Field("credit_sum")]
        [DisplayName("Приход")]
        public double? CreditSum { get; protected set; }

        [DisplayName("Расход")]
        [Field("debit_sum")]
        public double? DebitSum { get; protected set; }

        [Field("saldo")]
        [DisplayName("Сальдо")]
        public double? Saldo { get; protected set; }
    }
}