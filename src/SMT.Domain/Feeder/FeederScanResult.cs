namespace SMT.Domain.Feeder
{
    public enum FeederScanResult
    {
        Ok = 0,
        // Same RCode as the setup, but a different (alternate) part number than the one imported.
        OkAlternate = 1,
        Ng = 2,
    }
}
