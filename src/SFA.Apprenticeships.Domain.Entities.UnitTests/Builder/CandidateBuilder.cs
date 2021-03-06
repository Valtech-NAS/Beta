﻿namespace SFA.Apprenticeships.Domain.Entities.UnitTests.Builder
{
    using System;
    using Candidates;
    using Users;

    public class CandidateBuilder
    {
        private readonly Guid _candidateId;
        private string _phoneNumber;
        private string _mobileVerificationCode;
        private bool _allowEmail;
        private bool _allowMobile;
        private bool _verifiedMobile;
        private bool _allowEmailMarketing;
        private bool _allowSmsMarketing;

        public CandidateBuilder(Guid candidateId)
        {
            _candidateId = candidateId;
        }

        public CandidateBuilder PhoneNumber(string phoneNumber)
        {
            _phoneNumber = phoneNumber;
            return this;
        }

        public CandidateBuilder AllowEmail(bool allowEmail)
        {
            _allowEmail = allowEmail;
            return this;
        }

        public CandidateBuilder AllowMobile(bool allowMobile)
        {
            _allowMobile = allowMobile;
            return this;
        }

        public CandidateBuilder VerifiedMobile(bool verifiedMobile)
        {
            _verifiedMobile = verifiedMobile;
            return this;
        }

        public CandidateBuilder MobileVerificationCode(string  mobileVerificationCode)
        {
            _mobileVerificationCode = mobileVerificationCode;
            return this;
        }

        public CandidateBuilder AllowEmailMarketing(bool allowEmailMarketing)
        {
            _allowEmailMarketing = allowEmailMarketing;
            return this;
        }

        public CandidateBuilder AllowMobileMarketing(bool allowSmsMarketing)
        {
            _allowSmsMarketing = allowSmsMarketing;
            return this;
        }

        public Candidate Build()
        {
            var candidate = new Candidate
            {
                EntityId = _candidateId,
                RegistrationDetails = new RegistrationDetails
                {
                    PhoneNumber = _phoneNumber
                },
                CommunicationPreferences = new CommunicationPreferences
                {
                    AllowEmail = _allowEmail,
                    AllowMobile = _allowMobile,
                    VerifiedMobile = _verifiedMobile,
                    MobileVerificationCode = _mobileVerificationCode,
                    AllowEmailMarketing = _allowEmailMarketing,
                    AllowMobileMarketing = _allowSmsMarketing
                }
            };

            return candidate;
        }
    }
}