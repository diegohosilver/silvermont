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

public partial class ABMSeries : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            hfPanelMostrar.Value = "0";
            General.CargarComboQuery("SP_Series_Listar", Session["user"].ToString(), ddlGenero, "generos", "id", "Descripcion", "",  "150" , "10" , "50");
            General.CargarComboQuery("SP_Series_Listar", Session["user"].ToString(), ddlPais, "paises", "id", "Descripcion", "", "150", "10");
            mostrarListado();
        }
    }

    protected void guardarSerie_Click(object sender, EventArgs e)
    {
        string stResult = "";
        guardarSerie("insertar", "", txtTituloSerie.Text, ddlGenero.SelectedValue, ddlPais.SelectedValue, txtCreador.Text, txtFechaEstreno.Text, txtReseña.Text, fuImagen.PostedFile.FileName, fuMiniatura.PostedFile.FileName, ref stResult);
        if (stResult != "OK")
        {
            hfPanelMostrar.Value = "1";
            lblError.Visible = true;
            lblError.Text = stResult;
        }
        else
        {
            if (txtCantidadTemporadas.Text != "")
            {
                obtenerUltimoID();
                cargarTemporadas();
            }
            subirFoto(fuImagen);
            subirFoto(fuMiniatura);
            hfPanelMostrar.Value = "0";
            lblError.Text = stResult;
            limpiarCampos();
            mostrarListado();
        }

        lblError.Visible = true;
    }

    protected void modificarSerie_Click(object sender, EventArgs e)
    {
        string stResult = "";
        guardarSerie("modificar", hfIDSerie.Value, txtTituloSerie.Text, ddlGenero.SelectedValue, ddlPais.SelectedValue, txtCreador.Text, txtFechaEstreno.Text, txtReseña.Text, fuImagen.PostedFile.FileName, fuImagen.PostedFile.FileName, ref stResult);
        if (stResult != "OK")
        {
            hfPanelMostrar.Value = "1";
            lblError.Visible = true;
            lblError.Text = stResult;
            
        }
        else
        {
            if (txtCantidadTemporadas.Text != "")
            {
                EliminarTemporadas();
                cargarTemporadas();
            }
            subirFoto(fuImagen);
            subirFoto(fuMiniatura);
            hfPanelMostrar.Value = "0";
            lblError.Text = stResult;
            limpiarCampos();
            mostrarListado();
        }
        lblError.Visible = true;
    }

    private void limpiarCampos()
    {
        txtTituloSerie.Text = "";
        txtCreador.Text = "";
        txtFechaEstreno.Text = "";
        txtReseña.Text = "";
        txtCantidadTemporadas.Text = "";
        fuImagen.Attributes.Clear();
        fuMiniatura.Attributes.Clear();
        ddlGenero.SelectedValue = "0";
        ddlPais.SelectedValue = "0";
    }

    public void EliminarTemporadas()
    {
        SqlCommand cmd = new SqlCommand();
        string sql;
        try
        {
            Conexion.SqlCon.Open();
            cmd.Connection = Conexion.SqlCon;
            sql = "delete from tblTemporadasXSerie where idSerie = " + hfIDSerie.Value;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = sql;
            cmd.ExecuteNonQuery();
            Conexion.SqlCon.Close();
        }
        catch (Exception ex)
        {
            Conexion.SqlCon.Close();
            General.GenerateLOG("Log.log", "eliminarTemporadas - Excepcion" + ex.Message);
        }
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
            sql = "select top (1) id from tblSeries order by 1 desc";
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = sql;
            dr = cmd.ExecuteReader(CommandBehavior.SingleRow);
            while (dr.Read())
            {
                hfIDSerie.Value = dr["id"].ToString();
            }
            Conexion.SqlCon.Close();
        }
        catch (Exception ex)
        {
            Conexion.SqlCon.Close();
            General.GenerateLOG("Log.log", "obtenerUltimoID - Excepcion" + ex.Message);
        }
    }
    public void guardarSerie(string Accion, string id, string Nombre, string Genero, string Pais, string Creador, string Año, string Reseña, string Imagen, string Miniatura, ref string stResult)
    {
        stResult = "OK";
        DataSet ds = new DataSet();
        SqlDataAdapter da = new SqlDataAdapter();
        try
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SP_Series_ABM";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@Nombre", Nombre);
            cmd.Parameters.AddWithValue("@Genero", Genero);
            cmd.Parameters.AddWithValue("@Pais", Pais);
            cmd.Parameters.AddWithValue("@Creador", Creador);
            cmd.Parameters.AddWithValue("@Año", Año);
            cmd.Parameters.AddWithValue("@Reseña", Reseña);
            cmd.Parameters.AddWithValue("@Imagen", Imagen);
            cmd.Parameters.AddWithValue("@Miniatura", Miniatura);
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

    private DataSet obtenerListadoSeries(string op, string id = null, string Nombre = null, string Genero = null, string Pais = null, string Creador = null, string Año = null, string Reseña = null, string Imagen = null, string Miniatura = null)
    {
        DataSet ds = new DataSet();
        SqlDataAdapter da = new SqlDataAdapter();
        try
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SP_Series_Consultar";
            cmd.CommandType = CommandType.StoredProcedure;
            if (op == "con")
            {
                cmd.Parameters.AddWithValue("@op", op);
                cmd.Parameters.AddWithValue("@Nombre", Nombre);
                cmd.Parameters.AddWithValue("@Genero", Genero);
                cmd.Parameters.AddWithValue("@Pais", Pais);
                cmd.Parameters.AddWithValue("@Creador", Creador);
                cmd.Parameters.AddWithValue("@Año", Año);
                cmd.Parameters.AddWithValue("@Reseña", Reseña);
                cmd.Parameters.AddWithValue("@Imagen", Imagen);
                cmd.Parameters.AddWithValue("@Miniatura", Miniatura);
                cmd.Parameters.AddWithValue("@Usuario", Session["user"].ToString());
            }
            else if (op == "getOne")
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
            General.GenerateLOG("Log.log", "listadoNoticias - Excepcion" + ex.Message);
        }
        return ds;
    }

    protected void mostrarListado()
    {
        DataSet ds = new DataSet();
        ds = obtenerListadoSeries("con");
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
        ds = obtenerListadoSeries("getOne", id);
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            hfIDSerie.Value = General.validarNulo(dr["id"].ToString());
            txtTituloSerie.Text = General.validarNulo(dr["Nombre"].ToString());
            ddlGenero.SelectedValue = General.validarddlNulo(dr["idGenero"].ToString());
            ddlPais.SelectedValue = General.validarddlNulo(dr["idPais"].ToString());
            txtCreador.Text = General.validarNulo(dr["Creador"].ToString());
            txtFechaEstreno.Text = General.validarNulo(dr["Año"].ToString());
            txtReseña.Text = General.validarNulo(dr["Reseña"].ToString());
            txtCantidadTemporadas.Text = General.validarNulo(dr["Temporadas"].ToString());
        }
        hfPanelMostrar.Value = "1";
        btnGuardar.Visible = false;
        btnModificar.Visible = true;
    }

    private void eliminarSerie(string id)
    {
        string stResult = "";
        guardarSerie("eliminar", id, "", "0", "0", "", "", "", "", "", ref stResult);
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
    private void subirFoto(FileUpload fu)
    {
        if (fu.HasFile)
        {
            try
            {
                string filename = Path.GetFileName(fu.FileName);
                fu.SaveAs(Server.MapPath("~/images/") + filename);
            }
            catch (Exception ex)
            {
                General.GenerateLOG("Log.log", "subirFoto - Excepcion" + ex.Message);
            }
        }
    }

        private void cargarTemporadas()
        {
	    if (txtCantidadTemporadas.Text != "")
        {
            int cantTemporadas = int.Parse(txtCantidadTemporadas.Text);

	    DataSet ds = new DataSet();
	    SqlDataAdapter da = new SqlDataAdapter();
	    SqlConnection conn = new SqlConnection(Conexion.stSqlCon);

	    SqlCommand cmd = new SqlCommand();
	    cmd.Connection = conn;
	    cmd.CommandText = "SP_Temporadas_ABM";
	    cmd.CommandType = CommandType.StoredProcedure;
	    cmd.Parameters.AddWithValue("@idSerie", "");
	    cmd.Parameters.AddWithValue("@idTemporada", "");
        cmd.Parameters.AddWithValue("@Accion", "");
        cmd.Parameters.AddWithValue("@Usuario", "");
	    conn.Open();
	    SqlTransaction tran = conn.BeginTransaction();
	    cmd.Transaction = tran;
	    try {
		    for (int i = 1; i <= cantTemporadas; i++) {
			    cmd.Parameters["@idSerie"].Value = hfIDSerie.Value;
			    cmd.Parameters["@idTemporada"].Value = i;
                cmd.Parameters["@Accion"].Value = "insertar";
                cmd.Parameters["@Usuario"].Value = Session["user"].ToString();
			    cmd.ExecuteNonQuery();
		    }
		    tran.Commit();
	    } catch (SqlException ex) {
		    tran.Rollback();
		    General.GenerateLOG("TemporadasXSerie.txt", ex.Message);
	    }
	    conn.Close();
        }
        }
}