namespace TechTrack.MaintenanceService.Background.Processors
{
    public interface IBackgroundProcessor
    {
        public Task ProcessAsync(CancellationToken token);
    }
}
