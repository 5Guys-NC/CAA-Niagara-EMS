using CAA_Event_Management.Models;
using System.Collections.Generic;
/*************************
 * Created By: Jon Yade
 * **********************/
namespace CAA_Event_Management.Data.Interface_Repos
{
    /// <summary>
    /// Interface for the ModelAuditLine Repository
    /// </summary>
    public interface IModelAuditLineRepository
    {
        List<ModelAuditLine> GetModelAuditLines();
        ModelAuditLine GetModelAuditLine(string lineID);
        void AddModelAuditLine(ModelAuditLine addNewLine);
    }
}
