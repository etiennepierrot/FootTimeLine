using System;
using System.Collections.Generic;
using System.Linq;
using RestSharp;

namespace FootTimeLine.SportDeer
{
    public class Connector
    {
        private readonly string _refreshToken;
        private string _accessToken;
        private const string Uri = "https://api.sportdeer.com/";

        public Connector(string refreshToken)
        {
            _refreshToken = refreshToken;
        }

        private string GetAccessToken()
        {
            if (_accessToken != null) return _accessToken;

            var client = new RestClient(Uri);
            var request = new RestRequest("v1/accessToken");
            request.AddParameter("refresh_token", _refreshToken, ParameterType.GetOrPost);
            var response = client.Execute<AccessToken>(request);
            _accessToken = response.Data.new_access_token;
            return _accessToken;
        }

        class AccessToken
        {
            public string new_access_token { get; set; }
        }

        public IRestResponse<T> Request<T>(string endpoint, int page) where T : class, new()
        {
            var client = new RestClient(Uri);
            var request = new RestRequest(endpoint);
            request.AddParameter("access_token", GetAccessToken(), ParameterType.GetOrPost);
            request.AddParameter("page", page, ParameterType.GetOrPost);
            return client.Execute<T>(request);
        }

        public T SearchInList<T>(string name, string endpointList) where T : Entity
        {
            int page = 1;
            IRestResponse<Response<T>> result;
            do
            {
                result = Request<Response<T>>(endpointList, page);
                T @object = result.Data.docs.SingleOrDefault(x => x.name == name);
                if (@object != null)
                {
                    return @object;
                }
                page++;
            } while (result.Data.docs.Count == 0);

            throw new ApplicationException($"object not found");
        }

        public class Response<T> where T : Entity
        {
            public List<T> docs { get; set; }
        }

        public class Entity
        {
            public int _id { get; set; }
            public string name { get; set; }
        }


    }
}
