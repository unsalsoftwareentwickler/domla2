﻿namespace D2.ServiceBroker

open D2.Common
open Microsoft.AspNetCore.Authorization
open Microsoft.AspNetCore.Mvc

[<Route("[controller]")>]
type AppsController () =
    inherit Controller()

    [<Authorize>]
    [<HttpGet("{name}/{version:regex(^(\\d\\d)$)}/{service}")>]
    member this.Get(name : string, version : int, service : string) =
        Response.emit (ResolveRoutes.endpoints name version service)

    [<Authorize>]
    [<HttpPut("{name}/{version:regex(^(\\d\\d)$)}/register")>]
    member this.Put(name : string, version : int) =
        let service = this.Request.Body.AsUtf8String()
        Response.confirm (ResolveRoutes.register name version service)

    [<Authorize>]
    [<HttpGet("{name}/{version:regex(^(\\d\\d)$)}")>]
    member this.Get(name : string, version : int) =
        Response.emit (ResolveRoutes.routes name version)

    [<Authorize>]
    [<HttpGet("list")>]
    member this.Get() =
        Response.emit ResolveRoutes.applications
