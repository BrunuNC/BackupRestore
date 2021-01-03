using System;
using System.Windows.Forms;
using System.IO;
using FirebirdSql.Data.FirebirdClient;
using FirebirdSql.Data.Services;

namespace BackupRestore
{
    public partial class Form1 : Form
    {
        private string BD = "";
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
            BD = txtPath.Text;
            var conexao = new FbConnectionStringBuilder();
            conexao.DataSource = "localhost";
            conexao.Database = BD;
            conexao.UserID = "SYSDBA";
            conexao.Password = "masterkey";

            var backup = new FbBackup();
            backup.ConnectionString = conexao.ToString();
            string nome = Path.GetFileNameWithoutExtension(BD);
            var fileInfo = new FileInfo(BD);
            pathDest = fileInfo.DirectoryName + "\\" + nome + ".gbk";
            backup.BackupFiles.Add(new FbBackupFile(pathDest));
            backup.Verbose = true;
            backup.Options = FbBackupFlags.IgnoreLimbo;
            backup.ServiceOutput += ServiceOutput;
            backup.Execute();
        }

        static void ServiceOutput(object sender, ServiceOutputEventArgs e)
        {
            Console.WriteLine(e.Message);
        }
        private void Restore()
        {
            
        }
    }
}
