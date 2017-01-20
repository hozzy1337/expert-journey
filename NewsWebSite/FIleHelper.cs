using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NewsWebSite
{
    public class FIleHelper
    {
        public bool SaveOrUpdateArticleImage(string folderPath, HttpPostedFileBase image, int id)
        {
            var isChanged = true;
            var filename = Path.GetFileName(image.FileName);
            string userIdFolderPath = Path.Combine(folderPath, id.ToString());

            if (!(Directory.Exists(userIdFolderPath))) Directory.CreateDirectory(userIdFolderPath);
            else
            {
                var filesInDir = Directory.GetFiles(userIdFolderPath);
                foreach (var f in filesInDir)
                {
                    var fileInfo = new FileInfo(f);
                    var oldFileSize = fileInfo.Length;
                    if (image.ContentLength == oldFileSize) isChanged = false;
                    else
                    File.Delete(f);
                }
            }
            if(isChanged)
            image.SaveAs(Path.Combine(userIdFolderPath, filename));
            return isChanged; 
        }
    }
}