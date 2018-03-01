using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using SqlieTest_x;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SQLite;
using System.Net.Http.Headers;
using Plugin.Connectivity;
using Plugin.Connectivity.Abstractions;

namespace SqlieTest_x
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AuthPage : ContentPage
    {
        //public MemberDatabase memberDatabase;
        public Member member;
        private MemberDatabase memberdatabase;
        public SQLiteConnection conn;


        public AuthPage()
        {
            InitializeComponent();          
        }
             

        private async Task Access_Login() {

            if (string.IsNullOrEmpty(userTxt.Text))
            {
                await DisplayAlert("Error", "Por favor ingrese su nombre de usuario", "ok");
                userTxt.Focus();
                return;

            }
            if (string.IsNullOrEmpty(passTxt.Text))
            {
                await DisplayAlert("Error", "Por favor ingrese una contraseña", "ok");
                passTxt.Focus();
                return;
            }
            else {

                string userName = userTxt.Text;
                string pass = passTxt.Text;

                try
                {
                    waitActIndicator.IsRunning = true;
                    btnAuth.IsEnabled = false;
                    HttpClient client = new HttpClient();


                    var values = new Dictionary<string, string>
                         {
                            { "UserName",  "conapesca_sam" },
                            { "Password", "2{'9At)nuH$2&SzK" },
                            { "grant_type" , "password" }
                         };

                    var content = new FormUrlEncodedContent(values);

                    var response = await client.PostAsync("http://aige.sytes.net:81/ApiRestSAM/TOKEN",
                        content);
                    //handler / respuesta de status
                    string status;

                    switch (response.StatusCode)
                    {


                        // 200
                        case (System.Net.HttpStatusCode.OK):


                            var responseString = await response.Content.ReadAsStringAsync();

                            // var xjson = JsonConvert.DeserializeObject(responseString);
                            var xjson = JsonConvert.DeserializeObject<TokenRequest>(responseString);

                            status = xjson.access_token + " " + xjson.expires_in;
                            var xjson_token = xjson.access_token;
                            var xjson_type = xjson.token_type;
                            var xjson_exp = xjson.expires_in;

                            res_Label.Text = "Good Token Request";

                            string tok_ty = xjson_type;
                            string acc_tok = xjson_token;
                         

                            try
                            {

                                Login_Api(userName, pass, tok_ty, acc_tok);



                            }
                            catch (Exception ex)
                            {

                                await DisplayAlert("Alerta", ex.ToString(), "ok");

                            }
 
                            break;

                        // 400
                        case (System.Net.HttpStatusCode.BadRequest):
                            status = "Usuario o contraseña invalidos";
                            res_Label.Text = status;
                            break;

                        //500
                        case (System.Net.HttpStatusCode.InternalServerError):
                            status = "Nuestros servidores estan en mantenimiento";
                            res_Label.Text = status;
                            break;

                        // 502
                        case (System.Net.HttpStatusCode.BadGateway):
                            status = "Usuario o contraseña invalidos";
                            res_Label.Text = status;
                            break;

                        // 403 required

                        case (System.Net.HttpStatusCode.Forbidden):
                            status = "Acceso rechazado";
                            res_Label.Text = status;
                            await DisplayAlert("Error de acceso", "Es probable que tu sesión haya caducado. Ingresa tus datos de acceso nuevamente", "Continuar");
                            break;

                        // 404
                        case (System.Net.HttpStatusCode.NotFound):
                            status = "Error - 404 Servidor no encontrado";
                            res_Label.Text = status;
                            await DisplayAlert("Error de acceso", "Es probable que tu sesión haya caducado. Ingresa tus datos de acceso nuevamente", "Continuar");
                            break;


                    }

                }
                catch (Exception ex)
                {

                    throw;
                }

            }

        }

        public async void Login_Api(string userName, string pass, string tok_ty, string acc_tok)
        {
            
            // await Navigation.PushAsync(new PageNav());


            HttpClient client = new HttpClient();

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tok_ty, acc_tok);
            client.DefaultRequestHeaders.Add("api-version", "1.0");            
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


            var values = new Dictionary<string, string>
                         {
                            { "usuario",  userName },
                            { "contraseña", pass }
                            
                         };


            var content = new FormUrlEncodedContent(values);

            var response = await client.PostAsync("http://aige.sytes.net:81/ApiRestSAM/api/conapesca/acceso",
                content);

           
            switch (response.StatusCode)
            {
                case (System.Net.HttpStatusCode.OK):
                    res_Label_api.Text = "good";

                    var responseString = await response.Content.ReadAsStringAsync();



                    // var xjson = JsonConvert.DeserializeObject(responseString);
                    var xjson = JsonConvert.DeserializeObject<RootObject>(responseString);
                    
                    //int xid = Convert.ToInt32(xjson.DatosEnvio.IdUsuario);


                    try
                    {
                           member = new Member();
                           memberdatabase = new MemberDatabase();
                           member.UserName = userName;
                           member.Pass = pass;
                           member.Token_Type = tok_ty;
                           member.Access_Token = acc_tok;
                           member.ID = xjson.DatosEnvio.IdUsuario;

                           memberdatabase.AddMember(member);

                        await Navigation.PushAsync(new PageNav());
                    }
                    catch (Exception ex)
                    {
                        res_Label.Text = "Hubo un problema como los datos de acceso";
                        res_Label_api.Text = ex.ToString();
                    }



                    //Application.Current.MainPage = new NavigationPage(new PageNav());
                   
                    break;

                case (System.Net.HttpStatusCode.BadRequest):
                    res_Label_api.Text = "no good";
                    break;

                case (System.Net.HttpStatusCode.Forbidden):
                    res_Label_api.Text = "no good, Forbidden";
                    break;
                //500
                case (System.Net.HttpStatusCode.InternalServerError):
                    string status = "Nuestros servidores estan en mantenimiento";
                    res_Label_api.Text = status;
                    break;
            }


            waitActIndicator.IsRunning = false;
            btnAuth.IsEnabled = true;
            return;
        }
    }
	
}