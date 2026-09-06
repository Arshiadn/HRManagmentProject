using HrApi.Enums.Request;
using HrApi.Interfaces;

namespace HrApi.Policies.Request;

public sealed class RequestPolicyResolver
{
    private readonly IReadOnlyDictionary<
        RequestType, IRequestPolicy> _policies;

    public RequestPolicyResolver(
        IEnumerable<IRequestPolicy> policies)
    {
        _policies = policies.ToDictionary(x => x.Type);
    }

    public IRequestPolicy Resolve(RequestType type)
    {
        if(!_policies.TryGetValue(type, out var policy))
        {
            throw new NotSupportedException(
                $"Request type {type} is not supported.");
        }

        return policy;
    }
}
