namespace TechnicalInterviewHelper.Tests.Common
{
    using System;
    using System.Collections.Generic;
    using Model;

    /// <summary>
    /// The resources.
    /// </summary>
    public class Resources
    {
        public static readonly int TestCompetencyId;
        public static readonly string TestCompetencyName;
        public static readonly Competency TestCompetency;
        public static readonly int TestDomainId;
        public static readonly string TestDomainName;
        public static readonly Domain TestDomain;
        public static readonly int TestLevelId;
        public static readonly string TestLevelName;
        public static readonly Level TestLevel;
        public static readonly int TestSkillId;
        public static readonly Skill TestSkill;

        /// <summary>
        /// Initializes static members of the <see cref="Resources"/> class.
        /// </summary>
        static Resources()
        {
            TestCompetencyId = (int)(DateTime.UtcNow.Ticks % int.MaxValue);
            TestCompetencyName = Guid.NewGuid().ToString("D");
            TestDomainId = (int)(DateTime.UtcNow.Ticks % int.MaxValue);
            TestDomainName = Guid.NewGuid().ToString("D");
            TestLevelId = (int)(DateTime.UtcNow.Ticks % int.MaxValue);
            TestLevelName = Guid.NewGuid().ToString("D");
            TestCompetency = new Competency()
            {
                Id = TestCompetencyId.ToString(),
                Name = TestCompetencyName
            };
            TestLevel = new Level()
            {
                Id = TestLevelId.ToString(),
                Name = TestLevelName,
                CompetencyId = TestCompetencyId,
                Description = string.Empty
            };
            TestDomain = new Domain()
            {
                Id = TestDomainId.ToString(),
                Name = TestDomainName,
                CompetencyId = TestCompetencyId,
                LevelId = TestLevelId
            };
            TestSkill = new Skill()
            {
                Id = TestSkillId.ToString(),
                Description = TestDomainName,               
                Topics = new List<Topic>()
            };
        }
    }
}