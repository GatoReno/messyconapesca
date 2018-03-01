using SqlieTest_x.UWP;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(Windows_SQLite))]
namespace SqlieTest_x.UWP
{
    public class Windows_SQLite : ISQLite
    {
        public SQLiteConnection GetConnection()
        {

            var sqliteFilename = "Member.sqlite";
            string path = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, sqliteFilename);
            var conn = new SQLite.SQLiteConnection(path);
            return conn;
        }
    }
}