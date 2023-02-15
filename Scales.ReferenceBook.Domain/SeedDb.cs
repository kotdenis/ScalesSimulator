using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Scales.ReferenceBook.Domain.Entities;

namespace Scales.ReferenceBook.Domain
{
    public static class SeedDb
    {
        public static async Task PopulateAsync(this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            var context = serviceScope.ServiceProvider.GetRequiredService<ReferenceBookDbContext>();
            //context.Database.EnsureCreated();
            context.Database.Migrate();

            if (!context.Set<ReferenceTransport>().Any())
            {
                await context.AddRangeAsync(new ReferenceTransport[]
                {
                    new ReferenceTransport { NumberOfAxles = 3,  Brand = "КамАЗ-65207", CarPlate = "A256AK" },
                    new ReferenceTransport { NumberOfAxles = 4, Brand = "Hyundai Mighty", CarPlate = "H698KO"},
                    new ReferenceTransport {NumberOfAxles = 2, Brand = "ISUZU ELF", CarPlate = "O745HB"},
                    new ReferenceTransport {NumberOfAxles = 6, Brand = "МАЗ-6310", CarPlate = "A963KO"},
                    new ReferenceTransport {NumberOfAxles = 2, Brand = "ГАЗ «Садко»", CarPlate = "E123BH"},
                    new ReferenceTransport {NumberOfAxles = 4, Brand = "HOWO A7", CarPlate = "T574TB"},
                    new ReferenceTransport {NumberOfAxles = 2, Brand = "JAC N-56", CarPlate = "P897HC"},
                    new ReferenceTransport {NumberOfAxles = 5, Brand = "МАЗ-5440", CarPlate = "K321KK"},
                    new ReferenceTransport {NumberOfAxles = 3, Brand = "MAN TGS", CarPlate = "H477CC"},
                    new ReferenceTransport {NumberOfAxles = 3, Brand = "Scania «G-Series»", CarPlate = "P521OK"},
                    new ReferenceTransport {NumberOfAxles = 3, Brand = "КрАЗ М16.1Х", CarPlate = "H147KA"},
                    new ReferenceTransport {NumberOfAxles = 3, Brand = "Volvo серии «FH»", CarPlate = "O686PO"},
                    new ReferenceTransport {NumberOfAxles = 5, Brand = "ISUZU GIGA 6х4", CarPlate = "C114HA"},
                    new ReferenceTransport {NumberOfAxles = 6, Brand = "КрАЗ-6230C40", CarPlate = "K931CA"},
                    new ReferenceTransport {NumberOfAxles = 3, Brand = "КамАЗ-689011", CarPlate = "M665MH"},
                    new ReferenceTransport {NumberOfAxles = 2, Brand = "БелАЗ-75320", CarPlate = "B127KB"},
                    new ReferenceTransport {NumberOfAxles = 6, Brand = "Тонар-7502", CarPlate = "T788TT"},
                    new ReferenceTransport {NumberOfAxles = 3, Brand = "MAN TGS", CarPlate = "A441KM"},
                    new ReferenceTransport {NumberOfAxles = 3, Brand = "Тонар-7501", CarPlate = "P333OP"},
                    new ReferenceTransport {NumberOfAxles = 3, Brand = "Тонар-45251", CarPlate = "M149KC"}
                });
                await context.SaveChangesAsync();
            }
        }
    }
}
