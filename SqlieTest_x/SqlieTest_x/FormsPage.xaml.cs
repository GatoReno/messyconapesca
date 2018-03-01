using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlieTest_x;
using SQLite;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SqlieTest_x
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class FormsPage : ContentPage
	{
        public MemberDatabase memberDatabase;
        public Member member;
        public SQLiteConnection conn;

        public FormsPage ()
		{
			InitializeComponent ();
            memberDatabase = new MemberDatabase();
            var members = memberDatabase.GetMembers();
            
            /* if (members)
             {
                 DisplayAlert("no","no members", "i accept");
             }*/

            int RowCount = 0;
            int membcount = members.Count();
            RowCount = Convert.ToInt32(membcount);
            if (RowCount > 0)
            {
                DisplayAlert("", "nohayusers", "oki");
            }
     
                xlist.ItemsSource = members;
     
        }

        public void GetMemebers() {
            memberDatabase = new MemberDatabase();
            var members = memberDatabase.GetMembers();

            xlist.ItemsSource = members;
        }

        public void InsertMember(object o, EventArgs e)
        {
            try
            {
                member = new Member();
                memberDatabase = new MemberDatabase();
                member.UserName = memberName.Text;
                member.Pass = memberAge.Text;
                
                memberDatabase.AddMember(member);
                GetMemebers();
            }
            catch (Exception ex)
            {

                DisplayAlert("",ex.ToString(),"");
               
            }
        }

        public async void ShowMembers(object o, EventArgs e)
        {

            try
            {
                await Navigation.PushModalAsync(new MemberList());
            }
            catch (Exception ex)
            {

                await DisplayAlert("", ex.ToString(), "");
            
            }
            
        }
    }
}