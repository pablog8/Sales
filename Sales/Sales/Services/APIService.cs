﻿namespace Sales.Services
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using Sales.Common.Models;
    using Sales.Helpers;
    using Newtonsoft.Json;
    using Plugin.Connectivity;
    using System.Net.Http.Headers;

    public class APIService
    {

        public async Task<Response> CheckConnection()
        {
            //si tiene el internet esta conectado
            if (!CrossConnectivity.Current.IsConnected)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = Languages.TurnOnInternet,
                };
            }

            //si esta conectaco comprueba que se pueda conectar a una dirección cualquiera, comprobamos el ping
            var isReachable = await CrossConnectivity.Current.IsRemoteReachable("google.com");
            if (!isReachable)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = Languages.NoInternet,
                };
            }

            //cuando tenemos la conexion completada a internet
            return new Response
            {
                IsSuccess = true,
            };
        }

        //obtenemos el Token
        public async Task<TokenResponse> GetToken(
                string urlBase,
                string username,
                string password)
        {
            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                var response = await client.PostAsync("Token",
                    new StringContent(string.Format(
                    "grant_type=password&username={0}&password={1}",
                    username, password),
                    Encoding.UTF8, "application/x-www-form-urlencoded"));
                var resultJSON = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<TokenResponse>(
                    resultJSON);
                return result;
            }
            catch
            {
                return null;
            }
        }

        //si devuelve o no la lista del producto
        public async Task<Response> GetList<T>(string urlBase, string prefix, string controller)
        {
            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);

                //stringformat, concateno
                var url = $"{prefix}{controller}";

                var response = await client.GetAsync(url);

                var answer = await response.Content.ReadAsStringAsync();

                //comprobamos si el servicio es exitoso o no, si falla devolvemos que no fue posible
                if(!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = answer,
                    };
                }

                //deselializar pasamos de string A OBJETO
                var list = JsonConvert.DeserializeObject<List<T>>(answer);
                return new Response
                {
                    IsSuccess = true,
                    Result = list,
                };

            }
            catch(Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };

            }

        }

        public async Task<Response> GetList<T>(string urlBase, string prefix, string controller, string tokenType, string accessToken)
        {
            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, accessToken);
                //stringformat, concateno
                var url = $"{prefix}{controller}";

                var response = await client.GetAsync(url);

                var answer = await response.Content.ReadAsStringAsync();

                //comprobamos si el servicio es exitoso o no, si falla devolvemos que no fue posible
                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = answer,
                    };
                }

                //deselializar pasamos de string A OBJETO
                var list = JsonConvert.DeserializeObject<List<T>>(answer);
                return new Response
                {
                    IsSuccess = true,
                    Result = list,
                };

            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };

            }

        }

        public async Task<Response> Post<T>(string urlBase, string prefix, string controller, T model)
        {
            try
            {
                //el objeto hay que convertirlo a string, serializamos
                var request = JsonConvert.SerializeObject(model);
                //y lo codificamos, content es el cuerpo que enviamos al post
                var content = new StringContent(request, Encoding.UTF8, "application/json");

                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);

                //stringformat, concateno
                var url = $"{prefix}{controller}";

                //hacemos el post
                var response = await client.PostAsync(url,content);

                var answer = await response.Content.ReadAsStringAsync();

                //comprobamos si el servicio es exitoso o no, si falla devolvemos que no fue posible
                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = answer,
                    };
                }

                //deselializamos pasamos de string A OBJETO
                var obj = JsonConvert.DeserializeObject<T>(answer);
                return new Response
                {
                    IsSuccess = true,
                    Result = obj,
                };

            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };

            }
        }

        public async Task<Response> Post<T>(string urlBase, string prefix, string controller, T model, string tokenType, string accessToken)
        {
            try
            {
                //el objeto hay que convertirlo a string, serializamos
                var request = JsonConvert.SerializeObject(model);
                //y lo codificamos, content es el cuerpo que enviamos al post
                var content = new StringContent(request, Encoding.UTF8, "application/json");

                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, accessToken);

                //stringformat, concateno
                var url = $"{prefix}{controller}";

                //hacemos el post
                var response = await client.PostAsync(url, content);

                var answer = await response.Content.ReadAsStringAsync();

                //comprobamos si el servicio es exitoso o no, si falla devolvemos que no fue posible
                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = answer,
                    };
                }

                //deselializamos pasamos de string A OBJETO
                var obj = JsonConvert.DeserializeObject<T>(answer);
                return new Response
                {
                    IsSuccess = true,
                    Result = obj,
                };

            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };

            }
        }

        public async Task<Response> Put<T>(string urlBase, string prefix, string controller, T model, int id)
        {
            try
            {
                //el objeto hay que convertirlo a string, serializamos
                var request = JsonConvert.SerializeObject(model);
                //y lo codificamos, content es el cuerpo que enviamos al post
                var content = new StringContent(request, Encoding.UTF8, "application/json");

                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);

                //stringformat, concateno
                var url = $"{prefix}{controller}/{id}";

                //hacemos el post
                var response = await client.PutAsync(url, content);

                var answer = await response.Content.ReadAsStringAsync();

                //comprobamos si el servicio es exitoso o no, si falla devolvemos que no fue posible
                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = answer,
                    };
                }

                //deselializamos pasamos de string A OBJETO
                var obj = JsonConvert.DeserializeObject<T>(answer);
                return new Response
                {
                    IsSuccess = true,
                    Result = obj,
                };

            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };

            }
        }

        public async Task<Response> Put<T>(string urlBase, string prefix, string controller, T model, int id, string tokenType, string accessToken)
        {
            try
            {
                //el objeto hay que convertirlo a string, serializamos
                var request = JsonConvert.SerializeObject(model);
                //y lo codificamos, content es el cuerpo que enviamos al post
                var content = new StringContent(request, Encoding.UTF8, "application/json");

                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, accessToken);

                //stringformat, concateno
                var url = $"{prefix}{controller}/{id}";

                //hacemos el post
                var response = await client.PutAsync(url, content);

                var answer = await response.Content.ReadAsStringAsync();

                //comprobamos si el servicio es exitoso o no, si falla devolvemos que no fue posible
                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = answer,
                    };
                }

                //deselializamos pasamos de string A OBJETO
                var obj = JsonConvert.DeserializeObject<T>(answer);
                return new Response
                {
                    IsSuccess = true,
                    Result = obj,
                };

            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };

            }
        }

        //eliminar
        public async Task<Response> Delete(string urlBase, string prefix, string controller, int id)
        {
            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);

                //stringformat, concateno
                var url = $"{prefix}{controller}/{id}";

                var response = await client.DeleteAsync(url);

                var answer = await response.Content.ReadAsStringAsync();

                //comprobamos si el servicio es exitoso o no, si falla devolvemos que no fue posible
                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = answer,
                    };
                }
         
                return new Response
                {
                    IsSuccess = true,
                   
                };

            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };

            }

        }

        public async Task<Response> Delete(string urlBase, string prefix, string controller, int id, string tokenType, string accessToken)
        {
            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, accessToken);

                //stringformat, concateno
                var url = $"{prefix}{controller}/{id}";

                var response = await client.DeleteAsync(url);

                var answer = await response.Content.ReadAsStringAsync();

                //comprobamos si el servicio es exitoso o no, si falla devolvemos que no fue posible
                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = answer,
                    };
                }

                return new Response
                {
                    IsSuccess = true,

                };

            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };

            }

        }

        //
        public async Task<Response> GetUser(string urlBase, string prefix, string controller, string email, string tokenType, string accessToken)
        {
            try
            {
                var getUserRequest = new GetUserRequest
                {
                    Email = email,
                };

                var request = JsonConvert.SerializeObject(getUserRequest);
                var content = new StringContent(request, Encoding.UTF8, "application/json");

                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, accessToken);
                var url = $"{prefix}{controller}";
                var response = await client.PostAsync(url, content);
                var answer = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = answer,
                    };
                }

                var user = JsonConvert.DeserializeObject<MyUserASP>(answer);
                return new Response
                {
                    IsSuccess = true,
                    Result = user,
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }

        //le paso el token y me devuelve los datos de facebook
        public async Task<FacebookResponse> GetFacebook(string accessToken)
        {
            var requestUrl = "https://graph.facebook.com/v2.8/me/?fields=name," +
                "picture.width(999),cover,age_range,devices,email,gender," +
                "is_verified,birthday,languages,work,website,religion," +
                "location,locale,link,first_name,last_name," +
                "hometown&access_token=" + accessToken;
            var httpClient = new HttpClient();
            var userJson = await httpClient.GetStringAsync(requestUrl);
            var facebookResponse =
                JsonConvert.DeserializeObject<FacebookResponse>(userJson);
            return facebookResponse;
        }

        public async Task<InstagramResponse> GetInstagram(string accessToken)
        {
            var client = new HttpClient();
            var userJson = await client.GetStringAsync(accessToken);
            var InstagramJson = JsonConvert.DeserializeObject<InstagramResponse>(userJson);
            return InstagramJson;
        }

        public async Task<TokenResponse> LoginTwitter(string urlBase, string servicePrefix, string controller, TwitterResponse profile)
        {
            try
            {
                var request = JsonConvert.SerializeObject(profile);
                var content = new StringContent(
                    request,
                    Encoding.UTF8,
                    "application/json");
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                var url = $"{servicePrefix}{controller}";
                var response = await client.PostAsync(url, content);

                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                var tokenResponse = await GetToken(
                    urlBase,
                    profile.IdStr,
                    profile.IdStr);
                return tokenResponse;
            }
            catch
            {
                return null;
            }
        }

        public async Task<TokenResponse> LoginInstagram(string urlBase, string servicePrefix, string controller, InstagramResponse profile)
        {
            try
            {
                var request = JsonConvert.SerializeObject(profile);
                var content = new StringContent(
                    request,
                    Encoding.UTF8,
                    "application/json");
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                var url = $"{servicePrefix}{controller}";
                var response = await client.PostAsync(url, content);

                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                var tokenResponse = await GetToken(
                    urlBase,
                    profile.UserData.Id,
                    profile.UserData.Id);
                return tokenResponse;
            }
            catch
            {
                return null;
            }
        }

        public async Task<TokenResponse> LoginFacebook(string urlBase, string servicePrefix, string controller, FacebookResponse profile)
        {
            try
            {
                var request = JsonConvert.SerializeObject(profile);
                var content = new StringContent(
                    request,
                    Encoding.UTF8,
                    "application/json");
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                var url = $"{servicePrefix}{controller}";
                var response = await client.PostAsync(url, content);

                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                var tokenResponse = await GetToken(
                    urlBase,
                    profile.Id,
                    profile.Id);
                return tokenResponse;
            }
            catch
            {
                return null;
            }
        }

    }
}
