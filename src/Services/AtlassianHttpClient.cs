using Dtos.Atlassian;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using Schedule.Importer.Service.Configuration.Abstract;
using Services.Abstract;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class AtlassianHttpClient : IAtlassianHttpClient
    {
        private readonly HttpClient httpClient;
        private readonly IAtlassianConfig atlassianConfig;
        private readonly IHostingEnvironment hostingEnvironment;

        public AtlassianHttpClient(IAtlassianConfig atlassianConfig, IHostingEnvironment hostingEnvironment)
        {
            this.atlassianConfig = atlassianConfig;
            this.hostingEnvironment = hostingEnvironment;
            httpClient = new HttpClient();

            // Setup Basic Authorization for the http client instance
            var byteArray = Encoding.ASCII.GetBytes($"{atlassianConfig.Username}:{atlassianConfig.ApiToken}");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
        }

        public async Task GetAttachments(string pageId)
        {
            var response = await httpClient.GetAsync($"{atlassianConfig.Host}/wiki/rest/api/content/{pageId}/child/attachment");

            if (response.IsSuccessStatusCode)
            {
                // ... Read the string.
                string result = await response.Content.ReadAsStringAsync();

                try
                {
                    var attachments = JsonConvert.DeserializeObject<Attachments>(result);
                    var downloadsPath = $"{hostingEnvironment.ContentRootPath}/../../downloads/";

                    foreach (var attachment in attachments.Results)
                    {
                        var attachmentResponse = await httpClient.GetAsync($"{atlassianConfig.Host}{attachments._Links.Context}{attachment._Links.Download}");

                        if (attachmentResponse.IsSuccessStatusCode)
                        {
                            var contentStream = await attachmentResponse.Content.ReadAsStreamAsync(); // get the actual content stream

                            using (Stream stream = new FileStream($"{downloadsPath}/{attachment.Title}", FileMode.Create, FileAccess.Write, FileShare.None, (int)contentStream.Length, true))
                            {
                                await contentStream.CopyToAsync(stream);
                            }

                            contentStream.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                }
            }
        }
    }
}
