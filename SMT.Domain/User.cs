﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMT.Domain
{
    public class User : IdentityUser
    {
        public string Telegram { get; set; }
    }
}
