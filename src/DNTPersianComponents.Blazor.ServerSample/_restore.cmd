dotnet tool update -g Microsoft.Web.LibraryManager.Cli
libman install bootstrap --provider unpkg --destination wwwroot/lib/bootstrap
libman install bootstrap-icons --provider unpkg --destination wwwroot/lib/bootstrap-icons
libman install samim-font --provider unpkg --destination wwwroot/lib/samim-font
libman restore
dotnet restore
pause