﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Persistence.SettingModels
{
    public class JWT
    {
        public string Key { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public double DurationInMinutes { get; set; }
    }
}
