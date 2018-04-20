﻿using D2.MasterData.Parameters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace D2.MasterData.Models
{
    public class AdministrationUnit : BaseModel
    {
        protected AdministrationUnit()
        { }

        public AdministrationUnit(AdministrationUnitParameters argument)
        {
            Id = argument.Id;
            UserKey = argument.UserKey;
            Title = argument.Title;
            var items = from entranceParameter in argument.Entrances
                        select new Entrance(entranceParameter, this);
            _entrances = new List<Entrance>(items);
            YearOfConstruction = argument.YearOfConstruction;
            Version = argument.Version;
        }

        [Required]
        [MaxLength(10)]
        public virtual string UserKey
        {
            get;
            private set;
        }

        [MaxLength(256)]
        public virtual string Title
        {
            get;
            private set;
        }

        private List<Entrance> _entrances;
        [Required]
        public virtual IReadOnlyList<Entrance> Entrances
        {
            get { return _entrances; }
        }

        public virtual DateTime? YearOfConstruction
        {
            get;
            private set;
        }

        [ConcurrencyCheck]
        public virtual uint Version
        {
            get;
            private set;
        }
    }
}
