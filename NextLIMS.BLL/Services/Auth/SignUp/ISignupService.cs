
using NexLIMS.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextLIMS.BLL.Services.SignupService
{
    public interface ISignupService
    {
        Task<bool> SignupAsync(RegisterDto request);
    }
}
