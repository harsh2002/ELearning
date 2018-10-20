using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ELearning.helper
{
    public class CookieClaims
    {
        private CookieClaims()
        {
        }

        private static readonly object padlock = new object();
        private static CookieClaims instance = null;
        public static CookieClaims Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (padlock)
                    {
                        if (instance == null)
                        {
                            instance = new CookieClaims();
                        }
                    }
                }
                return instance;
            }
        }

        public int UserId(ClaimsPrincipal user)
        {
            var identity = (ClaimsIdentity)user.Identity;

            var userid = identity.Claims.Where(c => c.Type == "userid")
                               .Select(c => c.Value).SingleOrDefault();
            //var auth = user.Identity.IsAuthenticated;

            return Convert.ToInt32(userid);
        }
    }
}
