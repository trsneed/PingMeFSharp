namespace FSharpPingMe.Apple

open System
open MonoTouch.UIKit
open MonoTouch.Foundation
open System.Drawing
open Helpers
open WebService
open Website

type AddWebsiteViewController(website:Website, isNew:bool) =
    inherit UIViewController ()
    new() = new AddWebsiteViewController (Website.CreateWebsite(), true)
    override this.ViewDidLoad () =
        base.ViewDidLoad ()

        let addView = new UIView(this.View.Bounds, BackgroundColor = UIColor.White)

        let description = new UITextField(RectangleF(20.f, 64.f, 280.f, 50.f),
                                          Text = website.Url,
                                          Placeholder = "website url")
        addView.Add description

        let phoneNumber = new  UITextField(RectangleF(20.f, 114.f, 280.f, 50.f),
                                          Text = website.Phone,
                                          KeyboardType = UIKeyboardType.NumberPad,
                                          Placeholder = "phone with areacode")

        addView.Add phoneNumber

        let statusLabel = new UILabel(RectangleF(20.f, 164.f, 100.f, 30.f), Text = "Site Status") 
        addView.Add statusLabel

        let statusDescription = new UILabel(RectangleF(160.f, 164.f, 100.f, 30.f), Text = website.Status)
        addView.Add statusDescription

        let addedLabel = new UILabel(RectangleF(20.f, 214.f, 280.f, 50.f))
        addView.Add addedLabel

        let addUpdateButton = UIButton.FromType(UIButtonType.RoundedRect, Frame = RectangleF(20.f, 214.f, 280.f, 50.f))

        addUpdateButton.TouchUpInside.AddHandler
            (fun _ _ -> 
                match isNew with 
                | true -> 
                    WebService.Shared.AddWebsite(description.Text, phoneNumber.Text) |> Async.RunSynchronously
                    addedLabel.Text <- "Added!"
                | false -> 
                    WebService.Shared.UpdateWebsite(description.Text, phoneNumber.Text, website.id) |> Async.RunSynchronously
                    addedLabel.Text <- "Updated!"
                )

        addUpdateButton.SetTitle("Save", UIControlState.Normal)
        addView.Add addUpdateButton

        description.TouchDown.AddHandler (fun _ _ -> addedLabel.Text <- "")

        this.View.Add addView
