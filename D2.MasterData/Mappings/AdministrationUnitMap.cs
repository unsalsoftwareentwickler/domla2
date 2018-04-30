﻿using D2.MasterData.Models;
using FluentNHibernate.Mapping;

namespace D2.MasterData.Mappings
{
    public class AdministrationUnitCreateMap : ClassMap<AdministrationUnit>
    {
        public AdministrationUnitCreateMap()
        {
            Table("administrationunits");
            Id(x => x.Id);
            Map(x => x.UserKey)
                .Access.BackingField()
                .Length(10)
                .Not
                .Nullable();
            Map(x => x.Edit)
                .Access.BackingField()
                .Generated.Always();
            Map(x => x.Title)
                .Access.BackingField()
                .Length(256)
                .Nullable();
            Map(x => x.YearOfConstruction)
                .Access.BackingField()
                .Nullable();
            HasMany(x => x.Entrances)
                .Cascade
                .AllDeleteOrphan()
                .Inverse();
        }
    }

    public class AdministrationUnitMap : AdministrationUnitCreateMap
    {
        public AdministrationUnitMap()
            : base()
        {
            Version(x => x.Version)
                .Column("xmin")
                .Generated.Always();
        }
    }
}