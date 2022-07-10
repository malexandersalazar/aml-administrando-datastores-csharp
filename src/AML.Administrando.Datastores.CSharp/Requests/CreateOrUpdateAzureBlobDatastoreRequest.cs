namespace AML.Administrando.Datastores.CSharp.Requests
{
    public class CreateOrUpdateAzureBlobDatastoreRequest
    {
        public Properties properties { get; set; }
    }

    public class Properties
    {
        public string description { get; set; }
        public Tags tags { get; set; }
        public object properties { get; set; }
        public Credentials credentials { get; set; }
        public string datastoreType { get; set; }
        public string accountName { get; set; }
        public string containerName { get; set; }
        public string endpoint { get; set; }
        public string protocol { get; set; }
    }

    public class Tags
    {
        public string _string { get; set; }
    }

    public class Credentials
    {
        public string credentialsType { get; set; }
        public Secrets secrets { get; set; }
    }

    public class Secrets
    {
        public string secretsType { get; set; }
        public string key { get; set; }
    }
}