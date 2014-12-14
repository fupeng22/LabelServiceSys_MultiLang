using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Data.OleDb;
using System.Data;

namespace Util
{
    public class ExcelHelper
    {
        /// <summary>
        /// 将数据导出至Excel
        /// </summary>
        /// <param name="list">数据</param>
        /// <param name="excelFilePath">Excel文件路径</param>
        /// <returns></returns>
        public static bool ExportExcel<T>(IList<T> list, string excelFilePath)
        {
            PropertyInfo[] propertyInfos = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            //连接字符串
            string connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + excelFilePath +
                             ";Extended Properties=\"Excel 12.0 Xml;HDR=Yes;IMEX=0\"";

            // SQL语句
            var tableStructBuilder = new StringBuilder(@"Create Table Sheet1 (");
            var insertSqlBuilder = new StringBuilder(@"Insert into Sheet1 (");
            var insertSqlValueBuilder = new StringBuilder(@"Values (");

            var paras = new Dictionary<string, OleDbParameter>();

            foreach (PropertyInfo pi in propertyInfos)
            {
                paras.Add(pi.Name, new OleDbParameter("@" + pi.Name, OleDbType.VarChar));
                tableStructBuilder.Append("[" + pi.Name + "] varchar,");

                insertSqlBuilder.Append("[" + pi.Name + "],");
                insertSqlValueBuilder.Append("@" + pi.Name + ",");
            }

            // 创建Excel文件
            using (var conn = new OleDbConnection(connString))
            {
                try
                {
                    conn.Open();
                    using (var cmd = new OleDbCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.Text;
                        // 创建表结构
                        cmd.CommandText = tableStructBuilder.ToString().TrimEnd(',') + ")";
                        cmd.ExecuteNonQuery();

                        // 插入数据
                        cmd.CommandText = insertSqlBuilder.ToString().TrimEnd(',') + ")" +
                                          insertSqlValueBuilder.ToString().TrimEnd(',') + ")";
                        cmd.Parameters.AddRange(paras.Select(p => p.Value).ToArray());

                        foreach (T entity in list)
                        {
                            foreach (PropertyInfo pi in propertyInfos)
                            {

                                paras[pi.Name].Value = Convert.ToString(pi.GetValue(entity, null) ?? "");
                            }
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception ex)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
