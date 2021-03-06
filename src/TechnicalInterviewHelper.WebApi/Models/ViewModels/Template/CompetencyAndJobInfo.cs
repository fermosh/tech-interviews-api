﻿namespace TechnicalInterviewHelper.WebApi.Model
{ 
    public class CompetencyAndJobInfo
    {
        public string CompetencyName { get; set; }

        public string DomainName { get; set; }

        public string JobDescription { get; set; }

        public CompetencyAndJobInfo()
        {
            CompetencyName = string.Empty;
            DomainName = string.Empty;
            JobDescription = string.Empty;
        }
    }
}