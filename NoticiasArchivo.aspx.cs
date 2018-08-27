using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
public partial class NoticiasArchivo : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        
        if (!IsPostBack)
        {
            hdScroll.Value = "0";
            mostrarListado();           
        }

    }

    //Llenar datalist en sidebar
    private void mostrarListado()
    {
        DataSet ds = obtenerListadoNoticias("side");
        dlNoticias.DataSource = ds.Tables[0];
        dlNoticias.DataBind();
    }

    //Devuelve un dataset con uno o mas elementos dependiendo del parametro de consulta // SERIES
    public DataSet obtenerListadoNoticias(string op, string id = null)
    {
        DataSet ds = new DataSet();

        try
        {
                SqlDataAdapter da = new SqlDataAdapter();
                SqlCommand cmd = new SqlCommand("SP_Noticias_Consultar", Conexion.SqlCon);
                cmd.Connection = Conexion.SqlCon;
                cmd.CommandType = CommandType.StoredProcedure;
                if (op == "side")
                {
                    cmd.Parameters.AddWithValue("@op", op);  
                }
                else if (op == "getShort" || op == "cantCom")
                {
                    cmd.Parameters.AddWithValue("@op", op);
                    cmd.Parameters.AddWithValue("@id", id);
                }
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


    //Cargar contenido de la noticia deseada
    private void cargarNoticia(string id)
    {
        DataSet ds = obtenerListadoNoticias("getShort", id);
        if (ds.Tables[0].Rows.Count != 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            hdID.Value = General.validarNulo(dr["id"].ToString());
            lblTitulo.Text = General.validarNulo(dr["Titulo"].ToString());
            lblCopete.Text = General.validarNulo(dr["Copete"].ToString());
            lblContenido.Text = General.validarNulo(dr["Contenido"].ToString());
            imgNoticia.ImageUrl = General.validarNulo(dr["Imagen"].ToString());
            lblTiempoPublicado.Text = General.calcularTiempoPublicado((Convert.ToDouble(dr["FechaPublicacion"])));
        }
    }

    //Cuando se hace click sobre una serie de la lista
    protected void dlNoticias_ItemCommand(object source, DataListCommandEventArgs e)
    {
        if (e.CommandName == "CargarNoticia")
        {
            //Sacamos el panel informativo de la pagina y mostramos el panel de la noticia
            PanelContenidoInicial.Visible = false;
            PanelContenidoNoticia.Visible = true;
            //Cargamos la noticia
            cargarNoticia(e.CommandArgument.ToString());
            //Le indicamos al JS que haga el scroll
            hdScroll.Value = "1";
        }
    }
    protected void btnVerMas_Click(object sender, EventArgs e)
    {
        Response.Redirect("Noticias.aspx?id=" + hdID.Value);
    }
    protected void dlNoticias_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        Label newLabel = (Label)e.Item.FindControl("lblTiempoPublicacion");
        string segundos = newLabel.Text;
        newLabel.Text = General.calcularTiempoPublicado(Convert.ToDouble(segundos));
        HiddenField newhf = (HiddenField)e.Item.FindControl("hfIDAux");
        string id = newhf.Value;
        newLabel = (Label)e.Item.FindControl("lblCantidadComentarios");
        newLabel.Text = cantComentarios(id);
    }
}
    