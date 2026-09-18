namespace Store.Infrastructure.Persistence;

using Domain.Entities;

public static class ProductSeed
{
    public static readonly Guid NotebookId = Guid.Parse("11111111-1111-1111-1111-111111111111");
    public static readonly Guid MouseId = Guid.Parse("22222222-2222-2222-2222-222222222222");
    public static readonly Guid KeyboardId = Guid.Parse("33333333-3333-3333-3333-333333333333");
    public static readonly Guid MonitorId = Guid.Parse("44444444-4444-4444-4444-444444444444");

    public static void Apply(AppDbContext context)
    {
        if (context.Products.Any())
        {
            return;
        }

        context.Products.AddRange(
            new Product(NotebookId, "Notebook", 3500m),
            new Product(MouseId, "Mouse", 100m),
            new Product(KeyboardId, "Teclado", 200m),
            new Product(MonitorId, "Monitor", 1200m));

        context.SaveChanges();
    }
}
