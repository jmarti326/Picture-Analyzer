using System.IO;
using System.Threading.Tasks;
using Microsoft.ProjectOxford.Vision.Contract;
using Xamarin.Forms;
using Microsoft.ProjectOxford.Vision;
using System.Linq;
using Plugin.Media;
using Plugin.Media.Abstractions;

namespace pictureAnalyzer
{
    public partial class pictureAnalyzerPage : ContentPage
    {
        public pictureAnalyzerPage()
        {
            InitializeComponent();
        }

        private async Task<AnalysisResult> GetImageDescription(Stream imageStream)
        {
            VisionServiceClient visionClient = new VisionServiceClient(
                "4b3b5c6acb3547079b4f830d8a8626b4", 
                "https://eastus2.api.cognitive.microsoft.com/vision/v1.0");

            VisualFeature[] features = { VisualFeature.Tags };

            return await visionClient.AnalyzeImageAsync(imageStream, features.ToList(), null);
        }

		private async Task SelectPicture()
		{
			if (CrossMedia.Current.IsPickPhotoSupported)
			{
                var image = await CrossMedia.Current.PickPhotoAsync();

				MyImage.Source = ImageSource.FromStream(() =>
				{
					return image.GetStream();
				});

				MyActivityIndicator.IsRunning = true;

				try
				{
					var result = await GetImageDescription(image.GetStream());

					MyLabel.Text = "";

					foreach (var tag in result.Tags)
					{
						MyLabel.Text += tag.Name + "\n";
					}
				}
				catch (ClientException ex)
				{
					MyLabel.Text = ex.Message;
				}

                MyActivityIndicator.IsRunning = false;
			}
        }

        async void Handle_Clicked(object sender, System.EventArgs e)
        {
            await SelectPicture();
        }
    }
}
