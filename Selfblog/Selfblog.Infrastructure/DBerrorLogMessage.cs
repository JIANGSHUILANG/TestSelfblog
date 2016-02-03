using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selfblog.Infrastructure
{
    public sealed class DBerrorLogMessage
    {
        public static Queue<ErrorModel> DBExceptionQueue = new Queue<ErrorModel>();
    }
    public class ErrorModel
    {
        public string UID { get; set; }
        public string TypeValue { get; set; }
        public string ControllName { get; set; }
        public DateTime? Time { get; set; }
        public string StackTrace { get; set; }
        public string Message { get; set; }
    }
}
