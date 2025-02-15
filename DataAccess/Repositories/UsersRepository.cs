﻿using Microsoft.EntityFrameworkCore;
using WebMarket.Contracts;
using WebMarket.DataAccess.Models;
using WebMarket.DataAccess.Repositories.Abstractions;

namespace WebMarket.DataAccess.Repositories
{
    public class UsersRepository : EntityRepository<User>, IUsersRepository
    {
        public UsersRepository(MarketContext context) : base(context, context => context.Users)
        {
        }

        public async override Task<User> Create(User user)
        {
            if (user == null)
                throw new ArgumentNullException("Adding user is null!");
            var res = await _dbSet.AddAsync(user);
            await _context.SaveChangesAsync();
            return res.Entity;
        }

        public async Task<User?> GetUserByLogin(string login)
        {
            return await _dbSet
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Login.Equals(login));
        }

        public async Task<int> Update(int id, string? login, string? email, string? address)
        {
            var affected = await _dbSet
                .Where(u => u.Id == id)
                .ExecuteUpdateAsync(u => u
                    .SetProperty(u => u.Login, u => login ?? u.Login)
                    .SetProperty(u => u.Email, u => email ?? u.Email)
                    .SetProperty(u => u.Address, u => address ?? u.Address));
            return affected > 0 ? id : -1;
        }
        //TODO: BAD OPTIMIZATION!
        public async Task<List<ShoppingCartElementPresentation>> GetShoppingCartElements(int id)
        {
            var user = await _dbSet
                .AsNoTracking()
                .Include(u => u.ShoppingCartElements)
                .ThenInclude(el => el.Product)
                .SingleOrDefaultAsync(u => u.Id == id);
            return user?.ShoppingCartElements?.Select(el =>
            new ShoppingCartElementPresentation(el.Id, el.ProductId, el.Product.Name, el.ProductAmount,
            el.Product.Price, el.Product.Image))?.ToList() ?? new List<ShoppingCartElementPresentation>();
        }
    }
}
