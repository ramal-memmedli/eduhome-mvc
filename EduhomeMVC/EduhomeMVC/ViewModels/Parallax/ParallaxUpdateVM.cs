using Microsoft.AspNetCore.Http;

namespace EduhomeMVC.ViewModels
{
    public class ParallaxUpdateVM
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public IFormFile MainImage { get; set; }
        public IFormFile BackgroundImage { get; set; }
    }
}
