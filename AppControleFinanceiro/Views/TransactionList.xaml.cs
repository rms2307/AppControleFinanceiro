using AppControleFinanceiro.Messages;
using AppControleFinanceiro.Repositories.Interfaces;
using CommunityToolkit.Mvvm.Messaging;

namespace AppControleFinanceiro.Views;

public partial class TransactionList : ContentPage
{

    private readonly ITransactionRepository _repository;

    public TransactionList(ITransactionRepository repository)
    {
        _repository = repository;

        InitializeComponent();

        Reload();
        WeakReferenceMessenger.Default.Register<TransactionAddMessage>(this, (e, msg) =>
        {
            Reload();
        });
    }

    private void Reload()
    {
        CollectionViewTransactions.ItemsSource = _repository.GetAll().OrderByDescending(t => t.Date);
    }

    private void OnButtonClicked_To_TransactionAdd(object sender, EventArgs args)
    {
        TransactionAdd? transactionAdd = Handler?.MauiContext?.Services.GetService<TransactionAdd>();
        Navigation.PushModalAsync(transactionAdd);
    }

    private void OnButtonClicked_To_TransactionEdit(object sender, EventArgs e)
    {
        TransactionEdit? transactionEdit = Handler?.MauiContext?.Services.GetService<TransactionEdit>();
        Navigation.PushModalAsync(transactionEdit);
    }
}