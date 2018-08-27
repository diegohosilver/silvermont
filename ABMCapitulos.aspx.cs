using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;

public partial class ABMCapitulos : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            hfPanelMostrar.Value = "0";
            General.CargarComboQuery("SP_Capitulos_Listar", Session["user"].ToString(), ddlSeries, "series", "id", "Nombre", "",  "200" , "10" , "50");
            //mostrarListado();
        }
    }

    protected void guardarCapitulo_Click(object sender, EventArgs e)
    {
        string stResult = "";
        guardarCapitulo("insertar", "", ddlSeries.SelectedValue, ddlTemporadas.SelectedValue, txtNumero.Text, txtNombreCapitulo.Text, ref stResult);
        if (stResult != "OK")
        {
            hfPanelMostrar.Value = "1";
            lblError.Visible = true;
            lblError.Text = stResult;
        }
        else
        {
            hfPanelMostrar.Value = "0";
            lblError.Text = stResult;
            limpiarCampos();
            mostrarListado();
        }

        lblError.Visible = true;
    }

    protected void modificarCapitulo_Click(object sender, EventArgs e)
    {
        string stResult = "";
        guardarCapitulo("modificar", hfIDCapitulo.Value, ddlSeries.SelectedValue, ddlTemporadas.SelectedValue, txtNumero.Text, txtNombreCapitulo.Text, ref stResult);
        if (stResult != "OK")
        {
            hfPanelMostrar.Value = "1";
            lblError.Visible = true;
            lblError.Text = stResult;
        }
        else
        {
            hfPanelMostrar.Value = "0";
            lblError.Text = stResult;
            mostrarListado();
            limpiarCampos();        
        }
        lblError.Visible = true;
    }

    private void limpiarCampos()
    {
        txtNombreCapitulo.Text = "";
        txtNumero.Text = "";
    }
    public void obtenerUltimoID()
    {
        SqlCommand cmd = new SqlCommand();
        SqlDataReader dr;
        string sql;
        try
        {
            Conexion.SqlCon.Open();
            cmd.Connection = Conexion.SqlCon;
            sql = "select COUNT(Numero) + 1 as cant from tblCapitulos where idSerie = " + ddlSeries.SelectedValue + " and Temporada = " + ddlTemporadas.SelectedValue;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = sql;
            dr = cmd.ExecuteReader(CommandBehavior.SingleRow);
            while (dr.Read())
            {
                txtNumero.Text = dr["cant"].ToString();
            }
            Conexion.SqlCon.Close();
        }
        catch (Exception ex)
        {
            Conexion.SqlCon.Close();
            General.GenerateLOG("Log.log", "obtenerUltimoID - Excepcion" + ex.Message);
        }
    }
    public void guardarCapitulo(string Accion, string id, string idSerie, string idTemporada, string idNumero, string Nombre, ref string stResult)
    {
        stResult = "OK";
        DataSet ds = new DataSet();
        SqlDataAdapter da = new SqlDataAdapter();
        try
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SP_Capitulos_ABM";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@idSerie", idSerie);
            cmd.Parameters.AddWithValue("@idTemporada", idTemporada);
            cmd.Parameters.AddWithValue("@idNumero", idNumero);
            cmd.Parameters.AddWithValue("@Nombre", Nombre);
            cmd.Parameters.AddWithValue("@Usuario", Session["user"].ToString());
            cmd.Parameters.AddWithValue("@Accion", Accion);
            cmd.Connection = Conexion.SqlCon;
            Conexion.SqlCon.Open();
            da.SelectCommand = cmd;
            da.Fill(ds);
            stResult = ds.Tables[0].Rows[0]["resultado"].ToString();
            Conexion.SqlCon.Close();
        }
        catch (SqlException ex)
        {
            stResult = "No se pudo grabar los datos";
            Conexion.SqlCon.Close();
            General.GenerateLOG("Log.log", "guardarCapitulo - Excepcion" + ex.Message);
        }

    }

    private DataSet obtenerListadoCapitulos(string op, string id = null, string idSerie = null, string idTemporada = null,string Numero = null, string Nombre = null)
    {
        DataSet ds = new DataSet();
        SqlDataAdapter da = new SqlDataAdapter();
        try
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SP_Capitulos_Consultar";
            cmd.CommandType = CommandType.StoredProcedure;
            if (op == "conABM")
            {
                cmd.Parameters.AddWithValue("@op", op);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@idSerie", idSerie);
                cmd.Parameters.AddWithValue("@idTemporada", idTemporada);
                cmd.Parameters.AddWithValue("@Numero", Numero);
                cmd.Parameters.AddWithValue("@Nombre", Nombre);
                cmd.Parameters.AddWithValue("@Usuario", Session["user"].ToString());
            }
            else if (op == "get")
            {
                cmd.Parameters.AddWithValue("@op", op);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@Usuario", Session["user"].ToString());
            }
            da.SelectCommand = cmd;
            cmd.Connection = Conexion.SqlCon;
            da.Fill(ds);
            Conexion.SqlCon.Close();
            Cache["data"] = ds;
        }
        catch (SqlException ex)
        {
            Conexion.SqlCon.Close();
            General.GenerateLOG("Log.log", "listadoCapitulos - Excepcion" + ex.Message);
        }
        return ds;
    }

    protected void mostrarListado()
    {
        DataSet ds = new DataSet();
        ds = obtenerListadoCapitulos("conABM","",ddlSeries.SelectedValue);
        try
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                gvNoticias.DataSource = ds.Tables[0];
                gvNoticias.DataBind();
            }
        }
        catch
        {
        }
    }

    protected void gvNoticias_PageIndexChanging(Object sender, GridViewPageEventArgs e)
    {
        gvNoticias.PageIndex = e.NewPageIndex;
        gvNoticias.DataSource = (DataSet)(Cache["data"]);
        gvNoticias.DataBind();
    }

    private void obtenerDatosSerie(string id)
    {
        DataSet ds = new DataSet();
        ds = obtenerListadoCapitulos("get", id);
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            hfIDCapitulo.Value = General.validarNulo(dr["id"].ToString());
            ddlSeries.SelectedValue = General.validarNulo(dr["idSerie"].ToString());
            General.CargarCombo(ddlTemporadas, "select * from tblTemporadasXSerie where idSerie = " + ddlSeries.SelectedValue, "Temporada", "Temporada", "", "150", "10", "50");
            ddlTemporadas.SelectedValue = General.validarddlNulo(dr["Temporada"].ToString());
            txtNumero.Text = General.validarddlNulo(dr["Numero"].ToString());
            hfIDCapitulo.Value = General.validarNulo(dr["id"].ToString());
            txtNombreCapitulo.Text = General.validarNulo(dr["Nombre"].ToString());
        }
        hfPanelMostrar.Value = "1";
        btnGuardar.Visible = false;
        btnModificar.Visible = true;
    }

    private void eliminarSerie(string id)
    {
        string stResult = "";
        guardarCapitulo("eliminar", id, "0", "0", "", "", ref stResult);
        mostrarListado();

        if (stResult != "OK")
        {
            General.GenerateLOG("Log.log", "eliminarCapitulo - Error" + stResult);
        }
        else
        {
            lblError.Text = stResult;
        }

        lblError.Visible = true;
    }

    public bool onConfirm()
    {
        string confirmValue = Request.Form["delete_value"];
        if (confirmValue == "Yes")
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    protected void gvNoticias_OnRowCommand(Object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "MODIFICAR")
        {
            obtenerDatosSerie(e.CommandArgument.ToString());
        }
        if (e.CommandName == "ELIMINAR")
        {
            if (onConfirm())
            {
               eliminarSerie(e.CommandArgument.ToString());
            }

        }
    }

    protected void ddlSeries_SelectedIndexChanged(object sender, EventArgs e)
    {
        General.CargarCombo(ddlTemporadas, "select * from tblTemporadasXSerie where idSerie = " + ddlSeries.SelectedValue, "Temporada", "Temporada", "", "150", "10", "50");
        mostrarListado();
        hfPanelMostrar.Value = "1";
    }

    protected void ddlTemporadas_SelectedIndexChanged(object sender, EventArgs e)
    {
        obtenerUltimoID();
    }

      
}