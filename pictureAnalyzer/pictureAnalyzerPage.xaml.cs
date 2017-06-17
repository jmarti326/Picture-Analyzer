using System.IO;
using System.Threading.Tasks;
using Microsoft.ProjectOxford.Vision.Contract;
using Xamarin.Forms;
using Microsoft.ProjectOxford.Vision;
using System.Linq;
using Plugin.Media;

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
                "ee8f52426e87468cbff5ab0f4c19bc8f", 
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
			}
		}
    }
}
