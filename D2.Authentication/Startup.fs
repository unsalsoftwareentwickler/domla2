namespace D2.Authentication

open IdentityServer4.Models
open IdentityServer4.Stores
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.DependencyInjection


type Startup private () =
    new (configuration: IConfiguration) as this =
        Startup() then
        this.Configuration <- configuration

    // This method gets called by the runtime. Use this method to add services to the container.
    member this.ConfigureServices(services: IServiceCollection) =
        let connectionOptions = {
            Database = this.Configuration.GetValue<string>("Database:Name");
            Host = this.Configuration.GetValue<string>("Database:Host");
            User = this.Configuration.GetValue<string>("Database:User");
            Password = this.Configuration.GetValue<string>("Database:Password");
            Port = this.Configuration.GetValue<int>("Database:Port");
        }
        
        let setupStorage = Storage.storages.setupStorage connectionOptions;

        setupStorage.initialize () |> Async.RunSynchronously

        let persistedGrantStore = Storage.storages.persistedGrantStorage connectionOptions
        let resourceStore = Storage.storages.resourceStorage connectionOptions
        let clientStore = Storage.storages.clientStorage connectionOptions
        let userStore = Storage.storages.userStorage connectionOptions

        services
            .AddSingleton(clientStore)
            .AddSingleton(resourceStore)
            .AddSingleton(persistedGrantStore)
            .AddSingleton(userStore)
            .AddScoped<IPersistedGrantStore, PersistedGrantStore>()
            .AddSingleton<TokenCleanup>()
            .AddIdentityServer()
            .AddClientStore<ClientStore>()
            .AddCorsPolicyService<CorsPolicyService>()
            .AddProfileService<ProfileService>()
            .AddResourceStore<ResourceStore>()
        |> ignore

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    member this.Configure(app: IApplicationBuilder, env: IHostingEnvironment, appLifetime : IApplicationLifetime) =
        app.UseIdentityServer() |> ignore
        
        let tokenCleanup = app.ApplicationServices.GetService<TokenCleanup>()
        appLifetime.ApplicationStarted.Register (fun () -> tokenCleanup.Start ()) |> ignore
        appLifetime.ApplicationStopping.Register (fun () -> tokenCleanup.Stop ()) |> ignore


    member val Configuration : IConfiguration = null with get, set