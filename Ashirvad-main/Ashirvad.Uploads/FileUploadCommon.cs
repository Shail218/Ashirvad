using Ashirvad.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Ashirvad.Uploads
{
   public  class FileUploadCommon
    {
        FileModel fileModel = new FileModel();
        public async Task<FileModel> SaveFileUploadweb(HttpPostedFileBase ImageFile,string Folderpath)
        {
            
            try
            {
                if (ImageFile != null)
                {
                    string currentDir = AppDomain.CurrentDomain.BaseDirectory;
                    string UpdatedPath = currentDir.Replace("Ashirvad.Web", "Ashirvad.Uploads");
                    string _FileName = Path.GetFileName(ImageFile.FileName);
                    string extension = System.IO.Path.GetExtension(ImageFile.FileName);
                    string randomfilename = Common.Common.RandomString(20);
                    string _Filepath =  Folderpath + "\\" + randomfilename + extension;
                    string _path = UpdatedPath+ _Filepath;
                    ImageFile.SaveAs(_path);
                    fileModel.FilePath = _path;
                    fileModel.FileName = _FileName;
                }
            }
            catch(Exception ex)
            {

            }

            return fileModel;

        }


        public async Task<FileModel> SaveFileUploadAPK(string Folderpath)
        {
            try
            {
                var httpRequest = HttpContext.Current.Request;
                foreach (string file in httpRequest.Files)
                {
                    string fileName;
                    string extension;
                    string currentDir = AppDomain.CurrentDomain.BaseDirectory;
                    string UpdatedPath = currentDir.Replace("Ashirvad.API", "Ashirvad.Uploads");
                    var postedFile = httpRequest.Files[file];
                    string randomfilename = Common.Common.RandomString(20);
                    extension = Path.GetExtension(postedFile.FileName);
                    fileName = Path.GetFileName(postedFile.FileName);
                    string _Filepath = Folderpath + "\\" + randomfilename + extension;
                    string _path = UpdatedPath + _Filepath;
                    postedFile.SaveAs(_path);
                    fileModel.FileName = fileName;
                    fileModel.FilePath = _Filepath;

                }
            }
            catch(Exception ex)
            {

            }
            
            return fileModel;
        }
}
}
