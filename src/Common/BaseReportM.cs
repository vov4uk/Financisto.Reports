using System;
using System.Data.SQLite;
using System.Reflection;

namespace fcrd
{
    public class BaseReportM
    {
        public void Init(SQLiteDataReader reader)
        {
            foreach (PropertyInfo property in this.GetType().GetProperties())
            {
                FieldAttribute customAttribute = (FieldAttribute)Attribute.GetCustomAttribute((MemberInfo)property, typeof(FieldAttribute));
                if (customAttribute != null)
                {
                    int ordinal = reader.GetOrdinal(customAttribute.Name);
                    object obj = ordinal != -1 ? reader.GetValue(ordinal) : throw new Exception(string.Format("В классе [{0}] определен атрибут несуществующего поля [{1}] в ридере", (object)this.GetType(), (object)customAttribute.Name));
                    if (obj != DBNull.Value)
                        property.SetValue((object)this, obj, (object[])null);
                }
            }
        }
    }
}