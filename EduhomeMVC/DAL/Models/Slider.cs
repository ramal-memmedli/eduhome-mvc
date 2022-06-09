using DAL.Base;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DAL.Models
{
    public class Slider : BaseEntity, IEntity
    {
        [MaxLength(48)]
        public string Title { get; set; }
        [MaxLength(255)]
        public string Body { get; set; }
        public int ImageId { get; set; }
        public Image Image { get; set; }
        public bool IsActive { get; set; }
    }
}
