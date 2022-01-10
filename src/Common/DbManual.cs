using System.Collections.ObjectModel;

namespace fcrd
{
    public static class DbManual
    {
        private static ObservableCollection<fcrd.Project> _project;
        private static ObservableCollection<fcrd.Category> _category;
        private static ObservableCollection<fcrd.Account> _account;
        private static ObservableCollection<fcrd.Payee> _payee;
        private static ObservableCollection<Currency> _currencies;
        private static ObservableCollection<fcrd.Years> _years;
        private static ObservableCollection<fcrd.YearMonths> _yearMonths;

        public static void ResetManuals()
        {
            DbManual._project = (ObservableCollection<fcrd.Project>)null;
            DbManual._category = (ObservableCollection<fcrd.Category>)null;
            DbManual._account = (ObservableCollection<fcrd.Account>)null;
            DbManual._currencies = (ObservableCollection<Currency>)null;
            DbManual._yearMonths = (ObservableCollection<fcrd.YearMonths>)null;
            DbManual._years = (ObservableCollection<fcrd.Years>)null;
        }

        public static ObservableCollection<fcrd.Project> Project
        {
            get
            {
                if (DbManual._project == null)
                {
                    DB.GetData<fcrd.Project>("select _id,  title from project where title is not null  order by 2 desc", DbManual._project = new ObservableCollection<fcrd.Project>());
                    DbManual._project.Insert(0, new fcrd.Project());
                }
                return DbManual._project;
            }
        }

        public static ObservableCollection<fcrd.Category> Category
        {
            get
            {
                if (DbManual._category == null)
                {
                    DB.GetData<fcrd.Category>("\r\n                    select\r\n                    _id,\r\n                    title, \r\n                    (select count(*) from category x where x.left < ctx.left and x.[right] > ctx.[right] ) as level\r\n                    from category ctx\r\n                    order by left, sort_order", DbManual._category = new ObservableCollection<fcrd.Category>());
                    DbManual._category.Insert(0, new fcrd.Category());
                }
                return DbManual._category;
            }
        }

        public static ObservableCollection<fcrd.Account> Account
        {
            get
            {
                if (DbManual._account == null)
                {
                    DB.GetData<fcrd.Account>("select _id,  title from account where title is not null  order by 2 desc", DbManual._account = new ObservableCollection<fcrd.Account>());
                    DbManual._account.Insert(0, new fcrd.Account());
                }
                return DbManual._account;
            }
        }

        public static ObservableCollection<fcrd.Payee> Payee
        {
            get
            {
                if (DbManual._payee == null)
                {
                    DB.GetData<fcrd.Payee>("select _id,  title from payee where title is not null order by 2 desc", DbManual._payee = new ObservableCollection<fcrd.Payee>());
                    DbManual._payee.Insert(0, new fcrd.Payee());
                }
                return DbManual._payee;
            }
        }

        public static ObservableCollection<Currency> Currencies
        {
            get
            {
                if (DbManual._currencies == null)
                {
                    DB.GetData<Currency>("select * from currency", DbManual._currencies = new ObservableCollection<Currency>());
                    DbManual._currencies.Insert(0, new Currency()
                    {
                        Name = "Все валюты"
                    });
                }
                return DbManual._currencies;
            }
        }

        public static ObservableCollection<fcrd.Years> Years
        {
            get
            {
                if (DbManual._years == null)
                {
                    DB.GetData<fcrd.Years>("select distinct date_year as year from transactions order by 1 desc", DbManual._years = new ObservableCollection<fcrd.Years>());
                    DbManual._years.Insert(0, new fcrd.Years());
                }
                return DbManual._years;
            }
        }

        public static ObservableCollection<fcrd.YearMonths> YearMonths
        {
            get
            {
                if (DbManual._yearMonths == null)
                {
                    DB.GetData<fcrd.YearMonths>("select distinct date_year as year, date_month as month from transactions order by 1 desc, 2 asc", DbManual._yearMonths = new ObservableCollection<fcrd.YearMonths>());
                    DbManual._yearMonths.Insert(0, new fcrd.YearMonths());
                }
                return DbManual._yearMonths;
            }
        }
    }
}