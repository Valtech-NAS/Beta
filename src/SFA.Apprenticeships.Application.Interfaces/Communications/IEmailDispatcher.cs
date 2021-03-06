﻿namespace SFA.Apprenticeships.Application.Interfaces.Communications
{
    using System;

    /// <summary>
    /// Used to queue / send an email message
    /// </summary>
    public interface IEmailDispatcher
    {
        void SendEmail(EmailRequest request);
    }
}
