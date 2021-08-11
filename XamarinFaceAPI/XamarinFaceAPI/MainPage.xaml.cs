using Microsoft.Azure.CognitiveServices.Vision.Face;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using Plugin.Media;
using Plugin.Media.Abstractions;
using SocialMojifier.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XamarinFaceAPI
{
    public partial class MainPage : ContentPage
    {
        public MediaFile capturedImage;
        public string filePath;
        public MainPage()
        {
            InitializeComponent();

            Capture.Source = null;
            GetEmotion.IsVisible = false;
            GetEmotion.IsEnabled = false;
        }

        private async void CaptureImage_Clicked(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("No Camera", ":( No camera available.", "OK");
                return;
            }

            var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
            {
                DefaultCamera = CameraDevice.Front,
                PhotoSize = PhotoSize.Medium,
                Directory = "FaceAPITest",
                Name = "capture.jpg",
                SaveToAlbum = true
            });

            if (file == null)
            {
                return;
            }

            capturedImage = file;
            filePath = file.Path;

            Capture.Source = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                return stream;
            });
            GetEmotion.IsVisible = true;
            GetEmotion.IsEnabled = true;
        }

        private async void GetEmotion_Clicked(object sender, EventArgs e)
        {
            if (Capture.Source != null)
            {
                await DetectEmotion();
            }
        }

        public async Task DetectEmotion()
        {
            var credentials = new FaceAPICredentials();
            var client = new FaceClient(new ApiKeyServiceClientCredentials(credentials.APIKey)) { Endpoint = credentials.Endpoint };
            var responseList = await client.Face.DetectWithStreamAsync(capturedImage.GetStream(), returnFaceAttributes: new List<FaceAttributeType> { FaceAttributeType.Emotion });
            var face = responseList.FirstOrDefault();
            var predominant = FindPredominantEmotion(face.FaceAttributes.Emotion);

            await DisplayAlert("Predominant Emotion", predominant, "Ok");
        }

        public string FindPredominantEmotion(Emotion emotion)
        {
            double max = 0;
            PropertyInfo prop = null;

            var emotionValues = typeof(Emotion).GetProperties();

            foreach(PropertyInfo property in emotionValues)
            {
                var value = (double)property.GetValue(emotion);

                if(value>max)
                {
                    max = value;
                    prop = property;
                }
            }

            return prop.Name.ToString();
        }
    }
}
