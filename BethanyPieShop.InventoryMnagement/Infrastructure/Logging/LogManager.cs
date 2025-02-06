using System.Text;

namespace BethanyPieShop.InventoryManagement.Infrastructure.Logging
{
    public class LogManager
    {
        private static LogManager _instance;
        private FileStream _fileStream;
        private StreamWriter _streamWriter;


        public static LogManager Instance 
        {  
            get {

                if(_instance == null)
                {
                    _instance = new LogManager();
                }

                return _instance; 

            } 
        }

        private LogManager()
        {
            _fileStream = File.OpenWrite(GetExecutionFolder() + "\\Application.log");
            _streamWriter = new StreamWriter(_fileStream);
        }

        private string GetExecutionFolder()
        {
            return Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        }

        public void WriteLog(string message)
        {
            StringBuilder formattedMessage = new StringBuilder();
            formattedMessage.AppendLine("Date: " + DateTime.Now.ToString());
            formattedMessage.AppendLine("Message: " + message);

            _streamWriter.WriteLine(formattedMessage.ToString());
            _streamWriter.Flush();
        }
    }
}
