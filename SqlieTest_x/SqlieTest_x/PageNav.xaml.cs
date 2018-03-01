using Plugin.Geolocator;
using SQLite;
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
	public partial class PageNav : ContentPage
	{
        public MemberDatabase memberDatabase;
        public Member member;
        public SQLiteConnection conn;
        private MemberDatabase memberdatabase;

        public PageNav ()
		{
			InitializeComponent ();
           
        }

        private void ToolbarItem_InfoUser(object sender, EventArgs e)
        {
            //navigation a una page llamada QrPage()
            Navigation.PushAsync(new InfoUser());
        }
        private void ToolbarItem_Instrucciones(object sender, EventArgs e)
        {
            //navigation a una page llamada QrPage()
            Navigation.PushAsync(new Instrucciones());
        }

        private async void BtnGetLoc_Clickied(object sender, EventArgs e)
        {
            btnLoc.IsEnabled = false;
            await RetriveLocation();

            btnLoc.IsEnabled = true;
        }

        public async Task RetriveLocation()
        {

            if (!CrossGeolocator.Current.IsGeolocationEnabled)
            {
                await DisplayAlert("Advertencia !", "Active su GPS para utilizar esta aplicación correctamente", "Continuar");

            }
            else {
                testx.Text = "";

                memberDatabase = new MemberDatabase();
                var members = memberDatabase.GetMembers();
                var member = members.FirstOrDefault();
                var userName = member.UserName;
                var id = member.ID;
                var tok_ty = member.Token_Type;
                var acc_tok = member.Access_Token;

                string str_id = id.ToString();

                waitActIndicator.IsRunning = true;
                var locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = 100;

                var position = await locator.GetPositionAsync(TimeSpan.FromSeconds(10000), null, true);

                txtLat.Text = "Latitude: " + position.Latitude.ToString();
                txtLong.Text = "Longitude: " + position.Longitude.ToString();

                HttpClient client = new HttpClient();

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tok_ty, acc_tok);
                client.DefaultRequestHeaders.Add("api-version", "1.0");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


                var values = new Dictionary<string, string>
                         {
                            { "Longitud" , position.Longitude.ToString()},
                            { "Latitud" , position.Latitude.ToString()},
                            { "IdUsuario", str_id },
                            { "opcion", "Insertar"}

                         };


                var content = new FormUrlEncodedContent(values);

                var response = await client.PostAsync("http://aige.sytes.net:81/ApiRestSAM/api/conapesca/Geolocalizacion",
                    content);


                switch (response.StatusCode)
                {
                    case (System.Net.HttpStatusCode.OK):
                        testx.Text = "success";

                        break;

                    case (System.Net.HttpStatusCode.BadRequest):
                        testx.Text = "no good";
                        break;

                    case (System.Net.HttpStatusCode.Forbidden):
                        await DisplayAlert("Su sesión ha caducado", "Reingrese sus datos", "ok");
                        memberDatabase.DeleteMember(id);

                        Application.Current.MainPage = new NavigationPage(new AuthPage());
                        break;

                }

            }



            waitActIndicator.IsRunning = false;
        }
    }
}