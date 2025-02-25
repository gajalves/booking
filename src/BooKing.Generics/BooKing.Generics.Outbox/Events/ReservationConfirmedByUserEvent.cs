﻿using BooKing.Generics.EventSourcing;

namespace BooKing.Generics.Outbox.Events;
public class ReservationConfirmedByUserEvent : Event
{    
    public ReservationConfirmedByUserEvent(Guid reservationId, Guid userId, string userEmail) : base(reservationId)
    {
        ReservationId = reservationId;
        UserId = userId;
        UserEmail = userEmail;
    }

    public Guid ReservationId { get; set; } 
    public Guid UserId { get; set; } 
    public string UserEmail { get; set; }
}
