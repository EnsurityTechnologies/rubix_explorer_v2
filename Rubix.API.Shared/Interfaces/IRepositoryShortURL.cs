﻿using Rubix.API.Shared.Entities;
using Rubix.API.Shared.Interfaces.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rubix.API.Shared.Interfaces
{
    public interface IRepositoryShortURL : IRepositoryBase<ShortUrl>
    {
        Task<ShortUrl> FindByShortCodeAsync(string shortCode);
        Task UpsertAsync(ShortUrl shortUrl);
    }
}