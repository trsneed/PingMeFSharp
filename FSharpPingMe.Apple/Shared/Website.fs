module Website

    open System.Linq
    open Helpers
    open Newtonsoft.Json
    open System.Runtime.Serialization 

    [<DataContract;CLIMutable>]
    [<JsonObject(MemberSerialization=MemberSerialization.OptOut)>]
    type Website = 
        { Url: string
          Status: string
          Phone: string 
          id: string}

        member this.GetJson (website:Website) =
            { Phone = website.Phone 
              Url = website.Url
              Status = website.Status
              id = website.id}
                |> JsonConvert.SerializeObject

          static member CreateWebsite(?Url, ?Phone, ?Status, ?Id) = 
            let initMember x = fun y-> Option.fold (fun state param->param) y x
            { Phone = initMember Phone ""
              Url = initMember Url ""
              Status = initMember Status ""
              id = initMember Id ""}

   