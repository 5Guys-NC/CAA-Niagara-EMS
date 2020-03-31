using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CAA_Event_Management.Models;

namespace CAA_Event_Management.Data.Interface_Repos
{
    public interface IModelAuditLineRepository
    {
        List<ModelAuditLine> GetModelAuditLines();
        ModelAuditLine GetModelAuditLine(string lineID);
        void AddModelAuditLine(ModelAuditLine addNewLine);
    }
}
