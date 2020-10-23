using System.Collections.Generic;
using System.Composition;
using System.Net;
using System.Text;
using System.Web.Mvc;

namespace MyWebApp.Services
{
    [Export(typeof(IFileService))]
    public class FileService
    {
        public FileService()
        {
        }
        
        
    }
}