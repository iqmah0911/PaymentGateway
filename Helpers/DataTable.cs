using System;
using System.Collections.Generic;
using System.Reflection;
using System.Data;
using System.Linq;

namespace PaymentGateway21052021.Helpers
{
    public partial class DataTableConvert
    {
        public static List<T> ConvertDataTable<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }
        public static T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            try
            {
                var columns = dr.Table.Columns;
                var props = temp.GetProperties();


                foreach (PropertyInfo pro in props)
                {
                    var propColumn = columns.Cast<DataColumn>().FirstOrDefault(c => c.ColumnName.ToUpper() == pro.Name.ToUpper());
                    var type = pro.PropertyType;
                    object dataRowValue = null;
                    
                    if (!string.IsNullOrWhiteSpace(propColumn?.ColumnName))
                    {
                        dataRowValue = (dr[propColumn?.ColumnName] == null || dr[propColumn?.ColumnName] == DBNull.Value) ? GetDefaultValue(type) : dr[propColumn?.ColumnName];
                    }

                    if (dataRowValue != null)
                    {
                        var dValue = Convert.ChangeType(dataRowValue, type);
                        pro.SetValue(obj, dValue, null);
                    }
                    else
                        continue;

                }

                //foreach (DataColumn column in columns)
                //{
                //    foreach (PropertyInfo pro in  props)
                //    {
                //        var propColumn= columns.Cast<DataColumn>().FirstOrDefault(c => c.ColumnName== pro.Name.ToUpper());

                //        var dataRowValue = (dr[propColumn.ColumnName] == null || dr[propColumn.ColumnName] == DBNull.Value) ? string.Empty : dr[propColumn.ColumnName];

                //        if (pro.Name.ToUpper() == "FirstName".ToUpper() && column.ColumnName.ToUpper() == "FirstName".ToUpper())
                //            pro.SetValue(obj, (dr[column.ColumnName] == null || dr[column.ColumnName] == DBNull.Value) ? string.Empty : dr[column.ColumnName], null);
                //        else if (dataRowValue!=null)
                //            pro.SetValue(obj, (dr[column.ColumnName] == null || dr[column.ColumnName] == DBNull.Value) ? string.Empty : dr[column.ColumnName], null);
                //        else
                //            continue;

                //        //if (pro.Name.ToUpper() =="FirstName".ToUpper() && column.ColumnName.ToUpper()== "FirstName".ToUpper())
                //        //    pro.SetValue(obj, (dr[column.ColumnName] == null|| dr[column.ColumnName] == DBNull.Value) ? string.Empty : dr[column.ColumnName], null);
                //        //else if (pro.Name.ToUpper() == column.ColumnName.ToUpper())
                //        //    pro.SetValue(obj, (dr[column.ColumnName] == null || dr[column.ColumnName] == DBNull.Value) ? string.Empty : dr[column.ColumnName], null);
                //        //else
                //        //    continue;
                //    }
                //}
            }
            catch(Exception ex)
            {

            }
            
            
            return obj;
        }

        public static TReturn ResolveObject<T,TReturn>(T dr) //where T:class, TReturn : class
        {
            Type temp = typeof(T);
            Type tempReturn = typeof(TReturn);
            TReturn objReturn = Activator.CreateInstance<TReturn>();

            try
            {
                //var columns = dr.Table.Columns;
                var props = temp.GetProperties();
                var propsReturn = tempReturn.GetProperties();

                foreach (PropertyInfo pro in propsReturn)
                {
                    var propColumn = props.FirstOrDefault(c => c.Name.ToUpper() == pro.Name.ToUpper());
                    var type = pro.PropertyType;
                    object dataRowValue = null;

                    //if (!string.IsNullOrWhiteSpace(propColumn?.ColumnName))
                    //{
                    //    dataRowValue = (dr[propColumn?.ColumnName] == null || dr[propColumn?.ColumnName] == DBNull.Value) ? GetDefaultValue(type) : dr[propColumn?.ColumnName];
                    //}
                    dataRowValue = propColumn?.GetValue(dr);
                    if (dataRowValue != null)
                    {
                        var dValue = Convert.ChangeType(dataRowValue, type);
                        pro.SetValue(objReturn, dValue, null);
                    }
                    else
                        continue;

                }

            }
            catch (Exception ex)
            {

            }

            return objReturn;
        }

        static object GetDefaultValue(Type t)
        {
            if (t.IsValueType)
                return Activator.CreateInstance(t);

            return null;
        }
    }
}
