using System;
using System.Collections.Generic;
using System.Text;

namespace SocialMojifier.Helpers
{
    public class FaceAPICredentials
    {
        public string APIKey { get; set; } = "<Add Azure Cognitive Services Face API Key Here>";
        public string Endpoint { get; set; } = "https://centralindia.api.cognitive.microsoft.com/"; //change the region in the endpoint based on the region of you Azure resource
    }
}
