namespace SFA.Apprenticeships.Application.Candidate.Strategies
{
    using System;
    using Domain.Entities.Applications;

    public class CreateApplicationStrategy : ICreateApplicationStrategy
    {
        public ApplicationDetail CreateApplication(Guid candidateId, int vacancyId)
        {
            //todo: check appl repo for existing application
            // if exists and status = submitted (or any other "post submission" state) then cannot create new one
            // if exists and vacancy has expired then return it so user can continue
            // if exists and status = draft then return it so user can continue
            //todo: create a new application entity, set template info, set vacancy summary info, save to app repo as "draft", return it

            throw new NotImplementedException();
        }
    }
}
