using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace ImageCompressionResearchApp.Servicies
{
    public static class LogService
    {
        private const string source = "Application";
        private const string log = "ErrorLog";


        public static void WriteLog(EventLogEntryType level, string componentCode, string eventSourceCode, 
            string eventId, string component, string errorDescription)
        {
            string logCode = GenerateLogCode(level, componentCode, eventSourceCode, eventId);
            try
            {
                if (!EventLog.SourceExists(source))
                {
                    EventLog.CreateEventSource(source, log);
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string logMessage = $"Code: {logCode}\n" +
                                $"Level: {level}\n" +
                                $"Component: {component}\n" +
                                $"Source: {source}\n" +
                                $"Description: {errorDescription}\n" +
                                $"Date and time: {DateTime.Now}";
            try
            {
                EventLog.WriteEntry(source, logMessage, level);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private static string GenerateLogCode(EventLogEntryType level, string componentCode, string eventSourceCode, string eventId)
        {
            string logLevelCode = GetLogLevelCode(level);
            return $"{logLevelCode}{componentCode}{eventSourceCode}{eventId}";
        }


        private static string GetLogLevelCode(EventLogEntryType logLevel)
        {
            return logLevel switch
            {
                EventLogEntryType.Information => "10",  
                EventLogEntryType.Warning => "20",      
                EventLogEntryType.Error => "30",        
                EventLogEntryType.FailureAudit => "40", 
                EventLogEntryType.SuccessAudit => "50", 
                _ => "00",                              
            };
        }
    }
}
