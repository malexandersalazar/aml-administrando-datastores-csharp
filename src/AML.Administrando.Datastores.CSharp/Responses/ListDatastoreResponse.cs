namespace AML.Administrando.Datastores.CSharp.Responses
{
    public class ListDatastoreResponse
    {
        public Datastore[] value { get; set; }
    }

    public class Datastore
    {
        public string name { get; set; }
        public DatastoreProperty properties { get; set; }
    }

    public class DatastoreProperty
    {
        public string datastoreType { get; set; }
    }
}