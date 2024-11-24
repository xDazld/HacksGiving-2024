using Aha.Models;
using Aha.Views.InitialContextQuiz;

namespace Aha
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            if (File.Exists(UserContextManager.getUserContext().FilePath))
            {
                MainPage = new AppShell();
            }
            else
            {
                MainPage = new QInitial();
            }
        }
    }
}
