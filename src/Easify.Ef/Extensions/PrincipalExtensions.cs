using System.Security.Principal;

namespace Easify.Ef.Extensions
{
    public static class PrincipalExtensions
    {
        // TODO: Need to be sourced from somewhere else
        public const string AnonymousUser = "Anonymous";

        public static string GetUserName(this IPrincipal principal)
        {
            var identity = principal?.Identity;
            if (identity == null)
                return AnonymousUser;

            return identity.IsAuthenticated == false ? AnonymousUser : identity.Name;
        }
    }
}