﻿using ECommerce.Application.Comman.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Comman
{
    public class ProductParams
    {
        private const int MaxPageSize = 50;

        private int _pageSize = 10;

        public int PageNumber { get; set; } = 1;

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
        }

        public string Search { get; set; }

        public int? CategoryId { get; set; }

        public ProductSortOption? SortOption { get; set; } = ProductSortOption.NameAsc;
    }

}
