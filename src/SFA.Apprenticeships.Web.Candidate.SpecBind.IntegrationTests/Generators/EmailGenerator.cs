﻿namespace SFA.Apprenticeships.Web.Candidate.SpecBind.IntegrationTests.Generators
{
    using System;

    public static class EmailGenerator
    {
        public static string GenerateEmailAddress()
        {
            const string format = "valtechnas+{0}@gmail.com";

            return string.Format(format, DateTime.Now.Ticks);
        }
    }
}