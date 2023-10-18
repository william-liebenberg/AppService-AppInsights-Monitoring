using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;

namespace AppService.AppInsights.Monitoring;

public class RootRoute404Filter : ITelemetryProcessor
{
    private readonly PathString _rootPath = PathString.FromUriComponent("/");

    private readonly ITelemetryProcessor next;

    public RootRoute404Filter(ITelemetryProcessor next)
    {
        this.next = next;
    }

    public void Process(ITelemetry item)
    {
        // Check if the item is a request telemetry item
        if (item is RequestTelemetry requestTelemetry)
        {
            // Check if the request is to the root route and resulted in a 401 status code
            if (requestTelemetry.Url.AbsolutePath.Equals(_rootPath) &&
                requestTelemetry.ResponseCode == "404") // HttpStatusCode.NotFound = 404 ... not "404"
            {
                // Do not pass the telemetry item to the next processor
                return;
            }
        }

        // Pass the telemetry item to the next processor
        this.next.Process(item);
    }
}
