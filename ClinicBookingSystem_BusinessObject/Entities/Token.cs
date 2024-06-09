using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicBookingSystem_BusinessObject.Entities
{
    public class Token : BaseEntities
    {
        [Key]
        public int Id { get; set; }
        public string AccessToken { get; set; }
        public DateTime Expired { get; set; }
        public string RefreshToken { get; set; }
        public DateTime ExpiredRefreshToken { get; set; }
        public DateTime TimeStamps { get; set; }
        //User
        public User User { get; set; }
       
    }
}
