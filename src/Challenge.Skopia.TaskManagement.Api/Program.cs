/* Ideias retiradas de:
 * https://youtu.be/vhNhcuht0J0
 * https://youtu.be/VgjHQvprRy0
 */
await WebApplication.CreateBuilder(args)
    .RegisterServices()
    .Build()
    .UseServices()
    .RunAsync();