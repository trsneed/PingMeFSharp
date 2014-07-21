module WebService

    open System
    open System.IO
    open System.Net
    open System.Runtime.Serialization
    open Newtonsoft.Json

    open Helpers
    open PingMeServiceClient
    open Website

    let mutable CurrentWebsite : Website = Website.CreateWebsite()

    let ReadResponseText (req : HttpWebRequest) =
        use response = req.GetResponse ()
        use s = response.GetResponseStream()
        use r = new StreamReader (s, System.Text.Encoding.UTF8)
        r.ReadToEnd()

    let CreateRequest request =
        let request = WebRequest.Create ("https://pingme.azure-mobile.net/tables/urls/"+request) :?> HttpWebRequest
        request.Method <- "GET"
        request.ContentType <- "application/json"
        request.Accept <- "application/json"
        request

    type WebService() =
        let client = new PingMeServiceClient ("https://pingme.azure-mobile.net/", "uXDzGggpuCtjxHCQupoSTDeHkGlUia66")
        let mutable websites = [||]

        member this.GetWebsitesAsync ():Async<Website[]> = async {
            match websites with
            | [||] -> try
                          return! System.Threading.Tasks.Task.Run(fun _->
                              let response = ReadResponseText (CreateRequest (""))
                              websites <- JsonConvert.DeserializeObject<Website array>(response)
                              websites ) |> Async.AwaitTask
                      with | e-> Console.WriteLine e
                                 return [||]
            | _ -> return websites }

        member this.AddWebsite (url:string, phone:string) = async {
                return! System.Threading.Tasks.Task.Run(fun _->
                    let website = Website.CreateWebsite(url, phone)
                    let json = CurrentWebsite.GetJson website
                    let content = encoding.GetBytes json
                    let request = CreateRequest ("")
                    request.Method <- "POST" 
                    request.ContentLength <- int64 content.Length
                     
                    use s = request.GetRequestStream()
                    s.Write(content, 0, content.Length)

                    ReadResponseText request) |> Async.AwaitTask }

        member this.UpdateWebsite (url:string, phone:string, id: string) = async {
                return! System.Threading.Tasks.Task.Run(fun _->
                    let website = Website.CreateWebsite(url, phone, "", id)
                    let json = CurrentWebsite.GetJson website
                    let content = encoding.GetBytes json
                    let request = CreateRequest (website.id)
                    request.Method <- "PATCH" 
                    request.ContentLength <- int64 content.Length
                     
                    use s = request.GetRequestStream()
                    s.Write(content, 0, content.Length)

                    ReadResponseText request) |> Async.AwaitTask }

    let Shared = new WebService()