using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Grupp4BankApp.AppConfig
{
    internal static class AppConfig
    {
       static public IConfiguration Configuration { get; set; } = null;
    }
}
