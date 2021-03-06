﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace ApiService
{
    public class ApiServiceRest
    {
        public async Task<Resource> GetListRest<T>(string urlBase, string controller)
        {
            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                var response = await client.GetAsync(controller);
                var resultAsync = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    return new Resource
                    {
                        IsSuccesful = response.IsSuccessStatusCode,
                        Message = $"{response.ReasonPhrase} {response.RequestMessage}",
                        Result = new List<T>() 
                    };                    
                }

                var listAsync = JsonConvert.DeserializeObject<List<T>>(resultAsync);

                return new Resource
                {
                    IsSuccesful = response.IsSuccessStatusCode,
                    Result = listAsync,
                    Message = $"{response.ReasonPhrase} {response.RequestMessage}"
                };
            }
            catch (Exception e)
            {
                return new Resource
                {
                    Message = $"Error: {e.Message}",
                    IsSuccesful = false,
                    Result = new List<T>()
                };
            }

        }
    }
}
