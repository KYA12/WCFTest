using log4net;
using log4net.Config;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using System;
using System.Configuration;
using System.IO;
using System.ServiceProcess;
using System.Threading;

namespace WindowsService
{
    public partial class NewService : ServiceBase
    {
        /* Absolute path */
        //private static readonly string watchPathAbsolute = ConfigurationManager.AppSettings["WatchPathAbsolute"];
        //private static readonly string garbagePathAbsolute = ConfigurationManager.AppSettings["GarbagePathAbsolute"];
        //private static readonly string completePathAbsolute = ConfigurationManager.AppSettings["CompletePathAbsolute"];

        /* Relative path to files*/
        private static readonly string watchPathRelative = ConfigurationManager.AppSettings["WatchPathRelative"];
        private static readonly string garbagePathRelative = ConfigurationManager.AppSettings["GarbagePathRelative"];
        private static readonly string completePathRelative = ConfigurationManager.AppSettings["CompletePathRelative"];
        private static ILog log;

        public NewService()
        {
            InitializeComponent();
            log = LogManager.GetLogger(typeof(NewService)); // Initialization of Log4Net
        }

        /* Check if file locked or unlocked */
        public bool WaitForFile(FileInfo file)
        {

            FileStream stream = null;
            try
            {
                // Try to open file
                stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None);
            }
            catch (IOException)
            {
                // If locked, returns true
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }
            // If file unlocked, returns false
            return false;
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                /* Relative path */
                string drive = Path.GetPathRoot(AppDomain.CurrentDomain.BaseDirectory);
                string path = Path.Combine(drive, watchPathRelative);
                fileSystemWatcher.Path = path;

                /* Absolute path */
                //fileSystemWatcher.Path = watchPathAbsolute;
            }
 
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        protected override void OnStop()
        {

        }

        /* fileSystemWatcher handles an event of file creation */
        private void FileSystemWatcher_Created(object sender, System.IO.FileSystemEventArgs e)
        {
            try
            {
                // Get access to the connected service from WSCService
                ServiceReference.ServiceClient reference = new ServiceReference.ServiceClient();

                // Check if file locked or unlocked for access
                //while (WaitForFile(new FileInfo(e.FullPath)))
                //{
                //    Thread.Sleep(500);
                //}

                string json;
                using (var read = File.Open(e.FullPath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    StreamReader reader = new StreamReader(read);   
                    json = reader.ReadToEnd();
                    reader.Close();
                    read.Close();
                    read.Dispose();
                }
                  
                // Deserialize cheque's data from json
                WCFService.Cheque cheque = JsonConvert.DeserializeObject<WCFService.Cheque>(json);

                /* Relative path */
                // Get drive of the path
                string drive = Path.GetPathRoot(AppDomain.CurrentDomain.BaseDirectory);
                // Create new path for directory "Complete"
                string pathComplete = Path.Combine(drive, completePathRelative);
                Directory.CreateDirectory(pathComplete);
                string pathRelative = System.IO.Path.Combine(pathComplete, e.Name);
                // Copy file from watched folder to the "Complete" folder
                File.Copy(e.FullPath, pathRelative, true);

                /* Absolute path */
                //if (!Directory.Exists(completePathAbsolute))
                //{
                //    Directory.CreateDirectory(completePathAbsolute);
                //}
                //string path = Path.Combine(completePathAbsolute, e.Name);
                //File.Copy(e.FullPath, path, true);

                // Logging information about the result of calling Save_Cheques() from WCFService
                log.Info("Method Save_Cheques() from WCFService called");
                if (reference.Save_Cheques(cheque))
                    log.Info("Cheque has been saved to database");
                else
                    log.Info("Cheque hasn't been saved to database");   
                File.Delete(e.FullPath);
            }
            catch (Exception ex)
            {
                /* Relative Path */
                string drive = Path.GetPathRoot(AppDomain.CurrentDomain.BaseDirectory);
                string pathGarbage = Path.Combine(drive, garbagePathRelative);
                Directory.CreateDirectory(pathGarbage);
                string pathRelative = System.IO.Path.Combine(pathGarbage, e.Name);
                File.Copy(e.FullPath, pathRelative, true);

                /* Absolute Path */
                //if (!Directory.Exists(garbagePathAbsolute))
                //{
                //    Directory.CreateDirectory(garbagePathAbsolute);
                //}
                //string pathAbsolute = System.IO.Path.Combine(garbagePathAbsolute, e.Name);
                //File.Copy(e.FullPath, pathAbsolute, true);

                File.Delete(e.FullPath);
                log.Error(ex);
            }
        }
    }
}

