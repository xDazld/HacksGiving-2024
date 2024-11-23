namespace Aha.Views
{
    public partial class AIAssistant : ContentPage
    {
        public AIAssistant()
        {
            InitializeComponent();
        }

        private void MediaCaptured(object sender, CommunityToolkit.Maui.Views.MediaCapturedEventArgs e)
        {
            ImageSource imageSource = ImageSource.FromStream(() => e.Media);

        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            await Camera.CaptureImage(CancellationToken.None);
        }
    }
}
