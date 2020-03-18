using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using CAA_Event_Management.Utilities;
using CAA_Event_Management.Models;

namespace CAA_Event_Management.Utilities
{
    internal class AuditLog
    {
        internal async Task<bool> WriteToAuditLog(string auditLine)
        {
            string fileName = "CAAAuditLog" + DateTime.Now.Year + ".txt";

            //Heavily used: https://docs.microsoft.com/en-us/windows/uwp/files/quickstart-reading-and-writing-files

            try
            {
                Windows.Storage.StorageFolder storageFolder =
                            Windows.Storage.ApplicationData.Current.LocalFolder;

                Windows.Storage.StorageFile sampleFile =
                      await storageFolder.CreateFileAsync(fileName, Windows.Storage.CreationCollisionOption.OpenIfExists);

                auditLine = "\n" + auditLine;
                await Windows.Storage.FileIO.AppendTextAsync(sampleFile, auditLine);
            }
            catch (Exception)
            {
                Jeeves.ShowMessage("Error", "Failure to update audit log; please contact adminstrator");
                return false;
            }
            return true;
        }
    }
}
