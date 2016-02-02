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
        PhysicalServerNeedsPayment = 13,
        PhysicalServerEndWarning = 14,
        PhysicalServerDeletionWarning = 15,
        PhysicalServerCreated = 16,
        PhysicalServerReady = 17,
        PhysicalServerDontCreate = 18
    }
}