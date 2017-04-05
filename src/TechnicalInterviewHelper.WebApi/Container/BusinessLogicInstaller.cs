﻿namespace TechnicalInterviewHelper.WebApi.Container
{    
    using System.Configuration;
    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.SubSystems.Configuration;
    using Castle.Windsor;
    using Services;
    using TechnicalInterviewHelper.Model;

    public class BusinessLogicInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                //// Queries
                Component.For<IQueryRepository<CompetencyCatalog, string>>()
                         .ImplementedBy<DocumentDbQueryRepository<CompetencyCatalog, string>>()
                         .DependsOn(Dependency.OnValue("collectionId", ConfigurationManager.AppSettings["CompetencyCollectionId"])),
                Component.For<IQueryRepository<TemplateCatalog, string>>()
                         .ImplementedBy<DocumentDbQueryRepository<TemplateCatalog, string>>()
                         .DependsOn(Dependency.OnValue("collectionId", ConfigurationManager.AppSettings["TemplateCollectionId"])),
                Component.For<IExerciseQueryRepository>()
                         .ImplementedBy<ExerciseDocumentDbQueryRepository>()
                         .DependsOn(Dependency.OnValue("collectionId", ConfigurationManager.AppSettings["ExerciseCollectionId"])),
                Component.For<IQuestionQueryRepository>()
                         .ImplementedBy<QuestionDocumentDbQueryRepository>()
                         .DependsOn(Dependency.OnValue("collectionId", ConfigurationManager.AppSettings["QuestionCollectionId"])),
                Component.For<ISkillMatrixQueryRepository>()
                         .ImplementedBy<SkillMatrixDocumentDbQueryRepository>()
                         .DependsOn(Dependency.OnValue("collectionId", ConfigurationManager.AppSettings["SkillCollectionId"])),
                //// Commands
                Component.For<ICommandRepository<InterviewCatalog>>()
                         .ImplementedBy<DocumentDbCommandRepository<InterviewCatalog>>()
                         .DependsOn(Dependency.OnValue("collectionId", ConfigurationManager.AppSettings["InterviewCollection"])),
                Component.For<ICommandRepository<TemplateCatalog>>()
                         .ImplementedBy<DocumentDbCommandRepository<TemplateCatalog>>()
                         .DependsOn(Dependency.OnValue("collectionId", ConfigurationManager.AppSettings["TemplateCollectionId"]))
                );
        }
    }
}