namespace EventSourcing.Application.Queries.Users
{
    using System;

    using Infrastructure.Queries;

    public class GetUserByIdQuery : IQuery
    {
        public GetUserByIdQuery(Guid id)
        {
            this.Id = id;
        }

        public Guid Id { get; set; }
    }
}