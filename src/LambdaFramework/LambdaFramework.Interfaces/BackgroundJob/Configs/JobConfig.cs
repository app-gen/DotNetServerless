using Command.Job;
using MsgQueue.Client.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class JobConfig
{
//    public List<TopicConfiguration> Topics1 { get; set; } = new();

    public List<JobTopicConfiguration> Topics { get; set; } = new();


    public int ErrorDelaySeconds { get; set; } = 115;


    public int PollIntervalSeconds { get; set; } = 1160; // Time to wait when queue is empty

}

public class MockJObs()
{
    public JobTopicConfiguration GetMockJob1()
    {
        var j = new JobTopicConfiguration();
        j.AssemblyName = "";
        j.TypeName = "Test";
        j.CronExpression = "Test";

        return j;
    }

    public JobTopicConfiguration GetMockJob2()
    {
        var j = new JobTopicConfiguration();
        return j;
    }

    public List<JobTopicConfiguration> GetMockJobs()
    {
        var j = new List<JobTopicConfiguration>();
        //j.Add(GetMockJob2());
        j.Add(GetMockJob1());

        return j;
    }



}