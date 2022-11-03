# Blazor_WASM_Server
Blazor Hybrid WASM and Server
Refer:  https://stackoverflow.com/questions/74230471/blazor-wasm-hosted-mixed-with-blazor-server-pages/74230472#74230472

This repository provides a solution how it is possible to mix Blazor Server and Web Assembly pages in multiple ways in the same solution.  The goal is to enable WASM or Server pages to be created and served at will from a single solution.

The goal was also to ensure that all parts of the solution are authenticated using Azure AD.

The solution has 4 projects:
 - Blazor_WASM_Server.ClientServer   (based upon  Sdk="Microsoft.NET.Sdk.Web")
 - Blazor_WASM_Server.ClientWASM     (based upon Sdk="Microsoft.NET.Sdk.BlazorWebAssembly")
 - Blazor_WASM_Server.Host           (based upon Sdk="Microsoft.NET.Sdk.Web")  This has been renamed from the template project name of Server for clarity
 - Blazor_WASM_Server.Shared         (based upon Sdk="Microsoft.NET.Sdk")
 
I have tried to annotate changes from the template solution using a common string of ***, so if you search for this you will find the annotations.

There are a fair few subtleties, that were required to get everything working - especially around the authentication.  I tried to annotate these where I could.

The main issue that I'd like to resolve in this solution is that it requires 2 signins.  The first for the WASM page, which is understandable.  The second is at the time of first navigation to a Server page.  I will eventually work out how to implment this as a single login.

I used some concepts that were contained in the following @ShaunCurtis repo - particularly around the use of the MapWhen code in the Host\Program.cs.
https://github.com/ShaunCurtis/AllinOne

In order to run this solution, 

1. Register 2 applications in Azure AD (one web and the other SPA)
2. Update the registration details in 
     - Blazor_WASM_Server.ClientWASM/wwwroot/appsettings.json
     - Blazor_WASM_Server.Host/appsettings.json
3. Set Blazor_WASM_Server.Host as the Startup project and Debug the solution
4. Press the [Log in WASM] button and enter your Microsoft credentials
![image](https://user-images.githubusercontent.com/15906406/199649857-7798ed61-0bf2-410a-b596-633807b992f1.png)

5. Navigate through the side bar to view the different page scenarios.  Show the Dev Tools to inspect the html.
![image](https://user-images.githubusercontent.com/15906406/199652388-88c0a5e8-db04-4600-9884-c6af1513d5bb.png)

![image](https://user-images.githubusercontent.com/15906406/199652307-2ca28d27-597a-4fb1-840e-ccc36c6ff810.png)




