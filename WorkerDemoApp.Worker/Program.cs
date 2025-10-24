// WorkerDemoApp.Worker/Program.cs
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using WorkerDemoApp.Core.Abstracts;
using WorkerDemoApp.Core.Extensions;
using WorkerDemoApp.DAL;
using WorkerDemoApp.Entity;
using WorkerDemoApp.Services;
using WorkerDemoApp.Services.Abstracts;
using WorkerDemoApp.Services.Concrete;
using WorkerDemoApp.Worker;

var builder = Host.CreateApplicationBuilder(args);

// SMTP
builder.Services.Configure<SmtpOptions>(builder.Configuration.GetSection("Smtp"));

// DbContext
builder.Services.AddDbContext<BaseContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("dbCon")));

// Identity (Guid)
builder.Services.AddIdentityCore<AppUser>(opt =>
{
    opt.User.RequireUniqueEmail = true;
})
.AddRoles<IdentityRole<Guid>>()
.AddEntityFrameworkStores<BaseContext>();

// Infra & Servisler
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IEFContext, BaseContext>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IAuthService, AuthService>();

// Worker
builder.Services.AddHostedService<EmailReminderWorker>();

var host = builder.Build();
host.Run();
