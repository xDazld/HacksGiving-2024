using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Aha.Models;

namespace Aha.ViewModels
{
    public class ChatViewModel : AbstractViewModel
    {
        public ObservableCollection<Message> Messages { get; } = new ObservableCollection<Message>();

        private string _userInput;
        public string UserInput
        {
            get => _userInput;
            set
            {
                if (_userInput != value)
                {
                    _userInput = value;
                    OnPropertyChanged(nameof(UserInput));
                }
            }
        }

        public ICommand SendCommand { get; }

        public ChatViewModel()
        {
            SendCommand = new Command(OnSend);
        }

        private void OnSend()
        {
            if (!string.IsNullOrWhiteSpace(UserInput))
            {
                // Add user's message
                Messages.Add(new Message { Text = UserInput, IsUser = true });

                // Clear input
                UserInput = string.Empty;

                // Simulate chatbot response (stubbed)
                Messages.Add(new Message { Text = "This is a stubbed chatbot response.", IsUser = false });
            }
        }
    }
}