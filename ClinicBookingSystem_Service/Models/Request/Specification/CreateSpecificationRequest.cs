using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicBookingSystem_Service.Models.Request.Specification
{
    public class CreateSpecificationRequest
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public IEnumerable<int> BusinessServiceId { get; set; }
    }
}
