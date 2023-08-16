using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Data;
using System.Reflection;
using System.ComponentModel;
//using Google.Protobuf.WellKnownTypes;

namespace GAServices.Common
{
    public static class Utilities
    {
        public static bool IsNothingOrDBNull(object objVal)
        {
            if (objVal == null)
            {
                return true;
            }
            else if (Convert.IsDBNull(objVal))
            {
                return true;
            }
            else if (objVal.ToString() == "")
            {
                return true;
            }

            return false;
        }

        public static void AddColumnsToDataTable(string[] Columns, DataTable dt)
        {
            try
            {
                if (dt.Columns.Count <= 0)
                {
                    foreach (string col in Columns)
                    {
                        dt.Columns.Add(col);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Creates a datatable with the columns and rows as given in the dataCollection dictionary
        /// </summary>
        /// <param name="dataCollection"></param>
        /// <returns></returns>
        public static DataTable CreateDataTable(Dictionary<string, string> dataCollection)
        {
            DataTable dt = new DataTable();
            DataRow dr;
            DataColumn dc;

            try
            {
                dr = dt.NewRow();

                foreach (KeyValuePair<string, string> kvp in dataCollection)
                {
                    dc = new DataColumn(kvp.Key);
                    dt.Columns.Add(dc);
                }

                foreach (KeyValuePair<string, string> kvp in dataCollection)
                {
                    dr[kvp.Key] = kvp.Value;
                }

                dt.Rows.Add(dr);

                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable CreateDataTable(string[] data, string columnName)
        {
            DataTable dt = new DataTable();
            DataRow dr;
            DataColumn dc;

            try
            {
                dc = new DataColumn(columnName);
                dt.Columns.Add(dc);

                foreach (string value in data)
                {
                    dr = dt.NewRow();

                    dr[columnName] = value;

                    dt.Rows.Add(dr);
                }

                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Clone and Returns the records from UpdatedTable that are actually modified from Original SourceTable
        /// It expects that the UpdatedTable should contain all or some of the columns that are present in SourceTable, with same column names
        /// Both should have the same column as the primary key
        /// </summary>
        /// <param name="SourceTable"></param>
        /// <param name="updatedTable"></param>
        /// <param name="PrimaryKeyColumnName"></param>
        /// <returns>Empty datatable or a datatable with the Modified records</returns>        
        public static DataTable GetUpdatedData(DataTable SourceTable, DataTable updatedTable, string PrimaryKeyColumnName)
        {
            DataTable dtTemp = SourceTable.Copy();
            DataTable dtResult = updatedTable.Clone();

            DataView originalView = SourceTable.DefaultView;
            string[] strColumnNames;
            DataRow rtRow;

            strColumnNames = (from dc in updatedTable.Columns.Cast<DataColumn>()
                              select dc.ColumnName).ToArray();

            dtTemp = originalView.ToTable(true, strColumnNames);

            dtTemp.PrimaryKey = new DataColumn[] { dtTemp.Columns[PrimaryKeyColumnName] };
            updatedTable.PrimaryKey = new DataColumn[] { updatedTable.Columns[PrimaryKeyColumnName] };

            if (updatedTable.Rows.Count > 0)
            {
                for (int i = 0; i <= updatedTable.Rows.Count - 1; i++)
                {
                    //If the source table has the record in the updated table, 
                    //then check if it is updated or not, to include in the return datatable.
                    if (dtTemp.Rows.Find(updatedTable.Rows[i][PrimaryKeyColumnName]) != null)
                    {
                        var sourceArray = dtTemp.Rows.Find(updatedTable.Rows[i][PrimaryKeyColumnName]).ItemArray;
                        var checkArray = updatedTable.Rows[i].ItemArray;

                        //Wrote the new Comparer as the default one does consider the datatype
                        //Like, it returns false when compares a numeric data as number format in one object and as string type in another object
                        //Ex: Condition 1 = "1" results as false. But we need to return true
                        //if (!sourceArray.ToArray.SequenceEqual(checkArray.ToArray, new DataRowComparer()))
                        if (!sourceArray.SequenceEqual(checkArray.ToArray(), new DataRowComparer()))
                        {
                            rtRow = dtResult.NewRow();
                            rtRow.ItemArray = checkArray;
                            dtResult.Rows.Add(rtRow);
                        }
                    }
                }
            }

            return dtResult;
        }

        /// <summary>
        /// Clone and Returns the records from UpdatedTable that are actually modified from Original SourceTable
        /// It expects that the UpdatedTable should contain all or some of the columns that are present in SourceTable, with same column names
        /// Both should have the same column as the primary key
        /// </summary>
        /// <param name="SourceTable"></param>
        /// <param name="updatedTable"></param>
        /// <param name="PrimaryKeyColumnName"></param>
        /// <returns>Empty datatable or a datatable with the Modified records</returns>
        public static DataTable GetUpdatedData(DataTable SourceTable, DataTable updatedTable, string[] PrimaryKeyColumnNames)
        {
            DataTable dtTemp = SourceTable.Copy();
            DataTable dtResult = updatedTable.Clone();
            List<DataColumn> tempPK, updatedPK;
            DataView originalView = SourceTable.DefaultView;
            string[] strColumnNames;
            DataRow rtRow;
            List<object> data;

            strColumnNames = (from dc in updatedTable.Columns.Cast<DataColumn>()
                              select dc.ColumnName).ToArray();

            dtTemp = originalView.ToTable(true, strColumnNames);

            tempPK = new List<DataColumn>();
            updatedPK = new List<DataColumn>();

            foreach (string PrimaryKeyColumnName in PrimaryKeyColumnNames)
            {
                tempPK.Add(dtTemp.Columns[PrimaryKeyColumnName]);
                updatedPK.Add(updatedTable.Columns[PrimaryKeyColumnName]);
            }

            dtTemp.PrimaryKey = tempPK.ToArray();   // new DataColumn[] { dtTemp.Columns[PrimaryKeyColumnName] };
            updatedTable.PrimaryKey = updatedPK.ToArray();  // new DataColumn[] { updatedTable.Columns[PrimaryKeyColumnName] };

            if (updatedTable.Rows.Count > 0)
            {
                for (int i = 0; i <= updatedTable.Rows.Count - 1; i++)
                {
                    data = new List<object>();

                    foreach (string PrimaryKeyColumnName in PrimaryKeyColumnNames)
                    {
                        data.Add(updatedTable.Rows[i][PrimaryKeyColumnName]);
                    }

                    //If the source table has the record in the updated table, 
                    //then check if it is updated or not, to include in the return datatable.
                    //if (dtTemp.Rows.Find(updatedTable.Rows[i][PrimaryKeyColumnName]) != null)
                    if (dtTemp.Rows.Find(data.ToArray()) != null)
                    {
                        //var sourceArray = dtTemp.Rows.Find(updatedTable.Rows[i][PrimaryKeyColumnName]).ItemArray;
                        var sourceArray = dtTemp.Rows.Find(data.ToArray()).ItemArray;
                        var checkArray = updatedTable.Rows[i].ItemArray;

                        //Wrote the new Comparer as the default one does consider the datatype
                        //Like, it returns false when compares a numeric data as number format in one object and as string type in another object
                        //Ex: Condition 1 = "1" results as false. But we need to return true
                        //if (!sourceArray.ToArray.SequenceEqual(checkArray.ToArray, new DataRowComparer()))
                        if (!sourceArray.SequenceEqual(checkArray.ToArray(), new DataRowComparer()))
                        {
                            rtRow = dtResult.NewRow();
                            rtRow.ItemArray = checkArray;
                            dtResult.Rows.Add(rtRow);
                        }
                    }
                }
            }

            return dtResult;
        }

        private static String[] units = { "Zero", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten",
                  "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };

        private static String[] tens = { "", "", "Twenty", "Thirty", "Fourty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };

        public static String NumberInWords(double amount)
        {
            try
            {
                Int64 amount_int = (Int64)amount;
                Int64 amount_dec = (Int64)Math.Round((amount - (double)(amount_int)) * 100);

                if (amount_dec == 0)
                {
                    return NumberToWords(amount_int) + " Only.";
                }
                else
                {
                    return NumberToWords(amount_int) + " and " + NumberToWords(amount_dec) + " Paise Only.";
                }
            }
            catch (Exception e)
            {
                // TODO: handle exception
                throw e;
            }
        }

        private static String NumberToWords(Int64 i)
        {
            if (i < 20)
            {
                return units[i];
            }
            if (i < 100)
            {
                return tens[i / 10] + ((i % 10 > 0) ? " " + NumberToWords(i % 10) : "");
            }
            if (i < 1000)
            {
                return units[i / 100] + " Hundred"
                        + ((i % 100 > 0) ? " And " + NumberToWords(i % 100) : "");
            }
            if (i < 100000)
            {
                return NumberToWords(i / 1000) + " Thousand "
                + ((i % 1000 > 0) ? " " + NumberToWords(i % 1000) : "");
            }
            if (i < 10000000)
            {
                return NumberToWords(i / 100000) + " Lakhs "
                        + ((i % 100000 > 0) ? " " + NumberToWords(i % 100000) : "");
            }
            if (i < 1000000000)
            {
                return NumberToWords(i / 10000000) + " Crores "
                        + ((i % 10000000 > 0) ? " " + NumberToWords(i % 10000000) : "");
            }
            return NumberToWords(i / 1000000000) + " Arab "
                    + ((i % 1000000000 > 0) ? " " + NumberToWords(i % 1000000000) : "");
        }

        public static DataTable ConvertListToDataTable<T>(IList<T> list)
        {
            DataTable table = new DataTable();
            FieldInfo[] fields = list.GetType().GetFields();
            try
            {
                if (list.Count > 0)
                {
                    if (typeof(T).Name.ToUpper() == "STRING" || typeof(T).IsPrimitive)
                    {
                        table.Columns.Add(new DataColumn(typeof(T).Name.ToUpper()));

                        foreach (T item in list)
                        {
                            DataRow row = table.NewRow();

                            row[0] = item;

                            table.Rows.Add(row);
                        }
                    }
                    else
                    {
                        foreach (PropertyInfo info in list[0].GetType().GetProperties())
                        {
                            if ((info.PropertyType).IsPrimitive || info.PropertyType.Name == "String")
                            {
                                table.Columns.Add(new DataColumn(info.Name, info.PropertyType));
                            }
                        }

                        foreach (T item in list)
                        {
                            DataRow row = table.NewRow();

                            foreach (DataColumn dc in table.Columns)
                            {
                                row[dc.ColumnName] = item.GetType().GetProperty(dc.ColumnName).GetValue(item, null);
                            }

                            table.Rows.Add(row);
                        }
                    }
                }
                else
                {
                    table = null;
                }
            }
            catch (Exception ex)
            { }
            return table;
        }

        public static DataTable ClassToDataTable<T>(this IEnumerable<T> source)
        {
            DataTable dt = new DataTable();
            var props = TypeDescriptor.GetProperties(typeof(T));

            foreach (PropertyDescriptor prop in props)
            {
                DataColumn dc = dt.Columns.Add(prop.Name, prop.PropertyType);
                dc.Caption = prop.DisplayName;
                dc.ReadOnly = prop.IsReadOnly;
            }
            foreach (T item in source)
            {
                DataRow dr = dt.NewRow();
                foreach (PropertyDescriptor prop in props)
                {
                    dr[prop.Name] = prop.GetValue(item);
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }

        public static T GetEnumVal<T>(string value)
        {
            if (value == "")
            {
                return GetEnumVal<T>(0);
            }
            else
            {
                return (T)Enum.Parse(typeof(T), value, true);
            }
        }

        public static T GetEnumVal<T>(int value)
        {
            return (T)Enum.GetValues(typeof(T)).GetValue(value);
        }

        public static string[] SplitToLines(string value, int maximumLineLength)
        {
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }

            string strValue = Regex.Replace(value, @"(.{1," + maximumLineLength + @"})(?:\s|$)", "$1\n");

            string[] returnValue = strValue.Split(new char[] { '\n' });

            if (returnValue.Length > 0)
                return returnValue.Where(x => !string.IsNullOrEmpty(x)).ToArray();
            else
                return null;
        }

        public static DataTable GetEnum_AsDataTable(System.Type enumType, string idColumnName, string valueColumnName)
        {
            DataTable dtValues = new DataTable();
            dtValues.Columns.Add(valueColumnName, typeof(string));
            dtValues.Columns.Add(idColumnName, Enum.GetUnderlyingType(enumType));

            foreach (string name in Enum.GetNames(enumType))
            {
                //Replace underscores with space from caption/key and add item to collection:
                dtValues.Rows.Add(name.Replace('_', ' '), Enum.Parse(enumType, name));
            }

            return dtValues;
        }

        // function that creates a list of an object from the given data table
        public static List<T> CreateListFromTable<T>(DataTable tbl) //where T : new()
        {
            List<T> lst = new List<T>();

            // go through each row
            foreach (DataRow r in tbl.Rows)
            {
                // add to the list
                lst.Add(CreateItemFromRow<T>(r));
            }

            // return the list
            return lst;
        }

        // function that creates an object from the given data row
        public static T CreateItemFromRow<T>(DataRow row) //where T : new()
        {
            // create a new object
            //T item = new T();
            T item = Activator.CreateInstance<T>();

            // set the item
            SetItemFromRow(item, row);

            // return 
            return item;
        }

        public static void SetItemFromRow<T>(T item, DataRow row) //where T : new()
        {
            // go through each column
            foreach (DataColumn c in row.Table.Columns)
            {
                c.ColumnName = c.ColumnName.ToLower();

                // find the property for the column
                PropertyInfo p = item.GetType().GetProperty(c.ColumnName.Replace("_", ""), BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                
                // if exists, set the value
                if (p != null && row[c] != DBNull.Value)
                {
                    p.Name.ToLower();
                    //p.SetValue(item, row[c], null);
                    //(T)Convert.ChangeType(Value, typeof(T));
                    //p.SetValue(item, Convert.ChangeType(row[c], typeof(item)), null);
                    p.SetValue(item, Convert.ChangeType(row[c], p.PropertyType), null);
                }
            }
        }
    }

    /// <summary>
    /// Custom class to implement the IEqualityComparer.
    /// To compare the values of two collections objects of same type
    /// </summary>
    public class DataRowComparer : IEqualityComparer<object>
    {
        public bool Equals(object x, object y)
        {
            bool xNumberCheck, yNumberCheck;
            double xNumericValue, yNumericValue;

            if (Convert.IsDBNull(x) & !Convert.IsDBNull(y) & y.ToString() != "")
            {
                return false;
            }
            else if (!Convert.IsDBNull(x) & x.ToString() != "" & Convert.IsDBNull(y))
            {
                return false;
            }
            else if (x.ToString().ToUpper() != y.ToString().ToUpper())
            {
                xNumberCheck = Double.TryParse(x.ToString(), out xNumericValue);
                yNumberCheck = Double.TryParse(y.ToString(), out yNumericValue);

                if (xNumberCheck && yNumberCheck)
                {
                    if (xNumericValue == yNumericValue)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }

                return false;
            }

            return true;
        }

        public int GetHashCode(object obj)
        {
            throw new NotImplementedException();
        }
    }

}
