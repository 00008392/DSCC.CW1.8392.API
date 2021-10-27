dotnet restore /var/www/DCSS_8392_API/DSCC.8392.API/DSCC.8392.API.csproj
dotnet publish -c release /var/www/DCSS_8392_API/DSCC.8392.API/DSCC.8392.API.csproj -o /var/www/DCSS_8392_API_publish/
systemctl enable 8392_API.service
systemctl start 8392_API.service