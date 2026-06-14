using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextLIMS.BLL.DTO.ForgetPassword
{
    public class ResetPassword
    {
        public string Password { get; set; }
        public string token { get; set; }
    }
}
