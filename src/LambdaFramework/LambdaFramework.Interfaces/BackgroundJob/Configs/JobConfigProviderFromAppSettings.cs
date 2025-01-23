using Microsoft.Extensions.Configuration;

namespace Command.Job;

public class JobConfigProviderFromAppSettings : IJobConfigurationProvider
{
    private readonly IConfiguration _configuration;

    public JobConfigProviderFromAppSettings(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public List<JobTopicConfiguration> GetJobConfigurationsAsync()
    {
        try
        {
            // Bind the JobConfigurations section of appsettings.json to a JobConfig object
            var jobConfig = new JobConfig();

            var s = _configuration.GetSection("JobConfigurations");

            _configuration.GetSection("JobConfigurations").Bind(jobConfig);

            return jobConfig.Topics;
        }
        catch (Exception ex) { 

            return new List<JobTopicConfiguration>();
        }
    }
}

