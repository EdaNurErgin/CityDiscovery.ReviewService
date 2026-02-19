//namespace IdentityService.Shared.MessageBus.Identity
//{
//    public class UserDeletedEvent
//    {
//        public Guid UserId { get; set; }
//        public string UserName { get; set; }
//        public DateTime DeletedAt { get; set; }
//    }
//}

using System;

namespace IdentityService.Shared.MessageBus.Identity
{
    // Class yerine Record kullanıyoruz.
    // Property isimleri Identity Service ile AYNI olmalı (DeletedAt -> DeletedAtUtc)
    public record UserDeletedEvent(
        Guid UserId,
        string UserName,
        string Email,
        string Role,
        DateTime DeletedAtUtc
    );
}