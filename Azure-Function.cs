#r "Newtonsoft.Json"

using System.Net;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System.Threading.Tasks;

// See: https://github.com/AdamPaternostro/GitHub-Azure-Function-Proxy-for-ARM-Template

public static async Task<IActionResult> Run(Microsoft.AspNetCore.Http.HttpRequest req, ILogger log)
{
    try
    {
        log.LogInformation("BEGIN: GitHub Proxy Request");
        
        string personalAcccessToken = string.Empty;
        if (Environment.GetEnvironmentVariable("GitHubAccessToken") != null)
        {
            personalAcccessToken = Environment.GetEnvironmentVariable("GitHubAccessToken");
        } else
        {
            throw new Exception("Environment Variable is missing");
        }
        
        
        string responseBody = "";

        string location = req.Query["location"];
        log.LogInformation("location: " + location);

        using (HttpClient client = new HttpClient())
        {
            client.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                Convert.ToBase64String(
                    System.Text.ASCIIEncoding.ASCII.GetBytes(
                        string.Format("{0}:{1}", "", personalAcccessToken))));

            using (HttpResponseMessage response = client.GetAsync(location).Result)
            {
                response.EnsureSuccessStatusCode();
                responseBody = await response.Content.ReadAsStringAsync();
                // log.LogInformation(responseBody);

                return responseBody != null
                    ? (ActionResult)new OkObjectResult(responseBody)
                    : new BadRequestObjectResult("No file returned from GitHub");


            }
        }
    }
    catch (Exception ex)
    {
        log.LogInformation(ex.ToString());
        return new BadRequestObjectResult("An Error occurred in the Azure Function");
    }
    finally
    {
        log.LogInformation("END: GitHub Proxy Request");
    }
}

