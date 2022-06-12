using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using DateApp.Extensions;

namespace DateApp.Pages
{
    [BindProperties]
    public class AdminPanelModel : PageModel
    {
        //[Display(Name = "Путь к pg_dump.exe")]
        //public string PgDump { get; set; }

        //[Display(Name = "Путь к pg_restore.exe")]
        //public string PgRest { get; set; }

        [Display (Name = "Название файла")]
        public string FileName { get; set; }

        private IConfiguration Config { get; }

        public AdminPanelModel(IConfiguration config)
        {
            Config = config;
        }

        public async Task OnPostSave()
        {
            BackupDB backupDB = new BackupDB();
            await backupDB.PostgreSqlDump(Config,FileName);
        }

        public async Task OnPostRestore()
        {
            BackupDB backupDB = new BackupDB();
            await backupDB.PostgreSqlRestore(Config, FileName);
            //await backupDB.PostgreSqlDump(PgDump, FileName);
        }

        public void OnGet()
        {
        }
    }
}
