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
    }
}
