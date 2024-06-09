using AppControleFinanceiro.Libraries.Utils.AndroidUtils;
using AppControleFinanceiro.Messages;
using AppControleFinanceiro.Models;
using AppControleFinanceiro.Repositories.Interfaces;
using CommunityToolkit.Mvvm.Messaging;
using System.Text;

namespace AppControleFinanceiro.Views;

public partial class TransactionAdd : ContentPage
{
    private readonly ITransactionRepository _repository;

    public TransactionAdd(ITransactionRepository repository)
    {
        _repository = repository;

        InitializeComponent();
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
            Name = EntryNome.Text,
            Type = RadioReceita.IsChecked ? TransactionType.Income : TransactionType.Expense,
            Date = DatePicker.Date,
            Value = double.Parse(EntryValor.Text)
        };

        _repository.Add(transaction);
    }

    private bool IsValidData()
    {
        bool isValid = true;
        StringBuilder sb = new();

        if (string.IsNullOrEmpty(EntryNome.Text) || string.IsNullOrWhiteSpace(EntryNome.Text))
        {
            sb.AppendLine("O campo 'Nome' deve ser preenchido!");
            isValid = false;
        }

        if (string.IsNullOrEmpty(EntryValor.Text) || string.IsNullOrWhiteSpace(EntryValor.Text))
        {
            sb.AppendLine("O campo 'Valor' deve ser preenchido!");
            isValid = false;
        }

        if (!string.IsNullOrEmpty(EntryValor.Text) && !double.TryParse(EntryValor.Text, out double result))
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