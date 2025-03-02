using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BrowserController : Controller
    {

        private readonly ILogger<BrowserController> _logger;

        public BrowserController(ILogger<BrowserController> logger)
        {
            _logger = logger;
        }

        [HttpPost(Name = "GetPath")]
        public IEnumerable<Info> GetPath(string path)
        {

            List<Info> result = new List<Info>();

            try
            {
                string[] dirs = Directory.GetDirectories(path, "*", SearchOption.TopDirectoryOnly);
                string[] fileEntries = Directory.GetFiles(path);
                foreach (var d in dirs)
                {
                    result.Add(new Info() { IsDirectory = true, Name = d });
                }
                foreach (var f in fileEntries)
                {
                    FileInfo fileInfo = new System.IO.FileInfo(f);
                    Info info = new Info() { Name = f, IsDirectory = false, Size = fileInfo.Length, CreationDate = fileInfo.CreationTimeUtc.ToLongDateString() + " " + fileInfo.CreationTimeUtc.ToLongTimeString(), ChangeDate = fileInfo.LastWriteTimeUtc.ToLongDateString() + " " + fileInfo.LastWriteTimeUtc.ToLongTimeString() };
                    result.Add(info);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
            }

            return result;
        }

        [HttpGet(Name = "GetDisks")]
        public IEnumerable<Info> GetDisks()
        {
            List<Info> result = new List<Info>();

            var driveInfo = System.IO.DriveInfo.GetDrives();
            foreach (var di in driveInfo)
            {
                result.Add(new Info() { IsDirectory = true, Name = di.Name });
            }

            return result;
        }

    }
}
