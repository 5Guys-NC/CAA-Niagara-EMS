using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CAA_Event_Management.Models;
using CAA_Event_Management.Data.Interface_Repos;

namespace CAA_Event_Management.Data.Repos
{
    public class ModelAuditLineRepository : IModelAuditLineRepository
    {
        /// <summary>
        /// Get all ModelAuditLines
        /// </summary>
        /// <returns>List of ModelAuditLines</returns>
        public List<ModelAuditLine> GetModelAuditLines()
        {
            using (CAAContext context = new CAAContext())
            {
                var auditLines = context.ModelAuditLines
                    .ToList();
                return auditLines;
            }
        }

        /// <summary>
        /// Get ModelAuditLine by ID
        /// </summary>
        /// <param name="lineID"></param>
        /// <returns>A Single AuditLine</returns>
        public ModelAuditLine GetModelAuditLine(string lineID)
        {
            using (CAAContext context = new CAAContext())
            {
                var oneAuditLine = context.ModelAuditLines
                    .Where(d => d.ID == lineID)
                    .FirstOrDefault();
                return oneAuditLine;
            }
        }

        /// <summary>
        /// Add a new Audit Line
        /// </summary>
        /// <param name="addNewLine"></param>
        public void AddModelAuditLine(ModelAuditLine addNewLine)
        {
            using (CAAContext context = new CAAContext())
            {
                context.ModelAuditLines.Add(addNewLine);
                context.SaveChanges();
            }
        }
    }
}
