using FinTracker.SharedKernel.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Domain.Entities
{
    public class RefreshToken:BaseEntity<Guid>
    {
        public Guid UserId { get;private set; } 
        public string TokenHash {  get;private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime ExpiresAt {  get;private set; }
        public DateTime? RevokedAt { get;private set; }
        public string DeviceInfo { get;private set; }
    }
}
