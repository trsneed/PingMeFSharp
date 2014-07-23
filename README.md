PingMeFSharp
============

A demonstration of using fsharp to create a simple client to monitor website status

This is a app based on a side-project I am working on in c-sharp and thought I would look at how to make it work in fsharp.

This app pings a site every 10 minutes and texts the person associated with the website, if the status is not 200.
I used xamarin, twillio, and Microsoft Azure's Mobile Services to create and track this application. There are a handful of issues
I am still working through ,including:

- ~~handling updates to existing websites.~~

- Using fsharp's async features (help me please)

- ~~Having a way to refresh the app without relaunching~~

- deleting a website

In the c-sharp version there is user
authentication to and not every site registered is shown. I just wanted to see how fsharp actually works with xamarin.

Writings about fsharp and xamarin:

[Introduction](http://www.trsneed.com/building-a-f-app-using-xamarin/)

[Adding a refresh button](http://www.trsneed.com/add-a-refresh-navigation-button-to-your-xamarin-f-app/)

![](http://images.trsneed.com/blogstuff/fsharp/Screen%20Shot%202014-07-21%20at%206.47.32%20AM.png)
