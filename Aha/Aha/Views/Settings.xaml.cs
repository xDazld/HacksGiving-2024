using Aha.Models;

namespace Aha.Views;

public partial class Settings : ContentPage
{
    public Settings()
    {
        InitializeComponent();
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        File.Delete(UserContextManager.getUserContext().FilePath);
        Application.Current.MainPage = new Views.InitialContextQuiz.QInitial();
    }
}