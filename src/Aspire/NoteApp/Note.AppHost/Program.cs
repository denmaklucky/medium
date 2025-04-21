using Amazon;

var builder = DistributedApplication.CreateBuilder(args);

var awsConfig = builder.AddAWSSDKConfig()
    .WithProfile("aspire")
    .WithRegion(RegionEndpoint.EUCentral1);

var cloudFormationStack = builder.AddAWSCloudFormationTemplate("NoteAppResources", "resources.template")
    .WithReference(awsConfig);

var cache = builder.AddRedis("cache");

var apiService = builder.AddProject<Projects.Note_ApiService>("apiservice")
    .WithReference(cloudFormationStack)
    .WaitFor(cloudFormationStack);

builder.AddProject<Projects.Note_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(cache)
    .WaitFor(cache)
    .WithReference(apiService)
    .WaitFor(apiService);

builder.Build().Run();