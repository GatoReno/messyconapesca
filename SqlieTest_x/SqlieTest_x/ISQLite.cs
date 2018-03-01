using System;
using System.Collections.Generic;
using System.Text;

namespace SqlieTest_x
{
    public interface ISQLite
    {
        SQLite.SQLiteConnection GetConnection();

    }
}
