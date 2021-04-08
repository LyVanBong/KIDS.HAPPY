using KIDS.MOBILE.APP.Configurations;
using KIDS.MOBILE.APP.Models.RequestProvider;
using KIDS.MOBILE.APP.Models.Response;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using FFImageLoading;
using Microsoft.AppCenter.Crashes;
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

        public async Task<ResponseModel<T>> GetAsync<T>(string uri, IReadOnlyCollection<RequestParameter> parameters = null, Dictionary<string, FileResult> parameterFile = null)
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
            catch (Exception)
            {
                return null;
            }
        }

        private void CreateClients(string uri, Method method = Method.GET)
        {
            try
            {
                uri = AppConstants.UrlApiApp + uri;
                _client.BaseUrl = new Uri(uri);
                _client.Timeout = 10000;
                _request = new RestRequest(method);
            }
            catch (Exception)
            {
            }
        }

        public async Task<ResponseModel<T>> PostAsync<T>(string uri, IReadOnlyCollection<RequestParameter> parameters = null, Dictionary<string, FileResult> parameterFile = null)
        {
            try
            {
                CreateClients(uri, Method.POST);
                CreateParameter(parameters, parameterFile);
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

        private async void CreateParameter(IReadOnlyCollection<RequestParameter> parameters, Dictionary<string, FileResult> parameterFile)
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


                if (parameterFile != null && parameterFile.Any())
                {
                    foreach (var file in parameterFile)
                    {
                        var data = (await file.Value.OpenReadAsync())?.ToByteArray();
                        if (data != null)
                        {
                            _request.AddFile(file.Key, data, file.Value.FileName, "multipart/form-data");
                        }
                    }

                    _request.AlwaysMultipartFormData = true;
                }
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
            }
        }

        public async Task<ResponseModel<T>> PutAsync<T>(string uri, IReadOnlyCollection<RequestParameter> parameters = null, Dictionary<string, FileResult> parameterFile = null)
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

        public async Task<ResponseModel<T>> DeleteAsync<T>(string uri, IReadOnlyCollection<RequestParameter> parameters = null, Dictionary<string, FileResult> parameterFile = null)
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
    }
}