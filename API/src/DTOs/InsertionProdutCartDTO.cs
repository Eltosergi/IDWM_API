using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.src.DTOs
{
    public class InsertionProdutCartDTO
    {
        public required string UserName { get; set; }
        public required string ProductName { get; set; }
        public required int Amount { get; set; }
    }
}