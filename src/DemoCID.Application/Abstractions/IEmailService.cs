using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoCICD.Application.Abstractions;

public interface IEmailService
{
    Task<bool> SendPasswordResetCodeAsync(string email, string resetCode, string userName);
    Task<bool> SendEmailAsync(string to, string subject, string body);
}