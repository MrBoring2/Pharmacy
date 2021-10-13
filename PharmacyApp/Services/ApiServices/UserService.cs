using Microsoft.AspNetCore.SignalR.Client;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyApp.Services.ApiServices
{
    public class UserService
    {
        private static UserService instance;

        public HubConnection HubConnection { get; set; }
        public RestClient RestClient { get; set; }
        private static object syncRoot = new Object();

        private UserService()
        {
            
        }

        public void InirializeService(HubConnection hubConnection, RestClient restClient)
        {
            HubConnection = hubConnection;
            RestClient = restClient;
        }

        public static UserService Instance
        {
            get 
            {
                lock (syncRoot)
                {
                    if (instance == null)
                        instance = new UserService();
                }
                return instance;
            }
        }
    }
}
