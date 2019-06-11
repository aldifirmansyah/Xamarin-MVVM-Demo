using SQLite;
using System.IO;
using ViewModelDemo.Droid.persistence;
using ViewModelDemo.Persistence;
using Xamarin.Forms;

[assembly: Dependency(typeof(SQLiteDb))]

namespace ViewModelDemo.Droid.persistence
{
    public class SQLiteDb : ISQLiteDb
    {
        public SQLiteAsyncConnection GetConnection()
        {
            var documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
			var path = Path.Combine(documentsPath, "MySQLite.db3");

			return new SQLiteAsyncConnection(path);
        }
        
    }
}