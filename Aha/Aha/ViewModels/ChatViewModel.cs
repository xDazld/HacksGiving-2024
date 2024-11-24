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
    public bool BackgroundTaskRunning { get; set; }

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
        closestLocation = LocationContextManager.getLocationContextManager().CurrentNearestLocationContext;
        WebRelayService.GetWebRelayService().MessageReceived += OnMessageReceived;
        WebRelayService.GetWebRelayService().MessageEnd += OnMessageEnd;

        SendCommand = new AsyncRelayCommand(OnSend);
    }

    private string messageCurrent = "";
    private Message messageToAddToChat = null;
    private void OnMessageEnd(string obj)
    {
        Messages.Add(new Message { Text = messageCurrent, IsUser = false });
        messageCurrent = string.Empty;
        OnPropertyChanged(nameof(Messages));
    }

    private void OnMessageReceived(string obj)
    {
        messageCurrent += obj;
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
            BackgroundTaskRunning = true;
            OnPropertyChanged(nameof(BackgroundTaskRunning));
            await WebRelayService.GetWebRelayService().WebRelaySendAsync("{\"event\": \"chatMessage\",\"prompt\": \"" + UserInput + "\",\"currentExhibit\": \"" + "All-Aboard" + "\"}");
            BackgroundTaskRunning = false;
            OnPropertyChanged(nameof(BackgroundTaskRunning));
        }
    }

    public async void SendInitialContextAndMessage()
    {
        string context = UserContextManager.getUserContext().getUserContextAsJson().Substring(1);
        BackgroundTaskRunning = true;
        OnPropertyChanged(nameof(BackgroundTaskRunning));
        //string message = $",\"event\": \"newChat\",\"currentExhibit\": \"{Location.LocationName}\"";
        string message = $",\"event\": \"newChat\",\"currentExhibit\": \"" + "All-Aboard" + "\"";
        var completeMessage = "{\"userContext\": {" + context + message + "}";
        await WebRelayService.GetWebRelayService().WebRelaySendAsync(completeMessage);
        BackgroundTaskRunning = false;
        OnPropertyChanged(nameof(BackgroundTaskRunning));
    }
}