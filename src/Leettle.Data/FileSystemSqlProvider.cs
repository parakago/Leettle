using System;
using System.IO;
using System.Collections.Generic;
using System.Xml.XPath;

namespace Leettle.Data
{
    public class FileSystemSqlProvider : IPreparedSqlProvider
    {
        private readonly Dictionary<string, SqlFile> sqlFiles = new Dictionary<string, SqlFile>();

        private readonly string sqlFileFolderPath;
        
        public FileSystemSqlProvider(string sqlFileFolderPath, bool reloadable = true)
        {
            Assert.NotNull(sqlFileFolderPath, "sqlFileFolderPath must not be null.");

            this.sqlFileFolderPath = sqlFileFolderPath;

            foreach (string sqlFilePath in Directory.GetFiles(sqlFileFolderPath))
            {
                if (sqlFilePath.EndsWith(".xml", StringComparison.OrdinalIgnoreCase))
                {
                    var sqlFile = SqlFile.Create(sqlFilePath, reloadable);
                    sqlFiles.Add(sqlFile.GroupId, sqlFile);
                }
            }
        }

        public string GetSql(string sqlId)
        {
            string[] s = sqlId.Split('.');
            if (s.Length < 2)
            {
                throw new ArgumentException("invalid sqlId: " + sqlId);
            }

            string groupId = s[0].ToLower();

            if (!sqlFiles.TryGetValue(groupId, out SqlFile sqlFile))
            {
                throw new Exception("can't find sqlFile: " + Path.Combine(sqlFileFolderPath, groupId + ".xml"));
            }

            string itemId = string.Join(".", s, 1, s.Length - 1);
            return sqlFile.FindSql(itemId);
        }
    }

    class SqlFile
    {
        public string GroupId { get; }

        private readonly string sqlFilePath;
        private readonly bool reloadable;
        private DateTime lastWriteTime;
        private DateTime lastCheckTime;
        private readonly Dictionary<string, string> sqlItems = new Dictionary<string, string>();

        private SqlFile(string sqlFilePath, bool reloadable)
        {
            this.sqlFilePath = sqlFilePath;
            this.reloadable = reloadable;

            GroupId = Path.GetFileNameWithoutExtension(sqlFilePath).ToLower();
            lastWriteTime = File.GetLastWriteTime(sqlFilePath);
        }

        public string FindSql(string sqlItemId)
        {
            CheckSqlFileChanged();

            if (!sqlItems.TryGetValue(sqlItemId, out string sql))
            {
                throw new Exception(string.Format("sql '{0}' not found in {1}.xml", sqlItemId, GroupId));
            }
            return sql;
        }

        private void CheckSqlFileChanged()
        {
            if (reloadable)
            {
                if ((DateTime.Now - lastCheckTime).TotalSeconds > 5)
                {
                    lock (sqlItems)
                    {
                        DateTime currentLastWriteTime = File.GetLastWriteTime(sqlFilePath);
                        if (lastWriteTime != currentLastWriteTime)
                        {
                            lastWriteTime = currentLastWriteTime;
                            LoadSqlItems();
                        }
                    }
                    lastCheckTime = DateTime.Now;
                }
            }
        }

        private void LoadSqlItems()
        {
            if (sqlItems.Count > 0)
            {
                sqlItems.Clear();
            }
            
            XPathNavigator navi = new XPathDocument(sqlFilePath).CreateNavigator();
            for (var it = navi.Select("/sqls/sql"); it.MoveNext(); )
            {
                string sqlItemId = it.Current.GetAttribute("id", string.Empty);
                string sql = it.Current.Value.Trim();
                sqlItems.Add(sqlItemId, sql);
            }

            lastCheckTime = DateTime.Now;
        }

        public static SqlFile Create(string sqlFilePath, bool reloadable)
        {
            var instance = new SqlFile(sqlFilePath, reloadable);
            instance.LoadSqlItems();
            return instance;
        }
    }
}
