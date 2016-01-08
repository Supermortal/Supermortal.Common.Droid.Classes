using System;
using System.IO;
using SQLite.Net;
using SQLite.Net.Async;
using Supermortal.Common.PCL.Abstract;
using SQLite.Net.Platform.XamarinAndroid;

namespace Supermortal.Common.Droid.Concrete
{
    public class DroidSQLite : ISQLiteGeneric
    {
        public DroidSQLite()
        {
        }

        public SQLiteConnection GetConnection(string fileName)
        {
            string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            var path = Path.Combine(documentsPath, fileName);

            var conn = new SQLiteConnection(new SQLitePlatformAndroid(), path);

            return conn;
        }

        public SQLiteAsyncConnection GetAsyncConnection(string fileName)
        {
            return null;
        }

        public void DeleteDatabase(string fileName)
        {
            string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            var path = Path.Combine(documentsPath, fileName);

            if (File.Exists(path))
                File.Delete(path);
        }
    }
}

