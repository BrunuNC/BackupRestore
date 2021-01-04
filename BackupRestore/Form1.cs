using System;
using System.Windows.Forms;
using System.IO;
using FirebirdSql.Data.FirebirdClient;
using FirebirdSql.Data.Services;

namespace BackupRestore
{
    public partial class Form1 : Form
    {
        private string pathDest = "";
        public Form1()
        {
            InitializeComponent();
        }


        private void btnBackup_Click(object sender, EventArgs e)
        {
            Backup();
        }

        private void btnRestore_Click(object sender, EventArgs e)
        {
            Restore();
        }

        
        private void Backup()
        {
            var DB = txtPath.Text;
            var conexao = new FbConnectionStringBuilder();
            conexao.DataSource = "localhost";
            conexao.Database = DB;
            conexao.UserID = "SYSDBA";
            conexao.Password = "masterkey";

            var backup = new FbBackup();
            backup.ConnectionString = conexao.ToString();
            var fileInfo = new FileInfo(DB);
            pathDest = $"{fileInfo.DirectoryName}\\{DB.Substring(0, DB.Length - 3)}gbk";
            backup.BackupFiles.Add(new FbBackupFile(pathDest));
            backup.Verbose = true;
            backup.Options = FbBackupFlags.IgnoreLimbo;
            backup.ServiceOutput += ServiceOutput;
            backup.Execute();
        }
        
        private void Restore()
        {
            var DB = txtPath.Text;
            if (DB.EndsWith(".gbk"))
            {
                var conexao = new FbConnectionStringBuilder();
                conexao.DataSource = "localhost";
                conexao.Database = DB;
                conexao.UserID = "SYSDBA";
                conexao.Password = "masterkey";

                var restore = new FbRestore();
                restore.ConnectionString = conexao.ToString();
                var fileInfo = new FileInfo(DB);
                pathDest = $"{fileInfo.DirectoryName}\\{DB.Substring(0, DB.Length - 3)}eco";
                restore.BackupFiles.Add(new FbBackupFile(pathDest));
                restore.Verbose = true;
                restore.ServiceOutput += ServiceOutput;
                restore.Execute();
            }
            else
            {
                Console.WriteLine("ERRO:Arquivo de Backup não encontrado!");    
            }
        }

        static void ServiceOutput(object sender, ServiceOutputEventArgs e)
        {
            Console.WriteLine(e.Message);
        }
    }
}
