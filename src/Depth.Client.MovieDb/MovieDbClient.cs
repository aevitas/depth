﻿using System;
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
    public class MovieDbClient : IMovieSearchProvider, IMovieDetailProvider
    {
        private readonly MovieDbOptions _options;
        private readonly HttpClient _httpClient;

        public MovieDbClient(IOptions<MovieDbOptions> options, HttpClient client)
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

            var uri = $"{_options.BaseUri}/search/movie?api_key={_options.ApiKey}&query={WebUtility.UrlEncode(o.Query)}";
            var response = await _httpClient.GetAsync(uri);

            switch (response)
            {
                case var r when response.IsSuccessStatusCode:
                    var content = await r.Content.ReadAsStringAsync();
                    var page = JsonConvert.DeserializeObject<PaginatedSearchResult>(content);

                    return page.Results;
                case var _ when response.StatusCode == HttpStatusCode.Unauthorized:
                    throw new AuthenticationException("Could not authenticate to MovieDB with the provided API key!");

                default:
                    return null;
            }
        }

        public async Task<MovieDetail> GetDetailAsync(int id)
        {
            var uri = $"{_options.BaseUri}/movie/{id}?api_key={_options.ApiKey}";
            var response = await _httpClient.GetAsync(uri);

            switch (response)
            {
                case var r when response.IsSuccessStatusCode:
                    var content = await r.Content.ReadAsStringAsync();

                    return JsonConvert.DeserializeObject<MovieDetail>(content);

                case var _ when response.StatusCode == HttpStatusCode.Unauthorized:
                    throw new AuthenticationException("Could not authenticate to MovieDB with the provided API key!");

                default:
                    return null;
            }
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
