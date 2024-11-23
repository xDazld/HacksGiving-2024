using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aha.ViewModels
{
    internal class ContextQuizViewModel : AbstractViewModel
    {
        private List<String> _languages = ["English","Espanol", "Francais"];
        List<String> Languages { get { return _languages; } }
        public String SelectedLanguage { get; set; }

        public void SetLanguage()
        {
            System.Globalization.CultureInfo.CurrentUICulture = new System.Globalization.CultureInfo(SelectedLanguage);
        }
    }
}
