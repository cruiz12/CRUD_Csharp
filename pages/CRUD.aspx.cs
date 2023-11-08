using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI;
using System.Data;
using System.Web.UI.WebControls;

namespace CRUD.pages
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        readonly SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conexion"].ConnectionString);
        public static string sID = "-1";
        public static string sOpc = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            /*Obtener el ID
            en el siguiente if, lo que se va  a hacer es que los datos no se recarguen
            una vez a pasado el primer intento, es decir cuando se cargue la pagina se van a cargar unos datos
            pero estos no se vana  volver a cargar al momento de ejecutar otro evento
            asi evitamos algunos errores*/
            if(!Page.IsPostBack)
            {
                if (Request.QueryString["id"] != null)
                {
                    sID = Request.QueryString["id"].ToString();
                    //si el ID no es null, se cargan los datos
                    CargarDatos();
                    tbdate.TextMode = TextBoxMode.DateTime;
                }
                if(Request.QueryString["op"]!= null)
                {
                    sOpc = Request.QueryString["op"].ToString();
                    

              // se crea un switc para verificar cada una de las opciuones de nuestro crud, ver a que opcion s eeta ingresando
                    switch(sOpc) 
                    {
                        case "C":
                            this.lbltitulo.Text = "ingresar nuevo usuario";
                            this.BtnCreate.Visible= true;
                            break;

                        case "R":
                            this.lbltitulo.Text = "Consulta de usuario";
                            break;

                        case "U":
                            this.lbltitulo.Text = "Modificar usuario";
                            this.BtnUpdate.Visible = true;
                            break;

                        case "D":
                            this.lbltitulo.Text = "Eliminar usuario";
                            this.BtnDelete.Visible = true;
                            break;
                    }
                }
            }
            
        }
        // se crea la clase para cargar los datos
        void CargarDatos()
        {
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter("sp_read", con);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            da.SelectCommand.Parameters.Add("@Id", SqlDbType.Int).Value = sID;
            DataSet ds = new DataSet();
            ds.Clear();
            da.Fill(ds);
            DataTable dt = ds.Tables[0];
            DataRow row = dt.Rows[0];
            tbnombre.Text = row[1].ToString();
            tbedad.Text = row[2].ToString();
            tbemail.Text = row[3].ToString();
            DateTime d = (DateTime)row[4];
            tbdate.Text = d.ToString("dd/mm/yyyy");
            con.Close();
        }

        //pasaremos a programar las opciones del crud
        protected void BtnCreate_Click(object sender, EventArgs e)
        {
           SqlCommand cmd = new SqlCommand("sp_create", con);
            con.Open();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@Nombre", SqlDbType.VarChar).Value = tbnombre.Text;
            cmd.Parameters.Add("@Edad", SqlDbType.Int).Value = tbedad.Text;
            cmd.Parameters.Add("@Correo", SqlDbType.VarChar).Value = tbemail.Text;
            cmd.Parameters.Add("@Fecha", SqlDbType.Date).Value = tbdate.Text;
            cmd.ExecuteNonQuery();
            con.Close();
            Response.Redirect("Index.aspx");
        }

        protected void BtnUpdate_Click(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand("sp_update", con);
            con.Open();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = sID;
            cmd.Parameters.Add("@Nombre", SqlDbType.VarChar).Value = tbnombre.Text;
            cmd.Parameters.Add("@Edad", SqlDbType.Int).Value = tbedad.Text;
            cmd.Parameters.Add("@Correo", SqlDbType.VarChar).Value = tbemail.Text;
            cmd.Parameters.Add("@Fecha", SqlDbType.Date).Value = tbdate.Text;
            cmd.ExecuteNonQuery();
            con.Close();
            Response.Redirect("Index.aspx");
        }

        protected void BtnDelete_Click(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand("sp_delete", con);
            con.Open();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = sID;
            cmd.ExecuteNonQuery();
            con.Close();
            Response.Redirect("Index.aspx");
        }

        protected void BtnVolver_Click(object sender, EventArgs e)
        {
            Response.Redirect("Index.aspx");
        }
    }
}