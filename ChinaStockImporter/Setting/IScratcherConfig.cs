namespace Yootech.ChinaStockImporter.Setting
{
    public interface IScratcherConfig
    {
        int GetScratchIntervalSecond();

        string GetScratchPage();

        string GetScratchDataPage();
    }
}