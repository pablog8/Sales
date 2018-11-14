﻿namespace Sales.Common.Models
{
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Video
    {
        [Key]
        public int VideoId { get; set; }

        [Required]
        [StringLength(50)]
        public string Description { get; set; }

        public string LinkVideo { get; set; }

        [Display(Name = "Image")]
        public string ImagePath { get; set; }

        public string ImageFullPath
        {
            get
            {
                if (string.IsNullOrEmpty(this.ImagePath))
                {
                    return "noproduct";
                }

                return $"http://salesbackend20180920053657.azurewebsites.net{this.ImagePath.Substring(1)}";
            }
        }
    }
}


