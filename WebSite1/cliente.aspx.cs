using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class About : Page
{
    string connectionString = @"Data Source = DESKTOP-C06UQ41; Initial Catalog = VentasDB; Integrated Security = True";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            getAll();
        }
    }

    void getAll()
    {
        DataTable dtbl = new DataTable();

        using (SqlConnection sqlCon = new SqlConnection(connectionString))
        {
            sqlCon.Open();
            SqlDataAdapter sqlDa = new SqlDataAdapter("SELECT * from cliente", sqlCon);
            sqlDa.Fill(dtbl);

        }
        if (dtbl.Rows.Count > 0)
        {
            gvPhoneBook.DataSource = dtbl;
            gvPhoneBook.DataBind();
        }
        else
        {
            dtbl.Rows.Add(dtbl.NewRow());
            gvPhoneBook.DataSource = dtbl;
            gvPhoneBook.DataBind();
            gvPhoneBook.Rows[0].Cells.Clear();
            gvPhoneBook.Rows[0].Cells.Add(new TableCell());
            gvPhoneBook.Rows[0].Cells[0].ColumnSpan = dtbl.Columns.Count;
            gvPhoneBook.Rows[0].Cells[0].Text = "ningun registro encontrado";
            gvPhoneBook.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
        }

    }

    protected void gvPhoneBook_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.Equals("AddNew"))
            {
                using (SqlConnection sqlCon = new SqlConnection(connectionString))
                {
                    sqlCon.Open();
                    string query = "INSERT INTO cliente (nit,nombre_uno,nombre_dos,apellido_uno,apellido_dos,fecha_nac) " +
                                    "VALUES (@nit,@nombre_uno,@nombre_dos,@apellido_uno,@apellido_dos,@fecha_nac)";

                    SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                    sqlCmd.Parameters.AddWithValue("@nit", (gvPhoneBook.FooterRow.FindControl("txtNitFooter") as TextBox).Text.Trim());
                    sqlCmd.Parameters.AddWithValue("@nombre_uno", (gvPhoneBook.FooterRow.FindControl("txtNombreUnoFooter") as TextBox).Text.Trim());
                    sqlCmd.Parameters.AddWithValue("@nombre_dos", (gvPhoneBook.FooterRow.FindControl("txtNombreDosFooter") as TextBox).Text.Trim());
                    sqlCmd.Parameters.AddWithValue("@apellido_uno", (gvPhoneBook.FooterRow.FindControl("txtApellidoUnoFooter") as TextBox).Text.Trim());
                    sqlCmd.Parameters.AddWithValue("@apellido_dos", (gvPhoneBook.FooterRow.FindControl("txtApellidoDosFooter") as TextBox).Text.Trim());
                    sqlCmd.Parameters.AddWithValue("@fecha_nac", (gvPhoneBook.FooterRow.FindControl("txtFechaNacFooter") as TextBox).Text.Trim());
                    sqlCmd.ExecuteNonQuery();
                    getAll();
                    lblSuccessMessage.Text = "Nuevo registro agregado";
                    lblErrorMessage.Text = "";
                }
            }
        }
        catch (Exception ex)
        {
            lblSuccessMessage.Text = "";
            lblErrorMessage.Text = ex.Message;
        }
    }

    protected void gvPhoneBook_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gvPhoneBook.EditIndex = e.NewEditIndex;
        getAll();
    }

    protected void gvPhoneBook_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvPhoneBook.EditIndex = -1;
        getAll();
    }

    protected void gvPhoneBook_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                string query = "UPDATE cliente SET nit=@nit, nombre_uno=@nombre_uno, nombre_dos=@nombre_dos," +
                    "apellido_uno=@apellido_uno, apellido_dos=@apellido_dos, fecha_nac=@fecha_nac WHERE id = @id";
                SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                sqlCmd.Parameters.AddWithValue("@nit", (gvPhoneBook.Rows[e.RowIndex].FindControl("txtNit") as TextBox).Text.Trim());
                sqlCmd.Parameters.AddWithValue("@nombre_uno", (gvPhoneBook.Rows[e.RowIndex].FindControl("txtNombreUno") as TextBox).Text.Trim());
                sqlCmd.Parameters.AddWithValue("@nombre_dos", (gvPhoneBook.Rows[e.RowIndex].FindControl("txtNombreDos") as TextBox).Text.Trim());
                sqlCmd.Parameters.AddWithValue("@apellido_uno", (gvPhoneBook.Rows[e.RowIndex].FindControl("txtApellidoUno") as TextBox).Text.Trim());
                sqlCmd.Parameters.AddWithValue("@apellido_dos", (gvPhoneBook.Rows[e.RowIndex].FindControl("txtApellidoDos") as TextBox).Text.Trim());
                sqlCmd.Parameters.AddWithValue("@fecha_nac", (gvPhoneBook.Rows[e.RowIndex].FindControl("txtFechaNac") as TextBox).Text.Trim());
                sqlCmd.Parameters.AddWithValue("@id", Convert.ToInt32(gvPhoneBook.DataKeys[e.RowIndex].Value.ToString()));
                sqlCmd.ExecuteNonQuery();
                gvPhoneBook.EditIndex = -1;
                getAll();
                lblSuccessMessage.Text = "Resgistro ah sido actualizado";
                lblErrorMessage.Text = "";
            }
        }
        catch (Exception ex)
        {
            lblSuccessMessage.Text = "";
            lblErrorMessage.Text = ex.Message;
        }
    }

    protected void gvPhoneBook_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                string query = "DELETE FROM cliente WHERE id = @id";
                SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                sqlCmd.Parameters.AddWithValue("@id", Convert.ToInt32(gvPhoneBook.DataKeys[e.RowIndex].Value.ToString()));
                sqlCmd.ExecuteNonQuery();
                getAll();
                lblSuccessMessage.Text = "registro ah sido eliminado";
                lblErrorMessage.Text = "";
            }
        }
        catch (Exception ex)
        {
            lblSuccessMessage.Text = "";
            lblErrorMessage.Text = ex.Message;
        }
    }
}