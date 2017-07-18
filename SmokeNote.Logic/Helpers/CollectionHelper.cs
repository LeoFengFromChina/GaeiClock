using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SmokeNote.Logic.Helpers
{
    public class CollectionHelper
    {
        private CollectionHelper()
        {
        }

        public static List<T> ConvertTo<T>(IList<DataRow> rows)
        {
            List<T> list = null;

            if (rows != null)
            {
                list = new List<T>();

                foreach (DataRow row in rows)
                {
                    T item = CreateItem<T>(row);
                    list.Add(item);
                }
            }

            return list;
        }

        public static List<T> ConvertTo<T>(DataTable table)
        {
            if (table == null)
            {
                return null;
            }

            List<DataRow> rows = new List<DataRow>();

            foreach (DataRow row in table.Rows)
            {
                rows.Add(row);
            }

            return ConvertTo<T>(rows);
        }

        public static T CreateItem<T>(DataRow row)
        {
            T obj = default(T);
            if (row != null)
            {
                obj = Activator.CreateInstance<T>();

                Type type = obj.GetType();

                foreach (DataColumn column in row.Table.Columns)
                {
                    PropertyInfo prop = type.GetProperty(column.ColumnName);

                    if (prop == null)
                    {
                        continue;
                    }

                    try
                    {
                        object value = row[column.ColumnName];
                        if (!(value is DBNull))
                        {
                            if (value.GetType() == prop.PropertyType)
                            {
                                prop.SetValue(obj, value, null);
                            }
                            else // 尝试做类型转换
                            {
                                if (prop.PropertyType == typeof(decimal))
                                {
                                    var v = Convert.ToDecimal(value);
                                    prop.SetValue(obj, v, null);
                                }
                            }
                        }
                    }
                    catch
                    {
                        // You can log something here   
                        throw;
                    }
                }
            }

            return obj;
        }
    }
}
