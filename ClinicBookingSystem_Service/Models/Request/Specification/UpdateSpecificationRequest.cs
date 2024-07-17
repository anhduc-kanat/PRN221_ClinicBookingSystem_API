using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicBookingSystem_Service.Models.Request.Specification
{
    public class UpdateSpecificationRequest
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public ICollection<int> BusinessServiceId { get; set; }
    }
}
