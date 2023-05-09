// Copyright (c) Jan Philipp Luehrig. All rights reserved.
// These files are licensed to you under the MIT license.

using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace FireTruckApi.HealthChecks;

public class ApiHealthCheck : IHealthCheck, IHealthProvidingService
{
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        HealthStatus lowest = HealthStatus.Healthy;

        foreach (var healthStatus in _healthStatus.Values)
        {
            if (healthStatus < lowest)
            {
                lowest = healthStatus;
            }
        }

        string description = "";
        switch (lowest)
        {
            case HealthStatus.Healthy:
                description = "All services are working properly";
                break;
            case HealthStatus.Degraded:
                description = "At least one service is degraded, system is working, but the experience is limited";
                break;
            case HealthStatus.Unhealthy:
                description = "At least one service is not working properly, system is not working";
                break;
        }

        return Task.FromResult(new HealthCheckResult(lowest, description));

    }

    private readonly IDictionary<string, HealthStatus> _healthStatus = new Dictionary<string, HealthStatus>();

    public void SetStatusOfService(string serviceName, HealthStatus status)
    {
        if (!_healthStatus.ContainsKey(serviceName))
        {
            _healthStatus.Add(serviceName, status);
        }
        else
        {
            _healthStatus[serviceName] = status;
        }
    }
}
