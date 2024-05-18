namespace DatingAPI.Helpers
{
    public class PaginationHeader
    {
        public PaginationHeader(int currentPage, int itemsPage, int totalItems, int totalPages)
        {
            this.currentPage = currentPage;
            ItemsPage = itemsPage;
            TotalItems = totalItems;
            TotalPages = totalPages;
        }

        public int currentPage { get; set; }
        public int ItemsPage { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
    }
}
