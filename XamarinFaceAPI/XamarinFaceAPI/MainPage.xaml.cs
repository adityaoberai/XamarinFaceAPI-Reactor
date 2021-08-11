using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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
                await DisplayAlert("Button Clicked", "Working", "Ok");
            }
        }
    }
}
