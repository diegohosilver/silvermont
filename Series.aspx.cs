using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
public partial class Series : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        
        if (!IsPostBack)
        {
            //Le indicamos al js en que posicion esta el boton del menu
            if (Session["userType"] != null)
            {
                if (Session["userType"].ToString() == "1")
                {
                    hfElementPosition.Value = "4";
                }
                else
                {
                    hfElementPosition.Value = "2";
                }
                pnComentar.Visible = true;
            }
            hdScroll.Value = "0";
            mostrarListado();
            string id = Request.QueryString["id"];
            if (id != null)
            {
                //Sacamos el panel informativo de la pagina y mostramos el panel de la serie
                PanelContenidoInicial.Visible = false;
                PanelContenidoSerie.Visible = true;
                //Cargamos sus series y temporadas
                cargarSerie(id);
                cargarTemporadas(id);
                cargarComentarios(id);
                //Le indicamos al JS que haga el scroll
                hdScroll.Value = "1";
            }            
        }

    }

    protected void guardarComentario_Click(object sender, EventArgs e)
    {
        string stResult = "";
        guardarComentario("insertarSerie", "", hdIDSerie.Value, txtCajaComentario.Text, ref stResult);
        if (stResult != "OK")
        {
            lblError.Visible = true;
            lblError.Text = stResult;
        }
        else
        {
            lblError.Text = stResult;
            cargarComentarios(hdIDSerie.Value);
            limpiarCampos();
        }

        lblError.Visible = true;
    }

    private void limpiarCampos()
    {
        txtCajaComentario.Text = "";
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

    protected void dlComentarios_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        try
        {
            Label newLabel = (Label)e.Item.FindControl("lblTiempoPublicado");
            string segundos = newLabel.Text;
            newLabel.Text = General.calcularTiempoPublicado(Convert.ToDouble(segundos));
        }
        catch { }    
    }

    private void cargarComentarios(string id)
    {
        DataSet ds = obtenerListadoSeries("getCom", id);
        dlComentarios.DataSource = ds;
        dlComentarios.DataBind();
    }

    //Llenar datalist en sidebar
    private void mostrarListado()
    {
        DataSet ds = obtenerListadoSeries("side");
        dlSeries.DataSource = ds.Tables[0];
        dlSeries.DataBind();
    }

    //Cargar datalist en temporadas
    private void cargarTemporadas(string id)
    {
        DataSet ds = obtenerListadoTemporadas("con", id);
        dlTemporadas.DataSource = ds.Tables[0];
        dlTemporadas.DataBind();
    }

    //Devuelve un dataset con uno o mas elementos dependiendo del parametro de consulta // SERIES
    public DataSet obtenerListadoSeries(string op, string id = null)
    {
        DataSet ds = new DataSet();

        try
        {
                SqlDataAdapter da = new SqlDataAdapter();
                SqlCommand cmd = new SqlCommand("SP_Series_Consultar", Conexion.SqlCon);
                cmd.Connection = Conexion.SqlCon;
                cmd.CommandType = CommandType.StoredProcedure;
                if (op == "side")
                {
                    cmd.Parameters.AddWithValue("@op", op);
                }
                else if (op == "get" || op == "getCom")
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
            General.GenerateLOG("Log.log", "listadoNoticias - ListadoSeries" + ex);
        }
        return ds;
    }

    //Devuelve un dataset con uno o mas elementos dependiendo del parametro de consulta // TEMPORADAS
    public DataSet obtenerListadoTemporadas(string op, string idSerie = null)
    {
        DataSet ds = new DataSet();

        try
        {
            SqlDataAdapter da = new SqlDataAdapter();
            SqlCommand cmd = new SqlCommand("SP_Temporadas_Consultar", Conexion.SqlCon);
            cmd.Connection = Conexion.SqlCon;
            cmd.CommandType = CommandType.StoredProcedure;
            if (op == "con")
            {
                cmd.Parameters.AddWithValue("@op", op);
                cmd.Parameters.AddWithValue("@idSerie", int.Parse(idSerie));
            }
            da.SelectCommand = cmd;
            Conexion.SqlCon.Open();
            da.Fill(ds);
            Conexion.SqlCon.Close();
        }
        catch (Exception ex)
        {
            Conexion.SqlCon.Close();
            General.GenerateLOG("Log.log", "listadoNoticias - ListadoTemporadas" + ex);
        }
        return ds;
    }

    //Devuelve un dataset con uno o mas elementos dependiendo del parametro de consulta // CAPITULOS X TEMPORADA
    public DataSet obtenerListadoCapitulos(string op, string idSerie = null, string idTemporada = null)
    {
        DataSet ds = new DataSet();

        try
        {
            SqlDataAdapter da = new SqlDataAdapter();
            SqlCommand cmd = new SqlCommand("SP_Capitulos_Consultar", Conexion.SqlCon);
            cmd.Connection = Conexion.SqlCon;
            cmd.CommandType = CommandType.StoredProcedure;
            if (op == "con")
            {
                cmd.Parameters.AddWithValue("@op", op);
                cmd.Parameters.AddWithValue("@idSerie", int.Parse(idSerie));
                cmd.Parameters.AddWithValue("@idTemporada", int.Parse(idTemporada));
            }
            da.SelectCommand = cmd;
            Conexion.SqlCon.Open();
            da.Fill(ds);
            Conexion.SqlCon.Close();
        }
        catch (Exception ex)
        {
            Conexion.SqlCon.Close();
            General.GenerateLOG("Log.log", "listadoNoticias - ListadoCapitulos" + ex);
        }
        return ds;

    }

    //Cargar contenido de la serie deseada
    private void cargarSerie(string id)
    {
        DataSet ds = obtenerListadoSeries("get", id);
        if (ds.Tables[0].Rows.Count != 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            lblNombreSerie.Text = General.validarNulo(dr["Nombre"].ToString());
            lblCreador.Text = General.validarNulo(dr["Creador"].ToString());
            lblTemporadas.Text = General.validarNulo(dr["Temporadas"].ToString());
            lblGenero.Text = General.validarNulo(dr["Genero"].ToString());
            lblPais.Text = General.validarNulo(dr["Pais"].ToString());
            lblReseña.Text = General.validarNulo(dr["Reseña"].ToString());
            lblAño.Text = General.Left(dr["Año"].ToString(), 10);
            img.ImageUrl = General.validarNulo(dr["Imagen"].ToString());

            //Hidden para cargar capitulos x temporada
            hdIDSerie.Value = General.validarNulo(dr["id"].ToString());
            hdTemporada.Value = "0";
        }
    }

    //Cuando se hace click sobre una serie de la lista
    protected void dlSeries_ItemCommand(object source, DataListCommandEventArgs e)
    {
        if (e.CommandName == "CargarSerie")
        {
            //Sacamos el panel informativo de la pagina y mostramos el panel de la serie
            PanelContenidoInicial.Visible = false;
            PanelContenidoSerie.Visible = true;
            //Cargamos sus series y temporadas
            cargarSerie(e.CommandArgument.ToString());
            cargarTemporadas(e.CommandArgument.ToString());
            cargarComentarios(e.CommandArgument.ToString());
            //Le indicamos al JS que haga el scroll
            hdScroll.Value = "1";
        }
    }

    //Cuando se hace click sobre una temporada
    protected void dlTemporadas_ItemCommand(object source, DataListCommandEventArgs e)
    {
        if (e.CommandName == "CargarCapitulosTemporada")
        {
        }
    }

    protected void dlCapitulos_ItemCommand(object source, DataListCommandEventArgs e)
    {
        if (e.CommandName == "CargarCapitulo")
        {
        }
    }

    private string calcularTemporada(string valor)
    {
        int num = Convert.ToInt32(valor);
        num = num + 1;     
        valor = Convert.ToString(num);
        hdTemporada.Value = valor;
        return valor;
    }

    protected void dlTemporadas_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        DataSet ds = obtenerListadoCapitulos("con", hdIDSerie.Value, calcularTemporada(hdTemporada.Value));
        DataList dt = (DataList)e.Item.FindControl("dlCapitulos");
        dt.DataSource = ds.Tables[0];
        dt.DataBind();
    }
}
    