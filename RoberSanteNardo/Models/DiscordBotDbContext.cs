using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace RoberSanteNardo.Models;

public class DiscordBotDbContext(DbContextOptions options) : DbContext(options)
{
    [Serializable]
    public class Options
    {
        [Required] public required string ConnectionString { get; set; }
    }
}