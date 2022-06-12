using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models
{
    public class ParallaxImage
    {
        public int Id { get; set; }
        public int ParallaxId { get; set; }
        public Parallax Parallax { get; set; }
        public int ImageId { get; set; }
        public Image Image { get; set; }
        public bool IsMain { get; set; }
    }
}
