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

public partial class ABMNoticias : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            hfPanelMostrar.Value = "0";
            mostrarListado();
        }
    }

    protected void guardarNoticia_Click(object sender, EventArgs e)
    {
        string stResult = "";
        guardarNoticia("insertar", "", txtTituloNoticia.Text, txtCopete.Text, fuImagen.PostedFile.FileName, txtContenidoNoticia.Text, ref stResult);
        if (stResult != "OK")
        {
            hfPanelMostrar.Value = "1";
            lblError.Visible = true;
            lblError.Text = stResult;
        }
        else
        {
            subirFoto();
            hfPanelMostrar.Value = "0";
            lblError.Text = stResult;
            limpiarCampos();
            mostrarListado();
        }

        lblError.Visible = true;
    }

    protected void modificarNoticia_Click(object sender, EventArgs e)
    {
        string stResult = "";
        guardarNoticia("modificar", hfIDNoticia.Value, txtTituloNoticia.Text, txtCopete.Text, fuImagen.PostedFile.FileName, txtContenidoNoticia.Text, ref stResult);
        if(stResult != "OK"){
            hfPanelMostrar.Value = "1";
            lblError.Visible = true;
            lblError.Text = stResult;
        }
        else
        {
            subirFoto();
            hfPanelMostrar.Value = "0";
            lblError.Text = stResult;
            limpiarCampos();
            mostrarListado();
        }
        lblError.Visible = true;
    }

    private void limpiarCampos()
    {
        txtTituloNoticia.Text = "";
        txtCopete.Text = "";
        fuImagen.Attributes.Clear();
        txtContenidoNoticia.Text = "";
    }

    public void guardarNoticia(string Accion, string id, string Titulo, string Copete, string Imagen, string Contenido, ref string stResult)
    {
        stResult = "OK";
        DataSet ds = new DataSet();
        SqlDataAdapter da = new SqlDataAdapter();
        try
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SP_Noticias_ABM";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@Titulo", Titulo);
            cmd.Parameters.AddWithValue("@Copete", Copete);
            cmd.Parameters.AddWithValue("@Imagen", Imagen);
            cmd.Parameters.AddWithValue("@Contenido", Contenido);
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
            General.GenerateLOG("Log.log", "guardarNoticia - Excepcion" + ex.Message);
        }

    }

    private DataSet obtenerListadoNoticias(string op, string id = null, string titulo = null, string copete = null, string imagen = null, string contenido = null)
    {
        DataSet ds = new DataSet();
        SqlDataAdapter da = new SqlDataAdapter();
        try
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SP_Noticias_Consultar";
            cmd.CommandType = CommandType.StoredProcedure;
            if (op == "con")
            {
                cmd.Parameters.AddWithValue("@op", op);
                cmd.Parameters.AddWithValue("@titulo", titulo);
                cmd.Parameters.AddWithValue("@copete", copete);
                cmd.Parameters.AddWithValue("@imagen", imagen);
                cmd.Parameters.AddWithValue("@contenido", contenido);
            }
            else if (op == "get")
            {
                cmd.Parameters.AddWithValue("@op", op);
                cmd.Parameters.AddWithValue("@id", id);
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
            General.GenerateLOG("Log.log", "listadoNoticias - Excepcion" + ex.Message);
        }
        return ds;
    }

    protected void mostrarListado()
    {
        DataSet ds = new DataSet();
        ds = obtenerListadoNoticias("con");
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

    private void obtenerDatosNoticia(string id)
    {
        DataSet ds = new DataSet();     
        ds = obtenerListadoNoticias("get", id);
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            hfIDNoticia.Value = General.validarNulo(dr["id"].ToString());
            txtTituloNoticia.Text = General.validarNulo(dr["Titulo"].ToString());
            txtContenidoNoticia.Text = General.validarNulo(dr["Contenido"].ToString());
            txtCopete.Text = General.validarNulo(dr["Copete"].ToString());
        }
        hfPanelMostrar.Value = "1";
        btnGuardar.Visible = false;
        btnModificar.Visible = true;
    }

    private void eliminarNoticia(string id)
    {
        string stResult = "";
        guardarNoticia("eliminar", id, "", "", "", "", ref stResult);
        mostrarListado();

        if (stResult != "OK")
        {
            General.GenerateLOG("Log.log", "listadoNoticias - Error" + stResult);
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
            obtenerDatosNoticia(e.CommandArgument.ToString());
        }
        if (e.CommandName == "ELIMINAR")
        {
            if (onConfirm()){
            eliminarNoticia(e.CommandArgument.ToString());
            }
            
        }
    }
    private void subirFoto()
    {
        if (fuImagen.HasFile)
        {
            try
            {
                string filename = Path.GetFileName(fuImagen.FileName);
                fuImagen.SaveAs(Server.MapPath("~/images/") + filename);
            }
            catch (Exception ex)
            {
                General.GenerateLOG("Log.log", "subirFoto - Excepcion" + ex.Message);
            }
        }
    }
}