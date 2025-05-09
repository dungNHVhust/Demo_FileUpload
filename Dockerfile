FROM mcr.microsoft.com/dotnet/framework/aspnet:4.8-windowsservercore-ltsc2019

WORKDIR /inetpub/wwwroot

# Copy the published files
COPY Demo_UpWebShell/ .

# Set the port
EXPOSE 8082

# Configure IIS to use port 8082
RUN powershell -Command \
    Import-Module IISAdministration; \
    $site = Get-IISSite -Name 'Default Web Site'; \
    $binding = $site.Bindings[0]; \
    $binding.BindingInformation = '*:8082:'; \
    Set-IISSite -Name 'Default Web Site' -BindingInformation $binding.BindingInformation -Protocol $binding.Protocol

# Start IIS
ENTRYPOINT ["C:\\ServiceMonitor.exe", "w3svc"] 