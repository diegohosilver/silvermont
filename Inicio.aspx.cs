using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class Inicio : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            mostrarListado();
            cargarNoticia();
            mostrarArchivo();
        }    
    }

    private void mostrarArchivo()
    {
        DataSet ds = obtenerListadoNoticias("arc");
        dlArchivos.DataSource = ds.Tables[0];
        dlArchivos.DataBind();
    }

    //ListView de series principales
    private void mostrarListado()
    {
        DataSet ds = obtenerListadoSeries("top");
        lvSeriesPrincipales.DataSource = ds.Tables[0];
        lvSeriesPrincipales.DataBind();
    }

    //Ultima noticia

    private void cargarNoticia()
    {
        DataSet ds = obtenerListadoNoticias("ult");
        if (ds.Tables[0].Rows.Count != 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            hdID.Value = General.validarNulo(dr["id"].ToString());
            lblTitulo.Text = General.validarNulo(dr["Titulo"].ToString());
            lblCopete.Text = General.validarNulo(dr["Copete"].ToString());
            lblContenido.Text = General.validarNulo(dr["Contenido"].ToString());
            imgNoticia.ImageUrl = General.validarNulo(dr["Imagen"].ToString());
            lblTiempoPublicado.Text = General.calcularTiempoPublicado((Convert.ToDouble(dr["FechaPublicacion"])));
            lblCantidadComentarios.Text = cantComentarios(hdID.Value);
            
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

    public DataSet obtenerListadoNoticias(string op, string id = null)
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
            General.GenerateLOG("Log.log", "listadoNoticias - Excepcion" + ex.Message);
        }
        return ds;
    }

    public DataSet obtenerListadoSeries(string op, string id = null)
    {
        DataSet ds = new DataSet();

        try
        {
            SqlDataAdapter da = new SqlDataAdapter();
            SqlCommand cmd = new SqlCommand("SP_Series_Consultar", Conexion.SqlCon);
            cmd.Connection = Conexion.SqlCon;
            cmd.CommandType = CommandType.StoredProcedure;
            if (op == "top" | op == "side")
            {
                cmd.Parameters.AddWithValue("@op", op);
            }
            else if (op == "get")
            {
                cmd.Parameters.AddWithValue("@op", op);
                cmd.Parameters.AddWithValue("@id", int.Parse(id));
            }
            da.SelectCommand = cmd;
            Conexion.SqlCon.Open();
            da.Fill(ds);
            Conexion.SqlCon.Close();
        }
        catch (Exception ex)
        {
            Conexion.SqlCon.Close();
            General.GenerateLOG("Log.log", "Inicio - Excepcion" + ex.Message);
        }
        return ds;
    }

    //Al clickear una serie

    protected void lvSeriesPrincipales_ItemCommand(object source, ListViewCommandEventArgs e)
    {
        if (e.CommandName == "redirectSerie")
        {
            Response.Redirect("Series.aspx?id=" + e.CommandArgument.ToString());
        }
    }
    protected void btnVerMas_Click(object sender, EventArgs e)
    {
        Response.Redirect("Noticias.aspx?id=" + hdID.Value);
    }
    protected void dlArchivos_ItemCommand(object source, DataListCommandEventArgs e)
    {
        if(e.CommandName == "CargarNoticia"){
            Response.Redirect("Noticias.aspx?id=" + e.CommandArgument.ToString());
        } 
    }
    protected void dlArchivos_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        Label newLabel =  (Label)e.Item.FindControl("lblTiempoPublicacion");
        string segundos = newLabel.Text;
        newLabel.Text = General.calcularTiempoPublicado(Convert.ToDouble(segundos));
        HiddenField newhf = (HiddenField)e.Item.FindControl("hfIDAux");
        string id = newhf.Value;
        newLabel = (Label)e.Item.FindControl("lblCantidadComentarios");
        newLabel.Text = cantComentarios(id);
    }
}