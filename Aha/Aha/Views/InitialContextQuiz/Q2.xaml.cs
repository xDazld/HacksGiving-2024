using Aha.ViewModels;

namespace Aha.Views.InitialContextQuiz;

public partial class Q2 : ContentPage
{
	public Q2(ContextQuizViewModel bindingContext)
	{
		InitializeComponent();
        BindingContext = bindingContext;
	}

    private void Button_Clicked(object sender, EventArgs e)
    {
        Application.Current.MainPage = new Q3((BindingContext as ContextQuizViewModel));
    }
}