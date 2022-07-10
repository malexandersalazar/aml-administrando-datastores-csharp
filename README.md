# Administrando Datastores de AML con C#

El Datastore representa una abstracción de almacenamiento sobre una cuenta de almacenamiento de Azure Machine Learning.

Administrarlos desde la interfaz de Azure Machine Learning Studio e incluso con el SDK de Python, que tiene múltiples ejemplos disponibles, es muy sencillo; como en la documentación oficial de Microsoft no hay ejemplos para C#, en esta ocasión lo resolveremos consumiendo la REST API de Azure Machine Learning con nuestro amado lenguaje a través de un flujo de autenticación del tipo **client_credentials**.

## Recuperar un token de autenticación de service principal

Empezamos registramos nuestra aplicación cliente en la sección de **App registrations** de nuestro **Azure Active Directory**.

Luego, con la nueva aplicación registrada, generamos un nuevo **Client secret** y seguido de ello ingresamos al **Access control (IAM)** del recurso **Azure Machine Learning Workspace** para asignar a nuestra aplicación como **service principal** con rol de **Contributor**.

Con esto establecido ya podemos recuperar nuestro token de acceso a través del siguiente código en C#:

<pre><code>var formContent = new Dictionary<string, string>();
formContent.Add("grant_type", "client_credentials");
formContent.Add("resource", "https://management.azure.com/");
formContent.Add("client_id", clientId);
formContent.Add("client_secret", clientSecret);

var formUrlEncodedContent = new FormUrlEncodedContent(formContent);

var httpResponseMessage
    = await _httpClient.PostAsync($"https://login.microsoftonline.com/{tenantId}/oauth2/token", formUrlEncodedContent);

if (httpResponseMessage.IsSuccessStatusCode)
{
    var responseContent = await httpResponseMessage.Content.ReadAsStringAsync();
    Console.WriteLine("Token:");
    Console.WriteLine(responseContent);
    Console.WriteLine();
    var token = JsonSerializer.Deserialize<TokenResponse>(responseContent);
    return token.access_token;
}
else
    throw new(httpResponseMessage.ReasonPhrase);</code></pre>

## Listar Datastores

<pre><code>httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);

var httpResponseMessage = await httpClient.GetAsync($"https://management.azure.com/subscriptions/{subscriptionId}/resourceGroups/{resourceGroupName}/providers/Microsoft.MachineLearningServices/workspaces/{workspaceName}/datastores?api-version=2022-05-01");
if (httpResponseMessage.IsSuccessStatusCode)
{
    var responseContent = await httpResponseMessage.Content.ReadAsStringAsync();

    var listDatastoreResponse = JsonSerializer.Deserialize<ListDatastoreResponse>(responseContent);

    Console.WriteLine("Datastores:");
    foreach (var item in listDatastoreResponse.value)
        Console.WriteLine($"{item.name} ({item.properties.datastoreType})");
    Console.WriteLine();

    return listDatastoreResponse.value.ToList();
}
else throw new(httpResponseMessage.ReasonPhrase);</code></pre>

## Registrar Datastores    

Los siguientes son ejemplos de servicios de almacenamiento compatibles que se pueden registrar como Datastore en un Azure Machine Learning Workspace:

* **Azure Blob Storage**
* **Azure File Share**
* **Azure Data Lake**
* **Azure Data Lake Gen2**
* Azure SQL Database
* Azure Database for PostgreSQL
* Azure Database for MySQL

Sin embargo, solo Azure Blob, Azure File, Azure Data Lake Gen1 y Azure Data Lake Gen2 pueden registrarse a través de la REST API de Azure Machine Learning.

Cada servicio de almacenamiento requiere un conjunto de propiedades en particular para ser registrado exitosamente. Por ejemplo, el código para registrar un Azure Blob Storage requiere indicar el nombre del contenedor:

<pre><code>httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);

var request = new CreateOrUpdateAzureBlobDatastoreRequest
{
    properties = new Requests.Properties
    {
        accountName = accountName,
        containerName = containerName,
        credentials = new Credentials
        {
            credentialsType = "AccountKey",
            secrets = new Secrets
            {
                key = accountKey,
                secretsType = "AccountKey"
            }
        },
        datastoreType = "AzureBlob"
    }
};

var json = JsonSerializer.Serialize(request);
var stringContent = new StringContent(json, Encoding.UTF8, "application/json");

var httpResponseMessage = await httpClient.PutAsync($"https://management.azure.com/subscriptions/{subscriptionId}/resourceGroups/{resourceGroupName}/providers/Microsoft.MachineLearningServices/workspaces/{workspaceName}/datastores/{name}?api-version=2022-05-01", stringContent);

if (httpResponseMessage.IsSuccessStatusCode)
{
    if (httpResponseMessage.StatusCode == HttpStatusCode.Created)
        Console.WriteLine("The datastore was created successfully!");
    else
        Console.WriteLine("The datastore was updated successfully!");

    var responseContent = await httpResponseMessage.Content.ReadAsStringAsync();
    Console.WriteLine("Upsert Response:");
    Console.WriteLine(responseContent);

    return true;
}
else throw new(httpResponseMessage.ReasonPhrase);
</code></pre>

Para mayor información de sobre cómo registrar los otros tipos de servicios de almacenamiento de Azure como Datastore revisar la referencia de Azure Machine Learning REST API [[4]].

## Referencias

1. Create, run, and delete Azure ML resources using REST. https://docs.microsoft.com/en-us/azure/machine-learning/how-to-manage-rest

2. Datastores - List. https://docs.microsoft.com/en-us/rest/api/azureml/datastores/list

3. Datastores - Create Or Update. https://docs.microsoft.com/en-us/rest/api/azureml/datastores/create-or-update

4. Machine Learning REST API reference. https://docs.microsoft.com/en-us/rest/api/azureml/

[4]: https://www.datosabiertos.gob.pe/dataset/casos-positivos-por-covid-19-ministerio-de-salud-minsa