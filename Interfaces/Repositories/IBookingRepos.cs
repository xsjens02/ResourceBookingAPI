﻿using ResourceBookingAPI.Interfaces.Repositories.CRUD;
using ResourceBookingAPI.Models;

namespace ResourceBookingAPI.Interfaces.Repositories
{
    public interface IBookingRepos :
        ICrudRepos<Booking, string>
    {
        Task<IEnumerable<Booking>> GetAllWithinDates(DateTime startDate, DateTime endDate, string institutionId);
    }
}