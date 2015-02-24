namespace SFA.Apprenticeships.Web.Candidate.Providers
{
    using System;
    using Application.Interfaces.Candidates;
    using Domain.Entities.Communication;
    using Domain.Interfaces.Mapping;
    using ViewModels.Home;

    public class HomeProvider : IHomeProvider
    {
        private readonly ICandidateService _candidateService;
        private readonly IMapper _mapper;

        public HomeProvider(ICandidateService candidateService, IMapper mapper)
        {
            _candidateService = candidateService;
            _mapper = mapper;
        }

        public bool SendContactMessage(Guid? candidateId, ContactMessageViewModel viewModel)
        {
            try
            {
                var candidate = _mapper.Map<ContactMessageViewModel, ContactMessage>(viewModel);
                candidate.UserId = candidateId;
                _candidateService.SendContactMessage(candidate);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}