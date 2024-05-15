namespace AppControleFinanceiro.Views;

public partial class TransactionAdd : ContentPage
{
    public TransactionAdd()
    {
        InitializeComponent();
    }

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        Navigation.PopModalAsync();
    }

    public void OnButtonClicked_Save(object sencer, EventArgs e)
    {

    }

    private bool IsValidData()
    {
        bool valid = true;

        if (string.IsNullOrEmpty(EntryNome.Text) || string.IsNullOrWhiteSpace(EntryNome.Text))
        {
            valid = false;
        }

        if (string.IsNullOrEmpty(EntryValor.Text) || string.IsNullOrWhiteSpace(EntryValor.Text))
        {
            valid = false;
        }

        double result;
        if (string.IsNullOrEmpty(EntryValor.Text) && !double.TryParse(EntryValor.Text, out result))
        {
            valid = false;
        }

        return valid;
    }
}