﻿using Hotel360InteractiveServer.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace Hotel360InteractiveServer.Data
{
    public class AuthDbContext(DbContextOptions<AuthDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
        private readonly string? _connectionString;

        public AuthDbContext(DbContextOptions<AuthDbContext> options, IConfiguration configuration)
        : this(options) => _connectionString = configuration.GetConnectionString("PORTALGOVERNANTAS")!;
        public DbSet<ConfirmRefeicao> ConfirmRefeicao { get; set; }

    }
}
