using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;

public partial class SilverMont : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string pagina = Request.RawUrl;
        //Si no estamos en la pagina de bienvenida, mostrar barra de navegacion
        if (pagina != "/" && Session["user"] == null)
        {
            BarraNav.Visible = true;
        }
        //Caso contrario...
        else if (Session["user"] != null && pagina != "/")
        {
            //Si el usuario es administrador
            if (Session["userType"].ToString() == "1")
            {
                zzz.InnerHtml = "Hola " + Session["user"].ToString() + "!";
                lblUsuario.Text = Session["user"].ToString();
                BarraNavSesionAdmin.Visible = true;
            }
            //Usuario Comun
            else
            {
                xxx.InnerHtml = "Hola " + Session["user"].ToString() + "!";
                lblUsuario.Text = Session["user"].ToString();
                BarraNavSesion.Visible = true;   
            }        
        }
    }

    protected void btnLogin_Click(object sender, EventArgs e)
     {
        string stResult = "";

        checkLogin(txtUser.Text, txtPassword.Text, ref stResult);

        if (stResult != "OK")
        {
            lblError.Visible = true;
            lblError.Text = stResult;
        }
        else
        {
            lblError.Text = stResult;
            Session["user"] = txtUser.Text;
            Session["userType"] = validarPermisos(txtUser.Text);
            Response.Redirect("Inicio.aspx");
        }

        lblError.Visible = true;
    }

    private string validarPermisos(string User)
    {
        string stResult = "";
        DataSet ds = new DataSet();
        SqlDataAdapter da = new SqlDataAdapter();
        try
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SP_Usuarios_Iniciar";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@User", User);
            cmd.Parameters.AddWithValue("@op", "permisos");
            cmd.Connection = Conexion.SqlCon;
            Conexion.SqlCon.Open();
            da.SelectCommand = cmd;
            da.Fill(ds);
            stResult = ds.Tables[0].Rows[0]["Tipo"].ToString();
            Conexion.SqlCon.Close();
        }
        catch (SqlException ex)
        {
            Conexion.SqlCon.Close();
            General.GenerateLOG("Log.log", "validarPermisos - Excepcion" + ex.Message);
        }
        return stResult;
    }

    protected void btnConfirmarRegistro_Click(object sender, EventArgs e)
    {
        string stResult = "";

        guardarUsuario("insertar", txtRegistroUser.Text, txtRegistroPassword.Text, ref stResult, txtEmail.Text, txtNombreApellido.Text);

        if (stResult != "OK!")
        {
            lblErrorRegistro.Visible = true;
            lblErrorRegistro.Text = stResult;
        }
        else
        {
            lblErrorRegistro.Text = stResult;
            Session["user"] = txtRegistroUser.Text;
            Session["userType"] = validarPermisos(txtRegistroUser.Text);
            Response.Redirect("Inicio.aspx");
        }

        lblErrorRegistro.Visible = true;
    }

    protected void btnNewPassword_Click(object sender, EventArgs e)
    {
        string stResult = "";

        if (txtNewPassword.Text == txtConfirmNewPassword.Text)
        {
            guardarUsuario("modificar", Session["user"].ToString(), txtNewPassword.Text, ref stResult);

            if (stResult != "OK!")
            {
                lblErrorContraseña.Visible = true;
                lblErrorContraseña.Text = stResult;
            }
            else
            {
                lblErrorContraseña.Text = stResult;
            }

            lblErrorContraseña.Visible = true;
        }
        else
        {
            stResult = "Las contraseñas no coinciden";
            lblErrorContraseña.Text = stResult;
        }  
    }

    protected void btnCerrarSesion_Click(object sender, EventArgs e)
    {
        Session["user"] = null;
        Session["userType"] = null;
        Response.Redirect("Inicio.aspx");
    }

    protected void btnRegistro_Click(object sender, EventArgs e)
    {
        pnSesion.Visible = false;
        pnRegistro.Visible = true;
    }

    protected void btnSesion_Click(object sender, EventArgs e)
    {
        pnRegistro.Visible = false;
        pnSesion.Visible = true;
    }

    public void checkLogin(string user, string pass, ref string stResult)
    {
        stResult = "OK";
        DataSet ds = new DataSet();
        SqlDataAdapter da = new SqlDataAdapter();
        try
        {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "SP_Usuarios_Iniciar";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@User", user);
                cmd.Parameters.AddWithValue("@Password", pass);
                cmd.Parameters.AddWithValue("@op", "login");
                cmd.Connection = Conexion.SqlCon;
                Conexion.SqlCon.Open();
                da.SelectCommand = cmd;
                da.Fill(ds);
                stResult = ds.Tables[0].Rows[0]["resultado"].ToString();
                Conexion.SqlCon.Close();
        }
        catch (SqlException ex)
        {
            Conexion.SqlCon.Close();
            General.GenerateLOG("Log.log", "Login - Excepcion" + ex.Message);
        }

    }

    public void guardarUsuario(string op, string user, string pass, ref string stResult, string email = null, string nombre = null)
    {
        stResult = "OK";
        DataSet ds = new DataSet();
        SqlDataAdapter da = new SqlDataAdapter();
        try
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SP_Usuarios_ABM";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@User", user);
            cmd.Parameters.AddWithValue("@Password", pass);
            cmd.Parameters.AddWithValue("@Email", email);
            cmd.Parameters.AddWithValue("@NombreApellido", nombre);
            cmd.Parameters.AddWithValue("@op", op);
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
            General.GenerateLOG("Log.log", "guardarUsuario - Excepcion" + ex.Message);
        }

    }

}
