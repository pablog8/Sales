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
    }
}
