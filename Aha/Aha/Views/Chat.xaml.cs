// File: Aha/Views/Chat.xaml.cs
using Microsoft.Maui.Controls;
using Aha.ViewModels;
using Aha.Models;
using System.Collections.Specialized;
using System.Linq;

namespace Aha.Views;

public partial class Chat : ContentPage
{
    public Chat()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        (BindingContext as ChatViewModel).SendInitialContextAndMessage();
    }
}