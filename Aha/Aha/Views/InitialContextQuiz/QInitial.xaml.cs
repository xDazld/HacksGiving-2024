namespace Aha.Views.InitialContextQuiz;

public partial class QInitial : ContentPage
{
	public QInitial()
	{
		InitializeComponent();
	}

    private void Button_Clicked(object sender, EventArgs e)
    {
		Application.Current.MainPage = new Q1();
    }
}