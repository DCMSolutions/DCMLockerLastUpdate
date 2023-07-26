﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Json;
using DCMLocker.Shared.Locker;
using DCMLocker.Client.Authentication;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.AspNetCore.Components;
using DCMLocker.Shared;

namespace DCMLocker.Client.Cliente
{
    public class TLockerCliente
    {
        public event EventHandler OnChange;

        private HubConnection hubConnection;
        private readonly HttpClient _cliente;
        private readonly MOFAuthenticationStateProvider _auth;
        private readonly TLocker _locker;
        private readonly NavigationManager _nav;
        public TLockerCliente(HttpClient http, MOFAuthenticationStateProvider Auth, TLocker locker, NavigationManager Nav)
        {
            _cliente = http;
            _auth = Auth;
            _locker = locker;
            _nav = Nav;
        }

        /// <summary>---------------------------------------------------------------------
        ///  Solicitud de estado actual
        /// </summary>
        /// <returns></returns>-----------------------------------------------------------
        public async Task<LockerCU[]> GetState()
        {
            var token = await _auth.GetTokenAsync();
            if (!string.IsNullOrEmpty(token))
            {
                _cliente.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
            try
            {
                _locker.LockerCUs = await _cliente.GetFromJsonAsync<LockerCU[]>("Locker");
                return _locker.LockerCUs;
            }
            catch (HttpRequestException er)
            {
                if ((er.StatusCode == System.Net.HttpStatusCode.Forbidden) ||
                   (er.StatusCode == System.Net.HttpStatusCode.Unauthorized))
                {
                    _nav.NavigateTo("/login");
                    return null;
                }
                else throw;
            }
            catch
            {
                throw;
            }

        }

        /// <summary>--------------------------------------------------------------------
        ///  Solicitud de configuracion del locker
        /// </summary>
        /// <returns></returns>----------------------------------------------------------
        public async Task<LockerConfig> GetConfig()
        {
            LockerConfig retorno = null;
            var token = await _auth.GetTokenAsync();
            if (!string.IsNullOrEmpty(token))
            {
                _cliente.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
            try
            {
                retorno = await _cliente.GetFromJsonAsync<LockerConfig>("Locker/GetLockerConfig");
                return retorno;
            }
            catch (HttpRequestException er)
            {
                Console.WriteLine(er.Message);
                if ((er.StatusCode == System.Net.HttpStatusCode.Forbidden) ||
                   (er.StatusCode == System.Net.HttpStatusCode.Unauthorized))
                {
                    _nav.NavigateTo("/login");
                    return null;
                }
                else throw;
            }
            catch (Exception er)
            {
                Console.WriteLine(er.Message);
                throw;
            }
        }
        /// <summary>---------------------------------------------------------------------
        ///  Configura locker
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>-----------------------------------------------------------
        public async Task<bool> SetConfig(LockerConfig data)
        {
            var token = await _auth.GetTokenAsync();
            if (!string.IsNullOrEmpty(token))
            {
                _cliente.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
            try
            {
                var retorno = await _cliente.PostAsJsonAsync<LockerConfig>("Locker/SetLockerConfig", data);
                if (retorno.StatusCode == System.Net.HttpStatusCode.OK) return true;
                else return false;
            }
            catch (HttpRequestException er)
            {
                Console.WriteLine(er.Message);
                if ((er.StatusCode == System.Net.HttpStatusCode.Forbidden) ||
                   (er.StatusCode == System.Net.HttpStatusCode.Unauthorized))
                {
                    _nav.NavigateTo("/login");
                    return false;
                }
                else throw;
            }
            catch (Exception er)
            {
                Console.WriteLine(er.Message);
                throw;
            }
        }
        /// <summary>---------------------------------------------------------------------
        ///   Solicitud de configuracion de caja
        /// </summary>
        /// <param name="BoxAddr"></param>
        /// <returns></returns>-----------------------------------------------------------
        public async Task<TLockerMap> GetBoxConfig(int BoxAddr)
        {
            TLockerMap retorno = null;
            var token = await _auth.GetTokenAsync();
            if (!string.IsNullOrEmpty(token))
            {
                _cliente.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
            try
            {
                retorno = await _cliente.GetFromJsonAsync<TLockerMap>($"Locker/GetBoxConfig?BoxAddr={BoxAddr}");
                return retorno;
            }
            catch (HttpRequestException er)
            {
                Console.WriteLine(er.Message);
                if ((er.StatusCode == System.Net.HttpStatusCode.Forbidden) ||
                   (er.StatusCode == System.Net.HttpStatusCode.Unauthorized))
                {
                    _nav.NavigateTo("/login");
                    return null;
                }
                else throw;
            }
            catch (Exception er)
            {
                Console.WriteLine(er.Message);
                throw;
            }
        }
        /// <summary>---------------------------------------------------------------------
        ///  Set la configuracion de caja
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>-----------------------------------------------------------
        public async Task<bool> SetBoxConfig(TLockerMap data)
        {
            var token = await _auth.GetTokenAsync();
            if (!string.IsNullOrEmpty(token))
            {
                _cliente.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
            try
            {
                var retorno = await _cliente.PostAsJsonAsync<TLockerMap>("Locker/SetBoxConfig", data);
                if (retorno.StatusCode == System.Net.HttpStatusCode.OK) return true;
                else return false;
            }
            catch (HttpRequestException er)
            {
                Console.WriteLine(er.Message);
                if ((er.StatusCode == System.Net.HttpStatusCode.Forbidden) ||
                   (er.StatusCode == System.Net.HttpStatusCode.Unauthorized))
                {
                    _nav.NavigateTo("/login");
                    return false;
                }
                else throw;
            }
            catch (Exception er)
            {
                Console.WriteLine(er.Message);
                throw;
            }
        }
        /// <summary>---------------------------------------------------------------------
        ///  Solicitud de Cajas del usuario
        /// </summary>
        /// <returns></returns>----------------------------------------------------------
        public async Task<LockerUserPerfil> GetMyBox()
        {
            var token = await _auth.GetTokenAsync();
            if (!string.IsNullOrEmpty(token))
            {
                _cliente.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
            try
            {
                return await _cliente.GetFromJsonAsync<LockerUserPerfil>("Locker/GetMyPerfil");
            }
            catch (HttpRequestException er)
            {
                Console.WriteLine(er.Message);
                if ((er.StatusCode == System.Net.HttpStatusCode.Forbidden) ||
                   (er.StatusCode == System.Net.HttpStatusCode.Unauthorized))
                {
                    _nav.NavigateTo("/login");
                    return null;
                }
                else throw;
            }
            catch (Exception er)
            {
                Console.WriteLine(er.Message);
                return null;
            }

        }

        /// <summary>---------------------------------------------------------------------
        ///  Solicitud de Cajas del usuario
        /// </summary>
        /// <returns></returns>----------------------------------------------------------
        public async Task<LockerUserPerfil> GetBoxFromUser(string user)
        {
            string iduser = user.Replace("@", "%40").Replace(".","%_");
            var token = await _auth.GetTokenAsync();
            if (!string.IsNullOrEmpty(token))
            {
                _cliente.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
            try
            {
                return await _cliente.GetFromJsonAsync<LockerUserPerfil>($"Locker/GetPerfilFromUser?iduser={iduser}");
            }
            catch (HttpRequestException er)
            {
                Console.WriteLine(er.Message);
                if ((er.StatusCode == System.Net.HttpStatusCode.Forbidden) ||
                   (er.StatusCode == System.Net.HttpStatusCode.Unauthorized))
                {
                    _nav.NavigateTo("/login");
                    return null;
                }
                else throw;
            }
            catch (Exception er)
            {
                Console.WriteLine(er.Message);
                return null;
            }

        }


        /// <summary>-----------------------------------------------------------------------
        /// Buscador de usuarios
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>-------------------------------------------------------------
        public async Task<string[]> Search(string keyword)
        {
            var token = await _auth.GetTokenAsync();
            if (!string.IsNullOrEmpty(token))
            {
                _cliente.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
            try
            {
                return await _cliente.GetFromJsonAsync<string[]>($"Locker/Search?keyword={keyword}");
            }
            catch (HttpRequestException er)
            {
                Console.WriteLine(er.Message);
                if ((er.StatusCode == System.Net.HttpStatusCode.Forbidden) ||
                   (er.StatusCode == System.Net.HttpStatusCode.Unauthorized))
                {
                    _nav.NavigateTo("/login");
                }
               
            }
            catch (Exception er)
            {
                Console.WriteLine(er.Message);
                
            }
            return new string[0];
        }
        public async Task<List<LockerUserPerfil>> GetAllUsers()
        {
            var token = await _auth.GetTokenAsync();
            if (!string.IsNullOrEmpty(token))
            {
                _cliente.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
            try
            {
                return await _cliente.GetFromJsonAsync<List<LockerUserPerfil>>($"Locker/GetAllUsers");
            }
            catch (HttpRequestException er)
            {
                Console.WriteLine(er.Message);
                if ((er.StatusCode == System.Net.HttpStatusCode.Forbidden) ||
                   (er.StatusCode == System.Net.HttpStatusCode.Unauthorized))
                {
                    _nav.NavigateTo("/login");
                }

            }
            catch (Exception er)
            {
                Console.WriteLine(er.Message);

            }
            return null;
        }
        
        public async Task<bool> ChangeAdminPassword(string newPass)
        {
            var token = await _auth.GetTokenAsync();
            if (!string.IsNullOrEmpty(token))
            {
                _cliente.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
            try
            {
                return await _cliente.GetFromJsonAsync<bool>($"Locker/ChangeAdminPassword?newPass={newPass}");
            }
            catch (HttpRequestException er)
            {
                Console.WriteLine(er.Message);
                if ((er.StatusCode == System.Net.HttpStatusCode.Forbidden) ||
                   (er.StatusCode == System.Net.HttpStatusCode.Unauthorized))
                {
                    _nav.NavigateTo("/login");
                }

            }
            catch (Exception er)
            {
                Console.WriteLine(er.Message);

            }
            return false;
        }
        public async Task<string[]> SearchUserFromBox(int boxaddr)
        {
            var token = await _auth.GetTokenAsync();
            if (!string.IsNullOrEmpty(token))
            {
                _cliente.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
            try
            {
                return await _cliente.GetFromJsonAsync<string[]>($"Locker/GetUserFromBox?boxaddr={boxaddr}");
            }
            catch (HttpRequestException er)
            {
                Console.WriteLine(er.Message);
                if ((er.StatusCode == System.Net.HttpStatusCode.Forbidden) ||
                   (er.StatusCode == System.Net.HttpStatusCode.Unauthorized))
                {
                    _nav.NavigateTo("/login");
                }

            }
            catch (Exception er)
            {
                Console.WriteLine(er.Message);

            }
            return new string[0];
        }

        /// <summary>-----------------------------------------------------------------------
        /// Bloquea al usuario
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>-------------------------------------------------------------
        public async Task<bool> SetLockUser(string user)
        {
            var token = await _auth.GetTokenAsync();
            if (!string.IsNullOrEmpty(token))
            {
                _cliente.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
            try
            {
                var retorno = await _cliente.PostAsJsonAsync<string>("Locker/SetUserLockstate", user);
                if (retorno.StatusCode == System.Net.HttpStatusCode.OK) return true;
                else return false;
            }
            catch (HttpRequestException er)
            {
                Console.WriteLine(er.Message);
                if ((er.StatusCode == System.Net.HttpStatusCode.Forbidden) ||
                   (er.StatusCode == System.Net.HttpStatusCode.Unauthorized))
                {
                    _nav.NavigateTo("/login");
                    return false;
                }
                else throw;
            }
            catch (Exception er)
            {
                Console.WriteLine(er.Message);
                throw;
            }
        }


        /// <summary>--------------------------------------------------------------------
        ///  Apertura de caja
        /// </summary>
        /// <param name="Locker"></param>
        /// <param name="box"></param>
        /// <param name="user"></param>
        /// <param name="token"></param>-------------------------------------------------
        public async Task<bool> OpenLocker(int Locker, int box, string user, string tokens)
        {
            var token = await _auth.GetTokenAsync();
            if (!string.IsNullOrEmpty(token))
            {
                _cliente.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
            try
            {
               var r = await _cliente.PostAsJsonAsync<BoxAccessToken>("Locker/OpenBox", new BoxAccessToken() { CU = Locker, Box = box, User = user, Token = tokens });
                if (r.StatusCode == System.Net.HttpStatusCode.OK) return true;
            }
            catch (HttpRequestException er)
            {
                Console.WriteLine(er.Message);
                if ((er.StatusCode == System.Net.HttpStatusCode.Forbidden) ||
                   (er.StatusCode == System.Net.HttpStatusCode.Unauthorized))
                {
                    _nav.NavigateTo("/login");
                    

                }
                else throw;
            }
            return false;
        }
        /// <summary>--------------------------------------------------------------------
        /// Asigna una caja al usuario
        /// </summary>
        /// <param name="user"></param>
        /// <param name="box"></param>
        /// <returns></returns>----------------------------------------------------------
        public async Task<bool> SetUserToBox(string user, int box)
        {
            var token = await _auth.GetTokenAsync();
            if (!string.IsNullOrEmpty(token))
            {
                _cliente.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
            try
            {
                var retorno = await _cliente.PostAsJsonAsync<LockerUserBox>("Locker/BoxToUser", new LockerUserBox() { User=user, Box=box } );
                if (retorno.StatusCode == System.Net.HttpStatusCode.OK) return true;
                else return false;
            }
            catch (HttpRequestException er)
            {
                Console.WriteLine(er.Message);
                if ((er.StatusCode == System.Net.HttpStatusCode.Forbidden) ||
                   (er.StatusCode == System.Net.HttpStatusCode.Unauthorized))
                {
                    _nav.NavigateTo("/login");
                    return false;
                }
                else throw;
            }
            catch (Exception er)
            {
                Console.WriteLine(er.Message);
                throw;
            }
        }
        /// <summary>--------------------------------------------------------------------
        /// Remueve al usuario de la caja
        /// </summary>
        /// <param name="user"></param>
        /// <param name="box"></param>
        /// <returns></returns>----------------------------------------------------------
        public async Task<bool> SetUserRemoveBox(string user, int box)
        {
            var token = await _auth.GetTokenAsync();
            if (!string.IsNullOrEmpty(token))
            {
                _cliente.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
            try
            {
                Console.WriteLine(2);
                var retorno = await _cliente.PostAsJsonAsync<LockerUserBox>("Locker/BoxRemoveUser", new LockerUserBox() { User = user, Box = box });
                if (retorno.StatusCode == System.Net.HttpStatusCode.OK) return true;
                else return false;
            }
            catch (HttpRequestException er)
            {
                Console.WriteLine(er.Message);
                if ((er.StatusCode == System.Net.HttpStatusCode.Forbidden) ||
                   (er.StatusCode == System.Net.HttpStatusCode.Unauthorized))
                {
                    _nav.NavigateTo("/login");
                    return false;
                }
                else throw;
            }
            catch (Exception er)
            {
                Console.WriteLine(er.Message);
                throw;
            }
        }

        /// <summary>--------------------------------------------------------------------
        /// Retorna el estado de las cajas de configuración fija
        /// </summary>
        /// <returns></returns>----------------------------------------------------------
        public async Task<BoxState[]> GetBoxesFixedState()
        {
            var token = await _auth.GetTokenAsync();
            if (!string.IsNullOrEmpty(token))
            {
                _cliente.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
            try
            {
                return await _cliente.GetFromJsonAsync<BoxState[]>("Locker/GetBoxesFixedState");
            }
            catch (HttpRequestException er)
            {
                Console.WriteLine(er.Message);
                if ((er.StatusCode == System.Net.HttpStatusCode.Forbidden) ||
                   (er.StatusCode == System.Net.HttpStatusCode.Unauthorized))
                {
                    _nav.NavigateTo("/login");
                    return null;
                }
                else throw;
            }
            catch (Exception er)
            {
                Console.WriteLine(er.Message);
                return null;
            }

        }

        public async Task<int[]> GetBoxesSeftManagement()
        {
            var token = await _auth.GetTokenAsync();
            if (!string.IsNullOrEmpty(token))
            {
                _cliente.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
            try
            {
                return await _cliente.GetFromJsonAsync<int[]>("Locker/GetBoxSelfManagement");
            }
            catch (HttpRequestException er)
            {
                Console.WriteLine(er.Message);
                if ((er.StatusCode == System.Net.HttpStatusCode.Forbidden) ||
                   (er.StatusCode == System.Net.HttpStatusCode.Unauthorized))
                {
                    _nav.NavigateTo("/login");
                    return null;
                }
                else throw;
            }
            catch (Exception er)
            {
                Console.WriteLine(er.Message);
                return null;
            }

        }
        public async Task<bool> BoxesSeftManagementReserve(int boxaddr)
        {
            var token = await _auth.GetTokenAsync();
            if (!string.IsNullOrEmpty(token))
            {
                _cliente.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
            try
            {
                Console.WriteLine("Box en reserva");
                var r = await _cliente.PostAsJsonAsync<int>("Locker/BoxSelfManagementReserve", boxaddr);
                if (r.StatusCode == System.Net.HttpStatusCode.OK) return true;
                else return false;
            }
            catch (HttpRequestException er)
            {
                Console.WriteLine(er.Message);
                if ((er.StatusCode == System.Net.HttpStatusCode.Forbidden) ||
                   (er.StatusCode == System.Net.HttpStatusCode.Unauthorized))
                {
                    _nav.NavigateTo("/login");
                }
                
            }
            catch (Exception er)
            {
                Console.WriteLine(er.Message);
                
            }
            return false;
        }

        /// <summary>--------------------------------------------------------------------
        /// Genera Token de acceso
        /// </summary>
        /// <returns></returns>----------------------------------------------------------
        public async Task<string> GenerateTokenKey(int BoxAddr)
        {
            var token = await _auth.GetTokenAsync();
            if (!string.IsNullOrEmpty(token))
            {
                _cliente.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
            try
            {
                Console.WriteLine("TOKEN--------");

                var s = await _cliente.GetFromJsonAsync<string[]>($"Locker/GenerateTokenKey?BoxAddr={BoxAddr}");
                Console.WriteLine(s);
                Console.WriteLine(s[0]);

                return s[0];
            }
            catch (HttpRequestException er)
            {
                Console.WriteLine(er.Message);
                if ((er.StatusCode == System.Net.HttpStatusCode.Forbidden) ||
                   (er.StatusCode == System.Net.HttpStatusCode.Unauthorized))
                {
                    _nav.NavigateTo("/login");
                    return null;
                }
                else throw;
            }
            catch (Exception er)
            {
                Console.WriteLine(er.Message);
                return null;
            }

        }

        /// <summary>--------------------------------------------------------------------
        /// Genera Token de acceso
        /// </summary>
        /// <returns></returns>----------------------------------------------------------
        public async Task<int> TokenKey(string tokenkey)
        {
            var token = await _auth.GetTokenAsync();
            if (!string.IsNullOrEmpty(token))
            {
                _cliente.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
            try
            {
                Console.WriteLine("Verificacion de token");
                var r = await _cliente.PostAsJsonAsync<string>("Locker/TokenKey", tokenkey);
                if (r.StatusCode == System.Net.HttpStatusCode.OK) return await r.Content.ReadFromJsonAsync<int>();
                else throw new Exception("Token no valido");
            }
            catch (HttpRequestException er)
            {
                Console.WriteLine(er.Message);
                if ((er.StatusCode == System.Net.HttpStatusCode.Forbidden) ||
                   (er.StatusCode == System.Net.HttpStatusCode.Unauthorized))
                {
                    _nav.NavigateTo("/login");
                    throw;
                }
                else throw;
            }
            catch (Exception er)
            {
                Console.WriteLine(er.Message);
                throw;
            }

        }

        public async Task<TokenAccessBox[]> GetMyTokenKey()
        {
            var token = await _auth.GetTokenAsync();
            if (!string.IsNullOrEmpty(token))
            {
                _cliente.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
            try
            {
                Console.WriteLine("TOKEN--------");

                return await _cliente.GetFromJsonAsync<TokenAccessBox[]>($"Locker/GetMyTokens");
            }
            catch (HttpRequestException er)
            {
                Console.WriteLine(er.Message);
                if ((er.StatusCode == System.Net.HttpStatusCode.Forbidden) ||
                   (er.StatusCode == System.Net.HttpStatusCode.Unauthorized))
                {
                    _nav.NavigateTo("/login");
                    return null;
                }
                else throw;
            }
            catch (Exception er)
            {
                Console.WriteLine(er.Message);
                return null;
            }

        }

        public async Task<SystemNetwork[]> System_GetIP()
        {
            var token = await _auth.GetTokenAsync();
            if (!string.IsNullOrEmpty(token))
            {
                _cliente.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
            try
            {
                return await _cliente.GetFromJsonAsync<SystemNetwork[]>($"System/GetIP");
            }
            catch (HttpRequestException er)
            {
                Console.WriteLine(er.Message);
                if ((er.StatusCode == System.Net.HttpStatusCode.Forbidden) ||
                   (er.StatusCode == System.Net.HttpStatusCode.Unauthorized))
                {
                    _nav.NavigateTo("/login");
                    return null;
                }
                else throw;
            }
            catch (Exception er)
            {
                Console.WriteLine(er.Message);
                return null;
            }

        }
        public async Task<string[]> System_GetSSID()
        {
            var token = await _auth.GetTokenAsync();
            if (!string.IsNullOrEmpty(token))
            {
                _cliente.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
            try
            {
                Console.WriteLine("GETSSID");
                string[] f = await _cliente.GetFromJsonAsync<string[]>($"System/GetSSID");
                Console.WriteLine($"GETSSID FIN {f[0]}");

                return f;
            }
            catch (HttpRequestException er)
            {
                Console.WriteLine(er.Message);
                if ((er.StatusCode == System.Net.HttpStatusCode.Forbidden) ||
                   (er.StatusCode == System.Net.HttpStatusCode.Unauthorized))
                {
                    _nav.NavigateTo("/login");
                    return null;
                }
                else throw;
            }
            catch (Exception er)
            {
                Console.WriteLine("EEROR:" + er.Message);
                return null;
            }

        }
        public async Task<bool> System_SetSSID(string ssid, string pass)
        {
            var token = await _auth.GetTokenAsync();
            if (!string.IsNullOrEmpty(token))
            {
                _cliente.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
            try
            {
                var r = await _cliente.PostAsJsonAsync<SystemSSID>($"System/SetSSID", new SystemSSID() {  SSID = ssid, Pass=pass });
                if (r.StatusCode == System.Net.HttpStatusCode.OK) return true;
                return false;
            }
            catch (HttpRequestException er)
            {
                Console.WriteLine(er.Message);
                if ((er.StatusCode == System.Net.HttpStatusCode.Forbidden) ||
                   (er.StatusCode == System.Net.HttpStatusCode.Unauthorized))
                {
                    _nav.NavigateTo("/login");
                    return false;
                }
                else throw;
            }
            catch (Exception er)
            {
                Console.WriteLine("EEROR:" + er.Message);
                return false;
            }

        }
        public async Task<bool> System_SetWLan(bool state)
        {
            var token = await _auth.GetTokenAsync();
            if (!string.IsNullOrEmpty(token))
            {
                _cliente.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
            try
            {
                var r = await _cliente.PostAsJsonAsync<bool>($"System/SetWLAN", state);
                if (r.StatusCode == System.Net.HttpStatusCode.OK) return true;
                return false;
            }
            catch (HttpRequestException er)
            {
                Console.WriteLine(er.Message);
                if ((er.StatusCode == System.Net.HttpStatusCode.Forbidden) ||
                   (er.StatusCode == System.Net.HttpStatusCode.Unauthorized))
                {
                    _nav.NavigateTo("/login");
                    return false;
                }
                else throw;
            }
            catch (Exception er)
            {
                Console.WriteLine("EEROR:" + er.Message);
                return false;
            }

        }
        public async Task<bool> System_Reset()
        {
            var token = await _auth.GetTokenAsync();
            if (!string.IsNullOrEmpty(token))
            {
                _cliente.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
            try
            {
                var r = await _cliente.PostAsJsonAsync($"System/Reset","");
                if (r.StatusCode == System.Net.HttpStatusCode.OK) return true;
                return false;
            }
            catch (HttpRequestException er)
            {
                Console.WriteLine(er.Message);
                if ((er.StatusCode == System.Net.HttpStatusCode.Forbidden) ||
                   (er.StatusCode == System.Net.HttpStatusCode.Unauthorized))
                {
                    _nav.NavigateTo("/login");
                    return false;
                }
                else throw;
            }
            catch (Exception er)
            {
                Console.WriteLine("EEROR:" + er.Message);
                return false;
            }

        }
        public async Task<bool> System_Shutdown()
        {
            var token = await _auth.GetTokenAsync();
            if (!string.IsNullOrEmpty(token))
            {
                _cliente.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
            try
            {
                var r = await _cliente.PostAsJsonAsync($"System/Shutdown", "");
                if (r.StatusCode == System.Net.HttpStatusCode.OK) return true;
                return false;
            }
            catch (HttpRequestException er)
            {
                Console.WriteLine(er.Message);
                if ((er.StatusCode == System.Net.HttpStatusCode.Forbidden) ||
                   (er.StatusCode == System.Net.HttpStatusCode.Unauthorized))
                {
                    _nav.NavigateTo("/login");
                    return false;
                }
                else throw;
            }
            catch (Exception er)
            {
                Console.WriteLine("EEROR:" + er.Message);
                return false;
            }

        }
    }
}
