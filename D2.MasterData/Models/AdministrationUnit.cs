﻿using D2.MasterData.Parameters;
using System;
using System.Collections.Generic;

namespace D2.MasterData.Models
{
    public class AdministrationUnit
    {
        public AdministrationUnit(AdministrationUnitParameters argument)
        {
            Id = argument.Id;
            UserKey = argument.UserKey;
            Title = argument.Title;
            Address = argument.Address;
        }

        public Guid Id
        {
            get;
        }

        public string UserKey
        {
            get;
        }

        public string Title
        {
            get;
        }

        public Address Address
        {
            get;
        }
    }
}
