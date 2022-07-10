using AML.Administrando.Datastores.CSharp.Services;

var identityService = new IdentityService();
string bearerToken = await identityService.GetBearerTokenAsync("[tenantId]", "44816fff-d382-4a9e-a34d-f9a113ba9427", "duh8Q~GbUHrhXO3jMF~vYMvSs~32tAjoYD5lgbGd");

var amlService = new AMLService("[subscriptionId]", "rg-ma-aml-learn-001", "mlw-ma-aml-learn-dev");
var workspace = await amlService.GetWorkspaceAsync(bearerToken);

var datastores = await amlService.ListDatastoresAsync(bearerToken);

if (!datastores.Any(x => x.name == "contosoblobstore"))
{
    Console.WriteLine("Registering contosoblobstore datastore..");
    await amlService.CreateOrUpdateDatastoreAsync(bearerToken, "contosoblobstore", "stcontososummaries001", "productorders", "23KZClyUs60ym9R3TIicyWFgcVnTChMYjkO73HLMvqV4k0q+MyBfAc5aCoCcyD7et2KLpUmJ1tsm+AStWAhEEg==");
}
else
    Console.WriteLine("There contosoblobstore datastore is already registered!");

Console.WriteLine("...");