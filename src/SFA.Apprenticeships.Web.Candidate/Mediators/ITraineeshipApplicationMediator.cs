﻿namespace SFA.Apprenticeships.Web.Candidate.Mediators
{
    using System;
    using ViewModels.Applications;

    public interface ITraineeshipApplicationMediator
    {
        MediatorResponse<TraineeshipApplicationViewModel> Apply(Guid candidateId, int vacancyId);

        MediatorResponse<TraineeshipApplicationViewModel> Submit(Guid candidateId, int vacancyId, TraineeshipApplicationViewModel viewModel);

        MediatorResponse<TraineeshipApplicationViewModel> AddEmptyQualificationRows(TraineeshipApplicationViewModel viewModel);

        MediatorResponse<TraineeshipApplicationViewModel> AddEmptyWorkExperienceRows(TraineeshipApplicationViewModel viewModel);

        MediatorResponse<WhatHappensNextViewModel> WhatHappensNext(Guid candidateId, int vacancyId, string vacancyReference, string vacancyTitle);
    }
}