﻿using Rubix.API.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rubix.Explorer.API.Dtos
{
    public class GetAllTransactionsInput
    {
        public int Page { get; set;}
        public int PageSize { get; set;}
       // public ActivityFilter Filter { get; set;}
    }
    public class GetAllTokensInput 
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
    public class GetAllTransactionsForTokensInput
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string Token_Id { get; set;}
    }
}
