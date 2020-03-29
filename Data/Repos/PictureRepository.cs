﻿using CAA_Event_Management.Models;
using System.Collections.Generic;
using System.Linq;

namespace CAA_Event_Management.Data
{
    public class PictureRepository : IPictureRepository
    {

        public void AddPicture(Picture pictureToAdd)
        {
            using (CAAContext context = new CAAContext())
            {
                context.Pictures.Add(pictureToAdd);
                context.SaveChanges();
            }
        }

        public List<Picture> GetPictures()
        {
            using (CAAContext context = new CAAContext())
            {
                var items = context.Pictures.OrderByDescending(a => a.CreatedDate).ToList();
                return items;
            }
        }

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
