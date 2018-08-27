using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class Noticias : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        { 
            hfIDNoticia.Value = Request.QueryString["id"];
            if (hfIDNoticia.Value != null)
            {
                cargarNoticia(hfIDNoticia.Value);
                cargarComentarios(hfIDNoticia.Value);
                if (Session["user"] != null)
                {
                    pnComentar.Visible = true;
                }
            }  
        }
    }

    protected void dlComentarios_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        Label newLabel = (Label)e.Item.FindControl("lblTiempoPublicado");
        string segundos = newLabel.Text;
        newLabel.Text = General.calcularTiempoPublicado(Convert.ToDouble(segundos));
    }

    private void cargarComentarios(string id)
    {
        DataSet ds = obtenerListadoNoticias("getCom", id);
        dlComentarios.DataSource = ds;
        dlComentarios.DataBind();
    }

    private void cargarNoticia(string id)
    {
        DataSet ds = obtenerListadoNoticias("get", id);
        if (ds.Tables[0].Rows.Count != 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            lblTituloNoticia.Text = General.validarNulo(dr["Titulo"].ToString());
            lblCopete.Text = General.validarNulo(dr["Copete"].ToString());
            lblContenido.Text = General.validarNulo(dr["Contenido"].ToString());
            imgNoticia.ImageUrl = General.validarNulo(dr["Imagen"].ToString());
            lblTiempoPublicacion.Text = General.calcularTiempoPublicado((Convert.ToDouble(dr["FechaPublicacion"])));
            lblCantidadComentarios.Text = cantComentarios(id);
        }
    }

    private string cantComentarios(string id)
    {
        string cantComentarios = "";
        DataSet ds = obtenerListadoNoticias("cantCom", id);
        if (ds.Tables[0].Rows.Count != 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            cantComentarios = General.validarNulo(dr["cantidadComentarios"].ToString());
        }
        return cantComentarios;
    }

    public void guardarComentario(string Accion, string id, string idNoticia, string Comentario, ref string stResult)
    {
        stResult = "OK";
        DataSet ds = new DataSet();
        SqlDataAdapter da = new SqlDataAdapter();
        try
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SP_Comentarios_ABM";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@idNoticiaSerie", idNoticia);
            cmd.Parameters.AddWithValue("@Comentario", Comentario);
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
            General.GenerateLOG("Log.log", "guardarComentario - Excepcion" + ex.Message);
        }

    }

    protected void guardarComentario_Click(object sender, EventArgs e)
    {
        string stResult = "";
        guardarComentario("insertarNoticia", "", hfIDNoticia.Value, txtContenidoComentario.Text, ref stResult);
        if (stResult != "OK")
        {
            lblError.Visible = true;
            lblError.Text = stResult;
        }
        else
        {
            lblError.Text = stResult;
            cargarComentarios(hfIDNoticia.Value);
            limpiarCampos();
        }

        lblError.Visible = true;
    }

    private void limpiarCampos()
    {
        txtContenidoComentario.Text = "";
        hfIDNoticia.Value = null;
    }

    public DataSet obtenerListadoNoticias(string op, string id)
    {
        DataSet ds = new DataSet();

        try
        {
            SqlDataAdapter da = new SqlDataAdapter();
            SqlCommand cmd = new SqlCommand("SP_Noticias_Consultar", Conexion.SqlCon);
            cmd.Connection = Conexion.SqlCon;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@op", op);
            cmd.Parameters.AddWithValue("@id", id);
            da.SelectCommand = cmd;
            Conexion.SqlCon.Open();
            da.Fill(ds);
            Conexion.SqlCon.Close();
        }
        catch (Exception ex)
        {
            Conexion.SqlCon.Close();
            General.GenerateLOG("Log.log", "consultaNoticias - Excepcion" + ex.Message);
        }
        return ds;
    }
}