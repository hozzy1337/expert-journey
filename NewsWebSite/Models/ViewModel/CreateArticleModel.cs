using NewsWebSite.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NewsWebSite.Models.ViewModel
{
    public class CreateArticleModel
    {

        //public int Id { get; set; }
        [Required]
        [Display(Name = "Заголовок")]
        [StringLength(50, ErrorMessage = "Description Max Length is 50")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Текст статьи")]
        [DataType(DataType.MultilineText)]
        [StringLength(5000, ErrorMessage = "Description Max Length is 5000")]
        public string FullDescription { get; set; }

        [Required]
        [Display(Name = "Изображение")]
        [ValidImage]
        [AllowedExtensions(new string[] {".jpg", ".png" })]
        [DataType(DataType.Upload)]
        public HttpPostedFileBase Image { get; set; }
    }
}