using NewsWebSite.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NewsWebSite.Models.ViewModel
{
    public class EditArticleModel
    {
        [Required]
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Display(Name = "Заголовок статьи")]
        [StringLength(50, ErrorMessage = "Description Max Length is 50")]
        public string Title { get; set; }

        [Display(Name = "Текст статьи")]
        [DataType(DataType.MultilineText)]
        [StringLength(4000, ErrorMessage = "Description Max Length is 4000")]
        public string FullDescription { get; set; }

        [Display(Name = "Изображение")]
        [ValidImage(maxSizeMB = 5)]
        [AllowedExtensions(new string[] { ".jpg", ".png" })]
        [DataType(DataType.Upload)]
        public HttpPostedFileBase Image { get; set; }

        public EditArticleModel(Article a)
        {
            Id = a.Id;
            Title = a.Title;
            FullDescription = a.FullDescription;
        }
        public EditArticleModel()
        {
            Title = null;
            FullDescription = null;
            Image = null;
        }
    }
}