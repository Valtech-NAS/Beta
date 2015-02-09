﻿namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Builders
 {
     using Domain.Entities.Vacancies.Apprenticeships;

     public class ApprenticeshipSummaryBuilder
     {
         private readonly int _vacancyId;

         public ApprenticeshipSummaryBuilder(int vacancyId)
         {
             _vacancyId = vacancyId;
         }

         public ApprenticeshipSummary Build()
         {
             var summary = new ApprenticeshipSummary
             {
                 Id = _vacancyId
             };

             return summary;
         }
     }
 }