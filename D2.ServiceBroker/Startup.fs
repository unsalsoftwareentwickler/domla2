﻿namespace D2.ServiceBroker

open System
open D2.Common
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.AspNetCore.HttpOverrides
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.DependencyInjection
open System.Collections.Generic
open System.IdentityModel.Tokens.Jwt

type Startup private () =
    
    new (configuration: IConfiguration) as this =
        Startup() then
        this.Configuration <- configuration

    member this.ConfigureServices(services: IServiceCollection) =
        services.AddMvc() |> ignore
        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear()
        services
            .AddCors(
                fun options -> options.AddPolicy(
                                   "default",
                                   fun policy ->
                                       policy.AllowAnyOrigin()
                                             .AllowAnyHeader()
                                             .AllowAnyMethod()
                                    |> ignore
                            )
            )
            .AddServiceAuthentication()
            |> ignore

    member this.Configure(app: IApplicationBuilder, env: IHostingEnvironment) =

        let services = CompositionRoot.Storage.routes "Domla2" 1
                       |>
                       Async.RunSynchronously
                       |>
                       Seq.map(Persistence.Mapper.ServiceI.fromService)
        
        ServiceConnection.initializeConnectors services
        
        if env.EnvironmentName <> "Development" then
            app.UseForwardedHeaders(
                ForwardedHeadersOptions(
                    ForwardedHeaders = (
                        ForwardedHeaders.All
                    )
                )
            )
            |> ignore
        
        app
            .UseCors("default")
            .UseAuthentication()
            .UseMvc()
            |> ignore

    member val Configuration : IConfiguration = null with get, set