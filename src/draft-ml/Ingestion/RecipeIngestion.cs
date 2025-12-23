using draft_ml.Db;
using draft_ml.Ingestion.Source;
using Microsoft.EntityFrameworkCore;

namespace draft_ml.Ingestion;

public class RecipeIngestion(
    IEnumerable<IIngestionSource> sources,
    DietDbContext dietDb,
    ILogger<RecipeIngestion> logger
) : BackgroundService
{
    public static void ConfigureServices(IServiceCollection services)
    {
        services.AddHostedService<RecipeIngestion>();
    }

    // Run on startup and pull all changes since last pull
    protected async override Task ExecuteAsync(CancellationToken cancel)
    {
        logger.LogInformation("Recipe ingestion starting");

        var operations = new List<Task>();

        // Get lastest run times for each ingestion source
        var latestIngestionRecords = await dietDb
            .IngestionRecords.AsNoTracking()
            .GroupBy(rec => rec.Source)
            .Select(grp => grp.OrderByDescending(rec => rec.Timestamp).FirstOrDefault())
            .ToListAsync();

        foreach (var source in sources)
        {
            var sourceKey = source.Key();
            var latestRecord = latestIngestionRecords
                .Where(rec => rec!.Source == sourceKey)
                .FirstOrDefault();

            if (latestRecord is null)
            {
                operations.Add(source.IngestAll(cancel));
            }
            else
            {
                operations.Add(source.IngestNew(latestRecord.Timestamp, cancel));
            }
        }

        await Task.WhenAll(operations);

        logger.LogInformation("Recipe ingestion finished");
    }
}
