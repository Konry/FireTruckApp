// Copyright (c) Jan Philipp Luehrig. All rights reserved.
// These files are licensed to you under the MIT license.

using Microsoft.Extensions.Diagnostics.HealthChecks;

public class ApiHealthCheck : IHealthCheck
{
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        bool isHealthy = true;

        if (isHealthy)
        {
            return Task.FromResult(HealthCheckResult.Healthy("A healthy result."));
        }

        return Task.FromResult(new HealthCheckResult(context.Registration.FailureStatus, "An unhealthy result."));
    }
}
