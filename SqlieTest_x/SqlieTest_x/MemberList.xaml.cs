using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SqlieTest_x
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MemberList : ContentPage
	{
        public MemberDatabase memberDatabase;
        public MemberList ()
		{            
            InitializeComponent();
            memberDatabase = new MemberDatabase();
            var members = memberDatabase.GetMembers();
           
            listMembers.ItemsSource = members;
        }
        public async void onDelete(object obj, ItemTappedEventArgs args) {
            var member = args.Item as Member;
            await DisplayAlert("You selected", "" + member.ID + "", "OK");
            return;
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
                await DisplayAlert("",ex.ToString(),"");
                return;
            }
        }

        private async Task userOptionsHandlerAsync(int id)
        {
            
            var actionSheet = await DisplayActionSheet("Opciones de usuario "+id, "Cancel", null, "Borrar");

            
            switch (actionSheet)
            {
                case "Borrar":

                    try
                    {
                        memberDatabase.DeleteMember(id);
                        
                    }
                    catch (Exception ex)
                    {

                        res_lbl.Text = ex.ToString();
                    }
                      await Navigation.PushModalAsync(new FormsPage());
                   
                    break;
            }
           
        }

      

    }
}