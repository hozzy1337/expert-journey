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
        public static void SaveOrUpdateArticleImage(string folderPath, HttpPostedFileBase image, int id)
        {
            var filename = Path.GetFileName(image.FileName);
            string userIdFolderPath = Path.Combine(folderPath, id.ToString());

            if (!(Directory.Exists(userIdFolderPath))) Directory.CreateDirectory(userIdFolderPath);
            else
            {
                var filesInDir = Directory.GetFiles(userIdFolderPath);
                foreach (var f in filesInDir)
                    File.Delete(f);
            }
            image.SaveAs(Path.Combine(userIdFolderPath, filename));
        }
    }
}