using AppControleFinanceiro.Models;

namespace AppControleFinanceiro.Views;

public partial class TransactionEdit : ContentPage
{
    private Transaction? _transaction;

    public TransactionEdit()
    {
        InitializeComponent();
    }

    public void SetTransactionToEdit(Transaction transaction)
    {
        _transaction = transaction;

        if (transaction.Type == TransactionType.Income)
            RadioIncome.IsChecked = true;
        else
            RadioExpense.IsChecked = true;

        EntryName.Text = transaction.Name;
        EntryValue.Text = transaction.Value.ToString();
        DatePickerDate.Date = transaction.Date.Date;
    }

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        Navigation.PopModalAsync();
    }
}