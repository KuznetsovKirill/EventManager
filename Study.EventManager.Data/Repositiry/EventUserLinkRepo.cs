﻿using Microsoft.EntityFrameworkCore;
using Study.EventManager.Data.Contract;
using Study.EventManager.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Study.EventManager.Data.Repositiry
{
    public class EventUserLinkRepo : AbstractRepo<EventUserLink>, IEventUserLinkRepo
    {
        public EventUserLink GetRecordByEventAndUser(int UserId, int EventId)
        {
            var userEvents = _eventManagerContext.Set<EventUserLink>().FirstOrDefault(x => x.UserId == UserId && x.EventId == EventId);
            return userEvents;
        }

        public List<EventUserLink> GetAllUsers(int EventId)
        {
            var listUsers = _eventManagerContext.EventUsers.Where(x => x.EventId == EventId).Include(x => x.User).ToList();
            return listUsers;
        }

        public List<EventUserLink> GetEventsByUser(int UserId, int del = 0)
        {
            var listEvents = _eventManagerContext.EventUsers.Where(x => x.UserId == UserId && x.Event.Del == del).Include(x => x.Event).ToList();
            return listEvents;
        }
    }
}
