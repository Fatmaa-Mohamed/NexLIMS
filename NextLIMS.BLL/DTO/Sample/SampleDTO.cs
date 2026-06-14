using NextLIMS.BLL.DTO.Client;
using NextLIMS.DAL.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextLIMS.BLL.DTO.Sample
{
    public class SampleDTO
    {

        public string SampleName { get; set; }
        public string SampleType { get; set; }

        public ClientDTO Client { get; set; }
     
    }
}
