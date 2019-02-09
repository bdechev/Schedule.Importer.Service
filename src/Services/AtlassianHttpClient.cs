using Dtos.Atlassian;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using Schedule.Importer.Service.Configuration.Abstract;
using Services.Abstract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class AtlassianHttpClient : IAtlassianHttpClient
    {
        private readonly HttpClient _httpClient;
        private readonly IAtlassianConfig _atlassianConfig;
        private readonly IHostingEnvironment _hostingEnvironment;

        public AtlassianHttpClient(IAtlassianConfig atlassianConfig, IHostingEnvironment hostingEnvironment)
        {
            _atlassianConfig = atlassianConfig;
            _hostingEnvironment = hostingEnvironment;
            _httpClient = new HttpClient();

            // Setup Basic Authorization for the http client instance
            var byteArray = Encoding.ASCII.GetBytes($"{atlassianConfig.Username}:{atlassianConfig.ApiToken}");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
        }

        public async Task GetAttachments()
        {
            var pageId = "1148387756";

            var response = await _httpClient.GetAsync($"{_atlassianConfig.Host}/wiki/rest/api/content/{pageId}/child/attachment");

            if (response.IsSuccessStatusCode)
            {
                // ... Read the string.
                string result = await response.Content.ReadAsStringAsync();

                try
                {
                    var attachments = JsonConvert.DeserializeObject<Attachments>(result);
                    var downloadsPath = $"{_hostingEnvironment.ContentRootPath}/../../downloads/";
                    var directoryPath = $"Downloads_{DateTime.Today.Day}{DateTime.Today.Month}{DateTime.Today.Year}";

                    if (!Directory.Exists(downloadsPath + directoryPath))
                    {
                        Directory.CreateDirectory(downloadsPath + directoryPath);
                    }

                    foreach (var attachment in attachments.Results)
                    {
                        var attachmentResponse = await _httpClient.GetAsync($"{_atlassianConfig.Host}{attachments._Links.Context}{attachment._Links.Download}");

                        if (attachmentResponse.IsSuccessStatusCode)
                        {
                            var contentStream = await attachmentResponse.Content.ReadAsStreamAsync(); // get the actual content stream

                            using (Stream stream = new FileStream($"{downloadsPath}{directoryPath}/{attachment.Title}", FileMode.Create, FileAccess.Write, FileShare.None, (int)contentStream.Length, true))
                            {
                                await contentStream.CopyToAsync(stream);
                            }
                        }
                    }
                }
                catch(Exception ex)
                {
                }
            }
        }
    }
}
