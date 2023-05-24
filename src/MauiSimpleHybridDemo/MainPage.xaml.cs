namespace MauiSimpleHybridDemo;

public partial class MainPage : ContentPage
{
    public Command SendCommand { get; set; }

    public MainPage()
	{
		InitializeComponent();

        SendCommand = new Command(async () =>
        {
            await SendMessageToJs(Msg.Text);
            Msg.Text = string.Empty;
        });


        BindingContext = this;

#if DEBUG
        myHybridView.EnableWebDevTools = true;
#endif
    }

    private async void HybridWebView_RawMessageReceived(object sender, HybridWebView.HybridWebViewRawMessageReceivedEventArgs e)
    {
        if (!string.IsNullOrEmpty(e.Message))
        {
            if(e.Message == "bot")
            {
                BotAppears();
            }
            else { 
                await Dispatcher.DispatchAsync(async () =>
                {
                    await DisplayAlert("Msg", e.Message, "Okay");
                });
            }
        }
    }

    private async Task SendMessageToJs(string msg)
    {
        await myHybridView.InvokeJsMethodAsync("LogMessageFromCSharp", msg);
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        await SendMessageToJs(Msg.Text);
    }

    private async void BotAppears()
    {
        Bot.Text = string.Empty;
        Fullscreen();
        await Bot.FadeTo(1);
        Bot.ZIndex=1;
        string text = await ReadTextFile("bot.txt");
        int charCount = 0;
        foreach (char c in text)
        {
            Bot.Text += c;
            charCount++;
            if (charCount % 2 == 0)
            {
                await Task.Delay(1);
            }
        }
    }

    private void Fullscreen()
    {
        Dispatcher.Dispatch(() =>
        {
            var displayInfo = DeviceDisplay.MainDisplayInfo;
            var width = displayInfo.Width;
            var height = displayInfo.Height;
            this.Window.X = this.Window.Y = 0;
            this.Window.Width = width;
            this.Window.Height = height;
        });
        
    }

    public async Task<string> ReadTextFile(string filePath)
    {
        using Stream fileStream = await FileSystem.Current.OpenAppPackageFileAsync(filePath);
        using StreamReader reader = new StreamReader(fileStream);

        return await reader.ReadToEndAsync();
    }

    private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        await Bot.FadeTo(0);
        Bot.ZIndex = -1;
    }
}

