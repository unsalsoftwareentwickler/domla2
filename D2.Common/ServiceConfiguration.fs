﻿namespace D2.Common

module ServiceConfiguration =

    open Microsoft.AspNetCore.Server.Kestrel.Core
    open Microsoft.Extensions.Configuration
    open System.Net
    open System
    open System.Collections.Generic

    type ServiceAddress () =
        member val Protocol = String.Empty with get, set
        member val Address = String.Empty with get, set
        member val Port = 0 with get, set
    
    type AuthorityProperties () =
        member val Protocol = String.Empty with get, set
        member val Address = String.Empty with get, set
        member val Port = 0 with get, set
        member this.FullAddress
            with get () =
                sprintf "%s://%s:%d" this.Protocol this.Address this.Port
    
    type Service () = 
        member val Hosting = List<ServiceAddress>() with get, set
    
    let configuration =
        let builder = ConfigurationBuilder()
        builder.SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
               .AddJsonFile("appsettings.json") |> ignore
        
        let configuration = builder.Build()
        let config = Service ()
        configuration.GetSection("Service").Bind config

        config
    
    let authority () =
        let builder = ConfigurationBuilder()
        builder.SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
               .AddJsonFile("appsettings.json") |> ignore
        
        let configuration = builder.Build()
        let config = AuthorityProperties ()
        configuration.GetSection("Authority").Bind config

        config
    
    let configureKestrel (options : KestrelServerOptions) =
        for hosting in configuration.Hosting do
            match hosting.Protocol with
            | "http"  -> options.Listen (
                            IPEndPoint (
                                IPAddress.Parse hosting.Address,
                                hosting.Port
                            )
                        )
            | _       -> failwith "unsupported protocol"
        ()