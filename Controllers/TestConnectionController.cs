﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ResourceBookingAPI.Controllers
{
    [ApiController]
    [Route("api/test")]
    [AllowAnonymous]
    public class TestConnectionController : ControllerBase
    {
        [HttpGet]
        public string Get()
        {
            return "server is running";
        }
    }
}
