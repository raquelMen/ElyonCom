using ArezCrmBLL;
using ElyonComunicator.BLL;
using System;
using System.Data;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;

namespace ElyonComunicator
{
    public partial class Login : System.Web.UI.Page
    {

        UsuariosBLL usu = new UsuariosBLL();
        MensajesBLL mj = new MensajesBLL();
        ConfiguracionesBLL conf = new ConfiguracionesBLL();
        AsistenciaBLL abll = new AsistenciaBLL();



        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session.Abandon();
                FormsAuthentication.SignOut();
                lblCantInt.InnerText = "0";
            }
        }
        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            lblUsuario.Text = usu.get_usuID(Convert.ToString(txtUsuario.Value));
            if (usu.get_ExisteUsuarioActual(txtUsuario.Value))
            {
                int Contador = Convert.ToInt32(lblCantInt.InnerText); 

                if (usu.get_ExisteUsuario(Convert.ToString(txtUsuario.Value), ProcesosBLL.encriptar(txtPass.Value)))
                {
                    if (usu.get_Resetea(txtUsuario.Value))
                    {
                        Autenticar();
                        Response.Redirect("/UI/Seguridad/CambioPass/CambioPass2.aspx");
                    }
                    else
                    {
                        //if (usu.get_PassVencimiento(txtUsuario.Value))
                        //{
                        //    Autenticar();
                        //    mj.alert(MensajesBLL.passVencido);
                        //    Response.Redirect("/UI/Seguridad/CambioPass/CambioPass2.aspx");
                        //}
                        //else
                        //{ 
                        //pnlLogin.Visible = false;
                        //pnlAsistencia.Visible = true;
                        //  if (usu.get_Area(Convert.ToInt32(lblUsuario.Text)) == conf.get_conAreaID())
                        // {

                        Autenticar();   
                                Response.Redirect("/UI/SMS/SMSInbox.aspx");
                            //}
                            //else if (abll.get_ExisteEntrada(Convert.ToInt32(lblUsuario.Text)))
                            //{
                            //    //btnEntrada.Enabled = false;
                            //    //btnSalida.Enabled = true;
                            //    //lblEntrada.Text = abll.get_FechaHoraEntrada(Convert.ToInt32(lblUsuario.Text));
                            //    //btnIngreso.Visible = true;
                            //}
                            //else
                            //{
                            //    btnEntrada.Enabled = true;
                            //    btnSalida.Enabled = false;
                            //}
                            //if (abll.get_ExisteSalida(Convert.ToInt32(lblUsuario.Text)))
                            //{
                            //    //btnEntrada.Enabled = false;
                            //    //btnSalida.Enabled = false;
                            //    //lblEntrada.Text = abll.get_FechaHoraEntrada(Convert.ToInt32(lblUsuario.Text));
                            //    //lblSalida.Text = abll.get_FechaHoraSalida(Convert.ToInt32(lblUsuario.Text));
                            //    //btnIngreso.Visible = true;
                            //}
                        //}
                    }
                }
                else
                {
                    mj.alert(MensajesBLL.PassActual);
                    txtPass.Value = "";
                    txtPass.Focus();
                    Contador = Contador + 1;
                    lblCantInt.InnerText = Contador.ToString();
                } 
            }
            else
            {
                mj.alert(MensajesBLL.usuNoExiste);
                txtUsuario.Value = "";
                txtUsuario.Focus();
            }
           
        }
        private void Autenticar()
        {
            string usuID = usu.get_usuID(txtUsuario.Value);
            FormsAuthenticationTicket tkt;
            String cook;
            HttpCookie ck;
            tkt = new FormsAuthenticationTicket(1, usuID, DateTime.Now, DateTime.Now.AddHours(2), false, usuID);
            cook = FormsAuthentication.Encrypt(tkt);
            ck = new HttpCookie(FormsAuthentication.FormsCookieName, cook);
            Page.Response.Cookies.Add(ck);
        }
    }
}