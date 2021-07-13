# Blazor_WASM_Server
Blazor Hybrid WASM and Server
Refer:  https://stackoverflow.com/questions/68362521/blazor-app-run-as-wasm-or-server-cannot-get-dependency-injection-working-in-ser

...this Repo has been updated with solution from Shaun Curtis in SO thread.

This is an attempt to create a single Blazor app that can either be run as Webassembly (hosted) or Server.

Goal is to be able to have a single switch in Server app that enables development/debugging to use Blazor Server due to ease of use and then use WebAssembly for final deployment.

Issue is that cannot get Dependency Injection working in Server mode.

Process to create this example:
1. Blazor WASM app was created from standard template.
2. Generally followed example "HybridBlazor".  https://itnext.io/blazor-switching-server-and-webassembly-at-runtime-d65c25fd4d8

[Blazor_WASM_Server.Server]

3. Added Hybrid options flag in [Blazor_WASM_Server.Server].HybridOptions    (in Startup file)
4. Added _Host page in [Blazor_WASM_Server.Server], with options for either WASM or Server

[Blazor_WASM_Server.Client]

5. Added WebApiClient to be used as a Typed HttpClient
6. Register the service in Program.Main
7. Inject WebApiClient into Index.razor and confirm it has been set

Process to run:

8. Choose mode as WebAssembly in [Blazor_WASM_Server.Server].HybridOptions    (in Startup file)
9. Execute Server app and observe that app runs as expected
10. Choose mode as ServerSide, execute and observe error 
      
      "InvalidOperationException: 
      
      Cannot provide a value for property 'api' on type 'Blazor_WASM_Server.Client.Pages.Index'. 
      
      There is no registered service of type 'Blazor_WASM_Server.Client.WebApiClient'.


