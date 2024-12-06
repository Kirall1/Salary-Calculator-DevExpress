using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Security.Cryptography;
using XLabFileReader.Models;
using XLabFileReader.Services.Interfaces;

namespace XLabFileReader.Services
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly string _baseUrl = "https://testapi.x-lab.by";
        private readonly string _appGuid = "3A45151D-3AC6-4B83-8D38-39B83B03C677";
        public static Token Token { get; set; }

        public async Task AuthorizeAsync()
        {
            try
            {
                using var httpClient = new HttpClient { BaseAddress = new Uri(_baseUrl) };

                var hashedPassword = ComputeSha256Hash("12345");
                var requestBody = new Dictionary<string, string>
                {
                    { "grant_type", "password" },
                    { "username", "ато" },
                    { "password", "3380ba0b1615b8fcaa0da03a9f1b0101230689d2f1c54541d0d214afad9b6d8f" },
                    { "appGuid", _appGuid }
                };

                var response = await httpClient.PostAsync("/token", new FormUrlEncodedContent(requestBody));

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    Token = JsonSerializer.Deserialize<Token>(jsonResponse);

                }
                else
                {
                    throw new Exception("Authentication failed");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private string ComputeSha256Hash(string rawData)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(rawData));
            return BitConverter.ToString(bytes).Replace("-", string.Empty).ToLowerInvariant();
        }
    }
}
