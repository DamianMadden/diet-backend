namespace draft_ml.Ingestion.Source
{
    public interface IIngestionSource
    {
        public Task IngestAll(CancellationToken cancel);
        public Task IngestNew(DateTimeOffset lastRun, CancellationToken cancel);

        public string Key();
    }
}
