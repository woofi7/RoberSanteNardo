using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace RoberSanteNardo.Models;

public class DiscordBotDbContext : DbContext
{
    [Serializable]
    public class Options
    {
        [Required] public required string ConnectionString { get; set; }
    }
}