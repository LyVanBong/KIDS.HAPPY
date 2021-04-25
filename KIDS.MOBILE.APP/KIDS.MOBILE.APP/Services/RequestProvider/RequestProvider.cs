using KIDS.MOBILE.APP.Configurations;
using KIDS.MOBILE.APP.Models.RequestProvider;
using KIDS.MOBILE.APP.Models.Response;
using Microsoft.AppCenter.Crashes;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace KIDS.MOBILE.APP.Services.RequestProvider
{
    public class RequestProvider : IRequestProvider
    {
        private IRestClient _client;
        private IRestRequest _request;

        public RequestProvider()
        {
            _client = new RestClient();
        }

        public async Task<ResponseModel<T>> GetAsync<T>(string uri, IReadOnlyCollection<RequestParameter> parameters = null)
        {
            try
            {
                CreateClients(uri);
                if (parameters != null && parameters.Any())
                {
                    foreach (var item in parameters)
                    {
                        _request.AddQueryParameter(item.Key, item.Value);
                    }
                }

                var response = await _client.ExecuteAsync<ResponseModel<T>>(_request);
                var data = response.StatusCode == HttpStatusCode.OK
                    ? JsonConvert.DeserializeObject<ResponseModel<T>>(response.Content)
                    : default;
                return data;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private void CreateClients(string uri, Method method = Method.GET)
        {
            try
            {
                uri = AppConstants.UrlApiApp + uri;
                _client = new RestClient(new Uri(uri));
                _request = new RestRequest(method);
            }
            catch (Exception)
            {
            }
        }

        public async Task<ResponseModel<T>> PostAsync<T>(string uri, IReadOnlyCollection<RequestParameter> parameters = null)
        {
            try
            {
                CreateClients(uri, Method.POST);
                CreateParameter(parameters);
                var response = await _client.ExecuteAsync<ResponseModel<T>>(_request);
                var data = response.StatusCode == HttpStatusCode.OK
                    ? JsonConvert.DeserializeObject<ResponseModel<T>>(response.Content)
                    : default;
                return data;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private void CreateParameter(IReadOnlyCollection<RequestParameter> parameters)
        {
            try
            {
                if (parameters != null && parameters.Any())
                {
                    foreach (var item in parameters)
                    {
                        _request.AddParameter(item.Key, item.Value);
                    }
                }
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
            }
        }

        public async Task<ResponseModel<T>> PutAsync<T>(string uri, IReadOnlyCollection<RequestParameter> parameters = null)
        {
            try
            {
                CreateClients(uri, Method.PUT);
                if (parameters != null && parameters.Any())
                {
                    foreach (var item in parameters)
                    {
                        _request.AddQueryParameter(item.Key, item.Value);
                    }
                }

                var response = await _client.ExecuteAsync<ResponseModel<T>>(_request);
                var data = response.StatusCode == HttpStatusCode.OK
                    ? JsonConvert.DeserializeObject<ResponseModel<T>>(response.Content)
                    : default;
                return data;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<ResponseModel<T>> DeleteAsync<T>(string uri, IReadOnlyCollection<RequestParameter> parameters = null)
        {
            try
            {
                CreateClients(uri, Method.DELETE);
                if (parameters != null && parameters.Any())
                {
                    foreach (var item in parameters)
                    {
                        _request.AddQueryParameter(item.Key, item.Value);
                    }
                }

                var response = await _client.ExecuteAsync<ResponseModel<T>>(_request);
                var data = response.StatusCode == HttpStatusCode.OK
                    ? JsonConvert.DeserializeObject<ResponseModel<T>>(response.Content)
                    : default;
                return data;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<ResponseModel<T>> PostFileAsync<T>(string uri, IReadOnlyCollection<RequestParameter> parameters = null, Dictionary<string, FileResult> parameterFile = null)
        {
            try
            {
                uri = AppConstants.UrlApiApp + uri;
               var client = new RestClient(uri);
                var request = new RestRequest(Method.POST);
                request.AlwaysMultipartFormData = true;
                request.AddHeader("Content-Type", "multipart/form-data");
                if (parameters != null && parameters.Any())
                {
                    foreach (var item in parameters)
                    {
                        request.AddParameter(item.Key, item.Value);
                    }
                }

                if (parameterFile != null && parameterFile.Any())
                {
                    foreach (var result in parameterFile)
                    {
                        var data = await result.Value.OpenReadAsync();
                        using (var memory = new MemoryStream())
                        {
                            data.CopyTo(memory);
                            request.AddFile(result.Key, memory.ToArray(), result.Value.FileName, "multipart/form-data");
                        }
                    }
                }

                var response = client.Execute(request);
                return JsonConvert.DeserializeObject<ResponseModel<T>>(response.Content);
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
            }

            return null;
        }

        public Task<ResponseModel<T>> PutFileAsync<T>(string uri, IReadOnlyCollection<RequestParameter> parameters = null, Dictionary<string, FileResult> parameterFile = null)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseModel<T>> PostAsync<T>(string uri, IReadOnlyCollection<RequestParameter> parameters, Dictionary<string, string> files = null)
        {
            try
            {
                CreateClients(uri, Method.POST);
                if (parameters != null && parameters.Any())
                {
                    foreach (var item in parameters)
                    {
                        _request.AddParameter(item.Key, item.Value);
                    }
                }
                if (files?.Any() == true)
                {
                    foreach (var file in files)
                    {
                        _request.AddFile(file.Key, file.Value);
                    }
                }
                _request.AlwaysMultipartFormData = true;
                var response = await _client.ExecuteAsync<ResponseModel<T>>(_request);
                var data = response.StatusCode == HttpStatusCode.OK
                    ? JsonConvert.DeserializeObject<ResponseModel<T>>(response.Content)
                    : default;
                return data;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}