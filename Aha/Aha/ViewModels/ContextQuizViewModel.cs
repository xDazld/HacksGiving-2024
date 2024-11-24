using Aha.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aha.ViewModels
{
    public class ContextQuizViewModel : AbstractViewModel
    {
        private List<String> _languages = ["English", "Espanol", "Francais"];
        public List<String> Languages { get => _languages; }
        public string SelectedLanguage { get; set; } = "English";
        public int Age { get; set; }

        public Command SaveContextCommand { get; set; }

        #region Interest Subjects
        public Boolean Science { get; set; }
        public Boolean Geography { get; set; }
        public Boolean History { get; set; }
        public Boolean Oceans { get; set; }
        public Boolean Automation { get; set; }
        public Boolean Technology { get; set; }
        public Boolean Engineering { get; set; }
        public Boolean Art { get; set; }
        public Boolean Music { get; set; }
        #endregion

        public ContextQuizViewModel()
        {
            SaveContextCommand = new Command(SaveContext);
        }

        private async void SaveContext(object obj)
        {
            Debug.WriteLine("Saving context...");
            List<String> interests = new List<String>();
            if (Science) interests.Add("Science");
            if (Geography) interests.Add("Geography");
            if (History) interests.Add("History");
            if (Oceans) interests.Add("Oceans");
            if (Automation) interests.Add("Automation");
            if (Technology) interests.Add("Technology");
            if (Engineering) interests.Add("Engineering");
            if (Art) interests.Add("Art");
            if (Music) interests.Add("Music");

            Debug.WriteLine("Interests: " + interests);
            Debug.WriteLine("Age: " + Age);
            Debug.WriteLine("Language: " + SelectedLanguage);
            UserContext User = UserContextManager.getUserContext().User;
            User.age = Age;
            User.interests = interests;
            User.language = SelectedLanguage;
            UserContextManager.getUserContext().User = User;
            await UserContextManager.getUserContext().SaveAsync();
            Application.Current.MainPage = new AppShell();
        }

        public void SetLanguage()
        {
            switch (SelectedLanguage)
            {
                case "English":
                    System.Globalization.CultureInfo.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
                    break;
                case "Espanol":
                    System.Globalization.CultureInfo.CurrentUICulture = new System.Globalization.CultureInfo("es-ES");
                    break;
                case "Francais":
                    System.Globalization.CultureInfo.CurrentUICulture = new System.Globalization.CultureInfo("fr-FR");
                    break;
            }
        }
    }
}
