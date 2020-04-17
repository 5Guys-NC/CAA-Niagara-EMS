using CAA_Event_Management.Models;
using System.Collections.Generic;
/******************************
*  Model Created By: Max Cashmore
*******************************/
namespace CAA_Event_Management.Data
{
    /// <summary>
    /// Interface for the Picture Repository
    /// </summary>
    public interface IPictureRepository
    {
        void AddPicture(Picture pictureToAdd);
        List<Picture> GetPictures();
        Picture GetPicture(int ID);
    }
}
