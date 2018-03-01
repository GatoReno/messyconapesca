using Newtonsoft.Json;
using Plugin.Connectivity;
using Plugin.LocalNotifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;


using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SqlieTest_x
{

   
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class InfoUser : ContentPage
	{

        /*
        Please do stuff to dis´play user info
        */

            
        public void Notifyme() {
            AudioService auServ = new AudioService();
            auServ.Play();
            CrossLocalNotifications.Current.Show("Error!!!", "Activa tu GPS!!!",101,DateTime.Now);
        }


       

        public MemberDatabase memberDatabase;
        public InfoUser ()
		{
			InitializeComponent ();
            memberDatabase = new MemberDatabase();
            var members = memberDatabase.GetMembers();
            listMembers.ItemsSource = members;
        }


        protected async override void OnAppearing()
        {
            base.OnAppearing();

            res_x.Text = "";

            var mx = memberDatabase.GetMembers();
            var mx_first = mx.FirstOrDefault();
            var tok_ty = mx_first.Token_Type;
            var acc_tok = mx_first.Access_Token;
            var id = mx_first.ID;

            //try http GET
            var uri = "http://aige.sytes.net:81/ApiRestSAM/api/conapesca/Geolocalizacion?idusuario=" + id;

            var request = new HttpRequestMessage();
            request.RequestUri = new Uri(uri);
            
            request.Method = HttpMethod.Get;
           
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tok_ty, acc_tok);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = await client.SendAsync(request);

          
            switch (response.StatusCode)
            {
                //200
                case (System.Net.HttpStatusCode.OK):

                    HttpContent content = response.Content;
                    string xjson = await content.ReadAsStringAsync();

                    try
                    {
                        //List<Table_Loc> loc_list = JsonConvert.DeserializeObject<List<Table_Loc>>(xjson);
                       
                        Root myobject = JsonConvert.DeserializeObject<Root>(xjson);

                        res_x.Text = "Exito en solicitar datos";
                        ListLoc.ItemsSource = myobject.tablas.Table1;
                        // ListLoc
                    }
                    catch (Exception ex)
                    {
                       await DisplayAlert("",""+ ex.ToString(),"ok");
                       return ;
                    } 

                    break;
                //500
                case (System.Net.HttpStatusCode.InternalServerError):
                    await DisplayAlert("No existe registro de usuario", "Nuestros servidores estan en mantenimiento", "Continuar");
                    
                    memberDatabase.DeleteMember(id);
                    await Navigation.PushModalAsync(new AuthPage());
                    break;
                //404
                case (System.Net.HttpStatusCode.Forbidden):
                    try
                    {
                        await DisplayAlert("Su sesión ha caducado", "Reingrese sus datos", "ok");
                        memberDatabase.DeleteMember(id);
                        await Navigation.PushModalAsync(new AuthPage());
                    }
                    catch (Exception ex)
                    {

                        res_x.Text = ex.ToString();

                    }
                    break;
                
            }


            //res_lbl

            //await DisplayAlert("","fack user "+id+" we R good 2 Go! yei :D / :3 ","ok");
        }


        public async void OnSelected(object obj, ItemTappedEventArgs args)
        {
            var member = args.Item as Member;
            try
            {
                // await DisplayActionSheet("You selected", member.Name + " " + member.Age+" "+member.ID,);
                await userOptionsHandlerAsync(member.ID);
            }
            catch (Exception ex)
            {
                await DisplayAlert("", ex.ToString(), "");
                return;
            }
        }

        private async Task userOptionsHandlerAsync(int id)
        {

            var actionSheet = await DisplayActionSheet("Opciones de usuario " + id, "Cancel", null, "Borrar");


            switch (actionSheet)
            {
                case "Borrar":

                    try
                    {
                        memberDatabase.DeleteMember(id);


                    }
                    catch (Exception ex)
                    {

                        res_x.Text = ex.ToString();
                    }
                    await Navigation.PushModalAsync(new SplashPage());

                    break;
            }

        }
    }
}