using Crytex.Core.AppConfig;

namespace Crytex.Background.Config
{
    public interface IBackgroundConfig : IAppConfig
    {
        int GetSubscriptionVmWaitForPaymentActionPeriod();
        int GetSubscriptionVmWaitForDeletionActionPeriod();
        int GetSubscriptionVmEndWarnPeriod();
        int GetGameServerEndWarnPeriod();
        int GetGameServerWaitForPaymentPeriod();
        int GetSnapshotStoringDaysPeriod();
        int GetWebHostingWaitForPaymentActionPeriod();
        int GetWebHostingWaitForDeletionActionPeriod();
        int GetWebHostingEndWarnPeriod();
    }
}
