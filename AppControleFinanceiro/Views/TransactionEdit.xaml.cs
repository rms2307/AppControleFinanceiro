using AppControleFinanceiro.Libraries.Utils.AndroidUtils;
using AppControleFinanceiro.Messages;
using AppControleFinanceiro.Models;
using AppControleFinanceiro.Repositories.Interfaces;
using CommunityToolkit.Mvvm.Messaging;
using System.Text;

namespace AppControleFinanceiro.Views;

public partial class TransactionEdit : ContentPage
{
    private Transaction _transaction;
    private readonly ITransactionRepository _repository;

    public TransactionEdit(ITransactionRepository repository)
    {
        InitializeComponent();
        _repository = repository;
    }

    public void SetTransactionToEdit(Transaction transaction)
    {
        _transaction = transaction;

        if (transaction.Type == TransactionType.Income)
            RadioIncome.IsChecked = true;
        else
            RadioExpense.IsChecked = true;

        EntryName.Text = transaction.Name;
        DatePicker.Date = transaction.Date.Date;
        EntryValue.Text = transaction.Value.ToString();
    }

    private void TapGestureRecognizerTapped_ToClose(object sender, TappedEventArgs e)
    {
        CloseModal();
    }

    private void OnButtonClicked_Save(object sender, EventArgs e)
    {
        if (!IsValidData())
        {
            return;
        }

        SaveTransactionInDataBase();

        CloseModal();
        WeakReferenceMessenger.Default.Send<TransactionAddMessage>();
    }

    private void SaveTransactionInDataBase()
    {
        Transaction transaction = new()
        {
            Id = _transaction.Id,
            Name = EntryName.Text,
            Type = RadioIncome.IsChecked ? TransactionType.Income : TransactionType.Expense,
            Date = DatePicker.Date,
            Value = Math.Abs(double.Parse(EntryValue.Text))
        };

        _repository.Update(transaction);
    }

    private bool IsValidData()
    {
        bool isValid = true;
        StringBuilder sb = new();

        if (string.IsNullOrEmpty(EntryName.Text) || string.IsNullOrWhiteSpace(EntryName.Text))
        {
            sb.AppendLine("O campo 'Nome' deve ser preenchido!");
            isValid = false;
        }

        if (string.IsNullOrEmpty(EntryValue.Text) || string.IsNullOrWhiteSpace(EntryValue.Text))
        {
            sb.AppendLine("O campo 'Valor' deve ser preenchido!");
            isValid = false;
        }

        if (!string.IsNullOrEmpty(EntryValue.Text) && !double.TryParse(EntryValue.Text, out double result))
        {
            sb.AppendLine("O campo 'Valor' e invalido. Apenas numeros sao permitidos!");
            isValid = false;
        }

        if (!isValid)
        {
            LabelError.IsVisible = true;
            LabelError.Text = sb.ToString();
        }

        return isValid;
    }

    private void CloseModal()
    {
        KeyboardUtils.HideKeyboardAndroid();
        Navigation.PopModalAsync();
    }
}