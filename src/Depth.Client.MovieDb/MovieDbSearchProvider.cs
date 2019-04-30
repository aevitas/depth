using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Security.Authentication;
using System.Threading.Tasks;
using Depth.Client.MovieDb.Abstractions;
using Depth.Client.MovieDb.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Depth.Client.MovieDb
{
    public class MovieDbSearchProvider : IMovieSearchProvider
    {
        private readonly MovieDbOptions _options;
        private readonly HttpClient _httpClient;

        public MovieDbSearchProvider(IOptions<MovieDbOptions> options, HttpClient client)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            _options = options.Value;
            _httpClient = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task<IEnumerable<MovieEntry>> SearchAsync(Action<MovieQueryOptions> options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            var o = new MovieQueryOptions();

            options(o);

            if (!IsValidQuery(o))
                throw new ArgumentException("The specified movie query is not valid; make sure it has at least a query and is not null.");

            var uri = CreateRequestUri(o);
            var response = await _httpClient.GetAsync(uri);

            switch (response)
            {
                case var r when response.IsSuccessStatusCode:
                    var content = await r.Content.ReadAsStringAsync();
                    return DeserializeEntries(content);

                case var _ when response.StatusCode == HttpStatusCode.Unauthorized:
                    throw new AuthenticationException("Could not authenticate to MovieDB with the provided API key!");

                default:
                    return null;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static IEnumerable<MovieEntry> DeserializeEntries(string serialized)
        {
            var page = JsonConvert.DeserializeObject<PaginatedSearchResult>(serialized);

            return page.Results;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private string CreateRequestUri(MovieQueryOptions opts)
        {
            return $"{_options.BaseUri}/search/movie?api_key={_options.ApiKey}&query={WebUtility.UrlEncode(opts.Query)}";
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool IsValidQuery(MovieQueryOptions options)
        {
            if (options == null)
                return false;

            if (string.IsNullOrWhiteSpace(options.Query))
                return false;
            
            return true;
        }
    }
}
