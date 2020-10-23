using System.Collections.Generic;
using System.Web.Mvc;

namespace MyWebApp.Services
{
    public interface IFileService
    {
        FileResult CreateCsvFile(IList<object> data, string filename);
    }
}