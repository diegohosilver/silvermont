using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

/// <summary>
/// Descripción breve de Conexion
/// </summary>
public static class Conexion
{
    //public static SqlConnection SqlCon = new SqlConnection("Data Source=localhost\\sqlexpress;Initial Catalog=SilverMont_Series;Persist Security Info=True;User ID=test;Password=test01");
    public static SqlConnection SqlCon = new SqlConnection("Data Source=DIEGO-PC;Initial Catalog=SilverMont_Series;Persist Security Info=True;User ID=test;Password=test01");

    public static string stSqlCon = "Data Source=DIEGO-PC;Initial Catalog=SilverMont_Series;Persist Security Info=True;User ID=test;Password=test01";
}