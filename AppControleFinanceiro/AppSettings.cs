namespace AppControleFinanceiro
{
    public class AppSettings
    {
        private static readonly string DatabaseName = "appcontrolefinanceiroDB.db";
        private static readonly string DatabaseDirectory = FileSystem.AppDataDirectory;

        public static readonly string DatabasePath = Path.Combine(DatabaseDirectory, DatabaseName);
    }
}
