namespace Crytex.Model.Enums
{
    public enum EmailTemplateType
    {
        Registration = 0,
        ChangePassword = 1,
        ChangeProfile = 2,
        CreateVm = 3,
        UpdateVm = 4,
        SubscriptionNeedsPayment = 5,
        SubscriptionEndWarning = 6,
        SubscriptionDeletionWarning = 7,
        CreateVmCredentials = 8,
        GameServerNeedsPayment = 9,
        GameServerEndWarning = 10,
        GameServerDeletionWarning = 11,
        ResetPassword = 12,
        WebHostingWasDisabled = 13,
        WebHostingEndWaring = 14,
        WebHostingDeletionWarning = 15,
        PhysicalServerNeedsPayment = 16,
        PhysicalServerEndWarning = 17,
        PhysicalServerDeletionWarning = 18,
        PhysicalServerCreated = 19,
        PhysicalServerReady = 20,
        PhysicalServerDontCreate = 21
    }
}