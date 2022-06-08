using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace EduhomeMVC.ViewModels
{
    public class SliderCreate
    {
        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Body is required")]
        public string Body { get; set; }
        [Required(ErrorMessage = "Image is required")]
        public IFormFile ImageFile { get; set; }
    }
}
