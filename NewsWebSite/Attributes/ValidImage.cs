using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;

namespace NewsWebSite.Attributes
{
    public class ValidImage : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            //return false;
            if (value == null) return false;
            int maxContent = 5 * 1024 * 1024; //3 MB
            string[] sAllowedExt = new string[] { ".jpg", ".png" };


            var file = value as HttpPostedFileBase;

            if (file == null) return false;
            if (!sAllowedExt.Contains(Path.GetExtension(file.FileName)))
            {
                ErrorMessage = "Please upload Your Photo of type: " + string.Join(", ", sAllowedExt);
                return false;
            }
            if (file.ContentLength > maxContent)
            {
                ErrorMessage = "Your Photo is too large, maximum allowed size is : " + (maxContent / 1024).ToString() + "MB";
                return false;
            }
            
                return true;
        }
    }
}