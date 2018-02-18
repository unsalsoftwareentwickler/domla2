namespace D2.Authentication

module Program =

    open D2.Common
    open Microsoft.AspNetCore
    open Microsoft.AspNetCore.Hosting
    open NLog.Web

    let exitCode = 0

    let BuildWebHost args =
        WebHost
            .CreateDefaultBuilder(args)
            .UseKestrel(fun options -> ServiceConfiguration.configureKestrel options)
            .UseWebRoot("wwwroot")
            .UseStartup<Startup>()
            .Build()

    [<EntryPoint>]
    let main args =
        BuildWebHost(args).Run()

        exitCode
