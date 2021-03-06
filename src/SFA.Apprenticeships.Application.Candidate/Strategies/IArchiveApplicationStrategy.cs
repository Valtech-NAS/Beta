﻿namespace SFA.Apprenticeships.Application.Candidate.Strategies
{
    using System;

    public interface IArchiveApplicationStrategy
    {
        void ArchiveApplication(Guid candidateId, int vacancyId);
        void UnarchiveApplication(Guid candidateId, int vacancyId);
    }
}
