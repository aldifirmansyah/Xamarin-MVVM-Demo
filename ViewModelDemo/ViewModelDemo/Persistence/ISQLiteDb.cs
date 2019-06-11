using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace ViewModelDemo.Persistence
{
    public interface ISQLiteDb
    {
        SQLiteAsyncConnection GetConnection();
    }
}
