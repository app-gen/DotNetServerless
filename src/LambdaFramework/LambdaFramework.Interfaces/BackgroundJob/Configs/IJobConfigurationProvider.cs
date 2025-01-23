namespace Command.Job;

public interface IJobConfigurationProvider
{
    List<JobTopicConfiguration> GetJobConfigurationsAsync();
}


public class MockJobConfigurationProvider : IJobConfigurationProvider
{
    public List<JobTopicConfiguration> GetJobConfigurationsAsync()
    {
        var a = new MockJObs();
        return a.GetMockJobs();
    }
}

