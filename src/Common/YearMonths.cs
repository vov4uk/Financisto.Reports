namespace fcrd
{
    public class YearMonths : BaseReportM
    {
        [Field("year")]
        public long? Year { get; set; }

        [Field("month")]
        public long? Month { get; set; }

        public string Name => string.Format("{0} {1}", (object)this.Month, (object)this.Year);
    }
}