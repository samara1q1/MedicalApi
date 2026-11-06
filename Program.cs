var builder = WebApplication.CreateBuilder(args);

// Добавляем сервисы контроллеров
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Регистрируем генератор Swagger
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Настройка для режима разработки
if (app.Environment.IsDevelopment())
{
    // Включаем Swagger и его графический интерфейс (Swagger UI)
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();