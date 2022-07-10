using AML.Administrando.Datastores.CSharp.Requests;
using AML.Administrando.Datastores.CSharp.Responses;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace AML.Administrando.Datastores.CSharp.Services
{
    internal class AMLService
    {
        private static HttpClient httpClient = new HttpClient();

        private readonly string subscriptionId;
        private readonly string resourceGroupName;
        private readonly string workspaceName;

        public AMLService(string subscriptionId, string resourceGroupName, string workspaceName)
        {
            this.subscriptionId = subscriptionId;
            this.resourceGroupName = resourceGroupName;
            this.workspaceName = workspaceName;
        }

        public async Task<WorkspaceResponse> GetWorkspaceAsync(string bearerToken)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);

            var httpResponseMessage = await httpClient.GetAsync($"https://management.azure.com/subscriptions/{subscriptionId}/resourceGroups/{resourceGroupName}/providers/Microsoft.MachineLearningServices/workspaces/{workspaceName}?api-version=2022-05-01");
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var responseContent = await httpResponseMessage.Content.ReadAsStringAsync();
                Console.WriteLine("Workspace:");
                Console.WriteLine(responseContent);
                Console.WriteLine();

                var workspace = JsonSerializer.Deserialize<WorkspaceResponse>(responseContent);
                return workspace;
            }
            else throw new(httpResponseMessage.ReasonPhrase);
        }

        public async Task<List<Datastore>> ListDatastoresAsync(string bearerToken)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);

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
            else throw new(httpResponseMessage.ReasonPhrase);
        }

        public async Task<bool> CreateOrUpdateDatastoreAsync(string bearerToken, string name, string accountName, string containerName, string accountKey)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);

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
        }
    }
}