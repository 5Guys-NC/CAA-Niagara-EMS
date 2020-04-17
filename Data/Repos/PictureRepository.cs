using CAA_Event_Management.Models;
using System.Collections.Generic;
using System.Linq;
/******************************
*  Model Created By: Max Cashmore
*******************************/
namespace CAA_Event_Management.Data
{
    /// <summary>
    /// Picture Repository for CRUD operations of Pictures table
    /// </summary>
    public class PictureRepository : IPictureRepository
    {

        /// <summary>
        /// Add Picture
        /// </summary>
        /// <param name="pictureToAdd"></param>
        public void AddPicture(Picture pictureToAdd)
        {
            using (CAAContext context = new CAAContext())
            {
                context.Pictures.Add(pictureToAdd);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Get all Pictures
        /// </summary>
        /// <returns></returns>
        public List<Picture> GetPictures()
        {
            using (CAAContext context = new CAAContext())
            {
                var items = context.Pictures.OrderByDescending(a => a.CreatedDate).ToList();
                return items;
            }
        }

        /// <summary>
        /// Get Picture by ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public Picture GetPicture(int ID)
        {
            using (CAAContext context = new CAAContext())
            {
                var items = context.Pictures.Where(p => p.ID == ID).FirstOrDefault();
                return items;
            }
        }
    }
}
