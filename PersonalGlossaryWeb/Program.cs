using PersonalGlossary_API.Models.DTO;
using PersonalGlossaryWeb.Services;
using PersonalGlossaryWeb.Services.IServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddAutoMapper(o =>
{
    o.CreateMap<TermDTO, TermCreateDTO>();
    o.CreateMap<TermUpdateDTO, TermDTO>();
    //o.CreateMap<Term, TermDTO>();
    //Auth
    //o.CreateMap<User, UserDTO>();
    //Category
    //o.CreateMap<CategoryCreateDTO, Category>();
    //o.CreateMap<CategoryUpdateDTO, Category>();
    //o.CreateMap<Category, CategoryDTO>().
    //ForMember(dest => dest.TermTitle, opt => opt.MapFrom(src => src.Term != null ? src.Term.Title : null));
    //o.CreateMap<CategoryUpdateDTO, CategoryDTO>();
});

builder.Services.AddHttpClient("PersonalGlossaryAPI", client =>
{
    var termAPIUrl = builder.Configuration.GetValue<string>("ServiceUrls:TermAPI");
    client.BaseAddress = new Uri(termAPIUrl); //base address for all the http clients in the application
    client.DefaultRequestHeaders.Add("Accept", "application/json"); //this shows that the application accepts json response
});
builder.Services.AddScoped<ITermService, TermService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
