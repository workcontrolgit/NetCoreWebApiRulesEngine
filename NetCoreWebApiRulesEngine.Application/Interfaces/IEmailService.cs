using NetCoreWebApiRulesEngine.Application.DTOs.Email;
using System.Threading.Tasks;

namespace NetCoreWebApiRulesEngine.Application.Interfaces
{
    public interface IEmailService
    {
        Task SendAsync(EmailRequest request);
    }
}