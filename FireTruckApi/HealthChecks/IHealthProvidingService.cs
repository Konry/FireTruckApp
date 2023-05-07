// Copyright (c) Jan Philipp Luehrig. All rights reserved.
// These files are licensed to you under the MIT license.

using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace FireTruckApi.HealthChecks;

public interface IHealthProvidingService
{
    public void SetStatusOfService(string serviceName, HealthStatus status);
}
