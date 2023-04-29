using System;
using Microsoft.AspNetCore.Authorization;

namespace WebApp.Pages.Authorization
{
	public class HRManagerProbationRequirement : IAuthorizationRequirement
	{
		public HRManagerProbationRequirement(int probabtionMonths)
		{
            ProbabtionMonths = probabtionMonths;
        }

        public int ProbabtionMonths { get; }
    }


    public class HRManagerProbationRequirementHandler : AuthorizationHandler<HRManagerProbationRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, HRManagerProbationRequirement requirement)
        {
            if (!context.User.HasClaim(x => x.Type == "EmployementDate"))
                return Task.CompletedTask;

            var empDate = DateTime.Parse(context.User.FindFirst(x => x.Type == "EmployementDate").Value);
            var period = DateTime.Now - empDate;

            if (period.Days > 30 * requirement.ProbabtionMonths)
                context.Succeed(requirement);

            return Task.CompletedTask;

            //throw new NotImplementedException();
        }
    }
}

