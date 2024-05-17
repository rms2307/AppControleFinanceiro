using AppControleFinanceiro.Views;

namespace AppControleFinanceiro
{
    public partial class App : Application
    {
        public App(TransactionList transactionList)
        {
            InitializeComponent();

            MainPage = new NavigationPage(transactionList);
        }
    }
}
