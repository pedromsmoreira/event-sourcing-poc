﻿namespace EventSourcing.Domain.Users.Search
{
    using System;

    public class UserSearchResponse
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Job { get; set; }

        public DateTime LastSnapshot { get; set; }
    }
}