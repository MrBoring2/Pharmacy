using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmacyApi.Services.Hubs
{

    public class PharmacyHub : Hub
    {
        [Authorize]
        public override Task OnConnectedAsync()
        {
            Clients.Caller.SendAsync("Welcome", $"Добро пожаловать, {Context.User.FindFirst("user_name_surname")?.Value}!");
            return base.OnConnectedAsync();
        }
    }
}
