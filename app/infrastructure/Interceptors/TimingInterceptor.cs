using Castle.DynamicProxy;
using SignalBox.Core;

namespace SignalBox.Infrastructure
{
    public class TimingInterceptor : IInterceptor
    {
        private readonly ITelemetry telemetry;

        public TimingInterceptor(ITelemetry telemetry)
        {
            this.telemetry = telemetry;
        }

        public void Intercept(IInvocation invocation)
        {
            var stopwatch = telemetry.NewStopwatch();
            try
            {
                invocation.Proceed();
            }
            finally
            {
                stopwatch.Stop();
                var s = invocation.TargetType.Name + '.' + invocation.Method.Name;
                telemetry.TrackTimingMetric(s, stopwatch.Elapsed, s);
            }
        }
    }
}
