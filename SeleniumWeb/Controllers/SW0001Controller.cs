using Microsoft.AspNetCore.Mvc;
using SeleniumWeb.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeleniumWeb.Controllers
{
    /// <summary>
    /// 登录页面
    /// </summary>
    public class SW0001Controlerl: ControllerBase
    {
        private ISW0001Service _service;

        public SW0001Controlerl(ISW0001Service service)
        {
            _service = service;
        }


    }
}
