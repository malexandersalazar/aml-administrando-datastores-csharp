namespace AML.Administrando.Datastores.CSharp.Responses
{
    internal class WorkspaceResponse
    {
        public string id { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public string location { get; set; }
        public Tags tags { get; set; }
        public object etag { get; set; }
        public Properties properties { get; set; }
        public Identity identity { get; set; }
        public Sku sku { get; set; }
        public Systemdata systemData { get; set; }
    }

    public class Tags
    {
    }

    public class Properties
    {
        public string friendlyName { get; set; }
        public string description { get; set; }
        public string storageAccount { get; set; }
        public string keyVault { get; set; }
        public string applicationInsights { get; set; }
        public bool hbiWorkspace { get; set; }
        public string tenantId { get; set; }
        public object imageBuildCompute { get; set; }
        public string provisioningState { get; set; }
        public bool v1LegacyMode { get; set; }
        public bool softDeleteEnabled { get; set; }
        public object containerRegistry { get; set; }
        public Notebookinfo notebookInfo { get; set; }
        public bool storageHnsEnabled { get; set; }
        public string workspaceId { get; set; }
        public object linkedModelInventoryArmId { get; set; }
        public int privateLinkCount { get; set; }
        public string publicNetworkAccess { get; set; }
        public string discoveryUrl { get; set; }
        public string mlFlowTrackingUri { get; set; }
        public string sdkTelemetryAppInsightsKey { get; set; }
    }

    public class Notebookinfo
    {
        public string resourceId { get; set; }
        public string fqdn { get; set; }
        public bool isPrivateLinkEnabled { get; set; }
        public object notebookPreparationError { get; set; }
    }

    public class Identity
    {
        public string type { get; set; }
        public string principalId { get; set; }
        public string tenantId { get; set; }
    }

    public class Sku
    {
        public string name { get; set; }
        public string tier { get; set; }
    }

    public class Systemdata
    {
        public DateTime createdAt { get; set; }
        public string createdBy { get; set; }
        public string createdByType { get; set; }
        public DateTime lastModifiedAt { get; set; }
        public string lastModifiedBy { get; set; }
        public string lastModifiedByType { get; set; }
    }
}