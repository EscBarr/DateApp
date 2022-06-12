using DateApp.EntityContext;
using DateApp.Services.AuthService;
using DateApp.Services;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages().AddViewLocalization().AddDataAnnotationsLocalization();
builder.Services.AddScoped<ApplicationContext>();
builder.Services.AddSingleton<IAuthService>(new AuthService(builder.Configuration));
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();
builder.Services.AddHttpClient();

//ПАРАМЕТРЫ ТОКЕНА
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["JwtIssuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["JwtAudience"],
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtKey"])),
            ClockSkew = TimeSpan.Zero
        };
    });
//ПАРАМЕТРЫ ТОКЕНА
//Параметры авторизации
builder.Services.AddAuthorization(options =>
{
    options.DefaultPolicy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
    .RequireAuthenticatedUser()
    .Build();
});
//Параметры авторизации
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseDefaultFiles();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();
///ЧТО ЭТО ЗА ГОВНО?????????????????
///ВИДИМО ПОСТОЯННОЕ ХРАНЕНИЕ JWT-ТОКЕНА
app.Use(async (context, next) =>
{
    var token = context.Session.GetString("Token");
    if (!String.IsNullOrEmpty(token))
    {
        context.Request.Headers.Add("Authorization", "Bearer " + token);
    }
    await next();
});

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    //endpoints.MapControllerRoute("default", "{controller}/{action=Index}/{id?}");
    endpoints.MapRazorPages();
});

SetupDatabase();

app.Run();

void SetupDatabase()
{
    ApplicationContext context = new ApplicationContext();
    // check and add roles
    AuthService auth = new AuthService(builder.Configuration);
    if (!Directory.Exists("Backup"))
    {
        Directory.CreateDirectory("Backup");
    }
    if (!context.Educations.Any())
    {
        context.Educations.Add(new EducationDegrees { Id = 1, Degree = "Основное общее" });
        context.Educations.Add(new EducationDegrees { Id = 2, Degree = "Среднее общее" });
        context.Educations.Add(new EducationDegrees { Id = 3, Degree = "Cреднее профессиональное" });
        context.Educations.Add(new EducationDegrees { Id = 4, Degree = "Высшее" });
        context.SaveChanges();
    }
    if (!context.Users.Any())
    {
        var appUser = new User { Email = "tmpadmin@mail.com", PwHash = auth.GetHashedPassword("tmpadmin"), UserRole = "Admin", Avatar = System.Convert.FromBase64String("iVBORw0KGgoAAAANSUhEUgAAAMgAAADIBAMAAABfdrOtAAAAG1BMVEXMzMyWlpacnJyqqqrFxcWxsbGjo6O3t7e+vr6He3KoAAAACXBIWXMAAA7EAAAOxAGVKw4bAAACmElEQVR4nO3av2/aQBQH8O8ZbDPahJCOtpO2jE6aSB0PiDIbhswgIZHRphJdSfqP994ZEpwAigClrfr9SLyLeY7f/bIXAxAREREREREREREREREREREREf0/6ldfNDBr5svwrJ9MsDGxB6cZt+CG8VkZXsRJqDcm9jBI3RN0LxrNMjwz3w7STYl9jDWaKHJcLmwAihRFBngtqKiSOEBiJkYqPWY2mPn7JN/BieAElcQBfsKMJAa6qQ1Ao+3KCvi5jGQ9cRD/RLreHdlgjsdFVCbMxFUShxi0zFigIhvM8WNYblg31NXEAWrNRbXDtXaZuOzgWCOpxxNUp94p92uvrXGsNfFOYPdxkdlgDorYbqVx+jqxv0KuVbkdkl4Ldju8TuzPdBOVG9s9deX6nlQ61h0fJ0lSeUT1zN2hZWWSJDrWsys0Kg/bQWanUJnvg2M9hYmIiIiIiIiIiIiIiIjoD1HLz79SJGxev69IYI7ktaa8mHcCOTT68RS4CydQkX3PvUVQ//b+Ija2F5gvi9Q/618m3A61eoA/3l4EPVydw9G++cNNUqhhIE0tGUiR4Tm83GTWipyNMF0WqWVlcFM1h1fsKuJO7rJa3sUP3N/eQE1sc38rHVMm1Yhws14k6DSiZRFHl6EeKWfxtH0BzXQ5uh410mH+HRemz0rb5gJdKWJSmJrP6vWzFJl7+bKIWoVA+aPp9iJm4W0Ho84okgvJ/0gTrNYkwFMtr4zEmeHtSPA12jESOUu6O3oYme7bK0mzNhJniEoR9xRv1wSDbGcRWRMM81mGue7LlaR5WRP4Z9Ui9sDGtd2FXTeVnGx2Fx51V8OPO3KqNC+7C360oUg5pf1wdZ/sLPIO3kf8mGD2ATVU5wOKEP09fgOLdXyF2B0MogAAAABJRU5ErkJggg==") };
        context.Users.Add(appUser);
        context.SaveChanges();
    }
}