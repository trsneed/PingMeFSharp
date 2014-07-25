namespace FSharpPingMe.Apple

open MonoTouch.UIKit
open MonoTouch.Foundation
open System
open System.Collections.Generic
open System.Drawing
open System.IO
open Helpers
open WebService
open Website

type PingMeDataSource(websitesource: Website list, navigation: UINavigationController) = 
    inherit UITableViewSource()
    let sites = ResizeArray(websitesource)
    let cellIdentifier = "WebsiteCell"

    override x.RowsInSection(view, section) = sites.Count
    override x.GetCell(view, indexPath) = 
        let t = sites.[indexPath.Item]
        let cell =
            match view.DequeueReusableCell cellIdentifier with 
            | null -> new UITableViewCell(UITableViewCellStyle.Value1, cellIdentifier)
            | cell -> cell
        cell.TextLabel.Text <- t.Url
        cell.DetailTextLabel.Text <- t.Status 
        cell
    override x.RowSelected (tableView, indexPath) = 
        tableView.DeselectRow (indexPath, false)
        navigation.PushViewController (new AddWebsiteViewController(sites.[indexPath.Item], false), true)
    override x.CanEditRow (view, indexPath) = true
    override x.CommitEditingStyle(view, editingStyle, indexPath) = 
        match editingStyle with 
        | UITableViewCellEditingStyle.Delete ->
            WebService.Shared.DeleteWebsite(websitesource.[indexPath.Item].id) |> Async.RunSynchronously
            sites.RemoveAt(indexPath.Item)
            view.DeleteRows([|indexPath|], UITableViewRowAnimation.Fade)
        | _ -> Console.WriteLine "CommitEditingStyle:None called"

type PingMeViewController () as this =
    inherit UIViewController ()

    let refresh() = 
        let result = WebService.Shared.GetWebsites()
        new PingMeDataSource(result, this.NavigationController)

    let table = new UITableView()

    let refreshControl = new UIRefreshControl()

    do this.Title <- "Ping Me F#"

    override this.ViewDidLoad () =
        base.ViewDidLoad ()
        let addNewTask = 
            EventHandler(fun sender eventargs -> 
                this.NavigationController.PushViewController (new AddWebsiteViewController(), true))
        let refreshTask =
            EventHandler(fun sender eventargs -> 
                table.Source<- refresh()
                table.ReloadData())

        this.NavigationItem.SetRightBarButtonItem (new UIBarButtonItem(UIBarButtonSystemItem.Add, addNewTask), false)
        this.NavigationItem.SetLeftBarButtonItem(new UIBarButtonItem(UIBarButtonSystemItem.Refresh, refreshTask), true)
        table.Frame <- this.View.Bounds
        this.View.Add table 

    override this.ViewWillAppear animated =
        base.ViewWillAppear animated
        table.Source <- refresh()
        table.ReloadData()
    
