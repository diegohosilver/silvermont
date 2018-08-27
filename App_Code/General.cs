using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

/// <summary>
/// Descripción breve de General
/// </summary>
public class General
{
	public General()
	{
	}

    //Devuelve valor vacio si el string tiene NULL
    public static string validarNulo(string Valor)
    {
        if (String.IsNullOrEmpty(Valor))
        {
            return "";
        }
        else
        {
            return Valor;
        }
    }

    public static string validarddlNulo(string Valor)
    {
        if (String.IsNullOrEmpty(Valor))
        {
            return "0";
        }
        else
        {
            return Valor;
        }
    }

    //Tomar unicamente la fecha del DATE de la base de datos
    public static string Left(string valor, int numCaracteres)
    {
        if (validarNulo(valor) == "") return "";
        numCaracteres = Math.Abs(numCaracteres);

        return (valor.Length <= numCaracteres
            ? valor
            : valor.Substring(0, numCaracteres)
            );
    }

    public static string calcularTiempoPublicado(Double segundos)
    {
        TimeSpan t = TimeSpan.FromSeconds(segundos);
        if (t.Minutes < 60 && t.Hours == 0 && t.Days == 0)
        {
            return "Hace " + t.Minutes.ToString() + " minutos";
        }
        else if (t.Hours < 24 && t.Days == 0)
        {
            return "Hace " + t.Hours.ToString() + " hora/s";
        }
        else
        {
            return "Hace " + t.Days.ToString() + " dia/s";
        }
    }


    public static void GenerateLOG(string stFile, string stError)
    {
        string gPATH_APP = "c:\\Vista\\Logs\\";
        StreamWriter swFile = default(StreamWriter);

        try
        {
            stFile = gPATH_APP + stFile;
            swFile = new StreamWriter(stFile, true);
            swFile.Write(Convert.ToString(DateTime.Now) + "-" + stError + "\r\n");
            swFile.Close();
        }
        catch (Exception ex)
        {
            string asd;
        }
    }

    public static DataSet ObtenerTablaTipo(string SP, string Usuario, string tipo)
    {
        DataSet ds = new DataSet();

        try
        {
            SqlConnection cnn = new SqlConnection(Conexion.stSqlCon);
            cnn.Open();
            SqlDataAdapter da = new SqlDataAdapter();
            SqlCommand cmd = new SqlCommand(SP + "'" + Usuario + "', '" + tipo + "'", cnn);
            da.SelectCommand = cmd;    
            da.Fill(ds);
            da.Dispose();
            cmd.Dispose();
            cnn.Close();
            cnn.Dispose();
            return ds;
        }
        catch (Exception ex)
        {
            Conexion.SqlCon.Close();
            GenerateLOG("Log.log", "ObtenerTablaTipo - Excepcion" + ex.Message);
        }
        return ds;
    }

    public static void CargarComboQuery(string Procedure, string Usuario, DropDownList Combo, string Tipo, string Campo1, string Campo2, string Valor, string Width = "120", string mLeft = "0", string mRight = "0")
    {
        int i = 0;

        try
        {
            DataSet ds = new DataSet();

            ds = ObtenerTablaTipo(Procedure, Usuario, Tipo);
            Combo.Items.Clear();
            Combo.Items.Add(new ListItem("", "0"));
            for (i = 0; i <= ds.Tables[0].Rows.Count - 1; i++)
            {
                ListItem wcItem = new ListItem();
                if (Convert.ToString(ds.Tables[0].Rows[i][Campo1]) == Valor)
                {
                    wcItem.Selected = true;
                }
                wcItem.Text = "" + ds.Tables[0].Rows[i][Campo2];
                wcItem.Value = "" + ds.Tables[0].Rows[i][Campo1];
                Combo.Items.Add(wcItem);
            }
            Combo.Attributes["style"] = "width:" + Width + "px ; max-width: " + Width + "px ; height:50px ; max-height:50px; margin-left:" + mLeft + "px; margin-right: " + mRight + "px;";
        }
        catch (Exception ex)
        {
            GenerateLOG("Log.log", "CargarComboQuery - Excepcion" + ex.Message);
        }
    }

    public static void CargarCombo(DropDownList Combo, string sQuery, string Campo1, string Campo2, string Valor, string Width = "120", string mLeft = "0", string mRight = "0")
    {
        string stStatus = "";
        int i = 0;

        try
        {
            DataSet ds = new DataSet();

            ds = GetSQLDataSet(sQuery, ref stStatus);
            Combo.Items.Clear();
            Combo.Items.Add(new ListItem("", "0"));

            for (i = 0; i <= ds.Tables[0].Rows.Count - 1; i++)
            {
                ListItem wcItem = new ListItem();
                if (Convert.ToString(ds.Tables[0].Rows[i][Campo1]) == Valor)
                {
                    wcItem.Selected = true;
                }
                wcItem.Text = "" + ds.Tables[0].Rows[i][Campo2];
                wcItem.Value = "" + ds.Tables[0].Rows[i][Campo1];
                Combo.Items.Add(wcItem);
            }
            Combo.Attributes["style"] = "width:" + Width + "px ; max-width: " + Width + "px ; height:50px ; max-height:50px; margin-left:" + mLeft + "px; margin-right: " + mRight + "px;";

        }
        catch (Exception ex)
        {
        }
    }

    public static DataSet GetSQLDataSet(string stQuery1, ref string stStatus)
    {
        DataSet ds = new DataSet();

        try
        {
            SqlConnection cnn = new SqlConnection(Conexion.stSqlCon);
            cnn.Open();
            SqlDataAdapter da = new SqlDataAdapter();
            SqlCommand cmd = new SqlCommand(stQuery1, cnn);
            da.SelectCommand = cmd;
            da.Fill(ds);

            stStatus = "OK";

            da.Dispose();
            cmd.Dispose();
            cnn.Close();
            cnn.Dispose();

            return ds;

        }
        catch
        {
        }
        return ds;
    }
}