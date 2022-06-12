using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace EduhomeMVC.ViewModels
{
    public class ParallaxGetVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string ImageUrl { get; set; }
        public string BackgroundUrl { get; set; }
    }
}
