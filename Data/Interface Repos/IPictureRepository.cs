using CAA_Event_Management.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/******************************
*  Model Created By: Max Cashmore
*******************************/
namespace CAA_Event_Management.Data
{
    public interface IPictureRepository
    {
        void AddPicture(Picture pictureToAdd);
        List<Picture> GetPictures();
        Picture GetPicture(int ID);
    }
}
