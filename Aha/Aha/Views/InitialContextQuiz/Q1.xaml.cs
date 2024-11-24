using Aha.ViewModels;

namespace Aha.Views.InitialContextQuiz
{
    public partial class Q1 : ContentPage
    {
        public Q1()
        {
            InitializeComponent();
        }

        private void Picker_SelectedIndexChanged(object sender, EventArgs e)
        {
            (BindingContext as ContextQuizViewModel).SetLanguage();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            Application.Current.MainPage = new Q2((BindingContext as ContextQuizViewModel));
        }
    }
}