using System.Text;
using System.Diagnostics;
namespace DateApp.Extensions
{
    public class BackupDB
    {
        private string host = "localhost";
        private string port = "5433";
        private string database = "dateapp";
        private string user = "postgres";
        private string password = "root";
        public Task PostgreSqlDump(IConfiguration config,string OutFileName)
        {
            return Task.Run(() =>
            {
            DirectoryInfo basePath = new DirectoryInfo(Path.Combine(Environment.CurrentDirectory, "Backup",OutFileName+".sql"));

            string dumpCommand = "\"" + config["PgDump"] + "\"" + " -Fc" + " -h " + host + " -p " + port + " -d " + database + " -U " + user + "";

            string passFileContent = "" + host + ":" + port + ":" + database + ":" + user + ":" + password + "";

            string passFilePath = Path.Combine(
                "PostgreSQL",
                Guid.NewGuid().ToString() + ".conf");

            try
            {
                string batchContent = "";
                batchContent += "@" + "set PGPASSFILE=" + passFilePath + "\n";
                batchContent += "@" + dumpCommand + "  > " + "\"" + basePath.FullName + "\"" + "\n";

                File.WriteAllText(
                    Path.Combine("PostgreSQL", "postgresql-backup.bat"),
                    batchContent,
                    Encoding.ASCII);

                File.WriteAllText(
                    passFilePath,
                    passFileContent,
                    Encoding.ASCII);
                if (File.Exists(basePath.FullName))
                    File.Delete(basePath.FullName);
                var process = new Process();
                var startInfo = new ProcessStartInfo();
                startInfo.FileName = Path.Combine("PostgreSQL", "postgresql-backup.bat");

                startInfo.CreateNoWindow = true;
                startInfo.UseShellExecute = false;
                process.StartInfo = startInfo;
                process.Start();
                process.WaitForExit();
                process.Close();


            }
            finally
            {

                if (File.Exists(passFilePath))
                    File.Delete(passFilePath);
            }
            }
            );

        }

        public Task PostgreSqlRestore(IConfiguration config, string FileName)
        {
            return Task.Run(() =>
            {
                DirectoryInfo basePath = new DirectoryInfo(Path.Combine(Environment.CurrentDirectory, "Backup", FileName + ".sql"));

                string passFileContent = "" + host + ":" + port + ":" + database + ":" + user + ":" + password + "";

                string passFilePath = Path.Combine(
                    "PostgreSQL",
                    Guid.NewGuid().ToString() + ".conf");

                File.WriteAllText(
                   passFilePath,
                   passFileContent,
                   Encoding.ASCII);

                var process = new Process();
                var startInfo = new ProcessStartInfo();
                startInfo.FileName = Path.Combine("PostgreSQL", "postgresql-restore.bat");

               
                // use pg_restore, specifying the host, port, user, database to restore, and the output path.
                // the host, port, user, and database must be an exact match with what's inside your pgpass.conf (Windows)
                startInfo.Arguments = $@"{passFilePath} {config["PgRestore"]} {host} {port} {user} {database} ""{basePath.FullName}""";
                startInfo.CreateNoWindow = true;
                startInfo.UseShellExecute = false;
                process.StartInfo = startInfo;
                process.Start();
                process.WaitForExit();
                process.Close();
            }
            );

        }
    }
}
