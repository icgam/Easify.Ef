using System.Diagnostics.CodeAnalysis;
using System.Security.Principal;
using Easify.Ef.Extensions;
using ICG.Core.Http;

namespace Easify.Ef.Testing.Extensions
{
    [ExcludeFromCodeCoverage]
    public sealed class FakeOperationContext : IOperationContext
    {
        public FakeOperationContext()
        {
            User = new GenericPrincipal(new GenericIdentity(PrincipalExtensions.AnonymousUser), new string[] { });
        }

        public IPrincipal User { get; }
    }
}