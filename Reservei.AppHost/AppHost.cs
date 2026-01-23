using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var postgresServer = builder.AddPostgres("postgres");

var sqldb = postgresServer.AddDatabase("sqldb");

builder.AddProject<Reservei_Api>("api")
    .WithReference(sqldb)
    .WaitFor(postgresServer);

builder.Build().Run();
