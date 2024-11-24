using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Aha.Models;
using CommunityToolkit.Mvvm.Input;

namespace Aha.ViewModels;

public class ChatViewModel : AbstractViewModel
{
    private LocationContext closestLocation;
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
    private Message currentResponseFromAI;

    public ChatViewModel()
    {
        closestLocation = LocationContextManager.getLocationContextManager().CurrentNearestLocationContext;
        WebRelayService.GetWebRelayService().MessageReceived += OnMessageReceived;
        WebRelayService.GetWebRelayService().MessageEnd += OnMessageEnd;

        SendCommand = new AsyncRelayCommand(OnSend);
    }

    private void OnMessageEnd(string obj)
    {
        currentResponseFromAI = null;
    }

    private void OnMessageReceived(string obj)
    {
        if (currentResponseFromAI == null)
        {
            Message message = new Message { Text = obj, IsUser = false };
            Messages.Add(message);
            currentResponseFromAI = message;
        }
        currentResponseFromAI.Text += obj;
        OnPropertyChanged(nameof(currentResponseFromAI));
        OnPropertyChanged(nameof(Messages));
    }

    private async Task OnSend()
    {
        if (!string.IsNullOrWhiteSpace(UserInput))
        {
            // Add user's message
            Messages.Add(new Message { Text = UserInput, IsUser = true });

            // Clear input
            UserInput = string.Empty;

            /*
            {
                "event": "newChat",
                "prompt": "What's a turtle?",
                "currentExhibit": "Exhibit 1"
            }
            */
            //Possibly need to await this
            await WebRelayService.GetWebRelayService().WebRelaySendAsync("\"event\": \"newChat\",\"prompt\": \""+ UserInput +"\",\"currentExhibit\": \""+closestLocation.LocationName+"\"}");
        }
    }


    /*
     {
        "userContext": {
            "age": 5,
            "language": "English",
            "interests": [
                "automation",
                "science"
            ]
        },
        "event": "newChat",
        "currentExhibit": "Exhibit 1"
    }
     */
    public async void SendInitialContextAndMessage()
    {
        string context = UserContextManager.getUserContext().getUserContextAsJson().Substring(1);
        
        //string message = $",\"event\": \"newChat\",\"currentExhibit\": \"{Location.LocationName}\"";
        string message = $",\"event\": \"newChat\",\"currentExhibit\": \""+closestLocation.LocationName+"\"";
        await WebRelayService.GetWebRelayService().WebRelaySendAsync("{\"userContext\": " + context + message + "}");
    }
}