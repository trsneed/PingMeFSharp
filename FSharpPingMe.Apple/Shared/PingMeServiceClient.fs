﻿module PingMeServiceClient

    open System
    open System.Net
    open Helpers
    open Newtonsoft.Json

    let TimeoutSeconds = 30
    let user_agent = "FSharp Client"

    type PingMeServiceClient(baseUrl, apiKey) =

        let auth_api_url = baseUrl + "/tables/urls/"

        member this.SetupRequest requestMethod (url: string) =
            let request = WebRequest.Create url
            request.Method <- requestMethod

            match request with
            | :? System.Net.HttpWebRequest as request -> request.UserAgent <- user_agent
            | _ -> ()

            request.Credentials <- new NetworkCredential ( apiKey, "")
            request.PreAuthenticate <- true
            request.Headers.Add("Authorization", "Basic " + Convert.ToBase64String (System.Text.Encoding.UTF8.GetBytes(apiKey + ":")))
            request.Timeout <- TimeoutSeconds * 1000
            if requestMethod = "POST" || requestMethod = "PUT" then
                request.ContentType <- "application/json"

            request

        member this.DoRequest endpoint requestMethod body = async {

            let request = this.SetupRequest requestMethod endpoint

            match body with
            | "" -> ()
            | _ -> let bytes = encoding.GetBytes (body.ToString ())
                   request.ContentLength <- int64 bytes.Length
                   use! st = request.GetRequestStreamAsync() |> Async.AwaitTask
                   st.Write(bytes, 0, bytes.Length)
            try
                use! response = request.GetResponseAsync() |> Async.AwaitTask
                use sr = new System.IO.StreamReader (response.GetResponseStream(), encoding) 
                let! data = sr.ReadToEndAsync() |> Async.AwaitTask
                return data |> Success
            with | :? WebException as e -> return e.Message |> Failure }
