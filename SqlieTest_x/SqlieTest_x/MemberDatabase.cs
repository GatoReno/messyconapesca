using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace SqlieTest_x
{
   public class MemberDatabase
    {
        private SQLiteConnection conn;

        public MemberDatabase() {
            conn = DependencyService.Get<ISQLite>().GetConnection();        
                conn.CreateTable<Member>();    
        }

        public IEnumerable<Member> GetMembers() {
            var members = (from mem in conn.Table<Member>() select mem);
            return members.ToList();
        }

        public string AddMember(Member member) {
            conn.Insert(member);
            return "success baby bluye ;*";
        }

        public void DeleteMember(int ID) {        
                conn.Delete<Member>(ID);      
        }

        


    }
}
