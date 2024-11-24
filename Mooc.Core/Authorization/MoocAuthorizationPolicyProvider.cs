using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Mooc.Core.DependencyInjection;

namespace Mooc.Core.Authorization;

public class MoocAuthorizationPolicyProvider : DefaultAuthorizationPolicyProvider, IAuthorizationPolicyProvider, ITransientDependency
{
    private readonly AuthorizationOptions _options;

    public MoocAuthorizationPolicyProvider(IOptions<AuthorizationOptions> options) : base(options)
    {
        _options = options.Value;
    }

    public override async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
       
        var policy = await base.GetPolicyAsync(policyName);
        if (policy != null)
        {
            return policy;
        }

        var policyBuilder = new AuthorizationPolicyBuilder(Array.Empty<string>());
        policyBuilder.Requirements.Add(new PermissionRequirement(policyName));
        return policyBuilder.Build();

    }
}
