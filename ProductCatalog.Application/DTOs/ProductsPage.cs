using System;
using System.Collections.Generic;
using System.Text;

namespace ProductCatalog.Application.DTOs
{
    public class ProductsPage<T>
    {
        public int Page {  get; set; }
        public int PageSize { get; set; }
        public int Total {  get; set; }
        public int TotalPages { get; set; }
        public List<T> Items { get; set; } = new List<T>();
    }
}
