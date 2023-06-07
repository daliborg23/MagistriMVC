using MagistriMVC.Models;
using MagistriMVC.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//    options.UseSqlServer(connectionString));
//builder.Services.AddDatabaseDeveloperPageExceptionFilter();
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (!string.IsNullOrEmpty(connectionString)) {
	builder.Services.AddDbContext<ApplicationDbContext>(options =>
		options.UseSqlServer(connectionString));
}
//var connectionString = builder.Configuration.GetConnectionString("AzureDBConnection");
//if (!string.IsNullOrEmpty(connectionString)) {
//	builder.Services.AddDbContext<ApplicationDbContext>(options =>
//		options.UseSqlServer(connectionString));
//}


//if (!string.IsNullOrEmpty(connectionString)) {
//	builder.Services.AddDbContext<ApplicationDbContext>(options =>
//		options.UseNpgsql(connectionString));
//}
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

//builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
//	.AddEntityFrameworkStores<ApplicationDbContext>();
//builder.Services.AddControllersWithViews();
builder.Services.AddIdentity<AppUser,IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options => {
	options.Cookie.Name = ".AspNetCore.Identity.Application";
	options.ExpireTimeSpan = TimeSpan.FromHours(2);
	options.SlidingExpiration = true; // reset behem preklikavani
});

builder.Services.Configure<IdentityOptions>(options => {
	options.Password.RequireNonAlphanumeric = false;
	options.Password.RequiredLength = 4;
	options.Password.RequireUppercase = false;
	options.Password.RequireDigit = false;
});
builder.Services.AddScoped<StudentsService>();
builder.Services.AddScoped<SubjectsService>();
builder.Services.AddScoped<GradesService>();

builder.Services.AddMvc();

var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment()) {
//	app.UseMigrationsEndPoint();
//}
//else {
//	app.UseExceptionHandler("/Home/Error");
//	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//	app.UseHsts();
//}

app.UseDeveloperExceptionPage();
app.UseMigrationsEndPoint();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
