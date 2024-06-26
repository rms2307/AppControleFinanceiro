using AppControleFinanceiro.Messages;
using AppControleFinanceiro.Models;
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
        IEnumerable<Transaction> transactions = _repository.GetAll().OrderByDescending(t => t.Date);
        CollectionViewTransactions.ItemsSource = transactions;

        CalculateCardSummaryValuesAndSendScreen(transactions);
    }

    private void CalculateCardSummaryValuesAndSendScreen(IEnumerable<Transaction> transactions)
    {
        double income = transactions.Where(t => t.Type == TransactionType.Income).Sum(t => t.Value);
        double expense = transactions.Where(t => t.Type == TransactionType.Expense).Sum(t => t.Value);
        double balance = income - expense;

        LabelIncome.Text = income.ToString("C");
        LabelExpense.Text = expense.ToString("C");
        LabelBalance.Text = balance.ToString("C");
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

    private void TapGestureRecognizerTapped_To_TransactionEdit(object sender, TappedEventArgs e)
    {
        Grid grid = (Grid)sender;
        TapGestureRecognizer gesture = (TapGestureRecognizer)grid.GestureRecognizers[0];
        Transaction? transaction = (Transaction?)gesture.CommandParameter;

        TransactionEdit? transactionEdit = Handler?.MauiContext?.Services.GetService<TransactionEdit>();
        if (transaction != null) transactionEdit?.SetTransactionToEdit(transaction);

        Navigation.PushModalAsync(transactionEdit);
    }

    private async void TapGestureRecognizerTapped_ToDelete(object sender, TappedEventArgs e)
    {
        await AnimationBorder(sender as Border, true);
        bool result = await App.Current.MainPage.DisplayAlert("Excluir!", "Tem Certeza?", "Sim", "N�o");

        if (result)
        {
            Transaction? transaction = (Transaction)e.Parameter!;
            _repository.Delete(transaction);

            Reload();
        }
        else
        {
            await AnimationBorder(sender as Border, false);
        }
    }

    private Color _borderOriginalBackgroundColor;
    private string _labelOriginalText;

    private async Task AnimationBorder(Border border, bool isDeleteAnimation)
    {
        Label label = border.Content as Label;
        if (isDeleteAnimation)
        {
            _borderOriginalBackgroundColor = border.BackgroundColor;
            _labelOriginalText = label.Text;

            await border.RotateYTo(90, 300);

            border.BackgroundColor = Colors.Red;
            label.TextColor = Colors.White;
            label.Text = "X";

            await border.RotateYTo(180, 300);
        }
        else
        {
            await border.RotateYTo(90, 300);

            border.BackgroundColor = _borderOriginalBackgroundColor;
            label.TextColor = Colors.Black;
            label.Text = _labelOriginalText;

            await border.RotateYTo(0, 300);
        }
    }
}