﻿namespace FSharpPingMe.Apple

open System
open System.Drawing

open MonoTouch.UIKit
open MonoTouch.Foundation

[<Register ("FSharpPingMe_AppleViewController")>]
type FSharpPingMe_AppleViewController () =
    inherit UIViewController ()

    // Release any cached data, images, etc that aren't in use.
    override this.DidReceiveMemoryWarning () =
        base.DidReceiveMemoryWarning ()

    // Perform any additional setup after loading the view, typically from a nib.
    override this.ViewDidLoad () =
        base.ViewDidLoad ()

    // Return true for supported orientations
    override this.ShouldAutorotateToInterfaceOrientation (orientation) =
        orientation <> UIInterfaceOrientation.PortraitUpsideDown

