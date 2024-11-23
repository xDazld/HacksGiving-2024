using Aha.Views.InitialContextQuiz;

namespace Aha
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new QInitial();
        }
    }
}
