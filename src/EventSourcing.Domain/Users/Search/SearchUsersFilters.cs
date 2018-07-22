namespace EventSourcing.Domain.Users.Search
{
    using System;

    public class SearchUsersFilters
    {
        private int page;
        private int limit;

        public SearchUsersFilters(int page, int limit)
        {
            this.Page = page;
            this.Limit = limit;
        }

        public string Name { get; set; }

        public string Job { get; set; }

        public int Page
        {
            get => this.page;
            private set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Invalid page. Please insert a positive value.");
                }

                this.page = value;
            }
        }

        public int Limit
        {
            get => this.limit;
            private set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Invalid limit. Please insert a positive value.");
                }

                this.limit = value;
            }
        }
    }
}