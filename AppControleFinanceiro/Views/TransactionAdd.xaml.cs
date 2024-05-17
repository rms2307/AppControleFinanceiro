using AppControleFinanceiro.Models;
using AppControleFinanceiro.Repositories.Interfaces;
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

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        Navigation.PopModalAsync();
    }

    private void OnButtonClicked_Save(object sender, EventArgs e)
    {
        if (!IsValidData())
        {
            return;
        }

        SaveTransactionInDataBase();

        Navigation.PopModalAsync();

        int count = _repository.GetAll().Count;
        App.Current.MainPage.DisplayAlert("Mensagem!", $"Existem {count} registro(s) salvos", "OK");
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

        if (string.IsNullOrEmpty(EntryValor.Text) && !double.TryParse(EntryValor.Text, out double result))
        {
            sb.AppendLine("O campo 'Valor' e invalido. Apenas numeros sao permitidos!");
            isValid = false;
        }

        if (!isValid)
        {
            LabelError.Text = sb.ToString();
        }

        return isValid;
    }
}