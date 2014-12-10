using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Collections;
using System.Data;
using System.Configuration;

/// <summary>
///MySqlDBHelper 的摘要说明
/// </summary>
public class MySqlDBHelper
{
    //public static string Conn = "Database='wp';Data Source='localhost';User Id='root';Password='root';charset='utf8';pooling=true";
    #region 字段
    private string conStr;
    /// <summary>
    /// 设置数据库连接字符串,配置文件中定义过可以不设置此处
    /// </summary>
    public string ConStr
    {
        set { conStr = value; }
    }
    #endregion

    #region 构造函数
    public MySqlDBHelper()
        : this(ConfigurationManager.ConnectionStrings["MySqlConn"].ConnectionString)
    {

    }
    public MySqlDBHelper(string connectionString)
    {
        this.conStr = connectionString;
    }
    #endregion

    #region 方法
    public int ExecuteNonQuery(string sql)
    {
        return ExecuteNonQuery(sql, CommandType.Text, null);
    }

    public int ExecuteNonQuery(string sql, CommandType commandType)
    {
        return ExecuteNonQuery(sql, commandType, null);
    }

    public int ExecuteNonQuery(string sql, CommandType commandType, MySqlParameter[] parameters)
    {
        int count = 0;
        try
        {
            using (MySqlConnection connection = new MySqlConnection(conStr))
            {
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.CommandType = commandType;//设置command的CommandType为指定的CommandType
                    //如果同时传入了参数，则添加这些参数
                    if (parameters != null)
                    {
                        foreach (MySqlParameter parameter in parameters)
                        {
                            command.Parameters.Add(parameter);
                        }
                    }
                    connection.Open();//打开数据库连接
                    count = command.ExecuteNonQuery();
                }
            }
            return count;//返回执行增删改操作之后，数据库中受影响的行数
        }
        catch (Exception)
        {
            return 0;
        }
    }

    public Object ExecuteScalar(string sql)
    {
        return ExecuteScalar(sql, CommandType.Text, null);
    }

    public Object ExecuteScalar(string sql, CommandType commandType)
    {
        return ExecuteScalar(sql, commandType, null);
    }

    public Object ExecuteScalar(string sql, CommandType commandType, MySqlParameter[] parameters)
    {
        object result = null;
        try
        {
            using (MySqlConnection connection = new MySqlConnection(conStr))
            {
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.CommandType = commandType;//设置command的CommandType为指定的CommandType
                    //如果同时传入了参数，则添加这些参数
                    if (parameters != null)
                    {
                        foreach (MySqlParameter parameter in parameters)
                        {
                            command.Parameters.Add(parameter);
                        }
                    }
                    connection.Open();//打开数据库连接
                    result = command.ExecuteScalar();
                }
            }
            return result;//返回查询结果的第一行第一列，忽略其它行和列
        }
        catch (Exception)
        {
            return null;
        }
    }

    public MySqlDataReader ExecuteReader(string sql)
    {
        return ExecuteReader(sql, CommandType.Text, null);
    }

    public MySqlDataReader ExecuteReader(string sql, CommandType commandType)
    {
        return ExecuteReader(sql, commandType, null);
    }

    public MySqlDataReader ExecuteReader(string sql, CommandType commandType, MySqlParameter[] parameters)
    {
        try
        {
            MySqlConnection connection = new MySqlConnection(conStr);
            MySqlCommand command = new MySqlCommand(sql, connection);
            //如果同时传入了参数，则添加这些参数
            if (parameters != null)
            {
                foreach (MySqlParameter parameter in parameters)
                {
                    command.Parameters.Add(parameter);
                }
            }
            connection.Open();
            //CommandBehavior.CloseConnection参数指示关闭Reader对象时关闭与其关联的Connection对象
            return command.ExecuteReader(CommandBehavior.CloseConnection);
        }
        catch (Exception)
        {
            return null;
        }
    }

    public DataTable ExecuteDataTable(string sql)
    {
        return ExecuteDataTable(sql, CommandType.Text, null);
    }

    public DataTable ExecuteDataTable(string sql, CommandType commandType)
    {
        return ExecuteDataTable(sql, commandType, null);
    }

    public DataTable ExecuteDataTable(string sql, CommandType commandType, MySqlParameter[] parameters)
    {
        DataTable data = new DataTable();//实例化DataTable，用于装载查询结果集
        try
        {
            using (MySqlConnection connection = new MySqlConnection(conStr))
            {
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.CommandType = commandType;//设置command的CommandType为指定的CommandType
                    //如果同时传入了参数，则添加这些参数
                    if (parameters != null)
                    {
                        foreach (MySqlParameter parameter in parameters)
                        {
                            command.Parameters.Add(parameter);
                        }
                    }
                    //通过包含查询SQL的SqlCommand实例来实例化SqlDataAdapter
                    MySqlDataAdapter adapter = new MySqlDataAdapter(command);

                    adapter.Fill(data);//填充DataTable
                }
            }
            return data;
        }
        catch (Exception)
        {
            return new DataTable();
        }
    }
    #endregion
}