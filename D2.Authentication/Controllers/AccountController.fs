﻿namespace D2.Authentication

open IdentityServer4.Events
open IdentityServer4.Extensions
open IdentityServer4.Services
open IdentityServer4.Validation
open Microsoft.AspNetCore.Authentication
open Microsoft.AspNetCore.Http
open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.Logging

[<Route("[controller]")>]
[<SecurityHeaders>]
type AccountController
     (
         interaction : IIdentityServerInteractionService,
         users : UserStorage,
         events : IEventService,
         grantStore : PersistedGrantStorage,
         logger : ILogger<AccountController>,
         authorizer : Authorizer,
         requestValidator : IUserInfoRequestValidator
     ) =
    inherit Controller()

    [<HttpGet("login")>]
    member this.Get (returnUrl : string) =
        async {
            let request = sprintf "/app/login?returnUrl=%s" (returnUrl.Base64UrlEncode())
            logger.LogDebug request
            return RedirectResult (request)
        }
        |> Async.StartAsTask

    [<HttpPost("login")>]
    [<AutoValidateAntiforgeryToken>]
    member this.Post (model : LoginInputModel) =
        async {
            logger.LogDebug (sprintf "login requested, starting to authenticate %s" model.Username)
            let! result = users.findUser model.Username model.Password
            let parameters = this.HttpContext.Request.Query.AsNameValueCollection ()
            logger.LogDebug (parameters.ToString())
            match result with
            | None   -> logger.LogWarning (sprintf "failed to authenticate %s" model.Username)
                        return StatusCodeResult (StatusCodes.Status403Forbidden)
                        :> IActionResult
            
            | Some s -> logger.LogDebug (sprintf "authentication for %s succeeded" model.Username)
                        events.RaiseAsync(new UserLoginSuccessEvent(s.Login, s.Id.ToString("N"), s.Login))
                        |> Async.AwaitTask
                        |> Async.RunSynchronously

                        match interaction.IsValidReturnUrl model.ReturnUrl with
                        | true  -> this.HttpContext.SignInAsync (s.Id.ToString("N"), s.Login)
                                   |> Async.AwaitTask
                                   |> Async.RunSynchronously
                                   return authorizer.Authorize (this.HttpContext) (model.ReturnUrl.HtmlDecode ())

                        | false -> return StatusCodeResult (StatusCodes.Status403Forbidden)
                                   :> IActionResult
        }
        |> Async.StartAsTask

    [<HttpGet("logout")>]
    member this.Logout (logoutId : string) =
        async {
            let token = match this.HttpContext.Request.Headers.TryGetValue ("Authorization") with
                        | true, value -> value.[0].Substring(7)
                        | false, _    -> null
            let! validation = requestValidator.ValidateRequestAsync token
                              |> Async.AwaitTask
            
            match validation.IsError with
            | false -> let user = validation.Subject
                       do! this.HttpContext.SignOutAsync()
                           |> Async.AwaitTask

                       do! events.RaiseAsync(new UserLogoutSuccessEvent(user.GetSubjectId(), user.GetDisplayName()))
                           |> Async.AwaitTask

                       do! users.updateActive (user.Identity.GetSubjectId()) false
                       do! grantStore.removeAll (user.Identity.GetSubjectId()) "interactive"
        
                       let result = LogoutResponseModel (
                                        url = this.HttpContext.Request.Scheme
                                        +
                                        "://"
                                        +
                                        this.HttpContext.Request.Host.ToUriComponent()
                                        +
                                        "/app/goodbye"
                                    )
                       return JsonResult (result)
                              :> IActionResult

            
            | true  -> return StatusCodeResult (StatusCodes.Status200OK)
                              :> IActionResult
        }
        |> Async.StartAsTask
