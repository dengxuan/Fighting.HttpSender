using Hangfire.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Baibaocp.Hangfire
{
    public class AdministratorHangfireDashboardAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            return true;
            //var user = context.GetHttpContext().User;
            //return user.Identity.IsAuthenticated && user.IsInRole("Administrator");
        }
    }
}
