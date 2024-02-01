using System.Collections.Generic;
using System;
using System.Collections.Concurrent;
using Rubix.API.Shared.Interfaces;
using System.Threading.Tasks;

namespace Rubix.Explorer.API
{
    public class UrlShortener
    {
        private readonly IRepositoryShortURL _repositoryShortURL;
        public UrlShortener(IRepositoryShortURL repositoryShortURL) {
            _repositoryShortURL=repositoryShortURL;
        }

        private const string baseUrl = "https://api.rubix.network/Short?url=";

        public async Task<string> ShortenUrl(string longUrl)
        {
            string shortCode = GenerateShortCode();
            string shortUrl = baseUrl + shortCode;

            // Store the mapping between short and long URLs
            await _repositoryShortURL.UpsertAsync(new Rubix.API.Shared.Entities.ShortUrl(shortCode, longUrl));

            return shortUrl;
        }

        public async Task<string> ExpandUrl(string shortUrl)
        {
            if(!string.IsNullOrEmpty(shortUrl))
            {
                // Extract the short code from the short URL
                string shortCode = shortUrl.Replace(baseUrl, "");

                 var dataInfo= await _repositoryShortURL.FindByShortCodeAsync(shortCode);
                // Look up the long URL from the mapping
                if (dataInfo!=null)
                {
                    return dataInfo.URL;
                }
                else
                {
                    return "URL not found";
                }
            }
            return "https://rubix.net";
            
        }

        private string GenerateShortCode()
        {
            // This is a simple example; you might want to use a more complex algorithm
            // You could use a combination of timestamp, random characters, or hash functions
            return Guid.NewGuid().ToString().Substring(0, 6);
        }
    }

}
