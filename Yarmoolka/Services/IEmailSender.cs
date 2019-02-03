﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Yarmoolka.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
        //Task SendEmailConfirmationAsync(string email, string callbackUrl);
    }
}
