using CAA_Event_Management.Data.Interface_Repos;
using CAA_Event_Management.Data.Repos;
using CAA_Event_Management.Models;
using System;
using System.Threading.Tasks;

namespace CAA_Event_Management.Utilities
{
    internal class AuditLog
    {
        internal void WriteAuditLineToDatabase(string userName, string objectTable, string typeID, string newTypeInfo, string changeDate, string changeType, string changeInfo)
        {
            IModelAuditLineRepository modelAuditLineRepository;
            modelAuditLineRepository = new ModelAuditLineRepository();

            try
            {
                var newLine = new ModelAuditLine()
                {
                    ID = Guid.NewGuid().ToString(),
                    AuditorName = userName,
                    ObjectTable = objectTable,
                    ObjectID = typeID,
                    NewObjectInfo = newTypeInfo,
                    DateTimeOfChange = changeDate,
                    TypeOfChange = changeType,
                    ChangedFieldValues = changeInfo
                };

                modelAuditLineRepository.AddModelAuditLine(newLine);
            }
            catch
            {
                Jeeves.ShowMessage("Error", "Failure to update audit log; please contact adminstrator");
            }
        }

        internal async Task WriteToAuditLog(string auditLine)
        {
            string fileName = "CAAAuditLog" + DateTime.Now.Month + DateTime.Now.Year + ".txt";

            //Heavily used: https://docs.microsoft.com/en-us/windows/uwp/files/quickstart-reading-and-writing-files

            try
            {
                Windows.Storage.StorageFolder storageFolder =
                            Windows.Storage.ApplicationData.Current.LocalFolder;

                Windows.Storage.StorageFile sampleFile =
                      await storageFolder.CreateFileAsync(fileName, Windows.Storage.CreationCollisionOption.OpenIfExists);

                auditLine = System.Environment.NewLine + auditLine;
                await Windows.Storage.FileIO.AppendTextAsync(sampleFile, auditLine);
            }
            catch (Exception)
            {
                Jeeves.ShowMessage("Error", "Failure to update audit log; please contact adminstrator");
            }
        }
    }
}
