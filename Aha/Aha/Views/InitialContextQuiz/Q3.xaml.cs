using Aha.ViewModels;

namespace Aha.Views.InitialContextQuiz;

public partial class Q3 : ContentPage
{
	public Q3(ContextQuizViewModel bindingContext)
	{
		InitializeComponent();
        BindingContext = bindingContext;
    }
}