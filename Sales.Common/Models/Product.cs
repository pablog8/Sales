namespace Sales.Common.Models
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class Product
    { 
        //clave
        [Key]
        public int ProductId { get; set; }

        public int CategoryId { get; set; }

        //campo obligatorio
        [Required]
        [StringLength(50)]
        public string Description { get; set; }

        [DataType(DataType.MultilineText)]
        public string Remarks { get; set; }

        [Display(Name = "Image")]
        public string ImagePath { get; set; }

        [DisplayFormat(DataFormatString ="{0:C2}", ApplyFormatInEditMode = false)]
        public Decimal Price { get; set; }

        [Display(Name = "Is Available")]
        public bool IsAvailable { get; set; }

        [Display(Name = "Publish On")]
        [DataType(DataType.Date)]
        public DateTime PublishOn { get; set; }

        [Required]
        [StringLength(128)]
        public string UserId { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        [JsonIgnore]
        public virtual Category Category { get; set; }

        //para tener atributos que no esten en la base de datos pero si en el modelo
        [NotMapped]
        public byte[] ImageArray { get; set; }




        public string ImageFullPath
        {
            get
            {
                if (string.IsNullOrEmpty(this.ImagePath))
                {
                    return "noproduct.png";
                }
                //devuelve la imagen (pagina de backend)
                return $"http://salesapiservices2018.azurewebsites.net/{this.ImagePath.Substring(1)}";
            }
        }

        public override string ToString()
        {
            return this.Description;
        }
    }
}
