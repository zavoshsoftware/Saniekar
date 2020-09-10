using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAL;
using Models;

namespace Helpers
{
    public static class GetUserInfo
    {
        public static string GetUserFullName()
        {
            DatabaseContext db = new DatabaseContext();

            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {

                var identity = (System.Security.Claims.ClaimsIdentity)HttpContext.Current.User.Identity;

                Models.User user = db.Users.FirstOrDefault(current => current.CellNum == identity.Name);

                if (user != null)
                    return user.FullName;
            }

            return string.Empty;
        }
    }
}